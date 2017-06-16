using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class MarketBLL : BaseBLL<Market>, IBaseBLL<Market>
    {
        #region Constructors
        public MarketBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion
    }
}
