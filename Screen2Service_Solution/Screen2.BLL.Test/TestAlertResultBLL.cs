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
    public class TestAlertResultBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_AddAlertResultBatch()
        {
            AlertResultBLL arBll = new AlertResultBLL(_unit);

            List<AlertResult> arList = arBll.GetList().Take(2).ToList();

            arList[0].IsMatch = true;
            arList[0].Message = "test message";
            arList[0].ZoneId = 2;

            arBll.AddAlertResultBatch(arList);
        }
    }
}
