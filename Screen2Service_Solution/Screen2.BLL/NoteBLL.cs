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
    public class NoteBLL : BaseBLL<Note>, IBaseBLL<Note>
    {

        #region Constructors
        public NoteBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public List<Note> GetNoteListByZone(int? shareId, int? zoneId, string user)
        {
            List<Note> noteList = new List<Note>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetNoteByShareZone", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (shareId.HasValue)
                        {
                            cmd.Parameters.Add("ShareID", SqlDbType.Int).Value = shareId.Value;
                        }

                        if (zoneId.HasValue)
                        {
                            cmd.Parameters.Add("ZoneID", SqlDbType.Int).Value = zoneId.Value;
                        }

                        cmd.Parameters.Add("UserID", SqlDbType.VarChar).Value = user;

                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            Note n;
                            while (reader.Read())
                            {
                                n = new Note();

                                this.PopulateObjFromReader(reader, n);

                                noteList.Add(n);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error read note by zone to DB. ", ex);
                throw;
            }

            return noteList;
        }

        public List<Note> GetNotesByShareAndUser(int? shareId, int? tradingDate, string user)
        {
            List<Note> noteList = new List<Note>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetNoteByShareDate", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if(shareId.HasValue)
                        {
                            cmd.Parameters.Add("ShareID", SqlDbType.Int).Value = shareId.Value;
                        }

                        if (tradingDate.HasValue)
                        {
                            cmd.Parameters.Add("TradingDate", SqlDbType.Int).Value = tradingDate.Value;
                        }

                        cmd.Parameters.Add("UserID", SqlDbType.VarChar).Value = user;

                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            Note n;
                            while (reader.Read())
                            {
                                n = new Note();

                                this.PopulateObjFromReader(reader, n);

                                noteList.Add(n);
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



            return noteList;
        }


        private void PopulateObjFromReader(SqlDataReader reader, Note n)
        {
            n.Id = (int)reader["ID"];
            n.Comment = reader.GetString(reader.GetOrdinal("Comment"));
            n.Type = reader.GetString(reader.GetOrdinal("Type"));
            if (!reader.IsDBNull(reader.GetOrdinal("ShareId")))
            {
                n.ShareId = (int)reader["ShareId"];
            }

            if (!reader.IsDBNull(reader.GetOrdinal("TradingDate")))
            {
                n.TradingDate = (int)reader["TradingDate"];
            }
            n.CreatedBy = reader.GetString(reader.GetOrdinal("CreatedBy"));
            n.Create = reader.GetDateTime(reader.GetOrdinal("Create"));
            n.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
            n.LastName = reader.GetString(reader.GetOrdinal("LastName"));
            n.UserName = reader.GetString(reader.GetOrdinal("UserName"));


        }
        #endregion
    }
}
