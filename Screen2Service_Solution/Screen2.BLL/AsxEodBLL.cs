using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Utils;

namespace Screen2.BLL
{
    public class AsxEodBLL : BaseBLL<AsxEod>, IBaseBLL<AsxEod>
    {

        #region Constructors
        public AsxEodBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Saves the tickers to database.
        /// </summary>
        /// <param name="tickers">The tickers.</param>
        public void SaveAsxEodListToDB(List<AsxEod> tickers)
        {
            var xmlInput = XMLHelper.SerializeObject<List<AsxEod>>(tickers);
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_UploadAsxEod", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("TickersXML", SqlDbType.Xml).Value = xmlInput;
                        cmd.Connection.Open();

                        var result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error save tickers to DB. ", ex);
                throw;
            }
        }


        public Boolean IsAsxEodExisting(int tradingDate)
        {
            Boolean isExist = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetAsxEodByDate", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("TradingDate", SqlDbType.Int).Value = tradingDate;

                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            isExist = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error read latest to DB. ", ex);
                throw;
            }

            return isExist;
        }

        public long GetLatestTradingDate()
        {
            long latestTradingDate = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetLatestAsxTradingDate", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Read();

                            latestTradingDate = reader.GetInt32(reader.GetOrdinal("TradingDate"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error seach stock from DB. ", ex);
                throw;
            }


            return latestTradingDate;
        }


        #endregion region
    }
}
