using Microsoft.VisualStudio.TestTools.UnitTesting;
using Screen2.DAL;
using Screen2.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL.Test
{
    [TestClass]
    public class TestTradePositionBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_GetPositionSummaryList()
        {
            TradePositionBLL bll = new TradePositionBLL(_unit);
            List<OutPosition> aList = bll.GetPositionSummaryList(1060, null);

            aList = bll.GetPositionSummaryList(1060, 2);
        }

        [TestMethod]
        public void Test_UpdateSpanDays()
        {
            TradePositionBLL bll = new TradePositionBLL(_unit);
            TransactionBLL tbll = new TransactionBLL(_unit);
            List<TradePosition> aList = bll.GetListByAccount(1060);

            foreach(var p in aList)
            {
                if(p.ExitTransactionId.HasValue)
                {
                    var entryTrans = tbll.GetByID(p.EntryTransactionId);
                    var exitTrans = tbll.GetByID(p.ExitTransactionId.Value);

                    p.days = new TickerBLL(_unit).GetTradesDaySpan(p.ShareId, entryTrans.TradingDate, exitTrans.TradingDate);

                    bll.Update(p);
                }
            }
        }
    }
}
