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
    public class ScanBLL : BaseBLL<Scan>, IBaseBLL<Scan>
    {
        #region Constructors
        public ScanBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public List<Scan> GetListByCurrentUser(string userID)
        {
            List<Scan> rlist = _unit.DataContext.Scan.Where(c => c.Owner == userID).ToList();

            return rlist;
        }



        #endregion

    }
}
