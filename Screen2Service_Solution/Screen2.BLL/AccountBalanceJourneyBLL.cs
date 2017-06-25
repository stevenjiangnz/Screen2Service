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
    public class AccountBalanceJourneyBLL : BaseBLL<AccountBalanceJourney>, IBaseBLL<AccountBalanceJourney>
    {
        #region Constructors
        public AccountBalanceJourneyBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public void InitCreate(AccountBalance ab)
        {
            AccountBalanceJourney abj = new AccountBalanceJourney
            {
                AvailableFund = 0,
                Margin = 0,
                PositionValue = 0,
                TotalBalance = 0,
                Action = "AccountInit",
                AccountId = ab.AccountId,
                UpdateDT = DateTime.Now
            };

            AccountSummary aSummary = new AccountBLL(_unit).GetAccountSummary(ab.AccountId);

            if (aSummary != null)
            {
                string aSummaryString = XMLHelper.SerializeObject(aSummary);

                abj.AccountSummaryXML = aSummaryString;
            }

            base.Create(abj);
        }

        public void AddJourney(AccountBalance accB, string action)
        {
            AddJourneyOrder(accB, action, null);
        }

        public void AddJourneyOrder(AccountBalance accB, string action, TradeOrder order)
        {
            AccountSummary aSummary = new AccountBLL(_unit).GetAccountSummary(accB.AccountId);

            AccountBalanceJourney abj = new AccountBalanceJourney
            {
                FundAmount = aSummary.Balance.FundAmount,
                AvailableFund = aSummary.Balance.AvailableFund,
                Margin = aSummary.PositionMarginSum,
                Reserve = aSummary.OrderReserveSum,
                FeeSum = aSummary.Balance.FeeSum,
                PositionValue = aSummary.PositionValueSum,
                TotalBalance = aSummary.Balance.TotalBalance,
                Action = action,
                AccountId = accB.AccountId,
                UpdateDT = DateTime.Now
            };

            if(order != null)
            {
                abj.OrderId = order.Id;
                abj.TradingDate = order.TradingOrderDate;
            }


            if (aSummary != null)
            {
                string aSummaryString = XMLHelper.SerializeObject(aSummary);

                abj.AccountSummaryXML = aSummaryString;
            }

            base.Create(abj);
        }

        public void AddJourneyTransaction(AccountBalance accB, string action, Transaction tr)
        {
            AccountBLL abll = new AccountBLL(_unit);
            AccountSummary aSummary = abll.GetAccountSummary(accB.AccountId);

            AccountBalanceJourney abj = new AccountBalanceJourney
            {
                FundAmount = aSummary.Balance.FundAmount,
                AvailableFund = aSummary.Balance.AvailableFund,
                Margin = aSummary.PositionMarginSum,
                Reserve = aSummary.OrderReserveSum,
                FeeSum = aSummary.Balance.FeeSum,
                PositionValue = aSummary.PositionValueSum,
                TotalBalance = aSummary.Balance.TotalBalance,
                Action = action,
                AccountId = accB.AccountId,
                
                UpdateDT = DateTime.Now
            };

            if (tr != null)
            {
                abj.TransactionId = tr.Id;
                abj.TradingDate = tr.TradingDate;
            }

            

            if (aSummary != null)
            {
                string aSummaryString = XMLHelper.SerializeObject(aSummary);

                abj.AccountSummaryXML = aSummaryString;
            }

            base.Create(abj);
        }


        public List<AccountBalanceJourney> GetListByAccount(int accountId, int? size)
        {
            List<AccountBalanceJourney> abList = new List<AccountBalanceJourney>();

            AccountBalanceJourney t;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetAccountBalanceJourneyByAccount", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
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
                            while (reader.Read())
                            {
                                t = ConvertFromDB(reader);

                                abList.Add(t);
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

            return abList;
        }


        private AccountBalanceJourney ConvertFromDB(SqlDataReader reader)
        {
            AccountBalanceJourney j = new AccountBalanceJourney();

            if (reader != null)
            {
                j.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                j.AccountId = reader.GetInt32(reader.GetOrdinal("AccountId"));

                if (!reader.IsDBNull(reader.GetOrdinal("FundAmount")))
                    j.FundAmount = reader.GetDouble(reader.GetOrdinal("FundAmount"));

                if (!reader.IsDBNull(reader.GetOrdinal("TotalBalance")))
                    j.TotalBalance = reader.GetDouble(reader.GetOrdinal("TotalBalance"));

                if (!reader.IsDBNull(reader.GetOrdinal("AvailableFund")))
                    j.AvailableFund = reader.GetDouble(reader.GetOrdinal("AvailableFund"));

                if (!reader.IsDBNull(reader.GetOrdinal("Margin")))
                    j.Margin = reader.GetDouble(reader.GetOrdinal("Margin"));

                if (!reader.IsDBNull(reader.GetOrdinal("PositionValue")))
                    j.PositionValue = reader.GetDouble(reader.GetOrdinal("PositionValue"));

                if (!reader.IsDBNull(reader.GetOrdinal("Reserve")))
                    j.Reserve = reader.GetDouble(reader.GetOrdinal("Reserve"));

                if (!reader.IsDBNull(reader.GetOrdinal("FeeSum")))
                    j.FeeSum = reader.GetDouble(reader.GetOrdinal("FeeSum"));

                if (!reader.IsDBNull(reader.GetOrdinal("TradingDate")))
                    j.TradingDate = reader.GetInt32(reader.GetOrdinal("TradingDate"));

                if (!reader.IsDBNull(reader.GetOrdinal("TradeSetId")))
                    j.TradeSetId = reader.GetInt32(reader.GetOrdinal("TradeSetId"));

                if (!reader.IsDBNull(reader.GetOrdinal("UpdateDT")))
                    j.UpdateDT = reader.GetDateTime(reader.GetOrdinal("UpdateDT"));

                if (!reader.IsDBNull(reader.GetOrdinal("Action")))
                    j.Action = reader.GetString(reader.GetOrdinal("Action"));

                if (!reader.IsDBNull(reader.GetOrdinal("TransactionId")))
                    j.TransactionId = reader.GetInt32(reader.GetOrdinal("TransactionId"));

                if (!reader.IsDBNull(reader.GetOrdinal("OrderId")))
                    j.OrderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
            }

            return j;

        }
        #endregion
    }
}
