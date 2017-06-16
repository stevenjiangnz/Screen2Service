using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class WatchListBLL : BaseBLL<WatchList>, IBaseBLL<WatchList>
    {
        #region Properties
        #endregion
        #region Constructors
        public WatchListBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public void AddShareToWatchList(int watchId, string userId, string shareListString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_AddSharesToWatchList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("WatchID", SqlDbType.Int).Value = watchId;
                        cmd.Parameters.Add("UserID", SqlDbType.VarChar).Value = userId;
                        cmd.Parameters.Add("ShareListString", SqlDbType.VarChar).Value = shareListString;

                        cmd.Connection.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error AddShareToWatchList to DB. ", ex);
                throw;
            }
        }



        public void RemoveShareFromWatchList(int watchId, string userId, string shareListString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_RemoveSharesFromWatchList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("WatchID", SqlDbType.Int).Value = watchId;
                        cmd.Parameters.Add("UserID", SqlDbType.VarChar).Value = userId;
                        cmd.Parameters.Add("ShareListString", SqlDbType.VarChar).Value = shareListString;

                        cmd.Connection.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error RemoveShareFromWatchList to DB. ", ex);
                throw;
            }
        }

        public List<int> GetWatchListByShare(int shareId, string userId)
        {
            List<int> watchIDList = new List<int>();

            var watchDetails = from w in _unit.DataContext.WatchListDetail
                               where w.ShareId == shareId &&
                                w.UserId == userId
                               select w;

            List<WatchListDetail> dList = watchDetails.ToList();

            foreach (var d in dList)
            {
                watchIDList.Add(d.WatchListId);
            }

            return watchIDList;
        }

        public List<WatchListDetail> GetWatchListDetail(int watchId, string userId)
        {
            List<WatchListDetail> watchListDetail = null;

            var query = from w in _unit.DataContext.WatchListDetail
                        where w.WatchListId == watchId &&
                               w.UserId == userId
                        select w;

            watchListDetail = query.ToList();

            return watchListDetail;
        }


        public List<WatchList> GetWatchListByZone(string userId, int? zoneId)
        {
            List<WatchList> wList = this.GetWatchList(userId);

            if(zoneId.HasValue)
            {
                wList = wList.Where(c => c.ZoneId == zoneId.Value).OrderBy(c=>c.DisplayOrder).ToList();
            }
            else
            {
                wList = wList.Where(c => c.ZoneId == null).OrderBy(c => c.DisplayOrder).ToList();
            }

            return wList;
        }


        public List<WatchList> GetWatchList(string userId)
        {
            List<WatchList> wList = new List<WatchList>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetWatchList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("UserID", SqlDbType.VarChar).Value = userId;
                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            WatchList w;
                            while (reader.Read())
                            {
                                w = new WatchList();
                                w.Id =(int) reader["ID"];
                                w.Name = (string)reader["Name"];
                                w.Description = (string)reader["Description"];
                                w.Owner = (string)reader["Owner"];
                                w.IsSystem =(bool)reader["IsSystem"];
                                if (!reader.IsDBNull(reader.GetOrdinal("Status")))
                                {
                                    w.Status = (string)reader["Status"];
                                }
                                

                                if (!reader.IsDBNull(reader.GetOrdinal("MemberCount")))
                                {
                                    w.MemberCount = (int)reader["MemberCount"];
                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("DisplayOrder")))
                                {
                                    w.DisplayOrder = (int)reader["DisplayOrder"];
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("ZoneId")))
                                {
                                    w.ZoneId = (int)reader["ZoneId"];
                                }

                                wList.Add(w);
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
            return wList;
        }
        #endregion
    }
}
