using Microsoft.VisualStudio.TestTools.UnitTesting;
using Screen2.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL.Test
{
    [TestClass]
    public class TestTradeManager
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_ProcessAccountOrders()
        {
            TradeManager bll = new TradeManager(_unit);

            bll.ProcessAccountOrders(1076);
        }

        [TestMethod]
        public void Test_ProcessAccountPositions()
        {
            TradeManager bll = new TradeManager(_unit);

            bll.ProcessAccountPositions(1066);
        }
    }
}
