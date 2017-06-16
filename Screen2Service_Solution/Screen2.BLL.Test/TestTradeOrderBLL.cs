using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Screen2.DAL;
using System.Linq;
using System.Diagnostics;
using Screen2.Shared;
using Screen2.Entity;
using System.Collections.Generic;
using Screen2.Utils;

namespace Screen2.BLL.Test
{
    [TestClass]
    public class TestTradeOrderBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void TestCreate()
        {
            TradeOrderBLL bll = new TradeOrderBLL(_unit);
            List<Account> aList = new AccountBLL(_unit).GetAccountListByUser("2b658482-6a38-4ed3-b356-77fe9b1569f1", null);

            TradeOrder a = new TradeOrder
            {

                UpdatedBy = "2b658482-6a38-4ed3-b356-77fe9b1569f1",
                UpdateDate = DateTime.Now,
                Direction = OrderDirection.Long.ToString(),
                LatestPrice = 5.48,
                LatestTradingDate = 20160802,
                Status = OrderStatus.Open.ToString(),
                ShareId = 1585,
                Size = 1000,
                OrderPrice = 5.65,
                OrderType = OrderType.Stop.ToString(),
                Note = "test order",
                Stop = 5.50,
                Limit = 5.90,
                AccountId = aList[0].Id

            };

            bll.Create(a);
        }

        [TestMethod]
        public void Test_CreateOrder()
        {
            TradeOrderBLL bll = new TradeOrderBLL(_unit);

            TradeOrder a = new TradeOrder
            {
                UpdatedBy = "2b658482-6a38-4ed3-b356-77fe9b1569f1",
                UpdateDate = DateTime.Now,
                LatestPrice = 5.58,
                OrderPrice = 5.65,
                Size = 1000,
                Direction = OrderDirection.Long.ToString(),
                OrderType = OrderType.Stop.ToString(),
                Stop = 5.50,
                Limit = 5.90,
                ShareId = 445,
                Status = OrderStatus.Open.ToString(),
                LatestTradingDate = 20100208,
                Note = "test order",
                AccountId = 1033,
                Fee = 0,
                Reserve = 0
            };

            bll.CreateOrder(a);
        }

        [TestMethod]
        public void Test_RemoveOrder()
        {
            TradeOrderBLL bll = new TradeOrderBLL(_unit);

            bll.RemoveOrder(39);
        }

        [TestMethod]
        public void Test_UpdateOrder()
        {
            TradeOrderBLL bll = new TradeOrderBLL(_unit);

            var order = bll.GetByID(58);
            TradeOrder to = new TradeOrder();

            to = ObjHelper.DeepCopy(order);
            to.Id = order.Id;
            to.Size = 1000;

            bll.UpdateOrder(to);

        }

        //[TestMethod]
        //public void Test_ProcessAccountOrder()
        //{
        //    TradeOrderBLL bll = new TradeOrderBLL(_unit);

        //    bll.ProcessAccountOrders(1048);

        //}
    }
}