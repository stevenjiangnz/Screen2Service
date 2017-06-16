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
    public class DailyScanResultBLL : BaseBLL<DailyScanResult>, IBaseBLL<DailyScanResult>
    {

        #region Constructors
        public DailyScanResultBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public List<DailyScanResult> GetDailyScanResult(int tradingDate, int dailyScanId)
        {
            List<DailyScanResult> dsList = new List<DailyScanResult>();

            dsList = _unit.DataContext.DailyScanResult.Where(d => d.DailyScanId == dailyScanId &&
                        d.TradingDate == tradingDate).ToList();

            return dsList;
        }

        public void DeleteDailyScanResultByDailyScan(int dailyScanId, int tradingDate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_DeleteDailyScanResultByDailyScan", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("DailyScanId", SqlDbType.Int).Value = dailyScanId;
                        cmd.Parameters.Add("TradingDate", SqlDbType.Int).Value = tradingDate;
                        cmd.Connection.Open();

                        var reader = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error delete daily scan result to DB. ", ex);
                throw;
            }
        }

        public void AddDailyScanResultBatch(List<DailyScanResult> dsrList)
        {
            var xmlInput = XMLHelper.SerializeObject<List<Screen2.Entity.DailyScanResult>>(dsrList);

            string cmnlString = XMLHelper.SerializeObject(xmlInput);
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_InsertDailyScanResultBatch", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("DailyScanResultsXML", SqlDbType.Xml).Value = xmlInput;
                        cmd.Connection.Open();

                        var result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error save daily scan results to DB. ", ex);
                throw;
            }

        }

        #endregion
    }
}
