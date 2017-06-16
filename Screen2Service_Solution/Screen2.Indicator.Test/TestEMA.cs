using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Screen2.BLL;
using Screen2.DAL;
using Screen2.Entity;
using System.Collections.Generic;

namespace Screen2.Indicator.Test
{
    [TestClass]
    public class TestEMA
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void TestCalculate()
        {
            double[] inputData = new double[] { 22.27, 22.19, 22.08, 22.17, 22.18,
                22.13, 22.23, 22.43, 22.24, 22.29,
                22.15, 22.39, 22.38, 22.61, 23.36,
                24.05, 23.75, 23.83, 23.95, 23.63,
                23.82, 23.87, 23.65, 23.19, 23.10,
                23.33, 22.68, 23.10, 22.40, 22.17};

            int len = inputData.Length;
            double?[] outData = new double?[len];

            int period = 10;

            EMA.Calculate(inputData, period, 2, outData);

        }


        [TestMethod]
        public void TestCalculateRealData()
        {

            TickerBLL tbll = new TickerBLL(_unit);

            List<Ticker> tList = tbll.GetTickerListByShareDB(1585, null, 21100000);

            double[] inputData = new double[tList.Count];
            double?[] outData = new double?[tList.Count];
            var i = 0;
            foreach (var t in tList)
            {
                inputData[i] = t.Close;
                i++;
            }


            Result res = EMA.Calculate(inputData, 20, 2, outData);
        }

    }
}
