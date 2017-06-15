using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class OutStockSearchResult
    {
        public int ShareId { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Industry { get; set; }
        public string Sector { get; set; }
        public string ShareType { get; set; }
        public bool IsCfd { get; set; }
        public bool IsActive { get; set; }
        public int TradingDate { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public long Volumn { get; set; }
        public double? AdjustedClose { get; set; }

        public double? SMA5 { get; set; }
        public double? SMA10 { get; set; }
        public double? SMA30 { get; set; }
        public double? SMA50 { get; set; }
        public double? SMA200 { get; set; }
        public double? EMA10 { get; set; }
        public double? EMA20 { get; set; }
        public double? EMA50 { get; set; }
        public double? BB_Middle { get; set; }
        public double? BB_Low { get; set; }
        public double? BB_High { get; set; }
        public double? ADX { get; set; }
        public double? ADX_Plus { get; set; }
        public double? ADX_Minus { get; set; }
        public double? MACD { get; set; }
        public double? MACD_Signal { get; set; }
        public double? MACD_Hist { get; set; }
        public double? Heikin_Open { get; set; }
        public double? Heikin_Close { get; set; }
        public double? Heikin_Low { get; set; }
        public double? Heikin_High { get; set; }
        public double? Stochastic_K { get; set; }
        public double? Stochastic_D { get; set; }
        public double? RSI { get; set; }
        public double? RSI2 { get; set; }
        public double? WR { get; set; }

        public double? Delt_Price { get; set; }
        public double? Delt_SMA5 { get; set; }
        public double? Delt_SMA10 { get; set; }
        public double? Delt_SMA50 { get; set; }
        public double? Delt_EMA20 { get; set; }
        public double? Delt_Diff_ADX { get; set; }
        public double? Delt_MACD_Hist { get; set; }
        public double? Delt_MACD { get; set; }
        public double? Delt_MACD_Signal { get; set; }
        public double? Delt_K { get; set; }
        public double? Delt_D { get; set; }
        public long? Vol_AVG5 { get; set; }
        public long? Vol_AVG10 { get; set; }
        public long? Vol_AVG20 { get; set; }

    }
}
