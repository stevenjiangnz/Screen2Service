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
    public class TradeSetBLL : BaseBLL<TradeSet>, IBaseBLL<TradeSet>
    {
      
        #region Constructors
        public TradeSetBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion
    }
}
