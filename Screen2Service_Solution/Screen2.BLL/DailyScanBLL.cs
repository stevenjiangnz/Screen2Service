using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Shared;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class DailyScanBLL : BaseBLL<DailyScan>, IBaseBLL<DailyScan>
    {
        #region properties
        private AuditLogBLL _auditBLL;
        private string Name_Space_Prefix = "Screen2.BLL.";
        #endregion

        #region Constructors
        public DailyScanBLL(IUnitWork unit) : base(unit)
        {
            _auditBLL = new AuditLogBLL(_unit);
        }
        #endregion

        #region Methods
        public List<DailyScan> GetDailyScanListByUser(string userId, int? zoneId)
        {
            List<DailyScan> aList = new List<DailyScan>();

            aList = _unit.DataContext.DailyScan
                .Where(p => p.Owner == userId && ((!zoneId.HasValue && p.ZoneId == null) || p.ZoneId == zoneId)).OrderBy(p=>p.Name).ToList();

            return aList;
        }


        public DailyScanResult EvaluateScanShare(int shareId, int tradingDate, string formula, DailyScan ds)
        {
            VerifyResult result = null;

            result = new ScanCalculator(_unit).CheckDailyMatch(shareId, tradingDate, formula);

            DailyScanResult aResult = new DailyScanResult
            {
                DailyScanId = ds.Id,
                IsMatch = result.IsMatch,
                Message = result.ErrorMessage,
                TradingDate = result.TradingDate,
                ShareId = shareId,
                ProcessDT = DateTime.Now
            };

            return aResult;
        }

        public List<OutStockSearchResult> SearchScan(int tradingDate, int scanId, bool force)
        {
            DailyScanResultBLL dsrBll = new DailyScanResultBLL(_unit);
            DailyScanBLL dsBll = new DailyScanBLL(_unit);
            List<OutStockSearchResult> searchResult = new List<OutStockSearchResult>();
            List<DailyScanResult> dsrList = null;

            if (!force)
            {
                dsrList = dsrBll.GetDailyScanResult(tradingDate, scanId);

                if (dsrList.Count == 0)
                {
                    force = true;
                }
            }

            if (force)
            {
                dsrList = new List<DailyScanResult>();
                DailyScan ds = dsBll.GetByID(scanId);
                DailyScanResult dsr;
                List<int> shareIDs = GetShareListByDailyScan(ds);
                string className = string.Empty;
                string formula = string.Empty;

                if (ds.UseRule)
                {
                    var rule = new RuleBLL(_unit).GetByID(ds.RuleId.Value);

                    if(rule.Type.Equals("Assembly", StringComparison.InvariantCultureIgnoreCase))
                    {
                        className = rule.Assembly;
                    }
                    else
                    {
                        formula = rule.Formula;
                    }
                }
                else
                {
                    formula = ds.Formula;
                }

                if (string.IsNullOrEmpty(className))
                {
                    foreach (int shareId in shareIDs)
                    {
                        try
                        {
                            dsr = EvaluateScanShare(shareId, tradingDate, formula, ds);

                            if (dsr.IsMatch)
                            {
                                dsrList.Add(dsr);
                            }
                        }
                        catch(Exception ex)
                        {
                            LogHelper.Error(_log, string.Format("Error Process daily on share ({0}) at {1}. ", shareId, tradingDate), ex);
                        }
                    }
                }
                else
                {
                    dsrList = this.RunDailyScanByAssembly(ds, className, tradingDate);
                }

                dsrBll.DeleteDailyScanResultByDailyScan(scanId, tradingDate);
                dsrBll.AddDailyScanResultBatch(dsrList);
            }

            dsrList = dsrList.Where(c => c.IsMatch).ToList();

            string shareListString = GetShareListString(dsrList);

            searchResult = new IndicatorBLL(_unit).SearchShareByShareString(shareListString, tradingDate);

            return searchResult;
        }

        //public string GetFormula(DailyScan dailyScan)
        //{
        //    string formula = string.Empty;

        //    if (dailyScan.UseRule)
        //    {
        //        var rule = new RuleBLL(_unit).GetByID(dailyScan.RuleId.Value);
        //        formula = rule.Formula;
        //    }
        //    else
        //    {
        //        formula = dailyScan.Formula;
        //    }

        //    return formula;
        //}


        public List<DailyScanResult> RunDailyScanByAssembly(DailyScan dailyScan, string className, int tradingDate)
        {
            List<DailyScanResult> dsResult = new List<DailyScanResult>();
            Type scanClassType = Type.GetType(Name_Space_Prefix + className);

            BaseStatScan scanner = (BaseStatScan)Activator.CreateInstance(scanClassType);
            scanner.InitUnit(_unit);

            List<int> watchIds = GetWtachLists(dailyScan);

            foreach(int w in watchIds)
            {
                List<DailyScanResultItem> resultList = scanner.DailyScanByWatch(w, tradingDate);

                foreach (var r in resultList)
                {
                    DailyScanResult ds = new DailyScanResult();
                    ds.DailyScanId = dailyScan.Id;
                    ds.IsMatch = true;
                    ds.Message = "Scan match by assembly " + className;
                    ds.ProcessDT = DateTime.Now;
                    ds.ShareId = r.ShareId;
                    ds.TradingDate = r.TradingDate;

                    dsResult.Add(ds);
                }
            }
            return dsResult;
        }


        public List<int> GetShareListByDailyScan(DailyScan dailyScan)
        {
            List<int> shareIDs = new List<int>();

            List<int> watchList = this.GetWtachLists(dailyScan);

            foreach (int w in watchList)
            {
                List<WatchListDetail> wdList = new WatchListDetailBLL(_unit).GetWatchListListByWatchID(w);

                foreach (WatchListDetail wd in wdList)
                {
                    if (shareIDs.IndexOf(wd.ShareId) == -1)
                    {
                        shareIDs.Add(wd.ShareId);
                    }
                }
            }

            return shareIDs;
        }

        public List<int> GetWtachLists(DailyScan ds)
        {
            List<int> watchIdList = new List<int>();

            string[] watchList = ds.WatchListString.Split(',');

            foreach (string w in watchList)
            {
                watchIdList.Add(int.Parse(w));
            }

            return watchIdList;
        }


        private string GetShareListString(List<DailyScanResult> resultList)
        {
            string shString = string.Empty;

            foreach (var result in resultList)
            {
                if (shString.Length > 0)
                {
                    shString += ",";
                }

                shString += result.ShareId.ToString();
            }

            return shString;
        }
        #endregion
    }
}
