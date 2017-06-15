using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class TradeSet :BaseEntity
    {
        public DateTime CreatedDT { get; set; }
        public int CreatedTradingDate { get; set; }
        public DateTime? ClosedDT { get; set; }
        public int? ClosedTradingDate { get; set; }
        public string Status { get; set; }
    }
}
