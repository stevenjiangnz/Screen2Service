using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Shared;
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
    public class AccountBLL : BaseBLL<Account>, IBaseBLL<Account>
    {

        #region Constructors
        public AccountBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public List<Account> GetAccountListByUser(string userId, int? zoneId)
        {
            List<Account> aList = new List<Account>();

            aList = _unit.DataContext.Account
                .Where(p => p.Owner == userId && p.ZoneId == zoneId).ToList();

            return aList;
        }


        public List<OutAccount> GetAccountFullList()
        {
            List<OutAccount> aList = new List<OutAccount>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetAccountFull", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            OutAccount oa;
                            while (reader.Read())
                            {
                                oa = ConvertFromDB(reader);
                                aList.Add(oa);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error read account full from DB. ", ex);
                throw;
            }

            return aList;
        }


        public List<OutAccount> GetAccountFullListByUser(string userId, int? zoneId)
        {
            List<OutAccount> aList = new List<OutAccount>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetAccountFullByZone", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("Owner", SqlDbType.VarChar).Value = userId;

                        if(zoneId.HasValue)
                        {
                            cmd.Parameters.Add("ZoneId", SqlDbType.Int).Value = zoneId.Value;
                        }
                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            OutAccount oa;
                            while (reader.Read())
                            {
                                oa = ConvertFromDB(reader);
                                aList.Add(oa);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error read account full from DB. ", ex);
                throw;
            }

            return aList;
        }


        public OutAccount TransferFund(int accountId, string operation, double amount)
        {
            OutAccount outAcc = null;
            Transaction ts = null;
            AccountBalanceBLL abBll = new AccountBalanceBLL(_unit);
            TransactionBLL tBLL = new TransactionBLL(_unit);
            
            Account acc = GetByID(accountId);

            AccountBalance existAB = abBll.TransferFund(accountId, operation, amount);

            outAcc = GetOutAccountFromObjs(acc, existAB);

            return outAcc;
        }

        public OutAccount Create(OutAccount outAccount)
        {
            AccountBalanceBLL abb = new AccountBalanceBLL(_unit);
            Account acc = this.GetAccountFromOut(outAccount);
            base.Create(acc);

            var balance = abb.InitCreate(acc.Id);

            if(outAccount.AvailableFund > 0)
            {
                balance = abb.TransferFund(acc.Id, "Deposit", outAccount.AvailableFund);
            }

            outAccount = GetOutAccountFromObjs(acc, balance);

            return outAccount;
        }

        public void DeleteFull(int accountId)
        {
            base.Delete(accountId);

            new TransactionBLL(_unit).DeleteByAccount(accountId);
        }




        public AccountSummary GetAccountSummary(int accountId)
        {
            AccountBLL aBLL = new AccountBLL(_unit);
            AccountBalanceBLL abBll = new AccountBalanceBLL(_unit);
            TradeOrderBLL orderBLL = new TradeOrderBLL(_unit);
            TradePositionBLL positionBLL = new TradePositionBLL(_unit);

            AccountSummary aSummary = new AccountSummary();

            aSummary.Account = aBLL.GetByID(accountId);
            aSummary.Balance = abBll.GetAccountBalanceByAccount(accountId);
            aSummary.OpenOrders = orderBLL.GetListByAccountStatus(accountId, "open");
            aSummary.CurrentPositions = positionBLL.GetOutstandingPositions(accountId);

            aSummary.Balance.Margin = aSummary.PositionMarginSum;
            aSummary.Balance.Reserve = aSummary.OrderReserveSum;
            aSummary.Balance.PositionValue = aSummary.PositionValueSum;

            return aSummary;
        }


        public Account GetAccountFromOut(OutAccount outAccount)
        {
            Account acc = null;

            if(outAccount != null)
            {
                acc = new Account
                {
                    Id = outAccount.Id,
                    Name = outAccount.Name,
                    Description = outAccount.Description,
                    Status = outAccount.Status,
                    CreatedBy = outAccount.CreatedBy,
                    CreateDate = outAccount.CreateDate,
                    Owner = outAccount.Owner,
                    BrokerId = outAccount.BrokerId,
                    ZoneId = outAccount.ZoneId
               };

                if (outAccount.IsTrackingAccount.HasValue)
                    acc.IsTrackingAccount = outAccount.IsTrackingAccount.Value;
            }

            return acc;
        }


        public OutAccount GetOutAccountFromObjs(Account account, AccountBalance balance)
        {
            OutAccount outAcc = null;

            if(account != null)
            {
                outAcc = new OutAccount
                {
                    Name = account.Name,
                    Description = account.Description,
                    Owner = account.Owner,
                    Id = account.Id,
                    Status = account.Status,
                    CreateDate = account.CreateDate,
                    CreatedBy =account.CreatedBy,
                    ZoneId = account.ZoneId,
                    BrokerId = account.BrokerId
                };

                if(balance != null)
                {
                    outAcc.TotalBalance = balance.TotalBalance;
                    outAcc.FundAmount = balance.FundAmount;
                    outAcc.AvailableFund = balance.AvailableFund;
                    outAcc.Reserve = balance.Reserve;
                    outAcc.Margin = balance.Margin;
                    outAcc.FeeSum = balance.FeeSum;
                    outAcc.PositionValue = balance.PositionValue;
                    outAcc.TradingDate = balance.TradingDate;
                    outAcc.BalanceUpdateDT = balance.UpdateDT;
                    outAcc.AccountId = balance.AccountId;
                }
            }

            return outAcc;
        }


        public OutAccount ConvertFromDB(SqlDataReader reader)
        {
            OutAccount j = null;

            if (reader != null)
            {
                j = new OutAccount();

                j.Id = reader.GetInt32(reader.GetOrdinal("ID"));

                if (!reader.IsDBNull(reader.GetOrdinal("BrokerId")))
                    j.BrokerId = reader.GetInt32(reader.GetOrdinal("BrokerId"));

                if (!reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                    j.CreateDate = reader.GetDateTime(reader.GetOrdinal("CreateDate"));

                if (!reader.IsDBNull(reader.GetOrdinal("CreatedBy")))
                    j.CreatedBy = reader.GetString(reader.GetOrdinal("CreatedBy"));

                if (!reader.IsDBNull(reader.GetOrdinal("Description")))
                    j.Description = reader.GetString(reader.GetOrdinal("Description"));
                 
                if (!reader.IsDBNull(reader.GetOrdinal("Name")))
                    j.Name = reader.GetString(reader.GetOrdinal("Name"));

                if (!reader.IsDBNull(reader.GetOrdinal("Owner")))
                    j.Owner = reader.GetString(reader.GetOrdinal("Owner"));

                if (!reader.IsDBNull(reader.GetOrdinal("Status")))
                    j.Status = reader.GetString(reader.GetOrdinal("Status"));

                if (!reader.IsDBNull(reader.GetOrdinal("ZoneId")))
                    j.ZoneId = reader.GetInt32(reader.GetOrdinal("ZoneId"));

                if (!reader.IsDBNull(reader.GetOrdinal("IsTrackingAccount")))
                    j.IsTrackingAccount = reader.GetBoolean(reader.GetOrdinal("IsTrackingAccount"));

                if (!reader.IsDBNull(reader.GetOrdinal("AccountId")))
                {
                    j.AccountId = reader.GetInt32(reader.GetOrdinal("AccountId"));

                    AccountSummary asummary = this.GetAccountSummary(j.AccountId.Value);

                    if (!reader.IsDBNull(reader.GetOrdinal("AvailableFund")))
                        j.AvailableFund = reader.GetDouble(reader.GetOrdinal("AvailableFund"));

                    if (!reader.IsDBNull(reader.GetOrdinal("FundAmount")))
                        j.FundAmount = reader.GetDouble(reader.GetOrdinal("FundAmount"));

                    if (!reader.IsDBNull(reader.GetOrdinal("FeeSum")))
                        j.FeeSum = reader.GetDouble(reader.GetOrdinal("FeeSum"));

                    if (!reader.IsDBNull(reader.GetOrdinal("AccountBalanceId")))
                        j.AccountBalanceId = reader.GetInt32(reader.GetOrdinal("AccountBalanceId"));

                    j.Reserve = asummary.OrderReserveSum;

                    j.Margin = asummary.PositionMarginSum;

                    j.PositionValue = asummary.PositionValueSum;

                    j.TotalBalance = asummary.Balance.TotalBalance;

                    if (!reader.IsDBNull(reader.GetOrdinal("TradingDate")))
                        j.TradingDate = reader.GetInt32(reader.GetOrdinal("TradingDate"));


                    if (!reader.IsDBNull(reader.GetOrdinal("UpdateDT")))
                        j.BalanceUpdateDT = reader.GetDateTime(reader.GetOrdinal("UpdateDT"));
                }
            }

            return j;
        }

        #endregion
    }
}
