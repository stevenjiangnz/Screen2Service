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
    public class RuleBLL : BaseBLL<Rule>, IBaseBLL<Rule>
    {
        #region Constructors
        public RuleBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public List<Rule> GetListByCurrentUser(string userID)
        {
            List<Rule> rlist = _unit.DataContext.Rule.Where(c => c.Owner == userID).OrderBy(c=>c.Name).ToList();

            return rlist;
        }
        #endregion
    }
}
