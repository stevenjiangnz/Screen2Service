using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
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
    public class ShareBLL : BaseBLL<Share>, IBaseBLL<Share>
    {
        #region Constructors
        public ShareBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the share by y symbol.
        /// </summary>
        /// <param name="symbol">The y symbol.</param>
        /// <returns></returns>
        public Share GetShareBySymbol(string symbol)
        {
            Share returnShare = null;

            // make sure this qury is lower case
            returnShare = base.Get(p => p.Symbol == symbol).FirstOrDefault();

            return returnShare;
        }

        public List<Share> GetShareListByZone(int zoneId)
        {
            List<Share> sList = new List<Share>();

            Zone z = new ZoneBLL(_unit).GetByID(zoneId);

            sList = this.GetShareListByTradingDate(z.TradingDate);

            return sList;
        }

        public List<Share> GetShareListByTradingDate(int tradingDate)
        {
            List<Share> sList = new List<Share>();
            Share t;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetShareListByTradingDate", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("TradingDate", SqlDbType.Int).Value = tradingDate;

                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                t = new Share();

                                this.PopulateObjFromReader(reader, t);

                                sList.Add(t);
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

            
            return sList;
        }

        public List<Share> GetShareListByWatch(int watchId, bool isReverse)
        {
            List<Share> sList = new List<Share>();
            Share t;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetShareByWatch", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("WatchId", SqlDbType.Int).Value = watchId;

                        if(isReverse)
                        {
                            cmd.Parameters.Add("Reverse", SqlDbType.Bit).Value = 1;
                        }

                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                t = new Share();

                                this.PopulateObjFromReader(reader, t);

                                sList.Add(t);
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

            return sList;
        }

        public void PopulateObjFromReader(SqlDataReader reader, Share t)
        {
            t.Id = reader.GetInt32(reader.GetOrdinal("ID"));

            if (t.Id == 2459)
                Debug.WriteLine(t.Id);

            if (!reader.IsDBNull(reader.GetOrdinal("Symbol")))
                t.Symbol = reader.GetString(reader.GetOrdinal("Symbol"));

            if (!reader.IsDBNull(reader.GetOrdinal("Name")))
                t.Name = reader.GetString(reader.GetOrdinal("Name"));

            if (!reader.IsDBNull(reader.GetOrdinal("Industry")))
                t.Industry = reader.GetString(reader.GetOrdinal("Industry"));
            t.IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
            t.IsYahooVerified = reader.GetBoolean(reader.GetOrdinal("IsYahooVerified"));

            if (!reader.IsDBNull(reader.GetOrdinal("LastProcessed")))
                t.LastProcessed = reader.GetDateTime(reader.GetOrdinal("LastProcessed"));

            if (!reader.IsDBNull(reader.GetOrdinal("ProcessComment")))
                t.ProcessComment = reader.GetString(reader.GetOrdinal("ProcessComment"));

            if (!reader.IsDBNull(reader.GetOrdinal("ShareType")))
                t.ShareType = reader.GetString(reader.GetOrdinal("ShareType"));

            if (!reader.IsDBNull(reader.GetOrdinal("MarketId")))
                t.MarketId = reader.GetInt32(reader.GetOrdinal("MarketId"));

            if (!reader.IsDBNull(reader.GetOrdinal("Sector")))
                t.Sector = reader.GetString(reader.GetOrdinal("Sector"));
            t.IsCFD = reader.GetBoolean(reader.GetOrdinal("IsCFD"));


        }


        public Share SetShareType (Share s, string type)
        {
            Share result = null;
            result = GetByID(s.Id);
            result.ShareType = type;
            result.LastProcessed = DateTime.Now;

            Update(result);

            return result;
        }

        public Share UpdateShare(Share input)
        {
            Share output = null;

            output = GetByID(input.Id);

            output.Industry = input.Industry;
            output.IsActive = input.IsActive;
            output.IsYahooVerified = input.IsYahooVerified;
            output.LastProcessed = DateTime.Now;
            output.MarketId = input.MarketId;
            output.Name = input.Name;
            output.ProcessComment = input.ProcessComment;
            output.ShareType = input.ShareType;
            output.Symbol = input.Symbol;

            Update(output);

            return output;
        }

        
        
        #endregion
    }
}
