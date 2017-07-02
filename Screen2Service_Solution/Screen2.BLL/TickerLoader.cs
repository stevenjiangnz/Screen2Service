using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screen2.BLL.Interface;

namespace Screen2.BLL
{
    public class TickerLoader : ITickerLoader
    {
        public bool LoadTickers()
        {
            return this.UploadAsxXEodRaw();
        }

        public virtual bool UploadAsxXEodRaw()
        {
            throw new NotImplementedException();
        }
    }
}
