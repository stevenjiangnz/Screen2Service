using MaasOne.Base;
using MaasOne.Finance.YahooFinance;
using Microsoft.WindowsAzure.Storage;
using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using System.Configuration;
using Screen2.Entity;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using ActiveUp.Net.Mail;
using Screen2.Shared;

namespace Screen2.BLL
{
    public class TickerBLL : BaseBLL<Ticker>, IBaseBLL<Ticker>
    {
        #region Properties
        private AuditLogBLL _auditBLL;
        #endregion

        #region Constructors
        public TickerBLL(IUnitWork unit, string connectionString = null) : base(unit, connectionString)
        {
            _auditBLL = new AuditLogBLL(_unit);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the ticker by date.
        /// </summary>
        /// <param name="symbolId">The symbol identifier.</param>
        /// <param name="tradingDate">The trading date.</param>
        /// <returns></returns>
        public Ticker GetTickerByDate(int symbolId, int tradingDate)
        {
            Ticker t = null;

            List<Ticker> tList = this.GetTickerListByShareDB(symbolId, tradingDate, tradingDate);

            if (tList.Count == 1)
            {
                t = tList[0];
            }

            return t;
        }

        public List<Ticker> GetTickerListByDate(int shareId, int tradingDate, int count)
        {
            List<Ticker> tList = new List<Ticker>();

            List<Ticker> returnList = new List<Ticker>();

            int endDate = DateHelper.DateToInt(DateHelper.IntToDate(tradingDate).AddDays(count * 2));

            tList = this.GetTickerListByShareDB(shareId, tradingDate, endDate);

            if(tList.Count > count)
            {
                returnList = tList.Take(count).ToList();
            }
            else
            {
                returnList = tList;
            }

            return returnList;
        }

        /// <summary>
        /// Gets the ticker list by share database.
        /// </summary>
        /// <param name="symbolId">The symbol identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public List<Ticker> GetTickerListByShareDB(int symbolId, int? startDate, int? endDate)
        {
            List<Ticker> tickerList = new List<Ticker>();
            Ticker t;
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetTickerByShare", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("ShareID", SqlDbType.Int).Value = symbolId;
                        if (startDate.HasValue)
                        {
                            cmd.Parameters.Add("StartDate", SqlDbType.Int).Value = startDate.Value;
                        }

                        if (endDate.HasValue)
                        {
                            cmd.Parameters.Add("EndDate", SqlDbType.Int).Value = endDate.Value;
                        }

                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                t = new Ticker();

                                this.PopulateObjFromReader(reader, t);

                                tickerList.Add(t);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error read latest to DB. ", ex);
                throw;
            }

            return tickerList;
        }

        public int GetTradesDaySpan(int shareId, int startDate, int endDate)
        {
            int daySpan = 0;

            daySpan = this.GetTickerListByShareDB(shareId, startDate, endDate).Count;

            if(daySpan >0)
            {
                daySpan = daySpan - 1;
            }

            return daySpan;
        }

