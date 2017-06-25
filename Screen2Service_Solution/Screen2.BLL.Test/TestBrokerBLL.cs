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
    public class TestBrokerBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_Create()
        {
            BrokerBLL bll = new BrokerBLL(_unit);

            Broker b = new Broker
            {
                Name = "BellDirect",
                Description = "Bell Direct",
                Shortable = false,
                MinFee = 15,
                FeeRate= 0.15
            };

            bll.Create(b);
        }

        [TestMethod]
        public void Test_Update()
        {
            BrokerBLL bll = new BrokerBLL(_unit);

            var b = bll.GetList().ToArray()[0];

            b.Owner = "2b658482-6a38-4ed3-b356-77fe9b1569f1";

            bll.Update(b);
        }
    }
}
