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
    public class AlertBLL : BaseBLL<Alert>, IBaseBLL<Alert>
    {
        #region properties
        private AuditLogBLL _auditBLL;
        #endregion

        #region Constructors
        public AlertBLL(IUnitWork unit) : base(unit)
        {
            _auditBLL = new AuditLogBLL(_unit);
        }
        #endregion

        #region Methods
        public List<Alert> GetAlertListByShareZone(string userId, int shareId, int? zoneId)
        {
            List<Alert> aList = new List<Alert>();

            if (zoneId.HasValue)
            {
                aList = _unit.DataContext.Alert.Where(c => c.Owner == userId && c.ShareId == shareId && c.ZoneId == zoneId.Value).ToList();
            }
            else
            {
                aList = _unit.DataContext.Alert.Where(c => c.Owner == userId && c.ShareId == shareId).ToList();
            }


            return aList;
        }

        public List<Alert> GetAlertListByZone(string userId, int? zoneId)
        {
            List<Alert> aList = new List<Alert>();

            if (zoneId.HasValue)
            {
                aList = _unit.DataContext.Alert.Where(c => c.Owner == userId && c.ZoneId == zoneId.Value).ToList();
            }
            else
            {
                aList = _unit.DataContext.Alert.Where(c => c.Owner == userId).ToList();
            }

            return aList;
        }

        /// <summary>
        /// Verifies the alert.
        /// </summary>
        /// <param name="alert">The alert.</param>
        /// <returns></returns>
        public VerifyResult VerifyAlert(Alert alert)
        {
            VerifyResult result = new VerifyResult();

            int tradingDate = new TickerBLL(_unit).GetLatestTradingDateByShareZone(alert.ShareId, alert.ZoneId);

            result = new ScanCalculator(_unit).CheckDailyMatch(alert.ShareId, tradingDate, alert.Formula);

            return result;
        }

        /// <summary>
        /// Processes the alerts full.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public void ProcessAlertsFull(bool force = false)
        {
            int successCount = 0;
            int failCount = 0;
            int tradingDate = DateHelper.DateToInt(DateTime.Now);

            StringBuilder resultString = new StringBuilder();
            List<Alert> alertList;

            _auditBLL.Create(new AuditLog
            {
                ActionMessage = "Start Process",

                ExtraData = "",
                ActionType = ActionType.ProcessAlert.ToString(),
                ActionResult = "Start to process alerts for " + tradingDate.ToString()
            });

            Console.WriteLine("Start to process alerts for " + tradingDate.ToString());
            alertList = _unit.DataContext.Alert.Where(a => a.IsActive).ToList();

            if (force)
            {
                new AlertResultBLL(_unit).DeleteAlertResultByAlert(DateHelper.DateToInt(DateTime.Now), null);
            }

            foreach (var a in alertList)
            {
                try
                {
                    ProcessAlert(a, tradingDate);

                    successCount++;

                    resultString.Append(string.Format("successfully process alert for alert id {0}, shareid {1}, tradingdate {2}\n", a.Id, a.ShareId, tradingDate.ToString()));
                    Console.WriteLine("successfully process alert for alert id {0}, shareid {1}, tradingdate {2}", a.Id, a.ShareId, tradingDate.ToString());
                }
                catch (Exception ex)
                {
                    failCount++;
                    resultString.Append(string.Format("Failed to process alert for alert id {0}, shareid {1}, tradingdate {2}\n", a.Id, a.ShareId, tradingDate.ToString()));
                    Console.WriteLine(string.Format("Failed to process alert for alert id {0}, shareid {1}, tradingdate {2}", a.Id, a.ShareId, tradingDate.ToString()));
                    Console.WriteLine("error details: " + ex.ToString());
                }
            }

            _auditBLL.Create(new AuditLog
            {
                ActionMessage = resultString.ToString(),
                ActionTime = DateTime.Now,
                ExtraData = "",
                ActionType = ActionType.ProcessAlert.ToString(),
                ActionResult = string.Format("Success: {0}; Failed {1}", successCount, failCount)
            });

            Console.WriteLine("Success: {0}; Failed {1}", successCount, failCount);
            LogHelper.Info(_log, resultString.ToString());
        }



        /// <summary>
        /// Processes the alert.
        /// </summary>
        /// <param name="alert">The alert.</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <returns></returns>
        public void ProcessAlert(Alert alert, int tradingDate)
        {
            VerifyResult result = null;

            AlertResult aResult = this.CheckAlert(alert, tradingDate);

            if(aResult != null)
            new AlertResultBLL(_unit).Create(aResult);

        }

        public AlertResult CheckAlert(Alert alert, int tradingDate)
        {
            VerifyResult result = null;
            AlertResult aResult = null;
            int latestTradingDate = new TickerBLL(_unit).GetLatestTradingDateByShareZone(alert.ShareId, alert.ZoneId);

            if (latestTradingDate >= tradingDate)
            {
                result = VerifyAlert(alert);

                aResult = new AlertResult
                {
                    AlertId = alert.Id,
                    IsMatch = result.IsMatch,
                    Message = result.ErrorMessage,
                    TradingDate = result.TradingDate,
                    ShareId = alert.ShareId,
                    ZoneId = alert.ZoneId,
                    ProcessDT = DateTime.Now
                };


            }
            return aResult;
        }


        public List<AlertResult> GetAlertResultDB(int tradingDate, int? zoneId)
        {
            List<AlertResult> arList = new List<AlertResult>();

            arList = _unit.DataContext.AlertResult.Where(c => c.TradingDate == tradingDate &&
            ((zoneId.HasValue && zoneId.Value == c.ZoneId) ||
            (!zoneId.HasValue && c.ZoneId == null))).ToList();

            return arList;
        }


        public List<OutStockSearchResult> SearchAlert(string userId, int tradingDate, bool force, int? zoneId)
        {
            List<OutStockSearchResult> searchResult = new List<OutStockSearchResult>();
            List<AlertResult> arList = this.GetAlertResultDB(tradingDate, zoneId);

            if (arList.Count == 0)
            {
                force = true;
            }

            if (force)
            {

                arList = new List<AlertResult>();
                var alertList = _unit.DataContext.Alert.Where(a => a.IsActive &&
                a.Owner == userId &&
                a.ZoneId == zoneId).ToList();

                foreach (Alert a in alertList)
                {
                    AlertResult aResult = CheckAlert(a, tradingDate);
                    arList.Add(aResult);
                }

                var arBll = new AlertResultBLL(_unit);
                // remove and add result
                arBll.DeleteAlertResultByAlert(tradingDate, zoneId);
                arBll.AddAlertResultBatch(arList);
            }
            arList = arList.Where(c => c.IsMatch).ToList();

            string shareListString = GetShareListString(arList);

            searchResult = new IndicatorBLL(_unit).SearchShareByShareString(shareListString, tradingDate);

            return searchResult;
        }

        private string GetShareListString(List<AlertResult> resultList)
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
