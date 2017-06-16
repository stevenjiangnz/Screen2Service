using NCalc;
using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Shared;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class ScanCalculator : BaseBLL<Scan>, IBaseBLL<Scan>
    {
        #region Fields
        private int OFFSET_DAY = 30;
        private int VERIFY_DAY = 30;
        private double stopValue;
        private double profitValue;

        public Screen2.Entity.Indicator[] Indicators;
        public int CurrentIndex;
        public Screen2.Entity.Indicator CurrentIndicator;
        public List<int> ShareIdList;
        #endregion

        #region Constructors
        public ScanCalculator(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region
        public VerifyResult CheckDailyMatch(int shareId, int tradingDate, string formula)
        {
            VerifyResult result = new VerifyResult();

            result.ShareId = shareId;
            result.TradingDate = tradingDate;

            var loadStart = DateHelper.DateToInt(DateHelper.IntToDate(tradingDate).AddDays(OFFSET_DAY * (-1)));

            var indiArray = LoadIndicators(shareId, loadStart, tradingDate);

            if(indiArray.Length >0)
            {
                ExpressionManager em = new ExpressionManager(indiArray, indiArray.Length - 1);

                try
                {
                    result.IsMatch = ExpressionManager.CheckIndicatorExpression(formula);
                    result.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = ex.ToString();
                }
            }
            else
            {
                result.IsSuccess = false;
                result.ErrorMessage = "no reecords returned";
            }

            return result;
        }



        public List<ScanResult> RunScan(Scan scan)
        {
            LoadShareList(scan);

            scan.Rule = new RuleBLL(_unit).GetByID(scan.RuleId);

            List<Screen2.Entity.Indicator> scanMatchs = new List<Screen2.Entity.Indicator>();
            List<ScanResult> resultList = new List<ScanResult>();

            foreach (int shareId in this.ShareIdList)
            {
                List<ScanResult> matchList = this.GetScanMatchList(shareId, scan);

                resultList.AddRange(matchList);
            }

            return resultList;
        }

        private void GetProfitSettings(Scan scan, int shareId)
        {
            Expression e = new Expression(scan.ProfitFormula);

            ExpressionManager.CurrentIndicator = new IndicatorBLL(_unit).GetIndicatorByShareDate(shareId, scan.StartDate);

            e.EvaluateFunction += ExpressionManager.DelegateMethod;

            profitValue = (double)e.Evaluate();

            e = new Expression(scan.StopFormula);
            e.EvaluateFunction += ExpressionManager.DelegateMethod;

            stopValue = (double)e.Evaluate();
        }

        public List<ScanResult> GetScanMatchList(int shareId, Scan scan)
        {
            List<ScanResult> matchList = new List<ScanResult>();

            //this.GetProfitSettings(scan, shareId);

            var loadStart = DateHelper.DateToInt(DateHelper.IntToDate(scan.StartDate).AddDays(OFFSET_DAY * (-1)));

            int? verifyEnd = null;

            if (scan.EndDate.HasValue)
            {
                verifyEnd = DateHelper.DateToInt(DateHelper.IntToDate(scan.EndDate.Value).AddDays(VERIFY_DAY));
            }

            LoadIndicators(shareId, loadStart, verifyEnd);

            ExpressionManager em = new ExpressionManager(this.Indicators, 0);

            for (int index =0; index < this.Indicators.Length; index++)
            {
                if (this.Indicators[index].TradingDate >= scan.StartDate 
                    && (this.Indicators[index].TradingDate <= scan.EndDate.Value || !scan.EndDate.HasValue))
                {
                    ExpressionManager.CurrentIndex = index;
                    if (ExpressionManager.CheckIndicatorExpression(scan.Rule.Formula))
                    {
                        ScanResult result = new ScanResult();

                        result.MatchIndicator = this.Indicators[index];

                        result.Verification = GetVerification(index);

                        matchList.Add(result);
                    }
                }
            }

            return matchList;
        }

        private ScanVerification GetVerification(int index)
        {
            ScanVerification v = new ScanVerification();
            Screen2.Entity.Indicator[] inds;
            v.Day1High = this.Indicators[index + 1].High;
            v.Day1Low = this.Indicators[index + 1].Low;
            if(this.Indicators[index].Close.HasValue && this.Indicators[index + 1].Close.HasValue)
            {
                v.Day1Diff = 100 * (this.Indicators[index + 1].Close - this.Indicators[index].Close) / this.Indicators[index].Close;
            }

            inds = this.Indicators.SubArray(index +1, 2);
            v.Day2High = this.GetMaxHigh(inds).High;
            v.Day2Low = this.GetMinLow(inds).Low;

            if (this.Indicators[index].Close.HasValue && this.Indicators[index + 2].Close.HasValue)
            {
                v.Day2Diff = 100 * (this.Indicators[index + 2].Close - this.Indicators[index].Close) / this.Indicators[index].Close;
            }

            inds = this.Indicators.SubArray(index + 1, 3);
            v.Day3High = this.GetMaxHigh(inds).High;
            v.Day3Low = this.GetMinLow(inds).Low;

            if (this.Indicators[index].Close.HasValue && this.Indicators[index + 3].Close.HasValue)
            {
                v.Day3Diff = 100 * (this.Indicators[index + 3].Close - this.Indicators[index].Close) / this.Indicators[index].Close;
            }

            inds = this.Indicators.SubArray(index + 1, 5);
            v.Day5High = this.GetMaxHigh(inds).High;
            v.Day5Low = this.GetMinLow(inds).Low;

            if (this.Indicators[index].Close.HasValue && this.Indicators[index + 5].Close.HasValue)
            {
                v.Day5Diff = 100 * (this.Indicators[index + 5].Close - this.Indicators[index].Close) / this.Indicators[index].Close;
            }

            inds = this.Indicators.SubArray(index + 1, 10);
            v.Day10High = this.GetMaxHigh(inds).High;
            v.Day10Low = this.GetMinLow(inds).Low;

            if (this.Indicators[index].Close.HasValue && this.Indicators[index + 10].Close.HasValue)
            {
                v.Day10Diff = 100 * (this.Indicators[index + 10].Close - this.Indicators[index].Close) / this.Indicators[index].Close;
            }

            inds = this.Indicators.SubArray(index + 1, 20);
            v.Day20High = this.GetMaxHigh(inds).High;
            v.Day20Low = this.GetMinLow(inds).Low;

            if (this.Indicators[index].Close.HasValue && this.Indicators[index + 20].Close.HasValue)
            {
                v.Day20Diff = 100 * (this.Indicators[index + 20].Close - this.Indicators[index].Close) / this.Indicators[index].Close;
            }

            v.StopLine = stopValue;

            v.MA5PeakDay = this.GetMA5PerkDay(inds);
            v.MA5BottomDay = this.GetMA5BottomDay(inds);
            v.MA10PeakDay = this.GetMA10PerkDay(inds);
            v.MA10BottomDay = this.GetMA10BottomDay(inds);

            if (this.Indicators[index].Close.HasValue && v.Day20High.HasValue)
            {
                v.MaxGain = 100 * (v.Day20High.Value - this.Indicators[index].Close) / this.Indicators[index].Close;
            }

            if (this.Indicators[index].Close.HasValue && v.Day20Low.HasValue)
            {
                v.MaxLoss = 100 * (v.Day20Low.Value - this.Indicators[index].Close) / this.Indicators[index].Close;
            }

            v.MaxGainDay = this.GetMaxGainDay(inds).Value;

            v.MaxLossDay = this.GetMaxLossDay(inds).Value;

            this.HasProfitted(this.Indicators[index], v, inds);

            return v;
        }

        private void HasProfitted(Screen2.Entity.Indicator ind, ScanVerification v, Screen2.Entity.Indicator[] inds)
        {
            v.HasProfitted = false;
            for(int i =0; i<inds.Length; i++)
            {
                if (inds[i].Low <v.StopLine)
                {
                    v.StopDay = i + 1;
                    break;
                }
                else if(inds[i].High > (profitValue))
                {
                    v.HasProfitted = true;

                    v.ProfitDay = i + 1;
                    break;
                }
            }

        }


        private int? GetMA5PerkDay(Screen2.Entity.Indicator[] inds)
        {
            var indPerk = inds.Where(c => c.SMA5.HasValue).OrderByDescending(c => c.SMA5).First();

            var index = this.GetDayByTradingDate(inds, indPerk);
            return index;
        }

        private int? GetMA5BottomDay(Screen2.Entity.Indicator[] inds)
        {
            var indBottom = inds.Where(c => c.SMA5.HasValue).OrderBy(c => c.SMA5).First();

            var index = this.GetDayByTradingDate(inds, indBottom);

            return index;
        }

        private int? GetMA10PerkDay(Screen2.Entity.Indicator[] inds)
        {
            var indPerk = inds.Where(c => c.SMA10.HasValue).OrderByDescending(c => c.SMA10).First();
            var index = this.GetDayByTradingDate(inds, indPerk);
            return index ;
        }

        private int? GetMA10BottomDay(Screen2.Entity.Indicator[] inds)
        {
            var indBottom = inds.Where(c => c.SMA10.HasValue).OrderBy(c => c.SMA10).First();

            var index = this.GetDayByTradingDate(inds, indBottom);

            return index;
        }

        private int? GetMaxGainDay(Screen2.Entity.Indicator[] inds)
        {
            var indPerk = this.GetMaxHigh(inds);
            var index = this.GetDayByTradingDate(inds, indPerk);
            return index;
        }

        private int? GetMaxLossDay(Screen2.Entity.Indicator[] inds)
        {
            var indBottom = inds.Where(c => c.Low.HasValue).OrderBy(c => c.Low).First();
            var index = this.GetDayByTradingDate(inds, indBottom);
            return index;
        }

        private void LoadShareList(Scan scan)
        {
            if (scan.ScopeType == "Stock")
            {
                this.ShareIdList = this.GetShareIDList(scan.ShareString);
            }
        }

        private int? GetDayByTradingDate(Screen2.Entity.Indicator[] inds, Screen2.Entity.Indicator ind)
        {
            int index = 0;

            for (int i = 0; i < inds.Length; i++)
            {
                if (inds[i].TradingDate == ind.TradingDate)
                {
                    index = i + 1;
                    break;
                }
            }

            return index;
        }


        private List<int> GetShareIDList(string shareString)
        {
            List<int> shareIdList = new List<int>();

            string[] shareIds = shareString.Split(';');

            for (int i = 0; i < shareIds.Length; i++)
            {
                Match m = Regex.Match(shareIds[i], @"\d+");

                if (m != null && m.Success)
                {
                    int shareID = int.Parse(m.Value);

                    shareIdList.Add(shareID);
                }
            }

            return shareIdList;
        }

        
        public Screen2.Entity.Indicator GetMaxHigh(Screen2.Entity.Indicator[] indArray)
        {
            return indArray.Where(c=>c.High.HasValue).OrderByDescending(c => c.High).First();
        }

        public Screen2.Entity.Indicator GetMinLow(Screen2.Entity.Indicator[] indArray)
        {
            return indArray.Where(c => c.Low.HasValue).OrderBy(c => c.Low).First();
        }

        public Entity.Indicator[] LoadIndicators(int shareID, int startDate, int? endDate)
        {
            Screen2.Entity.Indicator[] indicatorResult = null;

            // -1 for no criteria
            int end = -1;

            if (endDate.HasValue)
            {
                end = endDate.Value;
            }

            indicatorResult = new IndicatorBLL(_unit).GetIndicatorListByShareDate(shareID, startDate, end).ToArray();

            var tickers = this.GetTickers(shareID, startDate, end).ToArray();

            this.PopulateIndicatorList(indicatorResult, tickers);

            this.Indicators = indicatorResult;

            return indicatorResult;
        }

        private void PopulateIndicatorList(Screen2.Entity.Indicator[] indicators, Ticker[] tickers)
        {
            if (tickers.Length == indicators.Length)
            {
                for (int i = 0; i < tickers.Length; i++)
                {
                    if (tickers[i].TradingDate == indicators[i].TradingDate)
                    {
                        this.PopulateIndicator(indicators[i], tickers[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < indicators.Length; i++)
                {
                    var t = tickers.Where(c => c.TradingDate == indicators[i].TradingDate).SingleOrDefault();

                    if (t != null)
                    {
                        this.PopulateIndicator(indicators[i], t);
                    }
                }
            }
        }

        private void PopulateIndicator(Screen2.Entity.Indicator ind, Ticker ticker)
        {
            ind.Close = ticker.Close;
            ind.Open = ticker.Open;
            ind.High = ticker.High;
            ind.Low = ticker.Low;
            ind.Volumn = ticker.Volumn;
            ind.AdjustedClose = ticker.AdjustedClose;

        }

        private Ticker[] GetTickers(int shareID, int startDate, int? endDate)
        {
            Ticker[] tickerResult = null;

            // -1 for no criteria
            int end = -1;

            if (endDate.HasValue)
            {
                end = endDate.Value;
            }

            tickerResult = new TickerBLL(_unit).GetTickerListByShareDB(shareID, startDate, end).ToArray();

            return tickerResult;
        }


        #endregion
    }
}
