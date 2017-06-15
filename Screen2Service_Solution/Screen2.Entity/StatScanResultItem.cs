using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class StatScanResultItem
    {
        public int StatScanSetId { get; set; }
        public int Flag { get; set; }

        public string SetRef { get; set; }

        public Entity.Indicator EntryIndicator { get; set; }
        public double EntryPrice { get; set; }

        public Entity.Indicator StopIndicator { get; set; }
        public double? stopLevel { get; set; }
        public double? StopPrice { get; set; }

        public Entity.Indicator LimitIndicator { get; set; }
        public double? LimitLevel { get; set; }
        public double? LimitPrice { get; set; }

        public int? Day5AboveDays { get; set; }
        public double? Day5Highest { get; set; }
        public double? Day5Lowest { get; set; }
        public string ExitMode { get; set; }

        public double? Diff
        {
            get
            {
                double? diff = null;

                if (StopPrice.HasValue)
                {
                    diff = (StopPrice.Value - EntryPrice) * Flag;
                }
                else if (LimitPrice.HasValue)
                {
                    diff = (LimitPrice.Value - EntryPrice) * Flag;
                }

                return diff;
            }
        }

        public double? Diff_Per
        {
            get
            {
                return 100 * Diff / EntryPrice;
            }
        }

        public int ShareId
        {
            get
            {
                return EntryIndicator.ShareId;
            }
        }
        public int EntryTradingDate
        {
            get
            {
                return EntryIndicator.TradingDate;
            }
        }

        public int? ExitTradingDate
        {
            get
            {
                if(StopIndicator != null)
                {
                    return StopIndicator.TradingDate;
                }
                else if(LimitIndicator != null)
                {
                    return LimitIndicator.TradingDate;
                }

                return null;
            }
        }

        public double? ExitPrice
        {
            get
            {
                if (StopPrice.HasValue)
                {
                    return StopPrice;
                }
                else if (LimitPrice.HasValue)
                {
                    return LimitPrice;
                }

                return null;
            }
        }

    }
}
