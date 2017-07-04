using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Screen2.BLL;
using Screen2.BLL.Interface;
using Screen2.DAL;
using Screen2.DAL.Interface;

namespace Screen2.Tests.BLL
{
    [TestClass]
    public class TickerBLLTest
    {
        private Mock<IUnitWork> unitMock = new Mock<IUnitWork>();
        private Mock<ITickerLoader> tickerMock = new Mock<ITickerLoader>();
        UnitWork _unit = new UnitWork(new DataContext());

        [TestInitialize]
        public void Initialize()
        {
            tickerMock.Setup(loader => loader.LoadTickers()).Returns(true);
        }


        [TestMethod]
        public void TestLoadTickers_Should_Return_True()
        {
            TickerBLL tbll = new TickerBLL(unitMock.Object, "testconn");

            Boolean result = tbll.LoadTickers(tickerMock.Object);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestLoadAsxRawFromAzure()
        {
            TickerBLL tbll = new TickerBLL(_unit, null);

            tbll.LoadAsxEodRawFromAzure();
        }
    }
}
