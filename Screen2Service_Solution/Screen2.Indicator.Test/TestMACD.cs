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
    public class TestMACD
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void TestCalculate()
        {
            TickerBLL tbll = new TickerBLL(_unit);

            List<Ticker> tList = tbll.GetTickerListByShareDB(1585, 0, 21100000);

            double[] inputData = new double[tList.Count];
            
            var i = 0;
            foreach (var t in tList)
            {
                inputData[i] = t.Close;
                i++;
            }

            double?[] m = new double?[tList.Count];
            double?[] s = new double?[tList.Count];
            double?[] h = new double?[tList.Count];

            Result res = MACD.Calculate(inputData, 26, 12, 9, m, s, h);
        }
    }
}
