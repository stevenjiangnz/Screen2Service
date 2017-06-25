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
    public class ETFInfoBLL : BaseBLL<ETFInfo>, IBaseBLL<ETFInfo>
    {
        #region Constructors
        public ETFInfoBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public ETFInfo GetByShareID(int shareID)
        {
            ETFInfo sInfo = null;

            sInfo = Get(p => p.ShareId == shareID).SingleOrDefault();

            return sInfo;
        }
        #endregion
    }
}
