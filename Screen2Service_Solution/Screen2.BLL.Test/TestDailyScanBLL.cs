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
    public class TestDailyScanBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_Create()
        {
            DailyScanBLL bll = new DailyScanBLL(_unit);

            DailyScan ds = new DailyScan{
                Name = "Daily Scan 11",
                Description = "Dail Scan 11 Description",
                Owner = "2b658482-6a38-4ed3-b356-77fe9b1569f1",
                UseRule = false,
                Formula = "test message formula",
                WatchListString = "15",
                Modified = DateTime.Now,
                LastProcessed = DateTime.Now,
                Status = "Active"
                };

            bll.Create(ds);
        }

        [TestMethod]
        public void Test_ProcessDailyScanFull()
        {
            DailyScanBLL bll = new DailyScanBLL(_unit);

           // bll.ProcessDailyScanFull(true);
        }

        [TestMethod]
        public void Test_ProcessDailyScan()
        {
            DailyScanBLL bll = new DailyScanBLL(_unit);

            DailyScan ds = bll.GetByID(8);

            //bll.ProcessDailyScan(ds,20160901, true);
        }


        [TestMethod]
        public void Test_SearchScan()
        {
            DailyScanBLL bll = new DailyScanBLL(_unit);

            bll.SearchScan(20160902, 8, false);
        }


        [TestMethod]
        public void Test_RunDailyScanByAssembly()
        {
            DailyScanBLL bll = new DailyScanBLL(_unit);
            DailyScan ds = bll.GetByID(15);
            var result = bll.RunDailyScanByAssembly(ds, "ScanManager_RSI2", 20100310);
        }
    }
}
