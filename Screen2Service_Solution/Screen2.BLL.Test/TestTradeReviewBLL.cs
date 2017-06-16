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
    public class TestTradeReviewBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_Create()
        {
            TradeReviewBLL bll = new TradeReviewBLL(_unit);

            TradeReview tr = bll.GetByID(5);

            bll.PopulateReview(tr);

            bll.Update(tr);

            //bll.CreateTradeReview(new Entity.TradeReview
            //{
            //    TradePositionId = 2008,
            //    Notes = "test notes",
            //    UpdatedBy = "System",
            //    UpdatedDT = DateTime.Now
            //});

        }

        [TestMethod]
        public void Test_GetTradeReviewByPositionId()
        {
            TradeReviewBLL bll = new TradeReviewBLL(_unit);

            var review = bll.GetTradeReviewByPositionId(2008);

        }

        [TestMethod]
        public void Test_PopulateReviewList()
        {
            TradeReviewBLL bll = new TradeReviewBLL(_unit);

            List<TradeReview> trList = bll.GetList().ToList();

            foreach (var tr in trList)
            {
                bll.PopulateReview(tr);

                bll.Update(tr);
            }

        }


    }
}
