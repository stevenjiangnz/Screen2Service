using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Shared;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class TradeManager : BaseBLL<AccountBalance>, IBaseBLL<AccountBalance>
    {
        #region Properties
        private AccountBLL aBll;
        private TradeOrderBLL toBll;
        private AccountBalanceBLL abBll;
        private TransactionBLL trBll;
        private TradePositionBLL tpBll;
        private TradeSetBLL tsBll;
        #endregion

        #region Constructors
        public TradeManager(IUnitWork unit) : base(unit)
        {
            aBll = new AccountBLL(_unit);
            toBll = new TradeOrderBLL(_unit);
            abBll = new AccountBalanceBLL(_unit);
            trBll = new TransactionBLL(_unit);
            tpBll = new TradePositionBLL(_unit);
            tsBll = new TradeSetBLL(_unit);
        }
        #endregion

        #region Methods
        public void ProcessAccountsByZone(string userId, int zoneId)
        {
            AccountBLL aBll = new AccountBLL(_unit);

            List<Account> aList = aBll.GetAccountListByUser(userId, zoneId);

            foreach(var a in aList)
            {
                this.ProcessAccountOrders(a.Id);

                this.ProcessAccountPositions(a.Id);
            }
        }

        public void ProcessAccountOrders(int accountId)
        {
            List<TradeOrder> toList = toBll.GetListByAccountStatus(accountId, "Open");
            Account acc = aBll.GetByID(accountId);

            int dateToProcess = this.GetDateToProcess(acc);

            foreach (TradeOrder o in toList)
            {
                try
                {
                    ProcessOrder(o, dateToProcess);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(_log, ex.ToString());
                }
            }
        }

        public void ProcessAccountPositions(int accountId)
        {
            List<TradePosition> tpList = tpBll.GetOutstandingPositions(accountId);
            Account acc = aBll.GetByID(accountId);
            int dateToProcess = this.GetDateToProcess(acc);

            foreach (TradePosition tp in tpList)
            {
                int startDate = this.GetPositionStartDate(tp);


                List<Screen2.Entity.Indicator> iList = this.GetIndicatorsForTrade(tp, dateToProcess);
                Transaction tr = trBll.GetByID(tp.EntryTransactionId);

                if (iList != null && iList.Count > 0)
                {
                    foreach (Screen2.Entity.Indicator ind in iList)
                    {
                        if (ind.TradingDate > tr.TradingDate)
                        {
                            if (tp.Size > 0)
                            {
                                if (tp.Stop.HasValue && tp.Stop.Value >= ind.Low && tp.Stop.Value<= ind.High)
                                {
                                    this.ProcessExitPosition(tp, ind, true);
                                    break;
                                }
                                else if (tp.Limit.HasValue && tp.Limit.Value >= ind.Low && tp.Limit.Value <= ind.High)
                                {
                                    this.ProcessExitPosition(tp, ind, false);
                                    break;
                                }
                                else
                                {
                                    tpBll.UpdatePositionValue(tp, ind);
                                }
                            }
                            else
                            {
                                if (tp.Stop.HasValue && tp.Stop.Value >= ind.Low && tp.Stop.Value <= ind.High)
                                {
                                    this.ProcessExitPosition(tp, ind, true);
                                    break;
                                }
                                else if (tp.Limit.HasValue && tp.Limit.Value <= ind.High && tp.Limit.Value >= ind.Low)
                                {
                                    this.ProcessExitPosition(tp, ind, false);
                                    break;
                                }
                                else
                                {
                                    tpBll.UpdatePositionValue(tp, ind);
                                }
                            }
                        }

                        if(ind.TradingDate == tr.TradingDate)
                        {
                            if (tp.Size > 0)
                            {
                                if (tp.Stop.HasValue && tp.Stop.Value >= ind.Low && tp.Stop.Value <= ind.High &&
                                    ind.Close < ind.Open)
                                {
                                    this.ProcessExitPosition(tp, ind, true);
                                    break;
                                }
                                else if (tp.Limit.HasValue && tp.Limit.Value >= ind.Low && tp.Limit.Value <= ind.High &&
                                    ind.Close > ind.Open)
                                {
                                    this.ProcessExitPosition(tp, ind, false);
                                    break;
                                }
                                else
                                {
                                    tpBll.UpdatePositionValue(tp, ind);
                                }
                            }
                            else
                            {
                                if (tp.Stop.HasValue && tp.Stop.Value >= ind.Low && tp.Stop.Value <= ind.High &&
                                    ind.Close > ind.Open)
                                {
                                    this.ProcessExitPosition(tp, ind, true);
                                    break;
                                }
                                else if (tp.Limit.HasValue && tp.Limit.Value <= ind.High && tp.Limit.Value >= ind.Low &&
                                    ind.Close < ind.Open)
                                {
                                    this.ProcessExitPosition(tp, ind, false);
                                    break;
                                }
                                else
                                {
                                    tpBll.UpdatePositionValue(tp, ind);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ProcessOrder(TradeOrder order, int dateToProcess)
        {
            List<Screen2.Entity.Indicator> iList = this.GetIndicatorsForTrade(order, dateToProcess);

            int startDate = this.GetOrderStartDate(order);

            if (iList != null && iList.Count > 0)
            {
                foreach (Screen2.Entity.Indicator ind in iList)
                {

                    if (ind.TradingDate > startDate)
                    {
                        if (order.OrderPrice >= ind.Low && order.OrderPrice <= ind.High)
                        {
                            if (order.Source == OrderSource.Entry.ToString())
                            {
                                ProcessEntryOrder(order, ind);
                                break;
                            }
                        }
                        else
                        {
                            //Update the processed date
                            order.ProcessedTradingDate = ind.TradingDate;
                            order.UpdateDate = DateTime.Now;

                            new TradeOrderBLL(_unit).Update(order);
                        }
                    }
                }
            }
        }

        
        private void ProcessExitPosition(TradePosition position, Entity.Indicator ind, bool isStop)
        {
            TradeOrder order = toBll.CreateExitOrder(position, ind, isStop);
            Transaction tr = trBll.CreateTransaction(order, ind);
            TradePosition tp = tpBll.UpdateExitPosition(position, tr, order, ind);
            AccountBalance ab = abBll.ApplyExitTransaction(tr, order, tp);
        }

        private void ProcessEntryOrder(TradeOrder order, Entity.Indicator ind)
        {
            Transaction tr = trBll.CreateTransaction(order, ind);

            TradePosition tp = tpBll.CreateEntryPosition(tr, order, ind);

            AccountBalance ab = abBll.ApplyEntryTransaction(tr, order, tp);

            this.PostTransactionProcess(order, tr, tp, ind);

            new AccountBalanceJourneyBLL(_unit).AddJourneyTransaction(ab, "T." + tr.Direction, tr);
        }

        private void PostTransactionProcess(TradeOrder order, Transaction tr, TradePosition tp, Entity.Indicator ind)
        {
            order.ProcessedTradingDate = ind.TradingDate;

            order.Status = "Fulfilled";

            toBll.Update(order);
        }

        private List<Entity.Indicator> GetIndicatorsForTrade(TradeOrder order, int dateToProcess)
        {
            List<Entity.Indicator> iList = new List<Entity.Indicator>();

            int startDate = this.GetOrderStartDate(order);

            IndicatorBLL iBll = new IndicatorBLL(_unit);

            iList = iBll.GetIndicatorListByShareDate(order.ShareId, startDate, dateToProcess).OrderByDescending(p => p.TradingDate).ToList();
            List<Ticker> tList = new TickerBLL(_unit).GetTickerListByShareDB(order.ShareId, startDate, dateToProcess).OrderByDescending(p => p.TradingDate).ToList();
            iBll.PopulateIndicatorsWithTickers(iList, tList);

            return iList.OrderBy(p=>p.TradingDate).ToList();
        }

        private List<Entity.Indicator> GetIndicatorsForTrade(TradePosition position, int dateToProcess)
        {
            List<Entity.Indicator> iList = new List<Entity.Indicator>();

            int startDate = this.GetPositionStartDate(position);

            IndicatorBLL iBll = new IndicatorBLL(_unit);

            iList = iBll.GetIndicatorListByShareDate(position.ShareId, startDate, dateToProcess).OrderByDescending(p => p.TradingDate).ToList();
            List<Ticker> tList = new TickerBLL(_unit).GetTickerListByShareDB(position.ShareId, startDate, dateToProcess).OrderByDescending(p => p.TradingDate).ToList();
            iBll.PopulateIndicatorsWithTickers(iList, tList);

            return iList.OrderBy(p=>p.TradingDate).ToList();
        }

        private int GetOrderStartDate(TradeOrder order)
        {
            int startDate = order.TradingOrderDate;

            if (order.ProcessedTradingDate.HasValue)
            {
                startDate = order.ProcessedTradingDate.Value;
            }

            return startDate;
        }

        private int GetPositionStartDate(TradePosition position)
        {
            Transaction tr = trBll.GetByID(position.EntryTransactionId);

            int startDate = tr.TradingDate;

            if (position.LastProcessedDate.HasValue)
            {
                startDate = DateHelper.NextTradingDay(position.LastProcessedDate.Value);
            }

            return startDate;
        }

        private int GetDateToProcess(Account acc)
        {
            int dateToProcess = DateHelper.DateToInt(DateTime.Now);
            if (acc.ZoneId.HasValue)
            {
                Zone z = new ZoneBLL(_unit).GetByID(acc.ZoneId.Value);
                dateToProcess = z.TradingDate;
            }

            return dateToProcess;

        }
        #endregion
    }
}
