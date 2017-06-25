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
    public class TestAccountBalanceJourneyBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_Create()
        {
            AccountBalanceJourneyBLL bll = new AccountBalanceJourneyBLL(_unit);

            AccountBalanceJourney abj = new AccountBalanceJourney();
            abj.AccountId = 1030;

            bll.Create(abj);
        }

        [TestMethod]
        public void Test_GetListByAccount()
        {
            AccountBalanceJourneyBLL bll = new AccountBalanceJourneyBLL(_unit);

            var sbList = bll.GetListByAccount(1036, -1);
        }
    }
}
