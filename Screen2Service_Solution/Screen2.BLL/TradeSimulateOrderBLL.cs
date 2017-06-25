using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class TradeSimulateOrderBLL : BaseBLL<TradeSimulateOrder>, IBaseBLL<TradeSimulateOrder>
    {
        private TickerBLL tBll;
        #region Constructors
        public TradeSimulateOrderBLL(IUnitWork unit) : base(unit)
        {
            tBll = new TickerBLL(_unit);
        }
        #endregion

        #region methods 
        public void AddSOrder(StatScanResultItem resultItem)
        {
            TradeSimulateOrder sorder = new TradeSimulateOrder();

            if (resultItem.ExitTradingDate.HasValue)
            {
                sorder.Days = tBll.GetTradesDaySpan(resultItem.ShareId, resultItem.EntryTradingDate, resultItem.ExitTradingDate.Value);
            }

            sorder.StatScanSetId = resultItem.StatScanSetId;
            sorder.ShareId = resultItem.ShareId;
            sorder.Flag = resultItem.Flag;
            sorder.Diff_Per = resultItem.Diff_Per;
            sorder.Diff = resultItem.Diff;
            sorder.EntryPrice = resultItem.EntryPrice;
            sorder.EntryTradingDate = resultItem.EntryTradingDate;
            sorder.ExitPrice = resultItem.ExitPrice;
            sorder.ExitTradingDate = resultItem.ExitTradingDate;
            sorder.Day5AboveDays = resultItem.Day5AboveDays;
            sorder.Day5Highest = resultItem.Day5Highest;
            sorder.Day5Lowest = resultItem.Day5Lowest;
            sorder.ExitMode = resultItem.ExitMode;
            sorder.UpdatedDT = DateTime.Now;

            this.Create(sorder);
        }
        #endregion
    }
}
