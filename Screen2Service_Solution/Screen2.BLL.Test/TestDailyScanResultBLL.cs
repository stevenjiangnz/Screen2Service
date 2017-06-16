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
    public class TestDailyScanResultBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_AddDailyScanResultBatch()
        {
            DailyScanResultBLL bll = new DailyScanResultBLL(_unit);

            //List<DailyScanResult> dsrResult = bll.GetDailyScanResult(20160902, 8);

            //dsrResult = dsrResult.Take(2).ToList();
            //dsrResult[0].Id = 0;
            //dsrResult[0].IsMatch = true;
            //dsrResult[0].Message = "test message";

            DailyScanBLL dbll = new DailyScanBLL(_unit);
            DailyScan ds = dbll.GetByID(15);
            var result = dbll.RunDailyScanByAssembly(ds, "ScanManager_RSI2", 20100310);

            bll.AddDailyScanResultBatch(result);
        }
    }
}
