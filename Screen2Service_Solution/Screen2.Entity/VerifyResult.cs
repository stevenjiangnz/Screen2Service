using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class VerifyResult
    {
        public int ShareId { get; set; }
        public int TradingDate { get; set; } 
        public bool IsSuccess { get; set; }
        public bool IsMatch { get; set; }
        public string ErrorMessage { get; set; }
    }
}
