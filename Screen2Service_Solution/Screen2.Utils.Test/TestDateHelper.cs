using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Utils.Test
{
    [TestClass]
    public class TestDateHelper
    {
        [TestMethod]
        public void Test_DateToInt()
        {
            int dateInt = DateHelper.DateToInt(DateTime.Now);

        }

        [TestMethod]
        public void Test_IntToDate()
        {
            int dateInt = DateHelper.DateToInt(DateTime.Now);
            DateTime dt = DateHelper.IntToDate(dateInt);
        }

        [TestMethod]
        public void Test_NextTradingDay()
        {
            int dateInt = DateHelper.DateToInt(DateTime.Now.AddDays(-2));
            var nextDay = DateHelper.NextTradingDay(dateInt);
        }

        [TestMethod]
        public void Test_IntToJSTicks()
        {
            DateTime dt= new DateTime(2001, 10, 10);

            var tk = DateHelper.IntToJSTicks(20011010);

            Assert.IsTrue(tk == 1002672000000);
        }
    }
}
