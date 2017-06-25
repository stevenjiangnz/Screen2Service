using HtmlAgilityPack;
using MaasOne.Base;
using MaasOne.Finance.YahooFinance;
using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Shared;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Screen2.BLL
{
    public class ShareInfoBLL : BaseBLL<ShareInfo>, IBaseBLL<ShareInfo>
    {
        public ShareBLL _sBLL;
        private AuditLogBLL _auditBLL;

        #region Constructors
        public ShareInfoBLL(IUnitWork unit) : base(unit)
        {
            _sBLL = new ShareBLL(_unit);
            _auditBLL = new AuditLogBLL(_unit);
        }
        #endregion

        #region Methods
        public ShareInfo UpdateShareInfo(ShareInfo input)
        {
            ShareInfo output = null;

            output = GetByID(input.Id);

            output.CompanySummary = input.CompanySummary;
            output.CompanyUrl = input.CompanyUrl;
            output.EmployeeNumber = input.EmployeeNumber;
            output.IsTop100 = input.IsTop100;
            output.IsTop200 = input.IsTop200;
            output.IsTop300 = input.IsTop300;
            output.IsTop50 = input.IsTop50;
            output.LastProcessed = DateTime.Now;
            output.MarketCapM = input.MarketCapM;
            output.ShareId = input.ShareId;
            output.ShareType = input.ShareType;
            output.YIndustry = input.YIndustry;
            output.YSector = input.YSector;

            Update(output);

            return output;
        }


        public void UploadShareProfileFromYahooAll()
        {
            int successCount = 0;
            int failCount = 0;
            StringBuilder resultString = new StringBuilder();

            List<Share> shareList = (new ShareBLL(_unit)).GetList().ToList();

            foreach (Share s in shareList)
            {
                try
                {
                    UploadShareProfileFromYahoo(s.Symbol);
                    successCount++;
                    Debug.WriteLine(successCount.ToString() + "    "
                        + string.Format("successfully upload stock info for {0}  \n", s.Symbol));
                    resultString.Append(string.Format("successfully upload stock info for {0} \n", s.Symbol));
                }
                catch (Exception ex)
                {
                    failCount++;
                    resultString.Append(string.Format("Fail to upload stock info for {0} \n {1}", s.Symbol, ex.ToString()));
                }
            }

            _auditBLL.Create(new AuditLog
            {
                ActionMessage = resultString.ToString(),
                ActionTime = DateTime.Now,
                ExtraData = "",
                ActionType = ActionType.UploadShareInfo.ToString(),
                ActionResult = string.Format("Success: {0}; Failed {1}", successCount, failCount)
            });

            LogHelper.Info(_log, resultString.ToString());
        }
        /// <summary>
        /// Gets the share profile.
        /// </summary>
        /// <param name="symbol">The y symbol.</param>
        /// <returns></returns>
        public void UploadShareProfileFromYahoo(string symbol)
        {
            SettingBLL settingBLL = new SettingBLL(_unit);


            Share s = _sBLL.GetShareBySymbol(symbol);
            try
            {
                // Load profile info
                string urlTemplate = settingBLL.GetSetting(Shared.SettingKey.YahooProfileUrl);
                string url = string.Format(urlTemplate, symbol);

                if (!string.IsNullOrEmpty(url))
                {
                    HtmlWeb web = new HtmlWeb();
                    HtmlDocument doc = web.Load(url);

                    if (doc != null)
                    {
                        //Process HTML Doc
                        this.ExtractProfileEntityFromHtmlDoc(s, doc);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                throw;
            }

            //return shareInfo;

        }

        public void ExtractProfileEntityFromHtmlDoc(Share s, HtmlDocument doc)
        {
            var title = from a in doc.DocumentNode.Descendants("h3")
                        where a.InnerText.Equals("ETF")
                        select a;

            var titleObj = title.SingleOrDefault();
            if (titleObj == null)
            {
                _sBLL.SetShareType(s, "Stock");
                PopulateStockStatDetails(s, doc);
            }
            else
            {
                _sBLL.SetShareType(s, "ETF");
                PopulateETFStatDetails(s, doc);
            }
        }

        public void PopulateETFStatDetails(Share s, HtmlDocument doc)
        {
            ETFInfo info = new ETFInfo();

            var profileTableQuery = from t in doc.DocumentNode.Descendants("table")
                                    where t.Id == "yfncsumtab"
                                    select t;

            var profileTable = profileTableQuery.FirstOrDefault();

            if (profileTable != null)
            {
                var businessSummaryTableQuery = from t in profileTable.Descendants("table")
                                                where t.InnerText.Contains("Fund Overview")
                                                select t;
                var businessSummaryTable = businessSummaryTableQuery.FirstOrDefault();

                if (businessSummaryTable != null)
                {
                    var fundDetailTable = businessSummaryTable.NextSibling;

                    if (fundDetailTable != null)
                    {
                        var trLine = from tr in fundDetailTable.Descendants("tr")
                                     select tr;

                        var trLineArray = trLine.ToArray();

                        info.Category = HttpUtility.HtmlDecode(trLineArray[1].LastChild.InnerText);
                        info.FundFamily = HttpUtility.HtmlDecode(trLineArray[2].LastChild.InnerText);

                        var netAssetString = HttpUtility.HtmlDecode(trLineArray[3].LastChild.InnerText);

                        netAssetString = netAssetString.Replace("M", "");

                        double netAsset;

                        if (double.TryParse(netAssetString, out netAsset))
                        {
                            info.NetAssetM = netAsset;
                        }


                        var yieldString = HttpUtility.HtmlDecode(trLineArray[4].LastChild.InnerText);

                        yieldString = yieldString.Replace("%", "");

                        double yield;

                        if (double.TryParse(yieldString, out yield))
                        {
                            info.YieldPercentage = yield;
                        }
                        info.InceptionDate = HttpUtility.HtmlDecode(trLineArray[5].LastChild.InnerText);
                    }

                    Share s2 = _sBLL.GetByID(s.Id);
                    s2.Industry = info.Category;
                    _sBLL.Update(s2);

                    ETFInfoBLL eBLL = new ETFInfoBLL(_unit);
                    ETFInfo eInfo2 = eBLL.GetByShareID(s.Id);

                    if (eInfo2 != null)
                    {
                        eInfo2.Category = info.Category;
                        eInfo2.FundFamily = info.FundFamily;
                        eInfo2.InceptionDate = info.InceptionDate;
                        eInfo2.NetAssetM = info.NetAssetM;
                        eInfo2.YieldPercentage = info.YieldPercentage;

                        eBLL.Update(eInfo2);
                    }
                    else
                    {
                        eInfo2 = info;
                        eInfo2.ShareId = s.Id;
                        eBLL.Create(eInfo2);
                    }
                }
            }
        }

        public void PopulateStockStatDetails(Share s, HtmlDocument doc)
        {
            ShareInfo info = new ShareInfo();

            // Get the company web url
            var homePage = from a in doc.DocumentNode.Descendants("a")
                           where a.InnerText.Contains("Home Page")
                           select a;

            var link = homePage.FirstOrDefault();

            if (link != null)
            {
                info.CompanyUrl = link.Attributes["href"].Value;
            }



            // Get business description
            var profileTableQuery = from t in doc.DocumentNode.Descendants("table")
                                    where t.Id == "yfncsumtab"
                                    select t;

            var profileTable = profileTableQuery.FirstOrDefault();

            if (profileTable != null)
            {
                var businessSummaryTableQuery = from t in profileTable.Descendants("table")
                                                where t.InnerText.Contains("Business Summary")
                                                select t;

                var businessSummary = businessSummaryTableQuery.SingleOrDefault();

                HtmlNode summaryContentNode = null;

                if (businessSummary != null)
                {
                    summaryContentNode = businessSummary.NextSibling;
                }

                if (summaryContentNode != null)
                {
                    info.CompanySummary = HttpUtility.HtmlDecode(summaryContentNode.InnerText);
                }
            }


            // Get Profile Detail
            if (profileTable != null)
            {
                var detailTableQuery = from t in profileTable.Descendants("table")
                                       where t.InnerText.Contains("Sector") &&
                                       t.InnerText.Contains("Industry") &&
                                       t.InnerText.Contains("Employees")
                                       select t;

                var tablesTemp = detailTableQuery.ToArray();

                if (tablesTemp != null && tablesTemp.Length > 0)
                {
                    var trLine = from tr in detailTableQuery.ToArray()[1].Descendants("tr")
                                 select tr;

                    var trLineArray = trLine.ToArray();

                    if (trLineArray.Length == 4)
                    {
                        info.YSector = HttpUtility.HtmlDecode(trLineArray[1].LastChild.InnerText);
                        info.YIndustry = HttpUtility.HtmlDecode(trLineArray[2].LastChild.InnerText);
                        string employeeNumberString = trLineArray[3].LastChild.InnerText;
                        int employeeNumner;

                        if (int.TryParse(employeeNumberString, NumberStyles.AllowThousands,
                 CultureInfo.InvariantCulture, out employeeNumner))
                        {
                            info.EmployeeNumber = employeeNumner;
                        }
                    }
                }
            }

            // Get details from yahoo
            GetShareStatFromYahoo(s.Symbol, info);

            Share s2 = _sBLL.GetByID(s.Id);
            s2.Industry = info.YIndustry;
            _sBLL.Update(s2);

            ShareInfo sInfo2 = GetByShareID(s.Id);

            if (sInfo2 != null)
            {
                sInfo2.CompanySummary = info.CompanySummary;
                sInfo2.CompanyUrl = info.CompanyUrl;
                sInfo2.EmployeeNumber = info.EmployeeNumber;
                sInfo2.LastProcessed = DateTime.Now;
                sInfo2.MarketCapM = info.MarketCapM;
                sInfo2.YIndustry = info.YIndustry;
                sInfo2.YSector = info.YSector;

                Update(sInfo2);
            }
            else
            {
                sInfo2 = info;
                sInfo2.LastProcessed = DateTime.Now;
                sInfo2.ShareId = s.Id;
                Create(sInfo2);
            }

        }


        /// <summary>
        /// Gets the share stat entity from yahoo.
        /// </summary>
        /// <param name="symbol">The y symbol.</param>
        /// <returns></returns>
        public void GetShareStatFromYahoo(string symbol, ShareInfo shareInfo)
        {

            try
            {
                CompanyStatisticsDownload csDownload = new CompanyStatisticsDownload();
                CompanyStatisticsDownloadSettings settings = new CompanyStatisticsDownloadSettings();

                settings.ID = symbol;

                csDownload.Settings = settings;

                Response<CompanyStatisticsResult> resp = csDownload.Download();

                if (resp != null)
                {
                    if (resp.Result.Item.ValuationMeasures != null)
                    {
                        var v = resp.Result.Item.ValuationMeasures;

                        if (v.MarketCapitalisationInMillion > 0)
                            shareInfo.MarketCapM = v.MarketCapitalisationInMillion;
                    }
                }
            }
            catch (Exception ex)
            {
                // Add system log
                _log.Error("Process Share Statics fail " + symbol, ex);

                throw;
            }

        }

        public ShareInfo GetByShareID(int shareID)
        {
            ShareInfo sInfo = null;

            sInfo = Get(p => p.ShareId == shareID).SingleOrDefault();

            return sInfo;
        }

        #endregion
    }
}