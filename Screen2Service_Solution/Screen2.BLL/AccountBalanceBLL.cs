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
    public class AccountBalanceBLL : BaseBLL<AccountBalance>, IBaseBLL<AccountBalance>
    {
        #region Constructors
        public AccountBalanceBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public AccountBalance GetAccountBalanceByAccount(int accountId)
        {
            AccountBalance balance = null;

            balance = _unit.DataContext.AccountBalance.FirstOrDefault(p => p.AccountId == accountId);

            return balance;
        }

        public AccountBalance InitCreate(int accountId)
        {
            AccountBalance newAcc = new AccountBalance
            {
                AccountId = accountId,
                FundAmount = 0,
                AvailableFund = 0,
                Margin = 0,
                FeeSum = 0,
                PositionValue = 0,
                TradingDate = null,
                UpdateDT = DateTime.Now
            };

            base.Create(newAcc);

            new AccountBalanceJourneyBLL(_unit).InitCreate(newAcc);

            return newAcc;
        }


        public AccountBalance TransferFund(int accountId, string operation, double amount)
        {
            AccountBalance existAB = this.GetAccountBalanceByAccount(accountId);
            AccountBalanceJourneyBLL abjBLL = new AccountBalanceJourneyBLL(_unit);

            if (existAB == null)
            {
                InitCreate(accountId);
            }


            if (operation.Equals("Deposit", StringComparison.InvariantCultureIgnoreCase))
            {
                existAB.FundAmount = existAB.FundAmount + amount;
                //existAB.TotalBalance = existAB.TotalBalance + amount;
                existAB.AvailableFund = existAB.AvailableFund + amount;
                existAB.UpdateDT = DateTime.Now;

                this.Update(existAB);

                abjBLL.AddJourney(existAB, "Deposit");
            }
            else if (operation.Equals("Withdraw", StringComparison.InvariantCultureIgnoreCase))
            {
                if (amount > existAB.AvailableFund)
                {
                    throw new Exception("not enough avaiable fund for withdraw.");
                }
                else
                {
                    existAB.FundAmount = existAB.FundAmount - amount;
                    existAB.AvailableFund = existAB.AvailableFund - amount;
                    existAB.UpdateDT = DateTime.Now;

                    this.Update(existAB);

                    abjBLL.AddJourney(existAB, "Withdraw");
                }
            }

            return existAB;
        }

        public AccountBalance NewOrder(TradeOrder order)
        {
            AccountBalance ab = null;
            TradeOrderBLL toBll = new TradeOrderBLL(_unit);

            ab = this.GetAccountBalanceByAccount(order.AccountId);

            if (ab != null)
            {
                //ab.Reserve = ab.Reserve + order.Reserve;
                ab.AvailableFund = ab.AvailableFund - order.Reserve;
                ab.TradingDate = order.TradingOrderDate;
                ab.UpdateDT = DateTime.Now;
            }

            base.Update(ab);

            new AccountBalanceJourneyBLL(_unit).AddJourneyOrder(ab, "O." + order.Direction, order);

            return ab;
        }

        public AccountBalance RemoveOrder(TradeOrder order, bool isWithdraw = false)
        {
            AccountBalance ab = null;

            ab = this.GetAccountBalanceByAccount(order.AccountId);

            if (ab != null)
            {
                ab.AvailableFund = ab.AvailableFund + order.Reserve;
                ab.UpdateDT = DateTime.Now;
                ab.TradingDate = order.TradingOrderDate;

                base.Update(ab);

                if (isWithdraw)
                {
                    new AccountBalanceJourneyBLL(_unit).AddJourneyOrder(ab, "Withdraw", order);
                }
                else
                {
                    new AccountBalanceJourneyBLL(_unit).AddJourneyOrder(ab, "RemoveOrder", order);
                }
            }
            else
            {
                throw new ApplicationException("invalid order in RemoveOrder");
            }

            return ab;
        }


        public AccountBalance UpdateOrder(TradeOrder oldOrder, TradeOrder newOrder)
        {
            AccountBalance ab = null;

            ab = this.GetAccountBalanceByAccount(oldOrder.AccountId);

            if (oldOrder != null && newOrder != null)
            {
                ab.AvailableFund = ab.AvailableFund + oldOrder.Reserve - newOrder.Reserve;
                ab.UpdateDT = DateTime.Now;
                ab.TradingDate = newOrder.TradingOrderDate;

                base.Update(ab);

                new AccountBalanceJourneyBLL(_unit).AddJourneyOrder(ab, "UpdateOrder", newOrder);
            }
            else
            {
                throw new Exception("invalid orders in UpdateOrder");
            }


            return ab;
        }

        public AccountBalance ApplyEntryTransaction(Transaction tr, TradeOrder order, TradePosition tp)
        {
            AccountBalance ab = null;
            TradeOrderBLL toBll = new TradeOrderBLL(_unit);

            ab = this.GetAccountBalanceByAccount(order.AccountId);

            if (ab != null)
            {
                ab.FeeSum += tr.Fee;
                ab.TradingDate = tr.TradingDate;
                ab.UpdateDT = DateTime.Now;
            }

            this.Update(ab);

            return ab;
        }


        public AccountBalance ApplyExitTransaction(Transaction tr, TradeOrder order, TradePosition tp)
        {
            AccountBalance ab = null;
            TradeOrderBLL toBll = new TradeOrderBLL(_unit);

            ab = this.GetAccountBalanceByAccount(order.AccountId);

            if (ab != null)
            {
                ab.AvailableFund += tp.Margin - tr.Fee;
                ab.FeeSum += tr.Fee;
                ab.TradingDate = tr.TradingDate;
                ab.UpdateDT = DateTime.Now;
            }

            this.Update(ab);

            new AccountBalanceJourneyBLL(_unit).AddJourneyTransaction(ab, "T." + tr.Direction, tr);

            return ab;
        }

        public double GetExitDiff(Transaction tr, TradePosition tp)
        {
            double diff = 0;

            if (tr != null && tp != null)
            {
                diff = (tr.Price - tp.CurrentPrice) * tr.Size * tp.Flag;
            }


            return diff;
        }


        public double GetTransactionDiff(Transaction tr, TradePosition tp)
        {
            double diff = 0;

            if (tr != null && tp != null)
            {
                diff = (tp.CurrentPrice - tr.Price) * tr.Size * tr.Flag;
            }

            return diff;
        }

        //public bool CheckAccountBalance(int accountId)
        //{

        //    AccountSummary asummary = new AccountBLL(_unit).GetAccountSummary(accountId);

        //    double accountPL = 0;
        //    double position_PL = 0;
        //    double position_EntrySum = 0;
        //    double position_ValueSum = 0;

        //    double order_ValueSum = 0;
        //    double order_FeeSum = 0;
        //    double order_ReserveSum = 0;

        //    accountPL = asummary.Balance.TotalBalance - asummary.Balance.FundAmount;

        //    // check position 
        //    var pList = asummary.CurrentPositions;

        //    foreach (TradePosition p in pList)
        //    {
        //        position_EntrySum += p.EntryCost * p.Flag;
        //        position_ValueSum += p.CurrentValue * p.Flag;
        //    }

        //    position_PL = position_ValueSum - position_EntrySum;


        //    // check Order
        //    var oList = asummary.OpenOrders;
        //    foreach (TradeOrder o in oList)
        //    {
        //        order_FeeSum += o.Fee;
        //        order_ReserveSum += o.Reserve;
        //        order_ValueSum += o.OrderValue;
        //    }


        //    if (Math.Round(asummary.Balance.TotalBalance, 2) != Math.Round(
        //        asummary.Balance.AvailableFund + asummary.Balance.Margin + asummary.Balance.Reserve, 2
        //        ))
        //    {
        //        return false;
        //    }

        //    if (Math.Round(asummary.Balance.FundAmount, 2) != Math.Round(
        //        asummary.Balance.AvailableFund - asummary.Balance.PL, 2))
        //    {
        //        return false;
        //    }


        //    return true;
        //}

        #endregion
    }
}
