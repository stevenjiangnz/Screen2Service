using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Shared;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class TradePositionBLL : BaseBLL<TradePosition>, IBaseBLL<TradePosition>
    {
        #region Constructors
        public TradePositionBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public List<TradePosition> GetListByAccount(int accountId)
        {
            List<TradePosition> positionList = new List<TradePosition>();

            positionList = _unit.DataContext.TradePosition.Where(o => o.AccountId == accountId).ToList();

            return positionList;
        }

        public List<TradePosition> GetOutstandingPositions(int accountId)
        {
            List<TradePosition> positionList = new List<TradePosition>();

            positionList = _unit.DataContext.TradePosition.Where(p=>p.AccountId == accountId &&
            (p.ExitTransactionId ==null || p.ExitTransactionId <=0)).ToList();

            return positionList;
        }

        public double GetOutstandingPositionValue(int accountId)
        {
            double posSum = 0;

            List<TradePosition> positionList = GetOutstandingPositions(accountId);

            foreach(var p in positionList)
            {
                posSum += p.CurrentValue;
            }

            return posSum;
        }

        public void UpdatePositionValue(TradePosition position, Entity.Indicator ind)
        {
            position.CurrentPrice = ind.Close.Value;
            position.CurrentTradingDate = ind.TradingDate;
            position.LastProcessedDate = ind.TradingDate;

            double diff = (position.CurrentPrice - position.EntryPrice) * position.Size;
            position.Margin = position.EntryPrice * position.Size * position.Flag * Global.MarginRate + diff;

            Update(position);

        }



        public OutPosition GetOutPositionById(int id)
        {
            OutPosition opResult = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetOutPositionById", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;

                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            OutPosition w;
                            while (reader.Read())
                            {
                                w = new OutPosition();

                                this.PopulateOutPositionReview(reader, w);

                                opResult = w;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error read latest to DB. ", ex);
                throw;
            }

            return opResult;
        }



        public List<OutPosition> GetPositionSummaryList(int accountId, int? size)
        {
            List<OutPosition> opList = new List<OutPosition>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetPositionListByAccount", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("AccountId", SqlDbType.Int).Value = accountId;
                        if(size.HasValue)
                        {
                            cmd.Parameters.Add("Take", SqlDbType.Int).Value = size.Value;
                        }

                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            OutPosition w;
                            while (reader.Read())
                            {
                                w = new OutPosition();

                                this.PopulateOutPositionReview(reader, w);

                                opList.Add(w);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error read latest to DB. ", ex);
                throw;
            }


            return opList;
        }

        private void PopulateOutPositionReview(SqlDataReader reader, OutPosition w)
        {
            w.Id = (int)reader["ID"];
            w.ShareId = (int)reader["ShareId"];
            w.Size = (int)reader["Size"];

            if (!reader.IsDBNull(reader.GetOrdinal("EntryPrice")))
                w.EntryPrice = reader.GetDouble(reader.GetOrdinal("EntryPrice"));

            if (!reader.IsDBNull(reader.GetOrdinal("EntryFee")))
                w.EntryFee = reader.GetDouble(reader.GetOrdinal("EntryFee"));

            if (!reader.IsDBNull(reader.GetOrdinal("EntryDate")))
                w.EntryDate = reader.GetInt32(reader.GetOrdinal("EntryDate"));

            if (!reader.IsDBNull(reader.GetOrdinal("EntryTransactionId")))
                w.EntryTransactionId = reader.GetInt32(reader.GetOrdinal("EntryTransactionId"));

            if (!reader.IsDBNull(reader.GetOrdinal("ExitPrice")))
                w.ExitPrice = reader.GetDouble(reader.GetOrdinal("ExitPrice"));

            if (!reader.IsDBNull(reader.GetOrdinal("ExitFee")))
                w.ExitFee = reader.GetDouble(reader.GetOrdinal("ExitFee"));

            if (!reader.IsDBNull(reader.GetOrdinal("ExitDate")))
                w.ExitDate = reader.GetInt32(reader.GetOrdinal("ExitDate"));

            if (!reader.IsDBNull(reader.GetOrdinal("ExitTransactionId")))
                w.ExitTransactionId = reader.GetInt32(reader.GetOrdinal("ExitTransactionId"));

            if (!reader.IsDBNull(reader.GetOrdinal("DaySpan")))
                w.Days = reader.GetInt32(reader.GetOrdinal("DaySpan"));

        }

        public TradePosition UpdatePosition(InUpdatePositionParams input)
        {
            TradePosition position = this.GetByID(input.Id);

            position.Stop = input.Stop;
            position.Limit = input.Limit;
            position.UpdateDT = DateTime.Now;

            Update(position);

            return position;
        }

        public TradePosition GetByAccountShare(int accountId, int shareId)
        {
            TradePosition tp = null;

            tp = _unit.DataContext.TradePosition.Where(p => p.AccountId == accountId && p.ShareId == shareId).SingleOrDefault();

            return tp;
        }

        public TradePosition CreateEntryPosition(Transaction trans, TradeOrder order, Entity.Indicator ind)
        {
            TradePosition tp = new TradePosition
            {
                AccountId = order.AccountId,
                CurrentPrice = ind.Close.Value,
                CurrentTradingDate = ind.TradingDate,
                EntryPrice = trans.Price,
                EntryFee = trans.Fee,
                EntryTransactionId = trans.Id,
                Limit = order.Limit,
                Stop = order.Stop,
                ShareId = order.ShareId,
                Size = trans.Size * order.Flag,
                Source = order.Source,
                UpdateDT = DateTime.Now
            };

            // Set margin on position
            double diff = (tp.CurrentPrice - trans.Price) * trans.Size * trans.Flag;
            tp.Margin = tp.EntryPrice * tp.Size * tp.Flag * Global.MarginRate + diff;

            this.Create(tp);

            return tp;
        }

        public TradePosition UpdateExitPosition(TradePosition position, Transaction trans, TradeOrder order, Entity.Indicator ind)
        {
            position.ExistFee = trans.Fee;
            position.ExistPrice = trans.Price;
            position.ExitTransactionId = trans.Id;
            position.LastProcessedDate = ind.TradingDate;
            position.UpdateDT = DateTime.Now;

            double diff = (position.ExistPrice - position.EntryPrice) * trans.Size * position.Flag;
            position.Margin = position.EntryPrice * position.Size * Global.MarginRate * position.Flag + diff;

            Transaction en = new TransactionBLL(_unit).GetByID(position.EntryTransactionId);
            position.days = new TickerBLL(_unit).GetTradesDaySpan(position.ShareId, en.TradingDate, trans.TradingDate);

            this.Update(position);

            return position;
        }

        /// <summary>
        /// Creates the trade position.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="ts">The ts.</param>
        /// <returns>is position closed </returns>
        public TradePosition CreateTradePosition(Transaction trans, TradeOrder order, TradeSet ts, Entity.Indicator ind, out bool isReverse)
        {
            TradePosition tp = null;
            TradeOrderBLL toBll = new TradeOrderBLL(_unit);
            //TradeOrder order = toBll.GetByID(trans.TradeOrderId);
            isReverse = false;

            tp = _unit.DataContext.TradePosition.Where(p => p.AccountId == order.AccountId && p.ShareId == order.ShareId).SingleOrDefault();

            if (tp == null)            {
                tp = new TradePosition
                {
                    ShareId = order.ShareId,
                    AccountId = order.AccountId,
                    Size = trans.Size * order.Flag,
                    EntryPrice = (trans.Size * trans.Price + trans.Fee * trans.Flag) / trans.Size,
                    UpdateDT = DateTime.Now,
                    CurrentPrice = ind.Close.Value,
                    CurrentTradingDate = ind.TradingDate,
                };

                this.Create(tp);
            }
            else {
                // update the position size 
                if(toBll.IsReverse(order, tp))
                {
                    isReverse = true;
                    tp.Size += order.Size * order.Flag;

                    if (tp.Size != 0)
                        tp.EntryPrice = tp.EntryCost / Math.Abs(tp.Size);

                    tp.UpdateDT = DateTime.Now;
                    tp.CurrentPrice = ind.Close.Value;
                    tp.CurrentTradingDate = ind.TradingDate;

                    this.Update(tp);

                }
                else
                {
                    tp.Size += order.Size * order.Flag;
                    
                    if (tp.Size != 0)
                        tp.EntryPrice = tp.EntryCost / Math.Abs(tp.Size);

                    tp.UpdateDT = DateTime.Now;
                    tp.CurrentPrice = ind.Close.Value;
                    tp.CurrentTradingDate = ind.TradingDate;

                    this.Update(tp);
                }
            }

            return tp;
        }
        #endregion
    }
}
