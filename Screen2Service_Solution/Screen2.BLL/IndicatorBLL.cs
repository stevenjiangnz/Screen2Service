using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Indicator;
using Screen2.Shared;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class IndicatorBLL : BaseBLL<Screen2.Entity.Indicator>, IBaseBLL<Screen2.Entity.Indicator>
    {
        #region Properties
        private AuditLogBLL _auditBLL;
        #endregion

        #region Constructors
        public IndicatorBLL(IUnitWork unit) : base(unit)
        {
            _auditBLL = new AuditLogBLL(_unit);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the indicators.
        /// </summary>
        /// <param name="tickerList">The ticker list.</param>
        /// <param name="inputString">The input string.</param>
        /// <returns></returns>
        public Dictionary<string, double?[]> GetIndicators(List<Ticker> tickerList, string inputString)
        {
            Dictionary<string, double?[]> indicatorDict = new Dictionary<string, double?[]>();
            char delimiter1 = '|';
            char delimiter2 = ',';
            string[] indicatorArray;
            int len = tickerList.Count;

            indicatorArray = inputString.Split(delimiter1);

            double[] close = new double[len];
            double[] hi = new double[len];
            double[] lo = new double[len];
            double[] open = new double[len];

            var p = 0;
            foreach (Ticker t in tickerList)
            {
                close[p] = t.Close;
                open[p] = t.Open;
                hi[p] = t.High;
                lo[p] = t.Low;
                p++;
            }


            for (int i = 0; i < indicatorArray.Length; i++)
            {
                string[] iParam = indicatorArray[i].Split(delimiter2);
                string iName = iParam[0];
                switch (iName)
                {
                    case "sma":
                        double?[] smaOuput = new double?[len];
                        int smaPeriod = int.Parse(iParam[1]);
                        SMA.Calculate(close, smaPeriod, smaOuput);
                        indicatorDict.Add(indicatorArray[i], smaOuput);
                        break;
                    case "ema":
                        double?[] emaOuput = new double?[len];
                        int emaPeriod = int.Parse(iParam[1]);
                        EMA.Calculate(close, emaPeriod, 2, emaOuput);
                        indicatorDict.Add(indicatorArray[i], emaOuput);
                        break;
                    case "bb": //BollingerBand
                        double?[] m = new double?[len];
                        double?[] h = new double?[len];
                        double?[] l = new double?[len];
                        int bbPeriod = int.Parse(iParam[1]);
                        double rate = double.Parse(iParam[2]);
                        BollingerBand.Calculate(close, bbPeriod, rate, m, h, l);
                        indicatorDict.Add(indicatorArray[i] + "_m", m);
                        indicatorDict.Add(indicatorArray[i] + "_h", h);
                        indicatorDict.Add(indicatorArray[i] + "_l", l);
                        break;
                    case "rsi":
                        double?[] rsiOutput = new double?[len];
                        int rsiPeriod = int.Parse(iParam[1]);
                        RSI.Calculate(close, rsiPeriod, rsiOutput);
                        indicatorDict.Add(indicatorArray[i], rsiOutput);
                        break;
                    case "rsi2":
                        double?[] rsi2Output = new double?[len];
                        int rsi2Period = int.Parse(iParam[1]);
                        RSI.Calculate(close, rsi2Period, rsi2Output);
                        indicatorDict.Add(indicatorArray[i], rsi2Output);
                        break;
                    case "adx":
                        double?[] diPlus = new double?[len];
                        double?[] diMinus = new double?[len];
                        double?[] adx = new double?[len];
                        ADX.Calculate(hi, lo, close, diPlus, diMinus, adx);
                        indicatorDict.Add(indicatorArray[i] + "_di+", diPlus);
                        indicatorDict.Add(indicatorArray[i] + "_di-", diMinus);
                        indicatorDict.Add(indicatorArray[i], adx);
                        break;
                    case "macd":
                        int slowPeriod = int.Parse(iParam[1]);
                        int fastPeriod = int.Parse(iParam[2]);
                        int signalPeriod = int.Parse(iParam[3]);
                        double?[] macd = new double?[len];
                        double?[] signal = new double?[len];
                        double?[] hist = new double?[len];
                        MACD.Calculate(close, slowPeriod, fastPeriod, signalPeriod, macd, signal, hist);
                        indicatorDict.Add("hist_" + indicatorArray[i], hist);
                        indicatorDict.Add(indicatorArray[i], macd);
                        indicatorDict.Add("signal_" + indicatorArray[i], signal);
                        break;
                    case "heikin":
                        double?[] oo = new double?[len];
                        double?[] oh = new double?[len];
                        double?[] ol = new double?[len];
                        double?[] oc = new double?[len];
                        HeikinAshi.Calculate(open, close, hi, lo, oo, oc, oh, ol);
                        indicatorDict.Add("open_heikin", oo);
                        indicatorDict.Add("high_heikin", oh);
                        indicatorDict.Add("low_heikin", ol);
                        indicatorDict.Add("close_heikin", oc);
                        break;
                    case "stochastic":
                        double?[] k = new double?[len];
                        double?[] d = new double?[len];
                        int kdPeriod = int.Parse(iParam[1]);
                        int kdSlow = int.Parse(iParam[2]);
                        Stochastic.CalculateSlow(close, hi, lo, kdPeriod, kdSlow, k, d);
                        indicatorDict.Add(indicatorArray[i] + "_k", k);
                        indicatorDict.Add(indicatorArray[i] + "_d", d);
                        break;
                    case "william":
                        double?[] william = new double?[len];
                        int wPeriod = int.Parse(iParam[1]);
                        WilliamR.Calculate(close, hi, lo, wPeriod, william);
                        indicatorDict.Add(indicatorArray[i], william);
                        break;
                    default:
                        break;
                }
            }
            return indicatorDict;
        }

        /// <summary>
        /// Processes the indicator full.
        /// </summary>
        public void ProcessIndicatorFull()
        {
            int success = 0;
            int fail = 0;
            List<Share> shareList = _unit.DataContext.Shares.Where(c => c.IsActive).ToList();

            _auditBLL.Create(new AuditLog
            {
                ActionMessage = "Start Process Indicators",
                ExtraData = "",
                ActionType = ActionType.ProcessIndicator.ToString(),
                ActionResult = string.Format("Start process indicator, Total: {0}", shareList.Count)
            });

            Console.WriteLine("Start process indicator, Total: {0}", shareList.Count);

            foreach (var s in shareList)
            {
                try
                {
                    ProcessIndicator(s.Id);
                    success++;

                    _auditBLL.Create(new AuditLog
                    {
                        ActionMessage = "Success Process Indicators",
                        ExtraData = "",
                        ActionType = ActionType.ProcessIndicator.ToString(),
                        ActionResult = string.Format("Success process indicator {0}", s.Symbol)
                    });

                    Console.WriteLine("Success process indicator {0}", s.Symbol);
                }
                catch (Exception ex)
                {
                    fail++;
                    _auditBLL.Create(new AuditLog
                    {
                        ActionMessage = "Fail Process Indicators",
                        ExtraData = "",
                        ActionType = ActionType.ProcessIndicator.ToString(),
                        ActionResult = string.Format("Fail to process indicator {0}", s.Id)
                    });

                    Console.WriteLine("Fail to process indicator {0}", s.Id);
                    LogHelper.Error(_log, "Error process indicator for share " + s.Id.ToString(), ex);
                }
            }

            _auditBLL.Create(new AuditLog
            {
                ActionMessage = "Finish Process Indicators",
                ExtraData = "",
                ActionType = ActionType.ProcessIndicator.ToString(),
                ActionResult = string.Format("Finish process indicator Total: {0}  Success: {1}  Fail: {2}", shareList.Count, success, fail)
            });

            Console.WriteLine("Finish process indicator Total: {0}  Success: {1}  Fail: {2}", shareList.Count, success, fail);
        }

        /// <summary>
        /// Gets the indicator by share date.
        /// </summary>
        /// <param name="shareId">The share identifier.</param>
        /// <param name="tradingDate">The trading date.</param>
        /// <returns></returns>
        public Screen2.Entity.Indicator GetIndicatorByShareDate(int shareId, int tradingDate)
        {
            return _unit.DataContext.Indicator.SingleOrDefault(c => c.ShareId == shareId && c.TradingDate == tradingDate);
        }

        /// <summary>
        /// Gets the indicator list by share date.
        /// </summary>
        /// <param name="shareId">The share identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="tradingDate">The trading date.</param>
        /// <returns></returns>
        public List<Screen2.Entity.Indicator> GetIndicatorListByShareDate(int shareId, int startDate, int tradingDate)
        {
            List<Screen2.Entity.Indicator> iList = new List<Entity.Indicator>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetIndicatorByShare", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("ShareID", SqlDbType.Int).Value = shareId;

                        if (startDate > 0)
                        {
                            cmd.Parameters.Add("StartDate", SqlDbType.Int).Value = startDate;
                        }

                        if (tradingDate > 0)
                        {
                            cmd.Parameters.Add("EndDate", SqlDbType.Int).Value = tradingDate;
                        }

                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var indicator = new Screen2.Entity.Indicator();

                                this.PopulateObjFromReader(reader, indicator);

                                iList.Add(indicator);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error read latest indicator from DB. ", ex);
                throw;
            }


            return iList;
        }

        /// <summary>
        /// Processes the indicator.
        /// </summary>
        /// <param name="shareID">The share identifier.</param>
        public void ProcessIndicator(int shareID)
        {
            const int CALCULATE_BUFFER = 400;
            Screen2.Entity.Indicator indicatorLast = null;
            Ticker tickerLast = null;
            int latestIndicatorDate = 0;
            //int latestTickerDate = 0;

            List<Ticker> tickerList;
            List<Screen2.Entity.Indicator> indicatorList = new List<Screen2.Entity.Indicator>();

            try
            {
                indicatorLast = this.GetLatestIndicator(shareID);
                tickerLast = new TickerBLL(_unit).GetLastTicker(shareID, null);

                if (indicatorLast == null)
                {
                    tickerList = new TickerBLL(_unit).GetTickerListByShareDB(shareID, null, null);

                    CalculateIndicators(tickerList, indicatorList);
                }
                else
                {
                    latestIndicatorDate = indicatorLast.TradingDate;

                    if (tickerLast != null && tickerLast.TradingDate > latestIndicatorDate)
                    {
                        int TickerToloadDate = DateHelper.DateToInt(DateHelper.IntToDate(latestIndicatorDate).AddDays(-1 * CALCULATE_BUFFER));

                        tickerList = new TickerBLL(_unit).GetTickerListByShareDB(shareID, TickerToloadDate, null);

                        CalculateIndicators(tickerList, indicatorList);
                    }
                }

                if (latestIndicatorDate > 0)
                {
                    indicatorList = indicatorList.Where(c => c.TradingDate > latestIndicatorDate).ToList();
                }

                this.SaveIndicatorsBatchDB(indicatorList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error process indicator for share " + shareID.ToString(), ex);
                throw;
            }
        }

        /// <summary>
        /// Calculates the indicators.
        /// </summary>
        /// <param name="tickerList">The ticker list.</param>
        /// <param name="indicatorList">The indicator list.</param>
        private void CalculateIndicators(List<Ticker> tickerList, List<Screen2.Entity.Indicator> indicatorList)
        {
            int len = tickerList.Count;

            double[] close = new double[len];
            double[] hi = new double[len];
            double[] lo = new double[len];
            double[] open = new double[len];
            double[] dbVol = new double[len];

            var p = 0;
            foreach (Ticker t in tickerList)
            {
                close[p] = t.Close;
                open[p] = t.Open;
                hi[p] = t.High;
                lo[p] = t.Low;
                dbVol[p] = t.Volumn;
                p++;
            }

            double?[] sma5Ouput = new double?[len];
            SMA.Calculate(close, 5, sma5Ouput);

            double?[] sma10Ouput = new double?[len];
            SMA.Calculate(close, 10, sma10Ouput);

            double?[] sma30Ouput = new double?[len];
            SMA.Calculate(close, 30, sma30Ouput);

            double?[] sma50Ouput = new double?[len];
            SMA.Calculate(close, 50, sma50Ouput);

            double?[] sma200Ouput = new double?[len];
            SMA.Calculate(close, 200, sma200Ouput);

            double?[] ema10Ouput = new double?[len];
            EMA.Calculate(close, 10, 2, ema10Ouput);

            double?[] ema20Ouput = new double?[len];
            EMA.Calculate(close, 20, 2, ema20Ouput);

            double?[] ema50Ouput = new double?[len];
            EMA.Calculate(close, 50, 2, ema50Ouput);

            double?[] BB_Middle = new double?[len];
            double?[] BB_High = new double?[len];
            double?[] BB_Low = new double?[len];

            BollingerBand.Calculate(close, 20, 2.5, BB_Middle, BB_High, BB_Low);

            double?[] ADX_Plus = new double?[len];
            double?[] ADX_Minus = new double?[len];
            double?[] adx = new double?[len];

            ADX.Calculate(hi, lo, close, ADX_Plus, ADX_Minus, adx);

            double?[] macd = new double?[len];
            double?[] macd_signal = new double?[len];
            double?[] macd_hist = new double?[len];

            MACD.Calculate(close, 26, 12, 9, macd, macd_signal, macd_hist);

            double?[] Heikin_Open = new double?[len];
            double?[] Heikin_High = new double?[len];
            double?[] Heikin_Low = new double?[len];
            double?[] Heikin_Close = new double?[len];
            HeikinAshi.Calculate(open, close, hi, lo, Heikin_Open, Heikin_Close, Heikin_High, Heikin_Low);

            double?[] Stochastic_K = new double?[len];
            double?[] Stochastic_D = new double?[len];

            Stochastic.CalculateSlow(close, hi, lo, 14, 3, Stochastic_K, Stochastic_D);

            double?[] rsiOutput = new double?[len];
            RSI.Calculate(close, 14, rsiOutput);

            double?[] rsi2Output = new double?[len];
            RSI.Calculate( close, 2, rsi2Output);

            double?[] william = new double?[len];
            WilliamR.Calculate(close, hi, lo, 14, william);

            double?[] closeOp = new double?[len];
            for (int i = 0; i < len; i++)
            {
                closeOp[i] = close[i];
            }

            double?[] Delt_Price = new double?[len];
            Delt.Calculate(closeOp, 1, Delt_Price);

            double?[] Delt_SMA5 = new double?[len];
            Delt.Calculate(sma5Ouput, 1, Delt_SMA5);

            double?[] Delt_SMA10 = new double?[len];
            Delt.Calculate(sma10Ouput, 1, Delt_SMA10);

            double?[] Delt_SMA50 = new double?[len];
            Delt.Calculate(sma50Ouput, 1, Delt_SMA50);

            double?[] Delt_EMA20 = new double?[len];
            Delt.Calculate(ema20Ouput, 1, Delt_EMA20);

            double?[] Delt_MACD = new double?[len];
            Delt.Calculate(macd, 1, Delt_MACD);

            double?[] Delt_MACD_Hist = new double?[len];
            Delt.Calculate(macd_hist, 1, Delt_MACD_Hist);

            double?[] Delt_MACD_Signal = new double?[len];
            Delt.Calculate(macd_signal, 1, Delt_MACD_Signal);

            double?[] Delt_K = new double?[len];
            Delt.Calculate(Stochastic_K, 1, Delt_K);

            double?[] Delt_D = new double?[len];
            Delt.Calculate(Stochastic_D, 1, Delt_D);


            double?[] Delt_Diff_ADX = new double?[len];
            double?[] diff_adx = new double?[len];

            for (int i = 0; i < len; i++)
            {
                if (ADX_Plus[i].HasValue && ADX_Minus[i].HasValue)
                {
                    diff_adx[i] = ADX_Plus[i].Value - ADX_Minus[i].Value;
                }
            }

            Delt.Calculate(diff_adx, 1, Delt_Diff_ADX);

            double?[] Vol_AVG5 = new double?[len];
            SMA.Calculate(dbVol, 5, Vol_AVG5);

            double?[] Vol_AVG10 = new double?[len];
            SMA.Calculate(dbVol, 10, Vol_AVG10);

            double?[] Vol_AVG20 = new double?[len];
            SMA.Calculate(dbVol, 20, Vol_AVG20);

            for (int i = 0; i < len; i++)
            {
                Screen2.Entity.Indicator ind = new Screen2.Entity.Indicator();

                ind.TradingDate = tickerList[i].TradingDate;
                ind.Close = tickerList[i].Close;
                ind.ShareId = tickerList[i].ShareId;
                ind.SMA5 = sma5Ouput[i];
                ind.SMA10 = sma10Ouput[i];
                ind.SMA30 = sma30Ouput[i];
                ind.SMA50 = sma50Ouput[i];
                ind.SMA200 = sma200Ouput[i];
                ind.EMA10 = ema10Ouput[i];
                ind.EMA20 = ema20Ouput[i];
                ind.EMA50 = ema50Ouput[i];
                ind.BB_Middle = BB_Middle[i];
                ind.BB_High = BB_High[i];
                ind.BB_Low = BB_Low[i];
                ind.ADX = adx[i];
                ind.ADX_Plus = ADX_Plus[i];
                ind.ADX_Minus = ADX_Minus[i];
                ind.MACD = macd[i];
                ind.MACD_Hist = macd_hist[i];
                ind.MACD_Signal = macd_signal[i];
                ind.Heikin_Open = Heikin_Open[i];
                ind.Heikin_Close = Heikin_Close[i];
                ind.Heikin_High = Heikin_High[i];
                ind.Heikin_Low = Heikin_Low[i];
                ind.Stochastic_D = Stochastic_D[i];
                ind.Stochastic_K = Stochastic_K[i];
                ind.RSI = rsiOutput[i];
                ind.RSI2 = rsi2Output[i];
                ind.WR = william[i];
                ind.Delt_Price = Delt_Price[i];
                ind.Delt_SMA5 = Delt_SMA5[i];
                ind.Delt_SMA10 = Delt_SMA10[i];
                ind.Delt_SMA50 = Delt_SMA50[i];
                ind.Delt_EMA20 = Delt_EMA20[i];
                ind.Delt_MACD = Delt_MACD[i];
                ind.Delt_MACD_Signal = Delt_MACD_Signal[i];
                ind.Delt_MACD_Hist = Delt_MACD_Hist[i];
                ind.Delt_K = Delt_K[i];
                ind.Delt_D = Delt_D[i];
                ind.Delt_Diff_ADX = Delt_Diff_ADX[i];
                ind.Vol_AVG5 = Vol_AVG5[i].HasValue ? (Int64)Vol_AVG5[i] : 0;
                ind.Vol_AVG10 = Vol_AVG10[i].HasValue ? (Int64)Vol_AVG10[i] : 0;
                ind.Vol_AVG20 = Vol_AVG20[i].HasValue ? (Int64)Vol_AVG20[i] : 0;

                if (i > 0 && Heikin_Open[i - 1].HasValue && Heikin_Close[i - 1].HasValue)
                {
                    if (Heikin_Open[i - 1].Value > Heikin_Close[i - 1].Value)
                        ind.Prev_Heikin = false;

                    if (Heikin_Open[i - 1].Value < Heikin_Close[i - 1].Value)
                        ind.Prev_Heikin = true;

                }

                indicatorList.Add(ind);
            }
        }

        /// <summary>
        /// Gets the latest indicator.
        /// </summary>
        /// <param name="shareId">The share identifier.</param>
        /// <returns></returns>
        public Screen2.Entity.Indicator GetLatestIndicator(int shareId)
        {
            Screen2.Entity.Indicator indicator = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetLatestIndicator", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("ShareID", SqlDbType.Int).Value = shareId;
                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                indicator = new Screen2.Entity.Indicator();

                                this.PopulateObjFromReader(reader, indicator);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error read latest indicator from DB. ", ex);
                throw;
            }

            return indicator;
        }

        /// <summary>
        /// Populates the object from reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="indicator">The indicator.</param>
        private void PopulateObjFromReader(SqlDataReader reader, Screen2.Entity.Indicator indicator)
        {
            indicator.Id = reader.GetInt32(reader.GetOrdinal("ID"));
            indicator.ShareId = reader.GetInt32(reader.GetOrdinal("ShareID"));
            indicator.TradingDate = reader.GetInt32(reader.GetOrdinal("TradingDate"));

            if (!reader.IsDBNull(reader.GetOrdinal("Close")))
                indicator.Close = reader.GetDouble(reader.GetOrdinal("Close"));

            if (!reader.IsDBNull(reader.GetOrdinal("SMA5")))
                indicator.SMA5 = reader.GetDouble(reader.GetOrdinal("SMA5"));

            if (!reader.IsDBNull(reader.GetOrdinal("SMA10")))
                indicator.SMA10 = reader.GetDouble(reader.GetOrdinal("SMA10"));

            if (!reader.IsDBNull(reader.GetOrdinal("SMA30")))
                indicator.SMA30 = reader.GetDouble(reader.GetOrdinal("SMA30"));

            if (!reader.IsDBNull(reader.GetOrdinal("SMA50")))
                indicator.SMA50 = reader.GetDouble(reader.GetOrdinal("SMA50"));

            if (!reader.IsDBNull(reader.GetOrdinal("SMA200")))
                indicator.SMA200 = reader.GetDouble(reader.GetOrdinal("SMA200"));

            if (!reader.IsDBNull(reader.GetOrdinal("EMA10")))
                indicator.EMA10 = reader.GetDouble(reader.GetOrdinal("EMA10"));

            if (!reader.IsDBNull(reader.GetOrdinal("EMA20")))
                indicator.EMA20 = reader.GetDouble(reader.GetOrdinal("EMA20"));

            if (!reader.IsDBNull(reader.GetOrdinal("EMA50")))
                indicator.EMA50 = reader.GetDouble(reader.GetOrdinal("EMA50"));

            if (!reader.IsDBNull(reader.GetOrdinal("BB_Middle")))
                indicator.BB_Middle = reader.GetDouble(reader.GetOrdinal("BB_Middle"));

            if (!reader.IsDBNull(reader.GetOrdinal("BB_Low")))
                indicator.BB_Low = reader.GetDouble(reader.GetOrdinal("BB_Low"));

            if (!reader.IsDBNull(reader.GetOrdinal("BB_High")))
                indicator.BB_High = reader.GetDouble(reader.GetOrdinal("BB_High"));

            if (!reader.IsDBNull(reader.GetOrdinal("ADX")))
                indicator.ADX = reader.GetDouble(reader.GetOrdinal("ADX"));

            if (!reader.IsDBNull(reader.GetOrdinal("ADX_Plus")))
                indicator.ADX_Plus = reader.GetDouble(reader.GetOrdinal("ADX_Plus"));

            if (!reader.IsDBNull(reader.GetOrdinal("ADX_Minus")))
                indicator.ADX_Minus = reader.GetDouble(reader.GetOrdinal("ADX_Minus"));

            if (!reader.IsDBNull(reader.GetOrdinal("MACD")))
                indicator.MACD = reader.GetDouble(reader.GetOrdinal("MACD"));

            if (!reader.IsDBNull(reader.GetOrdinal("MACD_Signal")))
                indicator.MACD_Signal = reader.GetDouble(reader.GetOrdinal("MACD_Signal"));

            if (!reader.IsDBNull(reader.GetOrdinal("MACD_Hist")))
                indicator.MACD_Hist = reader.GetDouble(reader.GetOrdinal("MACD_Hist"));

            if (!reader.IsDBNull(reader.GetOrdinal("Heikin_Open")))
                indicator.Heikin_Open = reader.GetDouble(reader.GetOrdinal("Heikin_Open"));

            if (!reader.IsDBNull(reader.GetOrdinal("Heikin_Close")))
                indicator.Heikin_Close = reader.GetDouble(reader.GetOrdinal("Heikin_Close"));

            if (!reader.IsDBNull(reader.GetOrdinal("Heikin_Low")))
                indicator.Heikin_Low = reader.GetDouble(reader.GetOrdinal("Heikin_Low"));

            if (!reader.IsDBNull(reader.GetOrdinal("Heikin_High")))
                indicator.Heikin_High = reader.GetDouble(reader.GetOrdinal("Heikin_High"));

            if (!reader.IsDBNull(reader.GetOrdinal("Stochastic_D")))
                indicator.Stochastic_D = reader.GetDouble(reader.GetOrdinal("Stochastic_D"));

            if (!reader.IsDBNull(reader.GetOrdinal("Stochastic_K")))
                indicator.Stochastic_K = reader.GetDouble(reader.GetOrdinal("Stochastic_K"));

            if (!reader.IsDBNull(reader.GetOrdinal("RSI")))
                indicator.RSI = reader.GetDouble(reader.GetOrdinal("RSI"));

            if (!reader.IsDBNull(reader.GetOrdinal("RSI2")))
                indicator.RSI2 = reader.GetDouble(reader.GetOrdinal("RSI2"));

            if (!reader.IsDBNull(reader.GetOrdinal("WR")))
                indicator.WR = reader.GetDouble(reader.GetOrdinal("WR"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_Price")))
                indicator.Delt_Price = reader.GetDouble(reader.GetOrdinal("Delt_Price"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_SMA5")))
                indicator.Delt_SMA5 = reader.GetDouble(reader.GetOrdinal("Delt_SMA5"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_SMA10")))
                indicator.Delt_SMA10 = reader.GetDouble(reader.GetOrdinal("Delt_SMA10"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_SMA50")))
                indicator.Delt_SMA50 = reader.GetDouble(reader.GetOrdinal("Delt_SMA50"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_EMA20")))
                indicator.Delt_EMA20 = reader.GetDouble(reader.GetOrdinal("Delt_EMA20"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_MACD")))
                indicator.Delt_MACD = reader.GetDouble(reader.GetOrdinal("Delt_MACD"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_MACD_Hist")))
                indicator.Delt_MACD_Hist = reader.GetDouble(reader.GetOrdinal("Delt_MACD_Hist"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_MACD_Signal")))
                indicator.Delt_MACD_Signal = reader.GetDouble(reader.GetOrdinal("Delt_MACD_Signal"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_K")))
                indicator.Delt_K = reader.GetDouble(reader.GetOrdinal("Delt_K"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_D")))
                indicator.Delt_D = reader.GetDouble(reader.GetOrdinal("Delt_D"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_Diff_ADX")))
                indicator.Delt_Diff_ADX = reader.GetDouble(reader.GetOrdinal("Delt_Diff_ADX"));

            if (!reader.IsDBNull(reader.GetOrdinal("Vol_AVG5")))
                indicator.Vol_AVG5 = reader.GetInt64(reader.GetOrdinal("Vol_AVG5"));

            if (!reader.IsDBNull(reader.GetOrdinal("Vol_AVG10")))
                indicator.Vol_AVG10 = reader.GetInt64(reader.GetOrdinal("Vol_AVG10"));

            if (!reader.IsDBNull(reader.GetOrdinal("Vol_AVG20")))
                indicator.Vol_AVG20 = reader.GetInt64(reader.GetOrdinal("Vol_AVG20"));

            if (!reader.IsDBNull(reader.GetOrdinal("Prev_Heikin")))
                indicator.Prev_Heikin = reader.GetBoolean(reader.GetOrdinal("Prev_Heikin"));

        }

        /// <summary>
        /// Gets the sma by share identifier.
        /// </summary>
        /// <param name="shareID">The share identifier.</param>
        /// <param name="period">The period.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="byCalculate">if set to <c>true</c> [by calculate].</param>
        /// <returns></returns>
        public List<SMAViewModel> GetSMAByShareID(int shareID, int period, int startDate, int endDate, bool byCalculate = true)
        {

            List<SMAViewModel> smaList = new List<SMAViewModel>();

            TickerBLL tBll = new TickerBLL(_unit);

            try
            {
                if (endDate == 0)
                {
                    endDate = DateHelper.DateToInt(DateTime.Now);
                }

                List<Ticker> tickerList = tBll.GetTickerListByShareDB(shareID, startDate, endDate);

                if (tickerList.Count > 0)
                {
                    double[] input = new double[tickerList.Count];
                    double?[] output = new double?[tickerList.Count];

                    Ticker[] tArray = tickerList.ToArray();

                    for (int i = 0; i < tArray.Length; i++)
                    {
                        input[i] = tArray[i].Close;
                    }

                    SMA.Calculate(input, period, output);

                    for (int i = 0; i < output.Length; i++)
                    {
                        SMAViewModel sma = new SMAViewModel();
                        sma.ShareID = shareID;
                        sma.Sma = output[i];
                        sma.TradingDate = tArray[i].TradingDate;
                        smaList.Add(sma);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error GetSMAByShareID. ", ex);
                throw;
            }


            return smaList;
        }

        /// <summary>
        /// Saves the indicators batch database.
        /// </summary>
        /// <param name="indicators">The indicators.</param>
        public void SaveIndicatorsBatchDB(List<Screen2.Entity.Indicator> indicators)
        {
            var xmlInput = XMLHelper.SerializeObject<List<Screen2.Entity.Indicator>>(indicators);
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_InsertIndicatorBatch", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("IndicatorsXML", SqlDbType.Xml).Value = xmlInput;
                        cmd.Connection.Open();

                        var result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error save indicators to DB. ", ex);
                throw;
            }

        }

        /// <summary>
        /// Updates the delt indicators full.
        /// </summary>
        public void UpdateDeltIndicatorsFull()
        {
            int success = 0;
            int fail = 0;

            List<Share> sList = _unit.DataContext.Shares.Where(s => s.IsActive == true).ToList();

            _auditBLL.Create(new AuditLog
            {
                ActionMessage = "Start Update Indicators",
                ExtraData = "",
                ActionType = ActionType.UploadDailyTicker.ToString(),
                ActionResult = string.Format("Start update indicator {0}", sList.Count)
            });

            foreach (var s in sList)
            {
                try
                {
                    this.UpdateDeltIndicators(s.Id);

                    success++;
                }
                catch (Exception ex)
                {
                    fail++;
                    LogHelper.Error(_log, "Error update indicator. ", ex);
                }
            }


            _auditBLL.Create(new AuditLog
            {
                ActionMessage = "Finidh Update Indicators",
                ExtraData = "",
                ActionType = ActionType.UpdateIndicator.ToString(),
                ActionResult = string.Format("finish update indicator Total {0}, success {1}, failed {2}", sList.Count, success, fail)
            });

        }

        /// <summary>
        /// Updates the delt indicators.
        /// </summary>
        /// <param name="shareId">The share identifier.</param>
        public void UpdateDeltIndicators(int shareId)
        {
            try
            {
                var iList = this.GetIndicatorListByShareDate(shareId, 0, 0);
                var tickerList = new TickerBLL(_unit).GetTickerListByShareDB(shareId, null, null);

                int len = iList.Count;
                double?[] close = new double?[len];
                double?[] delt_close = new double?[len];

                double?[] sma5Ouput = new double?[len];
                double?[] sma10Ouput = new double?[len];
                double?[] sma50Ouput = new double?[len];
                double?[] ema20Ouput = new double?[len];
                double?[] macd = new double?[len];
                double?[] macd_hist = new double?[len];
                double?[] macd_signal = new double?[len];
                double?[] Stochastic_K = new double?[len];
                double?[] Stochastic_D = new double?[len];
                double?[] diff_adx = new double?[len];
                double[] dbVol = new double[len];

                int i = 0;
                foreach (Entity.Indicator ind in iList)
                {
                    close[i] = ind.Close;
                    sma5Ouput[i] = ind.SMA5;
                    sma10Ouput[i] = ind.SMA10;
                    sma50Ouput[i] = ind.SMA50;
                    ema20Ouput[i] = ind.EMA20;
                    macd[i] = ind.MACD;
                    macd_hist[i] = ind.MACD_Hist;
                    macd_signal[i] = ind.MACD_Signal;
                    Stochastic_K[i] = ind.Stochastic_K;
                    Stochastic_D[i] = ind.Stochastic_D;
                    if (ind.ADX_Plus.HasValue && ind.ADX_Minus.HasValue)
                    {
                        diff_adx[i] = Math.Abs(ind.ADX_Plus.Value - ind.ADX_Minus.Value);
                    }

                    i++;
                }

                var p = 0;
                foreach (Ticker t in tickerList)
                {
                    dbVol[p] = t.Volumn;
                    p++;
                }


                Delt.Calculate(close, 1, delt_close);

                double?[] Delt_SMA5 = new double?[len];
                Delt.Calculate(sma5Ouput, 1, Delt_SMA5);

                double?[] Delt_SMA10 = new double?[len];
                Delt.Calculate(sma10Ouput, 1, Delt_SMA10);

                double?[] Delt_SMA50 = new double?[len];
                Delt.Calculate(sma50Ouput, 1, Delt_SMA50);

                double?[] Delt_EMA20 = new double?[len];
                Delt.Calculate(ema20Ouput, 1, Delt_EMA20);

                double?[] Delt_MACD = new double?[len];
                Delt.Calculate(macd, 1, Delt_MACD);

                double?[] Delt_MACD_Hist = new double?[len];
                Delt.Calculate(macd_hist, 1, Delt_MACD_Hist);

                double?[] Delt_MACD_Signal = new double?[len];
                Delt.Calculate(macd_signal, 1, Delt_MACD_Signal);

                double?[] Delt_K = new double?[len];
                Delt.Calculate(Stochastic_K, 1, Delt_K);

                double?[] Delt_D = new double?[len];
                Delt.Calculate(Stochastic_D, 1, Delt_D);

                double?[] Delt_Diff_ADX = new double?[len];
                Delt.Calculate(diff_adx, 1, Delt_Diff_ADX);

                double?[] Vol_AVG5 = new double?[len];
                SMA.Calculate(dbVol, 5, Vol_AVG5);

                double?[] Vol_AVG10 = new double?[len];
                SMA.Calculate(dbVol, 10, Vol_AVG10);

                double?[] Vol_AVG20 = new double?[len];
                SMA.Calculate(dbVol, 20, Vol_AVG20);

                i = 0;
                foreach (Entity.Indicator ind in iList)
                {
                    ind.Delt_Price = delt_close[i];
                    ind.Delt_SMA5 = Delt_SMA5[i];
                    ind.Delt_SMA10 = Delt_SMA10[i];
                    ind.Delt_SMA50 = Delt_SMA50[i];
                    ind.Delt_EMA20 = Delt_EMA20[i];
                    ind.Delt_MACD = Delt_MACD[i];
                    ind.Delt_MACD_Hist = Delt_MACD_Hist[i];
                    ind.Delt_MACD_Signal = Delt_MACD_Signal[i];
                    ind.Delt_K = Delt_K[i];
                    ind.Delt_D = Delt_D[i];
                    ind.Delt_Diff_ADX = Delt_Diff_ADX[i];
                    ind.Vol_AVG5 = Vol_AVG5[i].HasValue ? (long)Vol_AVG5[i].Value : 0;
                    ind.Vol_AVG10 = Vol_AVG10[i].HasValue ? (long)Vol_AVG10[i].Value : 0;
                    ind.Vol_AVG20 = Vol_AVG20[i].HasValue ? (long)Vol_AVG20[i].Value : 0;
                    i++;
                }

                UpdateIndicatorsBatchDB(iList);

                _auditBLL.Create(new AuditLog
                {
                    ActionMessage = "Success Update Indicator",
                    ExtraData = "",
                    ActionType = ActionType.UpdateIndicator.ToString(),
                    ActionResult = string.Format("Success update Indicator for {0}", shareId)
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error update indicators. " + shareId.ToString(), ex);

                _auditBLL.Create(new AuditLog
                {
                    ActionMessage = "Faile to Update Indicator",
                    ExtraData = "",
                    ActionType = ActionType.UpdateIndicator.ToString(),
                    ActionResult = string.Format("Fail to update Indicator for {0}", shareId)
                });
                throw;
            }
        }

        /// <summary>
        /// Updates the indicators batch database.
        /// </summary>
        /// <param name="indicators">The indicators.</param>
        public void UpdateIndicatorsBatchDB(List<Screen2.Entity.Indicator> indicators)
        {
            var xmlInput = XMLHelper.SerializeObject<List<Screen2.Entity.Indicator>>(indicators);
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_UpdateIndicatorBatch", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("IndicatorsXML", SqlDbType.Xml).Value = xmlInput;
                        cmd.Connection.Open();

                        var result = cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error save indicators to DB. ", ex);
                throw;
            }
        }


        public long GetNextTradingDate(int tradingDate)
        {
            long nextTradingDate = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetNextTradingDate", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("TradingDate", SqlDbType.Int).Value = tradingDate;

                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Read();

                            nextTradingDate = reader.GetInt32(reader.GetOrdinal("TradingDate"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error get next trading from DB. ", ex);
                throw;
            }


            return nextTradingDate;
        }


        public long GetLatestTradingDate()
        {
            long latestTradingDate = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetLatestTradingDate", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Read();

                            latestTradingDate = reader.GetInt32(reader.GetOrdinal("TradingDate"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error seach stock from DB. ", ex);
                throw;
            }


            return latestTradingDate;
        }



        public long GetLatestTradingDateByShare(int shareId)
        {
            long latestTradingDate = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetLatestTradingDateByShare", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("ShareId", SqlDbType.Int).Value = shareId;

                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Read();

                            latestTradingDate = reader.GetInt32(reader.GetOrdinal("TradingDate"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error seach stock from DB. ", ex);
                throw;
            }


            return latestTradingDate;
        }


        public List<OutStockSearchResult> GetShareDayTickerByWatch(int watchId, bool reverse = false)
        {
            return null;
        }

        public List<OutStockSearchResult> GetShareDayTickerLatest()
        {
            List<OutStockSearchResult> searchResult = new List<OutStockSearchResult>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetStockDataLatest", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var result = new OutStockSearchResult();

                                this.PopulateSearchObjFromReader(reader, result);

                                searchResult.Add(result);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error seach stock from DB. ", ex);
                throw;
            }

            return searchResult;

        }


        public List<OutStockSearchResult> SearchShareByWatch(int watchId, int tradingDate, bool isReserve = false)
        {
            List<OutStockSearchResult> searchResult = new List<OutStockSearchResult>();

            List<Share> sList = new ShareBLL(_unit).GetShareListByWatch(watchId, isReserve);

            // Prepare the share list string
            string shareListString = "";

            foreach (var s in sList)
            {
                if (shareListString.Length > 0)
                {
                    shareListString += ",";
                }

                shareListString += s.Id.ToString();
            }

            searchResult = SearchShareByShareString(shareListString, tradingDate, isReserve);

            return searchResult;
        }


        public List<OutStockSearchResult> SearchShareByShareString(string shareListString, int tradingDate, bool isReserve = false)
        {
            List<OutStockSearchResult> searchResult = new List<OutStockSearchResult>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_SearchStockByWatch", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("ShareListString", SqlDbType.VarChar).Value = shareListString;
                        cmd.Parameters.Add("TradingDate", SqlDbType.Int).Value = tradingDate;
                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var result = new OutStockSearchResult();

                                this.PopulateSearchObjFromReader(reader, result);

                                searchResult.Add(result);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error seach stock from DB. ", ex);
                throw;
            }

            return searchResult;
        }

        public void PopulateIndicatorsWithTickers(List<Screen2.Entity.Indicator> iList, List<Ticker> tList)
        {
            for (int i = 0; i < iList.Count; i++)
            {
                PopulateIndicatorWithTicker(iList[i], tList[i]);
            }
        }

        public void PopulateIndicatorWithTicker(Screen2.Entity.Indicator i, Ticker t)
        {
            if (i.ShareId == t.ShareId &&
                i.TradingDate == t.TradingDate)
            {
                i.Volumn = t.Volumn;
                i.Low = t.Low;
                i.High = t.High;
                i.Open = t.Open;
                i.Close = t.Close;
            }
        }

        /// <summary>
        /// Searches the share day ticker.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public List<OutStockSearchResult> SearchShareDayTicker(int? startDate, int? endDate)
        {
            List<OutStockSearchResult> searchResult = new List<OutStockSearchResult>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_SearchStock", conn))
                    {
                        cmd.CommandTimeout = _CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (startDate.HasValue && startDate.Value > 0)
                        {
                            cmd.Parameters.Add("StartDate", SqlDbType.Int).Value = startDate.Value;
                        }

                        if (endDate.HasValue && endDate.Value > 0)
                        {
                            cmd.Parameters.Add("EndDate", SqlDbType.Int).Value = endDate.Value;
                        }
                        cmd.Connection.Open();

                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var result = new OutStockSearchResult();

                                this.PopulateSearchObjFromReader(reader, result);

                                searchResult.Add(result);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error seach stock from DB. ", ex);
                throw;
            }

            return searchResult;
        }



        private void PopulateSearchObjFromReader(SqlDataReader reader, OutStockSearchResult result)
        {
            result.ShareId = reader.GetInt32(reader.GetOrdinal("ShareID"));
            result.TradingDate = reader.GetInt32(reader.GetOrdinal("TradingDate"));
            result.Name = reader.GetString(reader.GetOrdinal("Name"));
            result.Symbol = reader.GetString(reader.GetOrdinal("Symbol"));

            if (!reader.IsDBNull(reader.GetOrdinal("Industry")))
                result.Industry = reader.GetString(reader.GetOrdinal("Industry"));

            if (!reader.IsDBNull(reader.GetOrdinal("Sector")))
                result.Sector = reader.GetString(reader.GetOrdinal("Sector"));

            if (!reader.IsDBNull(reader.GetOrdinal("ShareType")))
                result.ShareType = reader.GetString(reader.GetOrdinal("ShareType"));
            result.IsCfd = reader.GetBoolean(reader.GetOrdinal("IsCFD"));
            result.IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
            result.Open = reader.GetDouble(reader.GetOrdinal("Open"));
            result.Close = reader.GetDouble(reader.GetOrdinal("Close"));
            result.Low = reader.GetDouble(reader.GetOrdinal("Low"));
            result.High = reader.GetDouble(reader.GetOrdinal("High"));
            result.Volumn = reader.GetInt64(reader.GetOrdinal("Volumn"));

            if (!reader.IsDBNull(reader.GetOrdinal("AdjustedClose")))
                result.AdjustedClose = reader.GetDouble(reader.GetOrdinal("AdjustedClose"));

            if (!reader.IsDBNull(reader.GetOrdinal("SMA5")))
                result.SMA5 = reader.GetDouble(reader.GetOrdinal("SMA5"));

            if (!reader.IsDBNull(reader.GetOrdinal("SMA10")))
                result.SMA10 = reader.GetDouble(reader.GetOrdinal("SMA10"));

            if (!reader.IsDBNull(reader.GetOrdinal("SMA30")))
                result.SMA30 = reader.GetDouble(reader.GetOrdinal("SMA30"));

            if (!reader.IsDBNull(reader.GetOrdinal("SMA50")))
                result.SMA50 = reader.GetDouble(reader.GetOrdinal("SMA50"));

            if (!reader.IsDBNull(reader.GetOrdinal("SMA200")))
                result.SMA200 = reader.GetDouble(reader.GetOrdinal("SMA200"));

            if (!reader.IsDBNull(reader.GetOrdinal("EMA10")))
                result.EMA10 = reader.GetDouble(reader.GetOrdinal("EMA10"));

            if (!reader.IsDBNull(reader.GetOrdinal("EMA20")))
                result.EMA20 = reader.GetDouble(reader.GetOrdinal("EMA20"));

            if (!reader.IsDBNull(reader.GetOrdinal("EMA50")))
                result.EMA50 = reader.GetDouble(reader.GetOrdinal("EMA50"));

            if (!reader.IsDBNull(reader.GetOrdinal("BB_Middle")))
                result.BB_Middle = reader.GetDouble(reader.GetOrdinal("BB_Middle"));

            if (!reader.IsDBNull(reader.GetOrdinal("BB_High")))
                result.BB_High = reader.GetDouble(reader.GetOrdinal("BB_High"));

            if (!reader.IsDBNull(reader.GetOrdinal("BB_Low")))
                result.BB_Low = reader.GetDouble(reader.GetOrdinal("BB_Low"));

            if (!reader.IsDBNull(reader.GetOrdinal("ADX")))
                result.ADX = reader.GetDouble(reader.GetOrdinal("ADX"));

            if (!reader.IsDBNull(reader.GetOrdinal("ADX_Plus")))
                result.ADX_Plus = reader.GetDouble(reader.GetOrdinal("ADX_Plus"));

            if (!reader.IsDBNull(reader.GetOrdinal("ADX_Minus")))
                result.ADX_Minus = reader.GetDouble(reader.GetOrdinal("ADX_Minus"));

            if (!reader.IsDBNull(reader.GetOrdinal("MACD")))
                result.MACD = reader.GetDouble(reader.GetOrdinal("MACD"));

            if (!reader.IsDBNull(reader.GetOrdinal("MACD_Hist")))
                result.MACD_Hist = reader.GetDouble(reader.GetOrdinal("MACD_Hist"));

            if (!reader.IsDBNull(reader.GetOrdinal("MACD_Signal")))
                result.MACD_Signal = reader.GetDouble(reader.GetOrdinal("MACD_Signal"));

            if (!reader.IsDBNull(reader.GetOrdinal("Heikin_Open")))
                result.Heikin_Open = reader.GetDouble(reader.GetOrdinal("Heikin_Open"));

            if (!reader.IsDBNull(reader.GetOrdinal("Heikin_Close")))
                result.Heikin_Close = reader.GetDouble(reader.GetOrdinal("Heikin_Close"));

            if (!reader.IsDBNull(reader.GetOrdinal("Heikin_High")))
                result.Heikin_High = reader.GetDouble(reader.GetOrdinal("Heikin_High"));

            if (!reader.IsDBNull(reader.GetOrdinal("Heikin_Low")))
                result.Heikin_Low = reader.GetDouble(reader.GetOrdinal("Heikin_Low"));

            if (!reader.IsDBNull(reader.GetOrdinal("Stochastic_K")))
                result.Stochastic_K = reader.GetDouble(reader.GetOrdinal("Stochastic_K"));

            if (!reader.IsDBNull(reader.GetOrdinal("Stochastic_D")))
                result.Stochastic_D = reader.GetDouble(reader.GetOrdinal("Stochastic_D"));

            if (!reader.IsDBNull(reader.GetOrdinal("RSI")))
                result.RSI = reader.GetDouble(reader.GetOrdinal("RSI"));

            if (!reader.IsDBNull(reader.GetOrdinal("RSI2")))
                result.RSI2 = reader.GetDouble(reader.GetOrdinal("RSI2"));

            if (!reader.IsDBNull(reader.GetOrdinal("WR")))
                result.WR = reader.GetDouble(reader.GetOrdinal("WR"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_Price")))
                result.Delt_Price = reader.GetDouble(reader.GetOrdinal("Delt_Price"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_SMA5")))
                result.Delt_SMA5 = reader.GetDouble(reader.GetOrdinal("Delt_SMA5"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_SMA10")))
                result.Delt_SMA10 = reader.GetDouble(reader.GetOrdinal("Delt_SMA10"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_SMA50")))
                result.Delt_SMA50 = reader.GetDouble(reader.GetOrdinal("Delt_SMA50"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_EMA20")))
                result.Delt_EMA20 = reader.GetDouble(reader.GetOrdinal("Delt_EMA20"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_Diff_ADX")))
                result.Delt_Diff_ADX = reader.GetDouble(reader.GetOrdinal("Delt_Diff_ADX"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_MACD")))
                result.Delt_MACD = reader.GetDouble(reader.GetOrdinal("Delt_MACD"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_MACD_Signal")))
                result.Delt_MACD_Signal = reader.GetDouble(reader.GetOrdinal("Delt_MACD_Signal"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_MACD_Hist")))
                result.Delt_MACD_Hist = reader.GetDouble(reader.GetOrdinal("Delt_MACD_Hist"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_K")))
                result.Delt_K = reader.GetDouble(reader.GetOrdinal("Delt_K"));

            if (!reader.IsDBNull(reader.GetOrdinal("Delt_D")))
                result.Delt_D = reader.GetDouble(reader.GetOrdinal("Delt_D"));

            if (!reader.IsDBNull(reader.GetOrdinal("Vol_AVG5")))
                result.Vol_AVG5 = reader.GetInt64(reader.GetOrdinal("Vol_AVG5"));

            if (!reader.IsDBNull(reader.GetOrdinal("Vol_AVG10")))
                result.Vol_AVG10 = reader.GetInt64(reader.GetOrdinal("Vol_AVG10"));

            if (!reader.IsDBNull(reader.GetOrdinal("Vol_AVG20")))
                result.Vol_AVG20 = reader.GetInt64(reader.GetOrdinal("Vol_AVG20"));

        }
        #endregion
    }
}
