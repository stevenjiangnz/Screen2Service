using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class TransactionBLL : BaseBLL<Transaction>, IBaseBLL<Transaction>
    {
        #region Constructors
        public TransactionBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Method
        public void DeleteByAccount(int accountID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_DeleteTransactionByAccount", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("AccountId", SqlDbType.Int).Value = accountID;
                        cmd.Connection.Open();

                        var reader = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error delete transaction from DB. ", ex);
                throw;
            }
        }

        public Transaction CreateTransaction(TradeOrder order, TradeSet ts, Entity.Indicator ind)
        {
            Share s = new ShareBLL(_unit).GetByID(order.ShareId);
            Transaction tr = new Transaction {
                 Direction = order.Direction,
                 Message = string.Format("{0} {1} * {2} @ ${3}", order.Direction, s.Symbol, order.Size, order.OrderPrice),
                 ModifiedBy = order.UpdatedBy,
                 ModifiedDate = DateTime.Now,
                 Price = order.OrderPrice,
                 Size = order.Size,
                 Fee = order.Fee,
                 TradingDate = ind.TradingDate,
                 TradeOrderId = order.Id
                 //,
                 //TradeSetID = ts.Id
            };

            this.Create(tr);
            return tr;
        }

        public Transaction CreateTransaction(TradeOrder order, Entity.Indicator ind)
        {
            Share s = new ShareBLL(_unit).GetByID(order.ShareId);
            Transaction tr = new Transaction
            {
                Direction = order.Direction,
                Message = string.Format("{0} {1} * {2} @ ${3}", order.Direction, s.Symbol, order.Size, order.OrderPrice),
                ModifiedBy = order.UpdatedBy,
                ModifiedDate = DateTime.Now,
                Price = order.OrderPrice,
                Size = order.Size,
                Fee = order.Fee,
                TradingDate = ind.TradingDate,
                TradeOrderId = order.Id
            };

            this.Create(tr);
            return tr;
        }


        public List<OutTransaction> GetTransactionList(int accountId, int? take)
        {
            List<OutTransaction> tList = new List<OutTransaction>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetTransactionList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("AccountId", SqlDbType.Int).Value = accountId;

                        if (take.HasValue)
                        {
                            cmd.Parameters.Add("Take", SqlDbType.Int).Value = take.Value;
                        }
                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            OutTransaction t;
                            while (reader.Read())
                            {
                                t = ConvertFromDB(reader);
                                tList.Add(t);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error read transaction from DB. ", ex);
                throw;
            }

            return tList;
        }

        public OutTransaction ConvertFromDB(SqlDataReader reader)
        {
            OutTransaction t = new OutTransaction();

            t.Id = reader.GetInt32(reader.GetOrdinal("Id"));

            if (!reader.IsDBNull(reader.GetOrdinal("Direction")))
                t.Direction = reader.GetString(reader.GetOrdinal("Direction"));

            if (!reader.IsDBNull(reader.GetOrdinal("Price")))
                t.Price = reader.GetDouble(reader.GetOrdinal("Price"));

            if (!reader.IsDBNull(reader.GetOrdinal("Size")))
                t.Size = reader.GetInt32(reader.GetOrdinal("Size"));

            if (!reader.IsDBNull(reader.GetOrdinal("Message")))
                t.Message = reader.GetString(reader.GetOrdinal("Message"));

            if (!reader.IsDBNull(reader.GetOrdinal("Fee")))
                t.Fee = reader.GetDouble(reader.GetOrdinal("Fee"));

            if (!reader.IsDBNull(reader.GetOrdinal("TradingDate")))
                t.TradingDate = reader.GetInt32(reader.GetOrdinal("TradingDate"));

            if (!reader.IsDBNull(reader.GetOrdinal("ModifiedBy")))
                t.ModifiedBy = reader.GetString(reader.GetOrdinal("ModifiedBy"));

            if (!reader.IsDBNull(reader.GetOrdinal("ModifiedDate")))
                t.CreateDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"));

            if (!reader.IsDBNull(reader.GetOrdinal("ShareId")))
                t.ShareId = reader.GetInt32(reader.GetOrdinal("ShareId"));

            if (!reader.IsDBNull(reader.GetOrdinal("Symbol")))
                t.Symbol = reader.GetString(reader.GetOrdinal("Symbol"));

            if (!reader.IsDBNull(reader.GetOrdinal("OrderId")))
                t.OrderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
            
            t.TotalValue = t.Price * Math.Abs(t.Size);
            return t;
        }

        #endregion

    }
}
