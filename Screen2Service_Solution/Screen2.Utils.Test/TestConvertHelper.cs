using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Screen2.Utils.Test
{
    [TestClass]
    public class TestConvertHelper
    {
        [TestMethod]
        public void Test_DateToInt()
        {
            DateTime date = DateTime.Now;

            var dateInt = DateHelper.DateToInt(date);

            Assert.IsTrue(dateInt == 20160301);
        }
    }
}
