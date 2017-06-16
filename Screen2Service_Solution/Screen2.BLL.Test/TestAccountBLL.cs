using Microsoft.VisualStudio.TestTools.UnitTesting;
using Screen2.DAL;
using Screen2.Entity;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL.Test
{
    [TestClass]
    public class TestAccountBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_Create()
        {
            AccountBLL bll = new AccountBLL(_unit);

            Account a = new Account
            {
                Name = "Accout 001",
                Description = "Account 001 Description",
                //TotalAmount = 50000,
                Owner = "2b658482-6a38-4ed3-b356-77fe9b1569f1",
                Status = "Active",
                CreatedBy = "2b658482-6a38-4ed3-b356-77fe9b1569f1",
                CreateDate = DateTime.Now
            };

            bll.Create(a);
        }

        [TestMethod]
        public void Test_GetAccountFullListByUser()
        {
            AccountBLL bll = new AccountBLL(_unit);
            var aList = bll.GetAccountFullListByUser("2b658482-6a38-4ed3-b356-77fe9b1569f1", 2);
        }

        [TestMethod]
        public void Test_TransferFund()
        {
            AccountBLL bll = new AccountBLL(_unit);

            bll.TransferFund(10, "Withdraw", 100);
        }

        [TestMethod]
        public void Test_GetAccountSummary()
        {
            AccountBLL bll = new AccountBLL(_unit);

            var summary = bll.GetAccountSummary(1039);

            string summaryString = XMLHelper.SerializeObject(summary);
        }


    }
}
