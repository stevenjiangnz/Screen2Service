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
    public class TestAccountBalanceBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        //[TestMethod]
        //public void Test_NewTransaction()
        //{
        //    int transactionId = 11;

        //    Transaction tr = new TransactionBLL(_unit).GetByID(transactionId);

        //    AccountBalance ab = new AccountBalanceBLL(_unit).NewTransaction(tr);
        //}

        [TestMethod]
        public void Test_CheckAccountBalance()
        {
            AccountBalanceBLL bll = new AccountBalanceBLL(_unit);

            //bool result =  bll.CheckAccountBalance(1051);

            //Assert.IsTrue(result);
        }
    }
}
