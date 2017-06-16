using Microsoft.VisualStudio.TestTools.UnitTesting;
using Screen2.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL.Test
{
    [TestClass]
    public class TestDailyScanManager
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_MACD_Long()
        {
            ScanManager_MACD_Long scan = new ScanManager_MACD_Long(_unit);

            var result = scan.DailyScanByWatch(24, 20100310);

        }

        [TestMethod]
        public void Test_MACD_Short()
        {
            ScanManager_MACD_Short scan = new ScanManager_MACD_Short(_unit);

            var result = scan.DailyScanByWatch(24, 20100310);

        }

        [TestMethod]
        public void Test_ADX_DailyScanSharebyWatch()
        {
            ScanManager_ADX_Long rsiSM = new ScanManager_ADX_Long(_unit);

            var result = rsiSM.DailyScanByWatch(24, 20100310);
        }
    }
}
