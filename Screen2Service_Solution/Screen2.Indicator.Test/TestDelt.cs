using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Indicator.Test
{
    [TestClass]
    public class TestDelt
    {

        [TestMethod]
        public void TestCalculate()
        {
            double?[] inputData = new double?[] { 22.27, 22.19, 22.08, 22.17, 22.18,
                22.13, 22.23, 22.43, 22.24, 22.29,
                22.15, 22.39, 22.38, 22.61, 23.36,
                24.05, 23.75, 23.83, 23.95, 23.63,
                23.82, 23.87, 23.65, 23.19, 23.10,
                23.33, 22.68, 23.10, 22.40, 22.17};

            int len = inputData.Length;
            double?[] outData = new double?[len];

            int period = 1;

            Delt.Calculate(inputData, period, outData);

        }

    }
}
