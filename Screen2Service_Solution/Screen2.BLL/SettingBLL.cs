using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class SettingBLL : BaseBLL<Setting>, IBaseBLL<Setting>
    {
        #region Constructors
        public SettingBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public string GetSetting (SettingKey key){
            var value =this.Get(p => p.Key == key.ToString()).Single().Value;
            return value;
        }
        #endregion
    }
}
