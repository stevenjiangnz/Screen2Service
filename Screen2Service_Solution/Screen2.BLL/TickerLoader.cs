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

            return this.UploadAsxEodRaw();
        }

        public virtual bool UploadAsxEodRaw()
        {

            throw new NotImplementedException();
        }

        public virtual int? GetLastestRawTickerDate()
        {
            return -1;
        }

    }
}
