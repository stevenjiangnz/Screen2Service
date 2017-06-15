using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class TradeSimulateOrder : BaseEntity
    {
        public int ShareId { get; set; }
        public int EntryTradingDate {get;set;}
        public double EntryPrice { get; set; }
        public int? ExitTradingDate { get; set; }
        public double? ExitPrice { get; set; }
        public int Days { get; set; }
        public int Flag { get; set; }
        public string ExitMode { get; set; }
        public double? Diff { get; set; }
        public double? Diff_Per { get; set; }
        public int? Day5AboveDays { get; set; }
        public double? Day5Highest { get; set; }
        public double? Day5Lowest { get; set; }
        public DateTime UpdatedDT { get; set; }

        [Index]
        public int StatScanSetId { get; set; }
        public virtual StatScanSet StatScanSet { get; set; }

    }
}
