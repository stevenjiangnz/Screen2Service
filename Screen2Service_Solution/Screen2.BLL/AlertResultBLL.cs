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
    public class AlertResultBLL : BaseBLL<AlertResult>, IBaseBLL<AlertResult>
    {
        #region Constructors
        public AlertResultBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public void AddAlertResultBatch(List<AlertResult> arList)
        {
            var xmlInput = XMLHelper.SerializeObject<List<Screen2.Entity.AlertResult>>(arList);

            string cmnlString = XMLHelper.SerializeObject(xmlInput);
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_InsertAlertResultBatch", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("AlertResultsXML", SqlDbType.Xml).Value = xmlInput;
                        cmd.Connection.Open();

                        var result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error save alert results to DB. ", ex);
                throw;
            }

        }


        public void DeleteAlertResultByAlert(int tradingDate, int? zoneId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_DeleteAlertResultByAlert", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("TradingDate", SqlDbType.Int).Value = tradingDate;
                        if (zoneId.HasValue)
                            cmd.Parameters.Add("ZoneId", SqlDbType.Int).Value = zoneId.Value;
                        cmd.Connection.Open();

                        var reader = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error delete alert result to DB. ", ex);
                throw;
            }
        }

        #endregion
    }
}