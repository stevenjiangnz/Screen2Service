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
    public class TestWatchListBLL
    {

        UnitWork _unit = new UnitWork(new DataContext());
        [TestMethod]
        public void Test_Create()
        {
            WatchListBLL tbll = new WatchListBLL(_unit);

            WatchList w = new WatchList
            {
                Name = "SysWL - Long Term",
                Description = "Long Term Watch List",
                Owner = "system"
            };

            tbll.Create(w);
        }


        [TestMethod]
        public void Test_Update()
        {
            WatchListBLL tbll = new WatchListBLL(_unit);

            var w = tbll.GetByID(1);

            w.IsSystem = true;

            tbll.Update(w);
        }

        [TestMethod]
        public void Test_AddShareToWatchList()
        {
            WatchListBLL tbll = new WatchListBLL(_unit);

            tbll.AddShareToWatchList(1, "2b658482-6a38-4ed3-b356-77fe9b1569f1", "1");

        }

        [TestMethod]
        public void Test_RemoveShareFromWatchList()
        {
            WatchListBLL tbll = new WatchListBLL(_unit);

            tbll.RemoveShareFromWatchList(1, "2b658482-6a38-4ed3-b356-77fe9b1569f1", "1,20,3,4,5");

        }

        [TestMethod]
        public void Test_GetWatchListDetail()
        {
            WatchListBLL tbll = new WatchListBLL(_unit);

            tbll.GetWatchListDetail(1, "2b658482-6a38-4ed3-b356-77fe9b1569f1");

        }

        

        [TestMethod]
        public void Test_GetWatchListByShare()
        {
            WatchListBLL tbll = new WatchListBLL(_unit);

            var dlist =  tbll.GetWatchListByShare(2433, "2b658482-6a38-4ed3-b356-77fe9b1569f1");

        }

        [TestMethod]
        public void Test_GetWatchList()
        {
            WatchListBLL tbll = new WatchListBLL(_unit);
            List<WatchList> wlist= tbll.GetWatchList("2b658482-6a38-4ed3-b356-77fe9b1569f1");
        }
    }
}