        public int GetDataStartDate(int shareId)
        {
            int dataStartDate = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetDataStartDate", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("shareId", SqlDbType.Int).Value = shareId;

                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Read();

                            dataStartDate = reader.GetInt32(reader.GetOrdinal("TradingDate"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error seach stock from DB. ", ex);
                throw;
            }


            return dataStartDate;
        }


        /// <summary>
        /// Populates the object from reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="indicator">The indicator.</param>
        private void PopulateObjFromReader(SqlDataReader reader, Ticker indicator)
        {
            indicator.Id = reader.GetInt32(reader.GetOrdinal("ID"));
            indicator.ShareId = reader.GetInt32(reader.GetOrdinal("ShareID"));
            indicator.TradingDate = reader.GetInt32(reader.GetOrdinal("TradingDate"));

            indicator.Open = reader.GetDouble(reader.GetOrdinal("Open"));
            indicator.Close = reader.GetDouble(reader.GetOrdinal("Close"));
            indicator.High = reader.GetDouble(reader.GetOrdinal("High"));
            indicator.Low = reader.GetDouble(reader.GetOrdinal("Low"));
            indicator.Volumn = reader.GetInt64(reader.GetOrdinal("Volumn"));
            indicator.AdjustedClose = reader.GetDouble(reader.GetOrdinal("AdjustedClose"));
            indicator.JSTicks = reader.GetInt64(reader.GetOrdinal("JSTicks"));
        }

        /// <summary>
        /// Gets the daily share ticker from yahoo.
        /// </summary>
        /// <param name="symbol">The y symbol.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public List<Ticker> GetDailyShareTickerFromYahoo(string symbol, DateTime startDate, DateTime endDate)
        {
            List<Ticker> enList = new List<Ticker>();

            HistQuotesDownload hsDownload = new HistQuotesDownload();
            HistQuotesDownloadSettings settings = new HistQuotesDownloadSettings();

            var share = new ShareBLL(_unit).GetShareBySymbol(symbol);
            try
            {
                if (share != null)
                {
                    settings.ID = symbol;
                    settings.FromDate = startDate;
                    settings.ToDate = endDate;
                    settings.Interval = HistQuotesInterval.Daily;

                    hsDownload.Settings = settings;

                    Response<HistQuotesResult> resp = hsDownload.Download();

                    if (resp != null && resp.Result != null && resp.Result.Chains.Length == 1)
                    {
                        var tickerArray = resp.Result.Chains[0];

                        foreach (var t in tickerArray)
                        {
                            Ticker en = new Ticker();

                            en.TradingDate = DateHelper.DateToInt(t.TradingDate);
                            en.AdjustedClose = t.CloseAdjusted;
                            en.High = t.High;
                            en.Low = t.Low;
                            en.Open = t.Open;
                            en.Close = t.Close;
                            en.Volumn = t.Volume;
                            en.ShareId = share.Id;
                            en.JSTicks = DateHelper.DateToJSTicks(t.TradingDate);

                            enList.Add(en);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error load share tickers from yahoo. ", ex);
                throw;
            }
            return enList;
        }


        /// <summary>
        /// Uploads the history price ticker.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        public void UploadHistoryPriceTicker(string symbol, DateTime start, DateTime end)
        {

            TickerBLL tbll = new TickerBLL(_unit);

            List<Ticker> tickerList = tbll.GetDailyShareTickerFromYahoo(symbol, start, end);

            if (tickerList != null && tickerList.Count > 0)
            {
                tbll.SaveTickersToDB(tickerList);
            }
        }


        /// <summary>
        /// Saves the tickers to database.
        /// </summary>
        /// <param name="tickers">The tickers.</param>
        public void SaveTickersToDB(List<Ticker> tickers)
        {
            var xmlInput = XMLHelper.SerializeObject<List<Ticker>>(tickers);
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_FeedPriceTickers", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("TickersXML", SqlDbType.Xml).Value = xmlInput;
                        cmd.Connection.Open();

                        var result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error save tickers to DB. ", ex);
                throw;
            }
        }



        public void ReloadHistoryPriceTicker(int shareId)
        {
            SettingBLL sBLL = new SettingBLL(_unit);
            ShareBLL shareBLL = new ShareBLL(_unit);

            try
            {
                //remove existing tickers
                this.RemoveHistoryPriceTicker(shareId);

                int historyYear = int.Parse(sBLL.Get(s => s.Key ==
                SettingKey.HistoryDataYears.ToString()).Single().Value);

                DateTime start = DateTime.Now.AddYears(-1 * historyYear);

                Share share = shareBLL.GetByID(shareId);

                this.UploadHistoryPriceTicker(share.Symbol, start, DateTime.Now);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                throw;
            }
        }

        public void RemoveHistoryPriceTicker(int shareId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_DeletePriceTickers", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("shareId", SqlDbType.Int).Value = shareId;
                        cmd.Connection.Open();

                        var result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error remove tickers from DB. ", ex);
                throw;
            }

        }
        public void UpoadHistoryPriceAllShares(DateTime start, DateTime end)
        {

            ShareBLL sBLL = new ShareBLL(_unit);

            List<Share> shareList = sBLL.GetList().ToList();

            string resultMessage;

            foreach (Share s in shareList)
            {
                try
                {
                    this.UploadHistoryPriceTicker(s.Symbol, start, end);

                    resultMessage = string.Format("Success load price for {0}", s.Symbol);
                }
                catch (Exception ex)
                {
                    resultMessage = string.Format("Fail load price for {0} \n {1}", s.Symbol, ex.ToString());
                }

                LogHelper.Info(_log, resultMessage);
            }
        }

        /// <summary>
        /// Loads the daily CSV from gmail.
        /// </summary>
        public void LoadDailyCSVFromGmail()
        {
            string eodEmailAccount = ConfigurationManager.AppSettings["EODEmailAccount"];
            string eodEmailPassword = ConfigurationManager.AppSettings["EODEmailPassword"];

            try
            {
                var mailRepository = new MailRepository(
                   "imap.gmail.com",
                   993,
                   true,
                   eodEmailAccount,
                   eodEmailPassword
               );


                var emailList = mailRepository.GetUnreadMails("inbox");

                foreach (Message email in emailList)
                {

                    if (email.Attachments.Count > 0 && email.Subject == "Daily Historical Data")
                    {
                        foreach (MimePart attachment in email.Attachments)
                        {
                            string result = System.Text.Encoding.UTF8.GetString(attachment.BinaryContent);
                            UploadDailyPriceTickerCSVToAzure(attachment.ContentName, result);

                            _auditBLL.Create(new AuditLog
                            {
                                ActionMessage = "Success upload ASX EOD to Azure",
                                ExtraData = "",
                                ActionType = ActionType.UploadEODToAzure.ToString(),
                                ActionResult = ""
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error on uploading EOD csv.");
                LogHelper.Error(_log, ex.ToString());

                throw;
            }
        }

        /// <summary>
        /// Uploads the daily price ticker CSV to azure.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileContent">Content of the file.</param>
        public void UploadDailyPriceTickerCSVToAzure(string fileName, string fileContent)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.ConnectionStrings["EODASXConnectionString"].ConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("asxeod");

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

            if (!blockBlob.Exists())
            {
                using (Stream s = StringHelper.GenerateStreamFromString(fileContent))
                {
                    blockBlob.UploadFromStream(s);
                }
            }
        }

        /// <summary>
        /// Updates the daily share ticker batch today.
        /// </summary>
        /// <param name="dateString">The date string.</param>
        public void UpdateDailyShareTickerBatchToday(string dateString)
        {
            int successCount = 0;
            int failCount = 0;
            List<AsxEod> tickerEODList;

            StringBuilder resultString = new StringBuilder();

            _auditBLL.Create(new AuditLog
            {
                ActionMessage = "Start Process",
                ExtraData = "",
                ActionType = ActionType.UploadDailyTicker.ToString(),
                ActionResult = "Start to update daily share tickers for " + dateString
            });

            Console.WriteLine("Start to update daily share tickers for " + dateString);
            // upload the email to azure
            LoadDailyCSVFromGmail();

            // load ticker list from azure
            tickerEODList = LoadDailyShareTickerFromAzure(dateString);

            List<Share> shareList = (new ShareBLL(_unit)).GetList().Where(p => p.IsActive).ToList();

            foreach (Share s in shareList)
            {
                try
                {
                    UpdateDailyShareTicker(s, tickerEODList);
                    successCount++;

                    resultString.Append(string.Format("successfully upload ticker for {0} {1}\n", s.Symbol, dateString));
                    Console.WriteLine("successfully upload ticker for {0} {1}\n", s.Symbol, dateString);
                }
                catch (Exception ex)
                {
                    failCount++;
                    resultString.Append(string.Format("Fail to upload ticker for {0} {1} \n {2}", s.Symbol, dateString, ex.ToString()));
                    Console.WriteLine("Fail to upload ticker for {0} {1} \n {2}", s.Symbol, dateString, ex.ToString());
                }
            }

            _auditBLL.Create(new AuditLog
            {
                ActionMessage = resultString.ToString(),
                ActionTime = DateTime.Now,
                ExtraData = "",
                ActionType = ActionType.UploadDailyTicker.ToString(),
                ActionResult = string.Format("Success: {0}; Failed {1}", successCount, failCount)
            });

            Console.WriteLine("Success: {0}; Failed {1}", successCount, failCount);
            LogHelper.Info(_log, resultString.ToString());
        }

        /// <summary>
        /// Updates the daily share ticker.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="eodList">The eod list.</param>
        public void UpdateDailyShareTicker(Share s, List<AsxEod> eodList)
        {
            Ticker lastTicker = GetLastTicker(s.Id, null);

            if (lastTicker != null)
            {
                int nextTradingDay = DateHelper.NextTradingDay(lastTicker.TradingDate);

                if (eodList != null && eodList.Count > 0 &&
                    nextTradingDay == eodList[0].TradingDate)
                {
                    try
                    {
                        // update daily from EOD file Azure
                        updateDailyTickerFromEODTicker(s, eodList);

                        _auditBLL.Create(new AuditLog
                        {
                            ActionMessage = string.Format("Success upload {0} via EOD azure", s.Symbol),
                            ExtraData = "",
                            ActionType = ActionType.UploadDailyTicker.ToString(),
                            ActionResult = ""
                        });
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(_log, string.Format("Error load ticker from EOD {0} {1}. Load from yahoo instead", s.Symbol, nextTradingDay));
                        LogHelper.Error(_log, ex.ToString());

                        // upload history data from yahoo
                        this.UploadHistoryPriceTicker(s.Symbol, DateHelper.IntToDate(nextTradingDay), DateTime.Now);

                        _auditBLL.Create(new AuditLog
                        {
                            ActionMessage = string.Format("Error load EOD. Then successfully upload {0} via EOD azure", s.Symbol),
                            ExtraData = "",
                            ActionType = ActionType.UploadDailyTicker.ToString(),
                            ActionResult = ""
                        });
                    }
                }
                else if (nextTradingDay <= DateHelper.DateToInt(DateTime.Now))
                {
                    // upload history data from yahoo
                    this.UploadHistoryPriceTicker(s.Symbol, DateHelper.IntToDate(nextTradingDay), DateTime.Now);

                    _auditBLL.Create(new AuditLog
                    {
                        ActionMessage = string.Format("Success load from yahoo more than 1 day {0}.", s.Symbol),
                        ExtraData = "",
                        ActionType = ActionType.UploadDailyTicker.ToString(),
                        ActionResult = string.Format("{0}  {1}  {2}", s.Symbol, DateHelper.IntToDate(nextTradingDay), DateTime.Now)
                    });

                }
                else
                {
                    _auditBLL.Create(new AuditLog
                    {
                        ActionMessage = string.Format("Ticker {0} already loaded, skipped.", s.Symbol),
                        ExtraData = "",
                        ActionType = ActionType.UploadDailyTicker.ToString(),
                        ActionResult = ""
                    });

                }
            }
            else
            {
                SettingBLL settingBLL = new SettingBLL(_unit);
                int historyYears = int.Parse(settingBLL.GetSetting(SettingKey.HistoryDataYears));
                DateTime defaultStart = DateTime.Now.AddYears(-1 * historyYears);

                // upload history data from yahoo from day 1
                this.UploadHistoryPriceTicker(s.Symbol, defaultStart, DateTime.Now);

                _auditBLL.Create(new AuditLog
                {
                    ActionMessage = string.Format("Success load from yahoo from day 1 {0}.", s.Symbol),
                    ExtraData = "",
                    ActionType = ActionType.UploadDailyTicker.ToString(),
                    ActionResult = string.Format("{0}  {1}  {2}", s.Symbol, defaultStart, DateTime.Now)
                });
            }

        }

        /// <summary>
        /// Updates the daily ticker from eod ticker.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="eodTickerList">The eod ticker list.</param>
        public void updateDailyTickerFromEODTicker(Share s, List<AsxEod> eodTickerList)
        {
            Ticker t = new Ticker();

            AsxEod tEOD = eodTickerList.Single(p => p.Symbol == s.Symbol);

            t.TradingDate = tEOD.TradingDate;
            t.Open = tEOD.Open;
            t.High = tEOD.High;
            t.Low = tEOD.Low;
            t.Close = tEOD.Close;
            t.Volumn = tEOD.Volumn;
            t.ShareId = s.Id;
            t.AdjustedClose = tEOD.AdjustedClose;
            t.JSTicks = DateHelper.IntToJSTicks(tEOD.TradingDate);

            this.Create(t);


        }

        /// <summary>
        /// Gets the last ticker.
        /// </summary>
        /// <param name="shareId">The share identifier.</param>
        /// <returns></returns>
        public Ticker GetLastTicker(int shareId, int? tradingDate)
        {
            Ticker t = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetLastTicker", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("ShareID", SqlDbType.Int).Value = shareId;

                        if(tradingDate.HasValue)
                        {
                            cmd.Parameters.Add("TradingDate", SqlDbType.Int).Value = tradingDate.Value;
                        }

                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                t = new Ticker();
                                t.Id = reader.GetInt32(0);
                                t.TradingDate = reader.GetInt32(1);
                                t.Open = reader.GetDouble(2);
                                t.Close = reader.GetDouble(3);
                                t.High = reader.GetDouble(4);
                                t.Low = reader.GetDouble(5);
                                t.Volumn = (long)reader.GetValue(6);
                                t.AdjustedClose = reader.GetDouble(7);
                                t.JSTicks = (long)reader.GetValue(8);
                                t.ShareId = reader.GetInt32(9);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error read latest to DB. ", ex);
                throw;
            }

            return t;
        }

        public void LoadAsxEodRawFromAzure()
        {
            List<AsxEod> tickerEODList = null;
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.ConnectionStrings["EODASXConnectionString"].ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference("asxeod");

                var blobs = container.ListBlobs();

                int i = 0;
                foreach(CloudBlockBlob blob in blobs)
                {
                    string text;
                    using (var memoryStream = new MemoryStream())
                    {
                        blob.DownloadToStream(memoryStream);
                        text = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                        tickerEODList = GetTickerListFromCSVString(text);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                throw;
            }

            //return tickerEODList;

        }

        /// <summary>
        /// Loads the daily share ticker from azure.
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <returns></returns>
        public List<AsxEod> LoadDailyShareTickerFromAzure(string dateString)
        {
            string fileName = string.Format("Metastock_{0}.txt", dateString);
            List<AsxEod> tickerEODList = null;
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.ConnectionStrings["EODASXConnectionString"].ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference("asxeod");

                CloudBlockBlob blockBlob2 = container.GetBlockBlobReference(fileName);

                if (blockBlob2.Exists())
                {
                    string text;
                    using (var memoryStream = new MemoryStream())
                    {
                        blockBlob2.DownloadToStream(memoryStream);
                        text = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                        tickerEODList = GetTickerListFromCSVString(text);

                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                throw;
            }

            return tickerEODList;
        }


        /// <summary>
        /// Gets the latest trading date by share zone.
        /// </summary>
        /// <param name="shareId">The share identifier.</param>
        /// <param name="zoneId">The zone identifier.</param>
        /// <returns></returns>
        public int GetLatestTradingDateByShareZone(int shareId, int? zoneId)
        {
            int tradingDate = -1;

            if (zoneId.HasValue)
            {
                Zone z = new ZoneBLL(_unit).GetByID(zoneId.Value);
                tradingDate = z.TradingDate;
            }
            else
            {
                var t = GetLastTicker(shareId, null);
                if (t != null)
                    tradingDate = t.TradingDate;
            }

            return tradingDate;
        }

        /// <summary>
        /// Load tickers
        /// </summary>
        /// <param name="loader"></param>
        /// <returns></returns>
        public Boolean LoadTickers(ITickerLoader loader)
        {
            Boolean result;

            result = loader.LoadTickers();
            
            return result;
        }
        /// <summary>
        /// Gets the ticker list from CSV string.
        /// </summary>
        /// <param name="tickerString">The ticker string.</param>
        /// <returns></returns>
        private List<AsxEod> GetTickerListFromCSVString(string tickerString)
        {
            List<AsxEod> tickerList = null;
            string aLine = null;

            if (!string.IsNullOrEmpty(tickerString))
            {
                tickerList = new List<AsxEod>();
                StringReader strReader = new StringReader(tickerString);
                while (true)
                {
                    aLine = strReader.ReadLine();
                    AsxEod t;
                    if (aLine != null)
                    {
                        t = GetASXTickerFromString(aLine);

                        if (t != null)
                        {
                            tickerList.Add(t);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return tickerList;
        }

        /// <summary>
        /// Gets the asx ticker from string.
        /// </summary>
        /// <param name="tickerString">The ticker string.</param>
        /// <returns></returns>
        private AsxEod GetASXTickerFromString(string tickerString)
        {
            AsxEod t = null;
            string suffix = ".AX";

            string[] tickerParts = tickerString.Split(',');

            if (tickerParts != null && tickerParts.Length > 0)
            {
                t = new AsxEod();

                t.Symbol = tickerParts[0] + suffix;
                t.TradingDate = int.Parse(tickerParts[1]);
                t.Open = double.Parse(tickerParts[2]);
                t.High = double.Parse(tickerParts[3]);
                t.Low = double.Parse(tickerParts[4]);
                t.Close = double.Parse(tickerParts[5]);
                t.Volumn = long.Parse(tickerParts[6]);
                t.AdjustedClose = double.Parse(tickerParts[7]);
            }
            return t;
        }


        #endregion
    }
}