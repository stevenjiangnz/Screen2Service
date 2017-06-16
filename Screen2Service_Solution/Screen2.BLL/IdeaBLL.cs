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
    public class IdeaBLL : BaseBLL<Idea>, IBaseBLL<Idea>
    {

        #region Constructors
        public IdeaBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public List<Idea> GetList(string owner)
        {
            List<Idea> jList = new List<Idea>();

            Idea t;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetIdeaList", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("Owner", SqlDbType.VarChar).Value = owner;

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

        public Idea ConvertFromDB(SqlDataReader reader)
        {
            Idea j = new Idea();

            if (reader != null)
            {
                new Idea();

                j.Id = reader.GetInt32(reader.GetOrdinal("ID"));

                if (!reader.IsDBNull(reader.GetOrdinal("Topic")))
                    j.Topic = reader.GetString(reader.GetOrdinal("Topic"));

                if (!reader.IsDBNull(reader.GetOrdinal("Status")))
                    j.Status = reader.GetString(reader.GetOrdinal("Status"));

                if (!reader.IsDBNull(reader.GetOrdinal("Type")))
                    j.Type = reader.GetString(reader.GetOrdinal("Type"));

                if (!reader.IsDBNull(reader.GetOrdinal("Owner")))
                    j.Owner = reader.GetString(reader.GetOrdinal("Owner"));

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
