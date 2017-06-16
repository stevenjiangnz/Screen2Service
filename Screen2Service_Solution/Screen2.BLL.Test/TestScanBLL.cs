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
    public class TestScanBLL
    {
        private UnitWork _unit = new UnitWork(new DataContext());

        #region Methods
        [TestMethod]
        public void Test_Create()
        {
            ScanBLL bll = new ScanBLL(_unit);

            Scan s = new Scan
            {
                Owner = "2b658482-6a38-4ed3-b356-77fe9b1569f1",
                //ShareId = 1585,
                IsActive = true,
                IsSystem = false,
                RuleId = 4,
                StartDate = 20160101,
                EndDate = 20160501,
                 Name = "test scan",
                 Description = "test scan description"
            };

            bll.Create(s);
        }

        [TestMethod]
        public void Test_GetList()
        {
            ScanBLL bll = new ScanBLL(_unit);
            var sList = bll.GetList();

            Assert.IsTrue(sList.Count() > 0);
        }


        [TestMethod]
        public void Test_Update()
        {
            ScanBLL bll = new ScanBLL(_unit);
            var sList = bll.GetList();

            var s = sList.ToArray()[0];

            s.Description = "updated description...";

            bll.Update(s);
        }

        [TestMethod]
        public void Test_Delete()
        {
            ScanBLL bll = new ScanBLL(_unit);
            var sList = bll.GetList();

            var s = sList.ToArray()[0];

            bll.Delete(s.Id);
        }


        #endregion
    }
}
