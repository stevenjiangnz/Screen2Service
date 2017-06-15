using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Shared
{
    public class Global
    {
        public static double MarginRate = 0.1;
    }

    public class CalcHelper
    {
        public static string PropName(string paraName)
        {
            string propName = "";

            switch (paraName.ToLower())
            {
                case "[high]":
                    propName = "High";
                    break;
                case "[low]":
                    propName = "Low";
                    break;
                case "[open]":
                    propName = "Open";
                    break;
                case "[close]":
                    propName = "Close";
                    break;
                case "[hopen]":
                    propName = "Heikin_Open";
                    break;
                case "[hclose]":
                    propName = "Heikin_Close";
                    break;
                case "[hhigh]":
                    propName = "Heikin_High";
                    break;
                case "[hlow]":
                    propName = "Heikin_Low";
                    break;
                case "[sma5]":
                    propName = "SMA5";
                    break;
                case "[sma10]":
                    propName = "SMA10";
                    break;
                case "[sma30]":
                    propName = "SMA30";
                    break;
                case "[sma50]":
                    propName = "SMA50";
                    break;
                case "[sma200]":
                    propName = "SMA200";
                    break;
                case "[ema10]":
                    propName = "EMA10";
                    break;
                case "[ema20]":
                    propName = "EMA20";
                    break;
                case "[ema50]":
                    propName = "EMA50";
                    break;
                case "[bmid]":
                    propName = "BB_Middle";
                    break;
                case "[blow]":
                    propName = "BB_Low";
                    break;
                case "[bhigh]":
                    propName = "BB_High";
                    break;
                case "[adx]":
                    propName = "ADX";
                    break;
                case "[adxp]":
                    propName = "ADX_Plus";
                    break;
                case "[adxm]":
                    propName = "ADX_Minus";
                    break;
                case "[macd]":
                    propName = "MACD";
                    break;
                case "[macds]":
                    propName = "MACD_Signal";
                    break;
                case "[macdh]":
                    propName = "MACD_Hist";
                    break;
                case "[k]":
                    propName = "Stochastic_K";
                    break;
                case "[d]":
                    propName = "Stochastic_D";
                    break;
                case "[rsi]":
                    propName = "RSI";
                    break;
                case "[rsi2]":
                    propName = "RSI2";
                    break;
                case "[wr]":
                    propName = "WR";
                    break;
                case "[vol]":
                    propName = "Volumn";
                    break;
                case "[vol5]":
                    propName = "Vol_AVG5";
                    break;
                case "[vol10]":
                    propName = "Vol_AVG10";
                    break;
                case "[vol20]":
                    propName = "Vol_AVG20";
                    break;
                case "[_price]":
                    propName = "Delt_Price";
                    break;
                case "[_sma5]":
                    propName = "Delt_SMA5";
                    break;
                case "[_sma10]":
                    propName = "Delt_SMA10";
                    break;
                case "[_sma50]":
                    propName = "Delt_SMA50";
                    break;
                case "[_ema20]":
                    propName = "Delt_EMA20";
                    break;
                case "[_adx]":
                    propName = "Delt_Diff_ADX";
                    break;
                case "[_macd]":
                    propName = "Delt_MACD";
                    break;
                case "[_macdh]":
                    propName = "Delt_MACD_Hist";
                    break;
                case "[_macds]":
                    propName = "Delt_MACD_Signal";
                    break;
                case "[_k]":
                    propName = "Delt_K";
                    break;
                case "[_d]":
                    propName = "Delt_D";
                    break;
                case "[_heikin]":
                    propName = "Prev_Heikin";
                    break;
                default:
                    throw new ArgumentException(string.Format("the parameter {0} can not be matched with indicator.", paraName));
                    break;
            }

            return propName;
        }
    }
}
