﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Screen2.BLL;
using Screen2.BLL.Interface;
using Screen2.DAL;

namespace Screen2.Tests.BLL
{
    [TestClass]
    public class TickerLoaderTest
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void TestMethod1()
        {
            var mock = new Mock<ITickerLoader>();
            mock.Setup(loader => loader.LoadTickers()).Returns(true);
            Assert.IsTrue(1 == 1, "Test message passed");
        }

        [TestMethod]
        public void LoadTickers_Should_Return_True()
        {
            var mock = new Mock<TickerLoader>() { CallBase = true };
            mock.Setup(loader => loader.UploadAsxEodRaw()).Returns(true);

            TickerBLL bll = new TickerBLL(null, "testConnectionString");

            var result = bll.LoadTickers(mock.Object);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LoadTickers_Should_Return_False()
        {
            var mock = new Mock<TickerLoader>() { CallBase = true };
            mock.Setup(loader => loader.UploadAsxEodRaw()).Returns(false);

            TickerBLL bll = new TickerBLL(null, "testConnectionString");

            var result = bll.LoadTickers(mock.Object);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Test_LoadAsxEodRawFromDisk()
        {
            TickerBLL bll = new TickerBLL(_unit);

            bll.LoadAsxEodRawFromDisk();
        }

    }
}
