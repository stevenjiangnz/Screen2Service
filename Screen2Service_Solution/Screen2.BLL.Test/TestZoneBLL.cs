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
    public class TestZoneBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_Create()
        {
            ZoneBLL tbll = new ZoneBLL(_unit);

            Zone w = new Zone
            {
                Name = "First Test Zone",
                Description = "Test Zone for main system",
                Owner = "system",
                StartDate = new DateTime(2015, 01, 01),
                TradingDate = 20150101,
                Status = "Active",
                Note = "test note"
            };

            tbll.Create(w);
        }

        [TestMethod]
        public void Test_GetList()
        {
            ZoneBLL tbll = new ZoneBLL(_unit);

            List<Zone> zList = tbll.GetList().ToList();

            Assert.IsTrue(zList.Count > 0);
        }

        [TestMethod]
        public void Test_GetById()
        {
            ZoneBLL tbll = new ZoneBLL(_unit);

            List<Zone> zList = tbll.GetList().ToList();

            Zone zone = tbll.GetByID(zList[0].Id);
            Assert.IsTrue(zone.Id > 0);
        }

        [TestMethod]
        public void Test_Update()
        {
            ZoneBLL tbll = new ZoneBLL(_unit);

            List<Zone> zList = tbll.GetList().ToList();

            Zone zone = tbll.GetByID(zList[0].Id);

            zone.Note = "udpate note on " + DateTime.Now.ToLongTimeString();
            zone.TradingDate = 20150101;
            tbll.Update(zone);
        }

        [TestMethod]
        public void Test_Delete()
        {
            ZoneBLL tbll = new ZoneBLL(_unit);

            List<Zone> zList = tbll.GetList().ToList();

            Zone zone = tbll.GetByID(zList[0].Id);

            tbll.Delete(zone.Id);
        }

        [TestMethod]
        public void Test_SetZoneNextDay()
        {
           ZoneBLL tbll = new ZoneBLL(_unit);

            var nZone = tbll.SetZoneNextDay(2);
        }

    }
}
