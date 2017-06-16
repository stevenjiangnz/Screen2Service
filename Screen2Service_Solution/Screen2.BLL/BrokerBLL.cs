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
    public class BrokerBLL : BaseBLL<Broker>, IBaseBLL<Broker>
    {

        #region Constructors
        public BrokerBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        #endregion
    }
}
