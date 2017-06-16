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
    public class TradeOrderBLL : BaseBLL<TradeOrder>, IBaseBLL<TradeOrder>
    {
        #region Constructors
        public TradeOrderBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public List<TradeOrder> GetListByAccount(int accountId, int size)
        {
            List<TradeOrder> orderList = new List<TradeOrder>();

            if (size > 0)
            {
                orderList = _unit.DataContext.Orders.Where(o => o.AccountId == accountId).Take(size).OrderByDescending(o => o.Id).ToList();
            }
            else
            {
                orderList = _unit.DataContext.Orders.Where(o => o.AccountId == accountId).OrderByDescending(o => o.Id).ToList();
            }


            return orderList;
        }

        public List<TradeOrder> GetListByAccountStatus(int accountId, string status)
        {
            List<TradeOrder> orderList = new List<TradeOrder>();

            orderList = _unit.DataContext.Orders.Where(o => o.AccountId == accountId && o.Status != null &&
            o.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();

            return orderList;
        }

        public TradeOrder CreateOrder(TradeOrder order)
        {
            TradeOrder to = null;
            AccountBalanceBLL abBll = new AccountBalanceBLL(_unit);

            this.CalculateOrder(order);

            ValidOrderFund(order);

            base.Create(order);

            to = order;

            abBll.NewOrder(order);

            return to;
        }

        public TradeOrder CreateExitOrder(TradePosition position, Entity.Indicator ind, bool isStop) 
        {
            TradeOrder to = new TradeOrder();
            Account acc = new AccountBLL(_unit).GetByID(position.AccountId);
            Broker broker = new BrokerBLL(_unit).GetByID(acc.BrokerId);
            AccountBalance ab = new AccountBalanceBLL(_unit).GetAccountBalanceByAccount(acc.Id);

            to.AccountId = position.AccountId;
            if (position.Size > 0)
            {
                to.Direction = "Short";
            }
            else
            {
                to.Direction = "Long";
            }
            to.Size = Math.Abs(position.Size);

            if(isStop)
            {
                to.OrderPrice = position.Stop.Value;
            }
            else
            {
                to.OrderPrice = position.Limit.Value;
            }

            to.Fee = this.GetFee(to.OrderValue, broker);
            to.ShareId = position.ShareId;
            to.Source = OrderSource.Stop.ToString();
            to.UpdateDate = DateTime.Now;
            to.ProcessedTradingDate = ind.TradingDate;
            to.LatestTradingDate = ind.TradingDate;
            to.Status = "Fulfilled";
            to.UpdatedBy = "System";
            to.TradingOrderDate = ind.TradingDate;
            to.OrderType = "Exit";

            this.Create(to);

            new AccountBalanceJourneyBLL(_unit).AddJourneyOrder(ab, "O." + to.Direction, to);

            return to;
        }


        public void RemoveOrder(int orderId)
        {
            TradeOrder o = null;
            AccountBalanceBLL abBll = new AccountBalanceBLL(_unit);
            int accountId = GetByID(orderId).AccountId;

            o = base.GetByID(orderId);

            abBll.RemoveOrder(o);
            base.Delete(orderId);

        }

        public void UpdateOrder(TradeOrder order)
        {
            bool isWithdraw = false;

            AccountBalanceBLL abBll = new AccountBalanceBLL(_unit);

            this.CalculateOrder(order);

            ValidOrderFund(order);

            var tmpOrder = base.GetByID(order.Id);

            if (tmpOrder.Status.Equals("Open", StringComparison.CurrentCultureIgnoreCase) &&
                order.Status.Equals("Withdrawn",StringComparison.CurrentCultureIgnoreCase))
            {
                isWithdraw = true;
            }

            TradeOrder oldOrder = new TradeOrder();

            this.CloneProperties(tmpOrder, oldOrder);

            this.CloneProperties(order, tmpOrder);

            //tmpOrder.ProcessedTradingDate = null;

            base.Update(tmpOrder);

            if(isWithdraw)
            {
                abBll.RemoveOrder(oldOrder, true);
            }
            else
            {
                abBll.UpdateOrder(oldOrder, order);
            }

        }



        public bool IsReverse(TradeOrder order, TradePosition position)
        {
            bool isReverse = false;
            if (position != null && ((position.Size > 0 && order.Flag < 0) ||
                    (position.Size < 0 && order.Flag > 0)))
            {
                isReverse = true;
            }

            return isReverse;
        }

        public bool IsReverseExceed(TradeOrder order, TradePosition position)
        {
            bool isExceed = false;

            if (position != null && ( (position.Size > 0 && order.Flag < 0) ||
                    (position.Size < 0 && order.Flag > 0)))
            {
                if (order.Size > Math.Abs(position.Size))
                {
                    isExceed = true;
                }
            }

            return isExceed;
        }

        private void ValidOrderFund(TradeOrder order)
        {
            var balance = new AccountBalanceBLL(_unit).GetAccountBalanceByAccount(order.AccountId);

                if (balance.AvailableFund < order.Reserve)
                {
                    throw new ApplicationException("Not enough fund for the order.");
                }
        }

        private void CloneProperties(TradeOrder src, TradeOrder trgt)
        {
            trgt.AccountId = src.AccountId;
            trgt.Direction = src.Direction;
            trgt.Fee = src.Fee;
            trgt.Id = src.Id;
            trgt.LatestPrice = src.LatestPrice;
            trgt.LatestTradingDate = src.LatestTradingDate;
            trgt.Limit = src.Limit;
            trgt.Reserve = src.Reserve;
            trgt.Reason = src.Reason;
            trgt.Note = src.Note;
            trgt.OrderPrice = src.OrderPrice;
            trgt.OrderType = src.OrderType;
            trgt.ProcessedTradingDate = src.ProcessedTradingDate;
            trgt.ShareId = src.ShareId;
            trgt.Size = src.Size;
            trgt.Status = src.Status;
            trgt.Stop = src.Stop;
            trgt.TradingOrderDate = src.TradingOrderDate;
            trgt.UpdateDate = src.UpdateDate;
            trgt.UpdatedBy = src.UpdatedBy;

        }


        private void CalculateOrder(TradeOrder order)
        {
            var account = new AccountBLL(_unit).GetByID(order.AccountId);
            var broker = new BrokerBLL(_unit).GetByID(account.BrokerId);

            order.Fee = GetFee(order.OrderValue, broker);

            order.Reserve = order.OrderValue * Global.MarginRate + order.Fee;
        }

        private void CalculateOrder(TradeOrder order, TradePosition position)
        {
            var account = new AccountBLL(_unit).GetByID(order.AccountId);
            var broker = new BrokerBLL(_unit).GetByID(account.BrokerId);

            if (IsReverse(order, position))
            {
                if (IsReverseExceed(order, position))
                {
                    int sizeDiff = GetSizeDiff(order, position);

                    order.Fee = GetFee(order.Size * order.OrderPrice, broker);
                    order.Reserve = sizeDiff * order.OrderPrice * Global.MarginRate + order.Fee;
                }
                else
                {
                    order.Fee = GetFee(order.OrderValue, broker);
                    order.Reserve = 0;
                }
            }
            else
            {
                order.Fee = GetFee(order.OrderValue, broker);

                order.Reserve = order.OrderValue * Global.MarginRate + order.Fee;
            }


        }

        public int GetSizeDiff(TradeOrder order, TradePosition position)
        {
            return Math.Abs(Math.Abs(position.Size) - order.Size);
        }

        public double GetFee(double orderValue, Broker broker)
        {
            double fee = 0;

            fee = orderValue * (broker.FeeRate / 100);

            if (fee < broker.MinFee)
                fee = broker.MinFee;

            return fee;
        }
        #endregion
    }
}
