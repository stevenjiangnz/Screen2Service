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
    public class TestRuleBLL
    {
        private UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void TestCreate()
        {
            RuleBLL bll = new RuleBLL(_unit);

            Rule r = new Rule
            {
                Name="name 1",
                Description ="description 1",
                Type = "Buy",
                Formula = "IValue(x)>0",
                Note = "This is a test",
                Owner = "a3b06d61-8fea-456c-ab1e-9207f3bfb875"
            };

            bll.Create(r);
        }

        [TestMethod]
        public void TestUpdate()
        {
            RuleBLL bll = new RuleBLL(_unit);

            Rule r = bll.GetByID(1);

            r.Note = DateTime.Now.ToLongTimeString() + "test ";
            bll.Update(r);
        }

        [TestMethod]
        public void TestDelete()
        {
            RuleBLL bll = new RuleBLL(_unit);

            bll.Delete(2);
        }

        [TestMethod]
        public void TestGetList()
        {
            RuleBLL bll = new RuleBLL(_unit);

            var list = bll.GetList();

            Assert.IsNotNull(list);
        }

    }
}
