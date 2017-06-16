using Microsoft.VisualStudio.TestTools.UnitTesting;
using Screen2.DAL;
using Screen2.Entity;
using Screen2.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL.Test
{
    [TestClass]
    public class TestIdeaBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_Create()
        {
            IdeaBLL bll = new IdeaBLL(_unit);

            Idea b = new Idea
            {
                Owner = "2b658482-6a38-4ed3-b356-77fe9b1569f1",
                Content = "test content",
                Topic = "test topic",
                Type = IdeaType.Strategy_Sell.ToString(),
                Status = "Open",
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            bll.Create(b);

            b = new Idea
            {
                Owner = "2b658482-6a38-4ed3-b356-77fe9b1569f1",
                Content = "test content222",
                Topic = "test topic 222",
                Type = IdeaType.Strategy_Buy.ToString(),
                Status = "Open",
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            bll.Create(b);

        }

        [TestMethod]
        public void Test_GetList()
        {
            IdeaBLL bll = new IdeaBLL(_unit);
            string owner = "2b658482-6a38-4ed3-b356-77fe9b1569f1";

            var jList = bll.GetList(owner);


            //jList = bll.GetListByZone(owner, zoneId);
        }
    }
}
