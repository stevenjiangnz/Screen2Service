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
    public class FunctoidBLL : BaseBLL<Functoid>, IBaseBLL<Functoid>
    {
        #region Properties
        private AuditLogBLL _auditBLL;
        #endregion

        #region Constructors
        public FunctoidBLL(IUnitWork unit) : base(unit)
        {
            _auditBLL = new AuditLogBLL(_unit);
        }
        #endregion
    }
}
