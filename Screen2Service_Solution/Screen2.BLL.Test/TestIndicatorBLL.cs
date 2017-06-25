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
    public class TestIndicatorBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_GetIndicators()
        {
            IndicatorBLL iBLL = new IndicatorBLL(_unit);
            TickerBLL tBll = new TickerBLL(_unit);

            var tickerList = tBll.GetTickerListByShareDB(1585, 20100101, 20160520);

            iBLL.GetIndicators(tickerList, "sma,10|ema,20");

        }


        [TestMethod]
        public void Test_GetSMAByShareID()
        {
            IndicatorBLL iBLL = new IndicatorBLL(_unit);

            iBLL.GetSMAByShareID(1585, 10, 20000000, 0);
        }

        [TestMethod]
        public void Test_ProcessIndicator()
        {
            IndicatorBLL iBLL = new IndicatorBLL(_unit);
            iBLL.ProcessIndicator(1585);
        }

        [TestMethod]
        public void Test_ProcessIndicatorFull()
        {
            IndicatorBLL iBLL = new IndicatorBLL(_unit);
            iBLL.ProcessIndicatorFull();
        }

        [TestMethod]
        public void Test_SaveIndicatorsBatch()
        {
            IndicatorBLL iBLL = new IndicatorBLL(_unit);

            List<Indicator> lIndicator = new List<Indicator>();

            Indicator ind = new Indicator{
                    TradingDate = 1,
                    ShareId =1,
                    Close =0.1,
                    //PreviousClose = 0.1,
                    SMA5 =5,
                    SMA10 = 10,
                    SMA30 =20,
                    SMA50 =50,
                    SMA200 =200
                };

            lIndicator.Add(ind);

            for(int i=0; i< 1000; i++)
            {
                ind = new Indicator
                {
                    TradingDate = 1,
                    ShareId = 2,
                    Close = 0.1,
                    SMA5 = i,
                    SMA10 = null,
                    SMA30 = 20,
                    SMA50 = 50,
                    SMA200 = 200,
                    EMA10 =10,
                    EMA20 =20,
                    EMA50 =50,
                    ADX =1,
                    ADX_Minus=1,
                    ADX_Plus =2,
                    BB_High =1,
                    BB_Low =2,
                    BB_Middle=1.5,
                    Heikin_Close =1,
                    Heikin_High =2,
                    Heikin_Low =1,
                    Heikin_Open=1,
                    MACD= 1,
                    MACD_Hist=2,
                    MACD_Signal =3,
                    RSI =2,
                    Stochastic_D =1,
                    Stochastic_K =3,
                    WR =100
                };

                if(i%3 ==0)
                {
                    //ind.PreviousClose = i;
                }

                lIndicator.Add(ind);

            }

            iBLL.SaveIndicatorsBatchDB(lIndicator);
        }

        [TestMethod]
        public void Test_GetLatestIndicator()
        {
            IndicatorBLL iBLL = new IndicatorBLL(_unit);

            Indicator ind = iBLL.GetLatestIndicator(2);

        }

        [TestMethod]
        public void Test_GetIndicatorListByShareDate()
        {
            IndicatorBLL iBLL = new IndicatorBLL(_unit);

            var iList = iBLL.GetIndicatorListByShareDate(1585, 0, 0);

            Assert.IsTrue(iList.Count > 0);
        }


        [TestMethod]
        public void Test_UpdateDeltIndicators()
        {
            IndicatorBLL iBLL = new IndicatorBLL(_unit);

            iBLL.UpdateDeltIndicators(1326);

        }

        [TestMethod]
        public void Test_GetLatestTradingDate()
        {
            IndicatorBLL iBLL = new IndicatorBLL(_unit);

            var tDate = iBLL.GetLatestTradingDate();
        }

        [TestMethod]
        public void Test_UpdateDeltIndicatorsFull()
        {
            IndicatorBLL iBLL = new IndicatorBLL(_unit);

            iBLL.UpdateDeltIndicatorsFull();

        }


        [TestMethod]
        public void Test_UpdateIndicatorsBatchDB()
        {
            IndicatorBLL iBLL = new IndicatorBLL(_unit);

            List<Indicator> iList = (from c in _unit.DataContext.Indicator
                                    where c.ShareId == 1585
                                    select c).ToList();

            iBLL.UpdateIndicatorsBatchDB(iList);
        }

        [TestMethod]
        public void Test_SearchShareDayTicker() {

            var tickerLast = new TickerBLL(_unit).GetLastTicker(1585, null);

            IndicatorBLL iBLL = new IndicatorBLL(_unit);

            if(tickerLast != null)
            {
                iBLL.SearchShareDayTicker(tickerLast.TradingDate, tickerLast.TradingDate);
            }
        }

        [TestMethod]
        public void Test_GetShareDayTickerLatest()
        {
            IndicatorBLL iBLL = new IndicatorBLL(_unit);

            var stockList = iBLL.GetShareDayTickerLatest();
        }


        [TestMethod]
        public void Test_GetShareDayTickerByWatch()
        {
            IndicatorBLL iBLL = new IndicatorBLL(_unit);

            var tickerList = iBLL.GetShareDayTickerByWatch(1, false);
        }

        [TestMethod]
        public void Test_SearchShareByWatch()
        {
            IndicatorBLL iBLL = new IndicatorBLL(_unit);

            var tickerList = iBLL.SearchShareByWatch(15, 20160427, false);

            tickerList = iBLL.SearchShareByWatch(15, 20160427, true);
        }

    }
}
