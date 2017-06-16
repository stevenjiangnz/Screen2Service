using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class TradeReviewBLL : BaseBLL<TradeReview>, IBaseBLL<TradeReview>
    {

        #region Constructors
        public TradeReviewBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public TradeReview CreateTradeReview(TradeReview review)
        {
            PopulateReview(review);

            Create(review);

            return review;
        }

        public TradeReview GetTradeReviewByPositionId(int positionId)
        {
            TradeReview review = null;

            review = _unit.DataContext.TradeReview.SingleOrDefault(p => p.TradePositionId == positionId);

            if(review == null)
            {
                review = new TradeReview();
                review.TradePositionId = positionId;
                review.UpdatedBy = "System";
                review.UpdatedDT = DateTime.Now;
                this.CreateTradeReview(review);
            }


            return review;
        }

        public void PopulateReview(TradeReview review)
        {
            TransactionBLL tBLL = new TransactionBLL(_unit);
            TradePositionBLL tpBll = new TradePositionBLL(_unit);
            IndicatorBLL iBll = new IndicatorBLL(_unit);
            TickerBLL tkBll = new TickerBLL(_unit);

            OutPosition pos = tpBll.GetOutPositionById(review.TradePositionId);

            Transaction entryTr = tBLL.GetByID(pos.EntryTransactionId);

            Entity.Indicator entryInd = iBll.GetIndicatorByShareDate(pos.ShareId, entryTr.TradingDate);
            Ticker entryT = tkBll.GetTickerByDate(pos.ShareId, entryTr.TradingDate);

            iBll.PopulateIndicatorWithTicker(entryInd, entryT);

            review.IsEntryLong = pos.Size > 0 ? true : false;

            if (entryInd.BB_Low.HasValue && entryInd.BB_High.HasValue)
            {
                review.BBEntryPercent = 100 * (entryTr.Price - entryInd.BB_Low.Value) / (entryInd.BB_High.Value - entryInd.BB_Low.Value);
            }

            review.EntryPercent = 100 * (entryTr.Price - entryInd.Low.Value) / (entryInd.High.Value - entryInd.Low.Value);

            if (pos.ExitTransactionId.HasValue)
            {
                Transaction exitTr = new TransactionBLL(_unit).GetByID(pos.ExitTransactionId.Value);
                Entity.Indicator exitInd = new IndicatorBLL(_unit).GetIndicatorByShareDate(pos.ShareId, exitTr.TradingDate);
                Ticker exitT = tkBll.GetTickerByDate(pos.ShareId, exitTr.TradingDate);
                iBll.PopulateIndicatorWithTicker(exitInd, exitT);

                if (exitInd.BB_Low.HasValue && exitInd.BB_High.HasValue)
                {
                    review.BBExitPercent = 100 * (exitTr.Price - exitInd.BB_Low.Value) / (exitInd.BB_High.Value - exitInd.BB_Low.Value);
                }

                review.ExitPercent = 100 * (exitTr.Price - exitInd.Low.Value) / (exitInd.High.Value - exitInd.Low.Value);

                review.DaysSpan = pos.Days;
                review.Diff = pos.Diff;
                review.Diff_Per = pos.Diff_Per;
            }
        }

        #endregion

    }
}
