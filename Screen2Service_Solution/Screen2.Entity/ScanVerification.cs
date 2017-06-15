using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class ScanVerification
    {
        public double? Day1Diff { get; set; }
        public double? Day1Low { get; set; }
        public double? Day1High { get; set; }
        public double? Day2Diff { get; set; }
        public double? Day2Low { get; set; }
        public double? Day2High { get; set; }
        public double? Day3Diff { get; set; }
        public double? Day3Low { get; set; }
        public double? Day3High { get; set; }
        public double? Day5Diff { get; set; }
        public double? Day5Low { get; set; }
        public double? Day5High { get; set; }
        public double? Day10Diff { get; set; }
        public double? Day10Low { get; set; }
        public double? Day10High { get; set; }
        public double? Day20Diff { get; set; }
        public double? Day20Low { get; set; }
        public double? Day20High { get; set; }
        public bool HasProfitted { get; set; }
        public double StopLine { get; set; }
        public int? MA5PeakDay { get; set; }
        public int? MA10PeakDay { get; set; }
        public int? MA5BottomDay { get; set; }
        public int? MA10BottomDay { get; set; }
        public int? ProfitDay { get; set; }
        public int? StopDay { get; set; }
        public double? MaxGain { get; set; }
        public int? MaxGainDay { get; set; }
        public double? MaxLoss { get; set; }
        public int? MaxLossDay { get; set; }
    }
}
