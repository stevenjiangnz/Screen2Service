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
    public class PlanBLL : BaseBLL<Plan>, IBaseBLL<Plan>
    {

        #region Constructors
        public PlanBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public List<Plan> GetListByZone(string owner, int? zoneId)
        {
            List<Plan> jList = new List<Plan>();

            Plan t;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetPlanListByZone", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("Owner", SqlDbType.VarChar).Value = owner;

                        if (zoneId.HasValue)
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

        public Plan ConvertFromDB(SqlDataReader reader)
        {
            Plan j = new Plan();

            if (reader != null)
            {
                new Plan();

                j.Id = reader.GetInt32(reader.GetOrdinal("ID"));

                if (!reader.IsDBNull(reader.GetOrdinal("TradingDate")))
                    j.TradingDate = reader.GetInt32(reader.GetOrdinal("TradingDate"));

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
