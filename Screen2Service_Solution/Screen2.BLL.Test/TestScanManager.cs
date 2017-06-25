using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCalc;
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
    public class TestScanManager
    {
        private UnitWork _unit = new UnitWork(new DataContext());
        private ScanCalculator _bll;

        public TestScanManager()
        {
            _bll = new ScanCalculator(_unit);
        }

        [TestMethod]
        public void Test_Cal()
        {
            Expression e = new Expression("2 + 3 * 5");
            Assert.IsTrue(17 == (int)e.Evaluate());
        }

        //[TestMethod]
        //public void TestGetIndicators()
        //{
        //    Indicator[] indicators = _bll.GetIndicators(1585, 20150101, 20150701);

        //    Assert.IsTrue(indicators.Length > 0);
        //}

        //[TestMethod]
        //public void TestGetTickers()
        //{
        //    Ticker[] tickers = _bll.GetTickers(1585, 20150101, 20150701);

        //    Assert.IsTrue(tickers.Length > 0);
        //}

        //[TestMethod]
        //public void TestPopulateIndicatorList()
        //{
        //    Indicator[] indicators = _bll.GetIndicators(1585, 20150101, 20150701);
        //    Ticker[] tickers = _bll.GetTickers(1585, 20150101, 20150701);

        //    _bll.PopulateIndicatorList(indicators, tickers);
        //}


        [TestMethod]
        public void TestProcessIndicatorExpression()
        {
            //Indicator[] indicators = _bll.GetIndicators(1585, 20150101, 20150701);

            string formula = "(ivalue('close') > (ivalue('open') * 2)) || (ivalue('close') * 2 > ivalue('open') ) ";

            //_bll.CheckIndicatorExpression(formula);
        }

        [TestMethod]
        public void TestRunScan()
        {
            ScanBLL sBLL = new ScanBLL(_unit);

            var s = sBLL.GetList().ToArray()[0];

            _bll.RunScan(s);
        }
    }
}
