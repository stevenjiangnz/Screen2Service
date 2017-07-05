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
        public void SaveAsxEodToDB(List<Ticker> tickers)
        {
            var xmlInput = XMLHelper.SerializeObject<List<Ticker>>(tickers);
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
        #endregion region
    }
}
