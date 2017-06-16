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
    public class TestRecordBLL
    {
        private UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void TestCreate()
        {
            RecordBLL bll = new RecordBLL(_unit);

            Record r = new Record
            {
                Title = "name 1",
                Type = "Buy",
                ZoneId = 2,
                Owner = "a3b06d61-8fea-456c-ab1e-9207f3bfb875",
                CreateDT = DateTime.Now
            };

            bll.Create(r);
        }
    }
}
