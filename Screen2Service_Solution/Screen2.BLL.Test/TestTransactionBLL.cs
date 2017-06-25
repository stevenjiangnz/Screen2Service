using Microsoft.VisualStudio.TestTools.UnitTesting;
using Screen2.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL.Test
{
    [TestClass]
    public class TestTransactionBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_GetTransactionList()
        {
            TransactionBLL tBLL = new TransactionBLL(_unit);

            var tList = tBLL.GetTransactionList(1039, 50);

        }
    }
}