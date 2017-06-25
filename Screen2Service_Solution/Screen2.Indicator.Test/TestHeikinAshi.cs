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
    public class TestHeikinAshi
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void TestCalculate()
        {
            TickerBLL tbll = new TickerBLL(_unit);

            List<Ticker> tList = tbll.GetTickerListByShareDB(1585, 20160401, 21100000);

            double[] o = new double[tList.Count];
            double[] h = new double[tList.Count];
            double[] l = new double[tList.Count];
            double[] c = new double[tList.Count];

            double?[] oo = new double?[tList.Count];
            double?[] oh = new double?[tList.Count];
            double?[] ol = new double?[tList.Count];
            double?[] oc = new double?[tList.Count];

            var i = 0;
            foreach (var t in tList)
            {
                o[i] = t.Open;
                h[i] = t.High;
                l[i] = t.Low;
                c[i] = t.Close;

                i++;
            }

            Result res = HeikinAshi.Calculate(o, c, h, l, oo, oc, oh, ol);
        }
    }
}
