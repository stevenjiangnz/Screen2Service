using Microsoft.VisualStudio.TestTools.UnitTesting;
using Screen2.BLL;
using Screen2.DAL;
using Screen2.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Indicator.Test
{
    [TestClass]
    public class TestRSI
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void TestCalculate()
        {
            double[] close = new double[]{
                44.34, 44.09, 44.15, 43.61, 44.33,
                44.83, 45.10, 45.42, 45.84, 46.08,
                45.89, 46.03, 45.61, 46.28, 46.28,
                46.00, 46.03, 46.41, 46.22, 45.64,
                46.21, 46.25, 45.71, 46.45, 45.78,
                45.35, 44.03, 44.18, 44.22, 44.57,
                43.42, 42.66, 43.13
            };

            double?[] outRSI = new double?[close.Length];

            RSI.Calculate(close, 14, outRSI);
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

            Result res = RSI.Calculate(inputData, 14, outData);
        }
    }
}
