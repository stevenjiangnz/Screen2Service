using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Screen2.BLL;
using Screen2.DAL;
using Screen2.Entity;
using System.Collections.Generic;

namespace Screen2.Indicator.Test
{
    [TestClass]
    public class TestSMA
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void TestCalculate()
        {
            double[] inputData = new double[] { 11, 12, 13, 14, 15, 16, 17 };
            int len = inputData.Length;

            int period = 5;

            var input = new SMAIn();
            input.Data = new SMAItem[inputData.Length];

            for(int i=0; i <inputData.Length; i++)
            {
                input.Data[i] = new SMAItem();
                input.Data[i].iClose = inputData[i];
            }

            var setting = new SMASetting();
            setting.Period = period;
            setting.Offset = 1;

            Result res = new SMA().Calculate(input, setting);
        }


        [TestMethod]
        public void TestCalculateRealData()
        {
            TickerBLL tbll = new TickerBLL(_unit);

            List<Ticker> tList= tbll.GetTickerListByShareDB(1585, 0, 21100000);
            var input = new SMAIn();
            input.Data = new SMAItem[tList.Count];

            var i = 0;
            foreach(var t in tList)
            {
                input.Data[i] = new SMAItem();
                input.Data[i].TradingDate = t.TradingDate;
                input.Data[i].iClose = t.Close;
                i++;
            }

            var setting = new SMASetting();
            setting.Period = 50;
            setting.Offset = 0;

            Result res = new SMA().Calculate(input, setting);
        }
    }
}
