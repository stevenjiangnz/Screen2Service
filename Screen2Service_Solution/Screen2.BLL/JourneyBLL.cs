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
    public class JourneyBLL : BaseBLL<Journey>, IBaseBLL<Journey>
    {

        #region Constructors
        public JourneyBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public List<Journey> GetListByZone(string owner, int? zoneId)
        {
            List<Journey> jList = new List<Journey>();

            Journey t;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetJourneyListByZone", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("Owner", SqlDbType.VarChar).Value = owner;

                        if(zoneId.HasValue)
                        {
                            cmd.Parameters.Add("ZoneId", SqlDbType.Int).Value = zoneId.Value;
                        }


                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                t = ConvertFromDB(reader);

                                jList.Add(t);
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


            return jList;
        }

        public Journey ConvertFromDB(SqlDataReader reader)
        {
            Journey j = new Journey();

            if(reader != null)
            {
                new Journey();

                j.Id = reader.GetInt32(reader.GetOrdinal("ID"));

                if (!reader.IsDBNull(reader.GetOrdinal("StartDay")))
                    j.StartDay = reader.GetString(reader.GetOrdinal("StartDay"));

                if (!reader.IsDBNull(reader.GetOrdinal("Status")))
                    j.Status = reader.GetString(reader.GetOrdinal("Status"));

                if (!reader.IsDBNull(reader.GetOrdinal("Owner")))
                    j.Owner = reader.GetString(reader.GetOrdinal("Owner"));

                if (!reader.IsDBNull(reader.GetOrdinal("ZoneId")))
                    j.ZoneId = reader.GetInt32(reader.GetOrdinal("ZoneId"));

                if (!reader.IsDBNull(reader.GetOrdinal("Created")))
                    j.Created = reader.GetDateTime(reader.GetOrdinal("Created"));

                if (!reader.IsDBNull(reader.GetOrdinal("Modified")))
                    j.Modified = reader.GetDateTime(reader.GetOrdinal("Modified"));
            }

            return j;
        }


        #endregion
    }
}
