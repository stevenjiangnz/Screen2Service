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
    public class TestAlertBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_Create()
        {
            AlertBLL bll = new AlertBLL(_unit);

            Alert b = new Alert
            {
                 Formula ="test formula",
                 Message = "alert trigerred",
                 ModifiedDate = DateTime.Now,
                 Owner = "2b658482-6a38-4ed3-b356-77fe9b1569f1",
                 ShareId =1585
            };

            bll.Create(b);
        }


        [TestMethod]
        public void Test_VerifyAlert()
        {
            AlertBLL bll = new AlertBLL(_unit);

            Alert a = bll.GetByID(9);

            var result = bll.VerifyAlert(a);
        }

        [TestMethod]
        public void Test_ProcessAlertsFull()
        {
            AlertBLL bll = new AlertBLL(_unit);

            bll.ProcessAlertsFull(true);
        }

        [TestMethod]
        public void Test_SearchAlert()
        {
            AlertBLL bll = new AlertBLL(_unit);

            var result = bll.SearchAlert("2b658482-6a38-4ed3-b356-77fe9b1569f1", 20160902, false, null);
        }
    }
}
