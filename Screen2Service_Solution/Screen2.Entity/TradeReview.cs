using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Screen2.Entity
{
    public class TradeReview : BaseEntity
    {
        public int? EntryTiming { get; set; }
        public int? TrendRating { get; set; }
        public bool? IsDirectionCorrect { get; set; }
        public int? ProfitRating { get; set; }
        public bool? IsReverse { get; set; }
        public bool? IsAddSize { get; set; }
        public bool? IsStopTriggered { get; set; }
        public bool? IsLimitTriggered { get; set; }
        public int? ExitTiming { get; set; }
        public int? Volatility { get; set; }
        public bool? IsExitBBTriggered { get; set; }
        public int? OverallRating { get; set; }
        public string Notes { get; set; }

        public bool? IsEntryLong { get; set; }
        public double? BBEntryPercent { get; set; }
        public double? BBExitPercent { get; set; }
        public double? EntryPercent { get; set; }
        public double? ExitPercent { get; set; }
        public int? DaysSpan { get; set; }
        public double? Diff { get; set; }
        public double? Diff_Per { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime UpdatedDT { get; set; }

        [Required]
        [Index]
        public int TradePositionId { get; set; }
        [XmlIgnore]
        public virtual TradePosition TradePosition { get; set; }
    }
}
