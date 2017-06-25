using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
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
    public abstract class BaseStatScan //:BaseBLL<WatchList>, IBaseBLL<WatchList>
    {
        #region Fields
        private int OFFSET_DAY = 30;
        private int VERIFY_DAY = 30;
        private double stopValue;
        private double profitValue;
        private int? startDate;
        private int? endDate;

        public Entity.Indicator[] Indicators;
        public int CurrentIndex;
        public Entity.Indicator CurrentIndicator;
        
        public ScanCalculator scanCalculator;
        public Entity.Indicator[] IndexInds;

        protected IUnitWork _unit;
        protected static log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected string _connectionString;
        protected int _CommandTimeout = 600;

        #endregion

        #region Constructors
        public BaseStatScan()
        {
        }

        public BaseStatScan(IUnitWork unit) //: base(unit)
        {
            InitUnit(unit);
        }
        #endregion

        public void InitUnit(IUnitWork unit)
        {
            _unit = unit;
            _connectionString = ConfigurationManager.ConnectionStrings["Screen2Connection"].ConnectionString;
            scanCalculator = new ScanCalculator(unit);
        }

        public virtual bool IsMacdPeekWithinDays(Entity.Indicator[] indArray, int index, out int days)
        {
            bool matched = false;

            days = index - 1;

            for (int i = 0; i < index; i++)
            {
                if (indArray[index - i].MACD_Hist.Value > 0 &&
                    Math.Abs(indArray[index - i - 2].MACD.Value) < Math.Abs(indArray[index - i - 1].MACD.Value) &&
                    Math.Abs(indArray[index - i].MACD.Value) < Math.Abs(indArray[index - i - 1].MACD.Value))
                {
                    days = i - 1;
                    matched = true;
                    break;
                }
            }

            return matched;
        }
        public virtual List<DailyScanResultItem> DailyScanByWatch(int watchId, int tradingDate)
        {
            List<DailyScanResultItem> resultList = new List<DailyScanResultItem>();
            List<int> ShareIdList = new WatchListDetailBLL(_unit).GetShareIdsByWatchID(watchId);

            this.startDate = tradingDate;

            var loadStart = DateHelper.DateToInt(DateHelper.IntToDate(tradingDate).AddDays(OFFSET_DAY * (-1)));

            foreach (int shareId in ShareIdList)
            {
                try
                {
                    Entity.Indicator[] indList = scanCalculator.LoadIndicators(shareId, loadStart, tradingDate);
                    PopulateExtendedIndicators(indList);

                    int len = indList.Length;
                    double entryPrice = -1;

                    if (indList.Length > 0)
                    {
                        if (IsDailyScanMatched(null, indList, indList[len - 1], out entryPrice))
                        {
                            DailyScanResultItem item = new DailyScanResultItem();

                            item.EntryPrice = entryPrice;
                            item.IsMatch = true;
                            item.ProcessDT = DateTime.Now;
                            item.SetRef = GetSetRef();
                            item.ShareId = indList[len - 1].ShareId;
                            item.TradingDate = indList[len - 1].TradingDate;

                            resultList.Add(item);
                        }
                    }
                }
                catch(Exception ex)
                {
                    LogHelper.Error(_log,string.Format("Error process daily scan {0}. ", shareId), ex);
                }
            }

            return resultList;
        }

        public void LoadIndex(int loadStart, int tradingDate)
        {
            int indexId = 2433;
            IndexInds = scanCalculator.LoadIndicators(indexId, loadStart, tradingDate);
            PopulateExtendedIndicators(IndexInds);
        }

        public Entity.Indicator GetIndexIndicator(int tradingDate)
        {
            Entity.Indicator indexInd = null;

            indexInd = this.IndexInds.SingleOrDefault(p => p.TradingDate == tradingDate);

            return indexInd;
        }

        public virtual bool IsAdxFlappedWithinDays(Entity.Indicator[] indArray, int index, out int days)
        {
            bool matched = false;
            days = index - 1;

            for (int i = 0; i < index; i++)
            {
                int flag = indArray[index - i].ADX_Plus.Value > indArray[index - i].ADX_Minus.Value ? 1 : -1;
                int flag_Prev = indArray[index - i - 1].ADX_Plus.Value > indArray[index - i - 1].ADX_Minus.Value ? 1 : -1;

                if (flag * flag_Prev < 0)
                {
                    matched = true;
                    days = i;
                    break;
                }
            }

            return matched;
        }

        public virtual List<StatScanResultItem> ScanWatchShares(int watchId, int startDate, int endDate)
        {
            List<StatScanResultItem> resultList = new List<StatScanResultItem>();
            List<int> ShareIdList = new WatchListDetailBLL(_unit).GetShareIdsByWatchID(watchId);
            StatScanSetBLL ssBLL = new StatScanSetBLL(_unit);

            this.startDate = startDate;
            this.endDate = endDate;

            //RemoveTradeSimulatorOrders();

            LoadIndex(startDate, endDate);

            StatScanSet sst = new StatScanSet()
            {
                WatchId = watchId,
                StartDate = startDate,
                EndDate = endDate,
                SetRef = GetSetRefName(),
                EntryLogic = GetEntryLogic(),
                Notes = GetNotes(),
                StartDt = DateTime.Now,
            };

            ssBLL.Create(sst);

            foreach (int shareId in ShareIdList)
            {
                List<StatScanResultItem> shareResultList = ScanShare(sst, shareId, startDate, endDate);

                resultList.AddRange(shareResultList);
            }

            sst.CompleteDt = DateTime.Now;

            ssBLL.Update(sst);

            return resultList;
        }


        public double? GetLastMatchDays(List<StatScanResultItem> resultList, int shareId, int tradingDate , int flag)
        {
            double? days = null;
            StatScanResultItem latestItem = GetLastMatch(resultList, shareId, tradingDate, flag);

            if(latestItem != null)
            {
                DateTime lastDT = DateHelper.IntToDate(latestItem.EntryTradingDate);
                DateTime currentDT = DateHelper.IntToDate(tradingDate);

                days = (currentDT - lastDT).TotalDays;
            }

            return days;
        }

        public StatScanResultItem GetLastMatch(List<StatScanResultItem> resultList, int shareId, int tradingDate, int flag)
        {
            StatScanResultItem latestItem = null;

            var items = resultList.Where(p => p.ShareId == shareId && 
            p.Flag ==flag).OrderByDescending(p => p.EntryTradingDate).ToList();

            if(items.Count > 0)
            {
                latestItem = items[0];
            }

            return latestItem;
        }

        public List<StatScanResultItem> ScanShare(StatScanSet scanSet, int shareId, int startDate, int endDate)
        {
            TradeSimulateOrderBLL tsBll = new TradeSimulateOrderBLL(_unit);
            List<StatScanResultItem> resultItems = new List<StatScanResultItem>();
            var loadStart = DateHelper.DateToInt(DateHelper.IntToDate(startDate).AddDays(OFFSET_DAY * (-1)));

            this.startDate = startDate;
            this.endDate = endDate;

            Entity.Indicator[] indList = scanCalculator.LoadIndicators(shareId, startDate, endDate);

            PopulateExtendedIndicators(indList);

            resultItems = GetMatchResult(indList);

            foreach (var item in resultItems)
            {
                item.StatScanSetId = scanSet.Id;
                tsBll.AddSOrder(item);
            }

            return resultItems;
        }

        public List<StatScanResultItem> GetMatchResult(Entity.Indicator[] indList)
        {
            List<StatScanResultItem> resultList = new List<StatScanResultItem>();

            for (int i = 0; i < indList.Length; i++)
            {
                try
                {
                    double entryPrice;
                    if (IsEntryCriteriaMatched(resultList, indList, indList[i], out entryPrice))
                    {
                        StatScanResultItem matchItem = new StatScanResultItem();
                        double highest;
                        double lowest;
                        int abovedays;

                        matchItem.SetRef = GetSetRef();
                        matchItem.Flag = GetFlag();
                        matchItem.EntryIndicator = indList[i];
                        matchItem.EntryPrice = entryPrice;
                        this.GetDay5Results(indList, i, entryPrice, out highest, out lowest, out abovedays);
                        matchItem.Day5Highest = highest;
                        matchItem.Day5Lowest = lowest;
                        matchItem.Day5AboveDays = abovedays;

                        SimulateTrade(matchItem, indList, i);

                        resultList.Add(matchItem);
                    } }
                catch (Exception ex) {
                }
            }

            return resultList;
        }

        public void GetDay5Results(Entity.Indicator[] indList, int index, double entryPrice, out double highest,out double lowest, out int aboveDays)
        {
            highest = 0;
            lowest = 10000;
            aboveDays = 0;

            int end = index + 5;

            if (end > indList.Length)
                end = indList.Length;

            for(int i = index; i<end; i++)
            {
                if (indList[i].High.Value > highest)
                    highest = indList[i].High.Value;

                if (indList[i].Low.Value < lowest)
                    lowest = indList[i].Low.Value;

                if (indList[i].Close.Value > entryPrice)
                    aboveDays++;
            }
        }

        public virtual void SimulateTrade(StatScanResultItem scanResult, Entity.Indicator[] indList, int offset)
        {
            double stopPrice = -1;
            double limitPrice = -1;

            for(int i=offset; i< indList.Length; i++)
            {
                if(IsStopCriteriaMatched(scanResult, indList, indList[i], out stopPrice))
                {
                    scanResult.StopIndicator = indList[i];
                    scanResult.StopPrice = stopPrice;
                    scanResult.ExitMode = "Stop";
                    break;
                }

                if (IsLimitCriteriaMatched(scanResult, indList, indList[i], out limitPrice))
                {
                    scanResult.LimitIndicator = indList[i];
                    scanResult.LimitPrice = limitPrice;
                    scanResult.ExitMode = "Limit";
                    break;
                }
            }
        }

        public virtual int GetIndex(Entity.Indicator[] indArray, Entity.Indicator ind)
        {
            int index = -1;

            for(int i =0; i< indArray.Length; i++)
            {
                if(indArray[i].ShareId == ind.ShareId && indArray[i].TradingDate == ind.TradingDate)
                {
                    index = i;
                    break;
                }
            }

            return index;
        } 

        public virtual void PopulateExtendedIndicators(Entity.Indicator[] indArray)
        {

        }

        public virtual bool IsStopCriteriaMatched(StatScanResultItem resultItem, Entity.Indicator[] indArray, Entity.Indicator ind, out double stopPrice)
        {
            //Default Stop mechanism
            bool isMatch = false;
            stopPrice = -1;
            int index = GetIndex(indArray, ind);

            if(resultItem.Flag > 0)
            {
                if (!resultItem.stopLevel.HasValue)
                {
                    // Initial Stop setup
                    double stopLevel = indArray[index - 1].Low.Value < indArray[index - 2].Low.Value ? indArray[index - 1].Low.Value : indArray[index - 2].Low.Value;

                    if (ind.Low.Value < stopLevel)
                    {
                        resultItem.stopLevel = ind.Low.Value * GetStopLevel();
                    }
                    else
                    {
                        resultItem.stopLevel = stopLevel;
                    }
                }
                else
                {
                    // Second & afterwards
                    if (ind.Low.Value < resultItem.stopLevel.Value)
                    {
                        stopPrice = resultItem.stopLevel.Value;
                        isMatch = true;
                    }
                    else
                    {
                        double stopLevel = indArray[index - 1].Low.Value < indArray[index - 2].Low.Value ? indArray[index - 1].Low.Value : indArray[index - 2].Low.Value;

                        if (stopLevel > resultItem.stopLevel.Value)
                        {
                            resultItem.stopLevel = stopLevel;
                        }
                    }
                }

                if (resultItem.stopLevel < resultItem.EntryPrice * GetStopLevel())
                {
                    resultItem.stopLevel = resultItem.EntryPrice * GetStopLevel();
                }
            }
            else
            {
                if (!resultItem.stopLevel.HasValue)
                {
                    // Initial Stop setup
                    double stopLevel = indArray[index - 1].High.Value > indArray[index - 2].High.Value ? indArray[index - 1].High.Value : indArray[index - 2].High.Value;

                    if (ind.High.Value > stopLevel)
                    {
                        resultItem.stopLevel = ind.High.Value * GetStopLevel();
                    }
                    else
                    {
                        resultItem.stopLevel = stopLevel;
                    }
                }
                else
                {
                    // Second & afterwards
                    if (ind.High.Value > resultItem.stopLevel.Value)
                    {
                        stopPrice = resultItem.stopLevel.Value;
                        isMatch = true;
                    }
                    else
                    {
                        double stopLevel = indArray[index - 1].High.Value > indArray[index - 2].High.Value ? indArray[index - 1].High.Value : indArray[index - 2].High.Value;

                        if (stopLevel < resultItem.stopLevel.Value)
                        {
                            resultItem.stopLevel = stopLevel;
                        }
                    }
                }

                if (resultItem.stopLevel > resultItem.EntryPrice * GetStopLevel())
                {
                    resultItem.stopLevel = resultItem.EntryPrice * GetStopLevel();
                }

            }

            return isMatch;
        }

        public virtual bool IsLimitCriteriaMatched(StatScanResultItem resultItem, Entity.Indicator[] indArray, Entity.Indicator ind, out double limitPrice)
        {
            // Default Limit mechanism
            bool isMatch = false;
            limitPrice = -1;

            int index = GetIndex(indArray, ind);

            if(resultItem.Flag > 0)
            {
                if (!resultItem.LimitLevel.HasValue)
                {
                    if (ind.High.Value > (ind.BB_High.Value + 0.01))
                    {
                        if (ind.SMA5.Value > ind.Low.Value && ind.SMA5.Value < ind.High.Value)
                        {
                            limitPrice = ind.SMA5.Value;
                        }
                        else
                        {
                            limitPrice = ind.Close.Value;
                        }

                        isMatch = true;
                    }
                    else
                    {
                        resultItem.LimitLevel = ind.BB_High;
                    }
                }
                else
                {
                    if ((ind.High.Value > resultItem.LimitLevel.Value) ||
                        (indArray[index].MACD_Hist.Value < 0 &&
                         Math.Abs(indArray[index - 2].MACD_Hist.Value) <= Math.Abs(indArray[index - 1].MACD_Hist.Value) &&
                    Math.Abs(indArray[index].MACD_Hist.Value) <= Math.Abs(indArray[index - 1].MACD_Hist.Value)))
                    {
                        limitPrice = ind.Close.Value;
                        isMatch = true;
                    }
                    else
                    {
                        resultItem.LimitLevel = ind.BB_High;
                    }
                }
            }
            else
            {
                if (!resultItem.LimitLevel.HasValue)
                {
                    if (ind.Low.Value < (ind.BB_Low.Value - 0.01))
                    {
                        if (ind.SMA5.Value > ind.Low.Value && ind.SMA5.Value < ind.High.Value)
                        {
                            limitPrice = ind.SMA5.Value;
                        }
                        else
                        {
                            limitPrice = ind.Close.Value;
                        }
                        isMatch = true;
                    }
                    else
                    {
                        resultItem.LimitLevel = ind.BB_Low;
                    }
                }
                else
                {
                    if ((ind.Low.Value < resultItem.LimitLevel.Value) ||
                        (indArray[index].MACD_Hist.Value < 0 &&
                         Math.Abs(indArray[index - 2].MACD_Hist.Value) <= Math.Abs(indArray[index - 1].MACD_Hist.Value) &&
                    Math.Abs(indArray[index].MACD_Hist.Value) <= Math.Abs(indArray[index - 1].MACD_Hist.Value)))
                    {
                        limitPrice = ind.Close.Value;
                        isMatch = true;
                    }
                    else
                    {
                        resultItem.LimitLevel = ind.BB_Low;
                    }
                }
            }

            return isMatch;
        }

        public virtual void RemoveTradeSimulatorOrders(int statScanId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_RemoveTradeSimulateOrders", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("StatScanSetId", SqlDbType.Int).Value = statScanId;

                        cmd.Connection.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error USP_RemoveTradeSimulateOrders to DB. ", ex);
                throw;
            }
        }

        public string GetSetRef()
        {
            string name = GetSetRefName();

            if(this.startDate.HasValue)
            {
                name += "_" + this.startDate.Value.ToString();
            }

            if (this.endDate.HasValue)
            {
                name += "_" + this.endDate.Value.ToString();
            }

            return name;
        }

        public abstract bool IsEntryCriteriaMatched(List<StatScanResultItem> resultList, Entity.Indicator[] indArray, Entity.Indicator ind, out double entryPrice);
        public abstract bool IsDailyScanMatched(List<StatScanResultItem> resultList, Entity.Indicator[] indArray, Entity.Indicator ind, out double entryPrice);
        public abstract int GetFlag();
        public abstract string GetSetRefName();
        public abstract double GetStopLevel();
        public virtual string GetEntryLogic()
        {
            return "";
        }
        public virtual string GetNotes()
        {
            return "";
        }

    }

}
