using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class StatScanSet : BaseEntity
    {
        public int WatchId { get; set; }
        public int StartDate { get; set; }
        public int EndDate { get; set; }
        public string SetRef { get; set; }
        public string EntryLogic { get; set; }
        public string Notes { get; set; }
        public DateTime StartDt { get; set; }
        public DateTime? CompleteDt { get; set; }

        [NotMapped]
        public int TradeCount { get; set; }
        [NotMapped]
        public int GainCount { get; set; }
        [NotMapped]
        public int LossCount { get; set; }
        [NotMapped]
        public double SumDiffPercent { get; set; }
        [NotMapped]
        public double AvgDiffPercent { get; set; }
        [NotMapped]
        public double AvgDays { get; set; }
        [NotMapped]
        public double AvgExposure { get; set; }

    }
}
