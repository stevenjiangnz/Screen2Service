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
    public class TestFunctoidBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_Create()
        {
            FunctoidBLL bll = new FunctoidBLL(_unit);

            Functoid func = new Functoid
            {
                Name = "ticker",
                Usage = "offset_day",
                ReturnType = "double",
                DefaultValue = "1.00"
            };

            bll.Create(func);
        }

        [TestMethod]
        public void Test_Update()
        {
            FunctoidBLL bll = new FunctoidBLL(_unit);

            Functoid func = bll.GetByID(1);

            func.DefaultValue = "2.0";

            bll.Update(func);
        }

        [TestMethod]
        public void Test_Delete()
        {
            FunctoidBLL bll = new FunctoidBLL(_unit);

            Functoid func = bll.GetByID(1);


            bll.Delete(func);
        }
    }
}
