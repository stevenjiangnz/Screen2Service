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
    public class TestShareInfoBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_GetShareProfileFromYahoo()
        {
            ShareInfoBLL sibll = new ShareInfoBLL(_unit);

            sibll.UploadShareProfileFromYahoo("ORG.AX");
            //sibll.UploadShareProfileFromYahoo("UBP.AX");

        }

        [TestMethod]
        public void Test_PopulateShareStatFromYahoo()
        {
            ShareInfoBLL sibll = new ShareInfoBLL(_unit);
            ShareInfo info = new ShareInfo();
            sibll.GetShareStatFromYahoo("ORG.AX", info);
        }

        [TestMethod]
        public void Test_UploadShareProfileFromYahooAll()
        {
            ShareInfoBLL sibll = new ShareInfoBLL(_unit);
            sibll.UploadShareProfileFromYahooAll();

        }

    }
}
