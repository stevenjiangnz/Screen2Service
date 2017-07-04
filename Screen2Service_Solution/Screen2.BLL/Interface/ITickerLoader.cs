using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL.Interface
{
    public interface ITickerLoader
    {
        Boolean LoadTickers();
        Boolean UploadAsxEodRaw();
        int? GetLastestRawTickerDate();
    }
}
