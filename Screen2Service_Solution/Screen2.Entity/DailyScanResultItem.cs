using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class DailyScanResultItem : BaseEntity
    {
        [Index]
        public int TradingDate { get; set; }
        public double EntryPrice { get; set; }
        public bool IsMatch { get; set; }
        public string SetRef { get; set; }
        public DateTime ProcessDT { get; set; }

        [Required]
        [Index]
        public int ShareId { get; set; }
    }
}
