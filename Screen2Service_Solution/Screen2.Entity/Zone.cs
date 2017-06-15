using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class Zone :BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public DateTime StartDate { get; set; }
        public int TradingDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
    }
}
