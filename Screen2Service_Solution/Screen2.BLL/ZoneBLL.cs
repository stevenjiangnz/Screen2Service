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
    public class ZoneBLL : BaseBLL<Zone>, IBaseBLL<Zone>
    {
        #region Constructors
        public ZoneBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public Zone SetZoneNextDay(int zoneID)
        {
            Zone dayZone = null;
            ZoneBLL bll = new ZoneBLL(_unit);
            TradeManager tm = new TradeManager(_unit);

            dayZone = bll.GetByID(zoneID);

            dayZone.TradingDate = (int)(new IndicatorBLL(_unit)).GetNextTradingDate(dayZone.TradingDate);

            bll.Update(dayZone);

            
            return dayZone;
        }
        #endregion
    }
}
