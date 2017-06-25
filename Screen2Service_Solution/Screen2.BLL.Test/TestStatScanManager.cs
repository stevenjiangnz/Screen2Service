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
    public class TestStatScanManager
    {
        private UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_RSI2_Scan()
        {
            ScanManager_RSI2 rsiSM = new ScanManager_RSI2(_unit);

            rsiSM.ScanWatchShares(24, 20120101, 20150101);
        }

        [TestMethod]
        public void Test_RSI2_ScanShare()
        {
            ScanManager_RSI2 rsiSM = new ScanManager_RSI2(_unit);

            var result = rsiSM.ScanShare(GetNewScanSet("test"), 1585, 20140501, 20161101);

        }

        [TestMethod]
        public void Test_RSI2_DailyScanShare()
        {
            ScanManager_RSI2 rsiSM = new ScanManager_RSI2(_unit);

            //var result = rsiSM.ScanWatch(24, 20140310, 20160310);
        }

        [TestMethod]
        public void Test_RSI2_DailyScanSharebyWatch()
        {
            ScanManager_RSI2 rsiSM = new ScanManager_RSI2(_unit);

            var result = rsiSM.DailyScanByWatch(27, 20100310);
        }

        [TestMethod]
        public void Test_ScanManager_MACD_Long()
        {
            ScanManager_MACD_Long scan = new ScanManager_MACD_Long(_unit);

            var result = scan.ScanWatchShares(24, 20080310, 20160310);
        }

        [TestMethod]
        public void Test_DailyScanSharebyWatch_Macd_short()
        {
            ScanManager_MACD_Short scan = new ScanManager_MACD_Short(_unit);

            var result = scan.ScanWatchShares(24, 20080310, 20160310);
        }


        [TestMethod]
        public void Test_ScanManager_ADX_Long()
        {
            ScanManager_ADX_Long scan = new ScanManager_ADX_Long(_unit);

            var result = scan.ScanWatchShares(24, 20080310, 20160310);
        }

        private StatScanSet GetNewScanSet(string setRef)
        {
            StatScanSet sst = new StatScanSet()
            {
                Id = -1,
                SetRef = setRef,
                EntryLogic = "",
                Notes = "",
                StartDt = DateTime.Now,
            };

            return sst;
        }


        [TestMethod]
        public void Test_ScanManager_ADX_Short()
        {
            ScanManager_ADX_Short scan = new ScanManager_ADX_Short(_unit);

            var result = scan.ScanWatchShares(24, 20120310, 20160310);
        }

        [TestMethod]
        public void Test_MACD_Short_ScanShare()
        {
            ScanManager_MACD_Short scanner = new ScanManager_MACD_Short(_unit);

            var result = scanner.ScanShare(GetNewScanSet("test"), 1928, 20120520, 20120611);

        }


        [TestMethod]
        public void Test_ADX_Long_ScanShare()
        {
            ScanManager_ADX_Long adxLong = new ScanManager_ADX_Long(_unit);

            var result = adxLong.ScanShare(GetNewScanSet("test"), 1585, 20140501, 20161101);
        }

        [TestMethod]
        public void Test_ADX_Short_ScanShare()
        {
            ScanManager_ADX_Short adxShort = new ScanManager_ADX_Short(_unit);

            var result = adxShort.ScanShare(GetNewScanSet("test"), 1928, 20120405, 20120611);
        }

        [TestMethod]
        public void Test_ScanManager_Pullback_Long()
        {
            ScanManager_Pullback_Long scan = new ScanManager_Pullback_Long(_unit);

            var result = scan.ScanWatchShares(24, 20080310, 20160310);
        }

        [TestMethod]
        public void Test_ScanManager_Pullback_Short()
        {
            ScanManager_Pullback_Short scan = new ScanManager_Pullback_Short(_unit);

            var result = scan.ScanWatchShares(24, 20080310, 20160310);
        }

        [TestMethod]
        public void Test_ScanManager_Breakout_Long()
        {
            ScanManager_BreakOut_Long scan = new ScanManager_BreakOut_Long(_unit);

            var result = scan.ScanWatchShares(24, 20120310, 20160310);
        }
    }
}
