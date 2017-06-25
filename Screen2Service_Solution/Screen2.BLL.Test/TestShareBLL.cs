using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Screen2.DAL;
using Screen2.Entity;
using Screen2.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL.Test
{
    [TestClass]
    public class TestShareBLL
    {
        private UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void TestUploadAsxSymbolFromASX()
        {
            UnitWork uw = new UnitWork(new DataContext());

            string symbolFilePath = (@"C:\devrepos\Screen2\Screen2Solution\Screen2.API\Data\ASX.txt");

            using (TextReader reader = File.OpenText(symbolFilePath))
            {
                CsvConfiguration config = new CsvConfiguration();

                config.Delimiter = "\t";
                config.HasHeaderRecord = true;

                var csv = new CsvReader(reader, config);

                while (csv.Read())
                {
                    var symbolField = csv.GetField<string>(0);
                    var DescriptionField = csv.GetField<string>(1);

                    Share share = new Share {

                        Name = DescriptionField,
                        Symbol = symbolField,
                        IsActive = true,
                        ShareType = StockType.Stock.ToString(),
                        MarketId = 1
                    };

                    if(share.Symbol.Length < 9)
                    {
                        uw.Share.Add(share);
                    }

                    //Debug.WriteLine(string.Format("{0}   {1}", symbolField, DescriptionField));
                }

            }

        }

        [TestMethod]
        public void TestGetShareListByZone()
        {
            ShareBLL sBLL = new ShareBLL(_unit);

            var sList = sBLL.GetShareListByZone(2);
        }

        [TestMethod]
        public void TestGetShareListByTradingDate()
        {
            ShareBLL sBLL = new ShareBLL(_unit);

            var sList = sBLL.GetShareListByTradingDate(20130101);
        }

        [TestMethod]
        public void Test_SetShareType()
        {
            ShareBLL sBLL = new ShareBLL(_unit);

            Share s = sBLL.GetByID(1585);

            sBLL.SetShareType(s, "Stock");
        }

        [TestMethod]
        public void Test_SetCFDFlag()
        {
            string path = @"c:\temp\shortablelist.csv";
            List<string> shortableList = new List<string>();
            ShareBLL bll = new ShareBLL(_unit);

            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    var s = sr.ReadLine();
                    shortableList.Add(s.Trim());
                }
            }

            var shareList = bll.GetList();

            foreach (var sh in shareList)
            {
                var Symbol = sh.Symbol;

                string v = shortableList.SingleOrDefault(c => c == Symbol);

                if (!string.IsNullOrEmpty(v))
                {
                    sh.IsCFD = true;
                    bll.Update(sh);
                    Debug.WriteLine("Updated cfd for {0}  {1}", Symbol, sh.Id);
                }

            }


        }

        [TestMethod]
        public void Test_GetShareListByWatch()
        {
            ShareBLL sBLL = new ShareBLL(_unit);

            var slist = sBLL.GetShareListByWatch(15, false);

            slist = sBLL.GetShareListByWatch(15, true);
        }
    }
}
