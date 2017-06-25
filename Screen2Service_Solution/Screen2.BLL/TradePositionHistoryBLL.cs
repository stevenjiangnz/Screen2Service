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
    //public class TradePositionHistoryBLL : BaseBLL<TradePositionHistory>, IBaseBLL<TradePositionHistory>
    //{
    //    #region Constructors
    //    public TradePositionHistoryBLL(IUnitWork unit) : base(unit)
    //    {
    //    }
    //    #endregion

    //    #region Methods
    //    public void CreateNewPositionHistory(TradePosition Position, Transaction tr)
    //    {
    //        TradePositionHistory tph = new TradePositionHistory
    //        {
    //            AccountId = Position.AccountId,
    //            EntryCost = Position.EntryCost,
    //            EntryPrice = Position.EntryPrice,
    //            ShareId = Position.ShareId,
    //            Size = Position.Size,
    //            TradeSetId = Position.TradeSetId,
    //            TransactionId = tr.Id,
    //            TradingDate = tr.TradingDate,
    //            UpdateDT = DateTime.Now
    //        };

    //        this.Create(tph);
    //    }
    //    #endregion
    //}
}
