using Microsoft.VisualStudio.TestTools.UnitTesting;
using Screen2.DAL;
using Screen2.Entity;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL.Test
{


    [TestClass]
    public class TestTickerBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());
        [TestMethod]
        public void Test_GetDailyShareTickerFromYahoo()
        {
            TickerBLL tbll = new TickerBLL(_unit);

            var result = tbll.GetDailyShareTickerFromYahoo("org.ax", DateTime.Now.AddDays(-20), DateTime.Now);

            var resultString = XMLHelper.SerializeObject<List<Ticker>>(result);
        }

        [TestMethod]
        public void Test_SaveTickersToDB()
        {
        }

        [TestMethod]
        public void Test_UploadDailyPriceTickerCSVToAzure()
        {
            TickerBLL tBll = new TickerBLL(_unit);

            //tBll.UploadDailyPriceTickerCSVToAzure();
        }

        [TestMethod]
        public void Test_LoadDailyCSVFromGmail()
        {
            TickerBLL tBll = new TickerBLL(_unit);
            tBll.LoadDailyCSVFromGmail();
        }

        [TestMethod]
        public void Test_UpdateDailyShareTickerBatch()
        {
            TickerBLL tBll = new TickerBLL(_unit);
            tBll.UpdateDailyShareTickerBatchToday("20160303");

        }
        [TestMethod]
        public void Test_GetLastTicker()
        {
            TickerBLL tBll = new TickerBLL(_unit);
            ShareBLL sBLL = new ShareBLL(_unit);

            Share s = sBLL.GetShareBySymbol("ORG.AX");
            var ticker = tBll.GetLastTicker(s.Id, null);


            s = sBLL.GetShareBySymbol("1PG.AX");
            ticker = tBll.GetLastTicker(s.Id, null);


        }

        [TestMethod]
        public void Test_UpdateDailyShareTicker()
        {
            TickerBLL tBll = new TickerBLL(_unit);
            ShareBLL sBLL = new ShareBLL(_unit);

            List<TickerEODEntity> tickerEODList;
            tickerEODList = tBll.LoadDailyShareTickerFromAzure("20160304");

            Share s = sBLL.GetShareBySymbol("MNE.AX");
            tBll.UpdateDailyShareTicker(s, tickerEODList);
        }

        [TestMethod]
        public void Test_UpdateDailyShareTickerBatchToday()
        {
            TickerBLL tBll = new TickerBLL(_unit);
            tBll.UpdateDailyShareTickerBatchToday("20170525");

        }

        [TestMethod]
        public void Test_GetTickerListBySymbol()
        {
            TickerBLL tBll = new TickerBLL(_unit);

            var tickerList = tBll.GetTickerListByShareDB(1585, 20100101, 20160320);

        }

        [TestMethod]
        public void Test_GetTickerListByShareDB()
        {
            TickerBLL tBll = new TickerBLL(_unit);

            var tickerList = tBll.GetTickerListByShareDB(1585, 20100101, 20160320);
            tickerList = tBll.GetTickerListByShareDB(1585, 20100101, null);
        }

        [TestMethod]
        public void Test_GetTickerListByShareDB_Full()
        {
            TickerBLL tBll = new TickerBLL(_unit);

            var tickerList = tBll.GetTickerListByShareDB(1585, null, null);

        }

        [TestMethod]
        public void Test_GetTickerListByShareDB_Full_AllShares()
        {
            ShareBLL sBll = new ShareBLL(_unit);
            TickerBLL tBll = new TickerBLL(_unit);

            var shareList = sBll.GetList().ToList();

            foreach(var s in shareList)
            {
                var tickerList = tBll.GetTickerListByShareDB(s.Id, null, null);
                Debug.WriteLine("share {0}  {1}", s.Id, tickerList.Count);

            }


        }

        [TestMethod]
        public void Test_ReloadHistoryPriceTicker() {
            TickerBLL tBll = new TickerBLL(_unit);
            tBll.ReloadHistoryPriceTicker(2433);
        }

        [TestMethod]
        public void Test_RemoveHistoryPriceTicker()
        {
            TickerBLL tBll = new TickerBLL(_unit);
            tBll.RemoveHistoryPriceTicker(2433);
        }

        [TestMethod]
        public void Test_GetTickerListByDate()
        {
            TickerBLL tBll = new TickerBLL(_unit);
            tBll.GetTickerListByDate(1585, 20150105, 21);
        }

        [TestMethod]
        public void Test_GetLatestTradingDateByShareZone()
        {
            TickerBLL tBll = new TickerBLL(_unit);
            tBll.GetLatestTradingDateByShareZone(1585, null);
        }
    }
}
