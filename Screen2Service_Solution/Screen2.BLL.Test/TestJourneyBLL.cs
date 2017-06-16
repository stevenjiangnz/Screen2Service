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
    public class TestJourneyBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_Create()
        {
            JourneyBLL bll = new JourneyBLL(_unit);

            Journey b = new Journey
            {
                Owner = "2b658482-6a38-4ed3-b356-77fe9b1569f1",
                Content = "test content",
                StartDay = "20160816",
        
                Status = "Open",
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            bll.Create(b);

            b = new Journey
            {
                Owner = "2b658482-6a38-4ed3-b356-77fe9b1569f1",
                Content = "test content222",
                StartDay = "20160817",
                ZoneId = 2,
                Status = "Open",
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            bll.Create(b);

        }

        [TestMethod]
        public void Test_GetListByZone()
        {
            JourneyBLL bll = new JourneyBLL(_unit);
            string owner = "2b658482-6a38-4ed3-b356-77fe9b1569f1";
            int? zoneId = 2;

            var jList = bll.GetListByZone(owner, null);


            jList = bll.GetListByZone(owner, zoneId);
        }
    }
}
