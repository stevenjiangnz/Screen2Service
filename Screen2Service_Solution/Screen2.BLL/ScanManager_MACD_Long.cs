using System;
using Screen2.DAL.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screen2.Entity;

namespace Screen2.BLL
{
    public class ScanManager_MACD_Long : BaseStatScan
    {
        #region
        private double Stop_Level = 0.99;
        private string SET_Ref = "MACD_Long";
        private int WR_LEVEL = 80;
        #endregion

        #region Constructors
        public ScanManager_MACD_Long()
        {

        }

        public ScanManager_MACD_Long(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public override bool IsEntryCriteriaMatched(List<StatScanResultItem> resultList, Entity.Indicator[] indArray, Entity.Indicator ind, out double entryPrice)
        {
            bool isMatch = false;
            int index = base.GetIndex(indArray, ind);

            entryPrice = -1;

            if (index > 2 && indArray[index].MACD.HasValue && indArray[index - 1].MACD.HasValue &&
                indArray[index - 2].MACD.HasValue
                )
            {
                if (Math.Abs(indArray[index - 2].MACD.Value) <= Math.Abs(indArray[index - 1].MACD.Value) &&
                    Math.Abs(indArray[index].MACD.Value) <= Math.Abs(indArray[index - 1].MACD.Value) &&
                   (Math.Abs(indArray[index - 2].WR.Value) >= WR_LEVEL || Math.Abs(indArray[index - 1].WR.Value) >= WR_LEVEL) &&
                   indArray[index].Close.Value > indArray[index].SMA50.Value)
                {

                    isMatch = true;
                    entryPrice = (indArray[index].Open.Value + indArray[index].Close.Value) * 0.5;

                }
            }

            return isMatch;
        }

        public override bool IsLimitCriteriaMatched(StatScanResultItem resultItem, Entity.Indicator[] indArray, Entity.Indicator ind, out double limitPrice)
        {
            // Default Limit mechanism
            bool isMatch = false;
            limitPrice = -1;

            int index = base.GetIndex(indArray, ind);

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

            return isMatch;
        }

        public override bool IsStopCriteriaMatched(StatScanResultItem resultItem, Entity.Indicator[] indArray, Entity.Indicator ind, out double stopPrice)
        {
            //Default Stop mechanism
            bool isMatch = false;
            stopPrice = -1;
            int index = GetIndex(indArray, ind);

            if (!resultItem.stopLevel.HasValue)
            {
                // Initial Stop setup
                double stopLevel = indArray[index - 1].Low.Value < indArray[index - 2].Low.Value ? indArray[index - 1].Low.Value : indArray[index - 2].Low.Value;

                if (ind.Low.Value < stopLevel)
                {
                    resultItem.stopLevel = ind.Low.Value * Stop_Level;
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

            if (resultItem.stopLevel < resultItem.EntryPrice * Stop_Level)
            {
                resultItem.stopLevel = resultItem.EntryPrice * Stop_Level;
            }

            return isMatch;
        }

        public override bool IsDailyScanMatched(List<StatScanResultItem> resultList, Entity.Indicator[] indArray, Entity.Indicator ind, out double entryPrice)
        {
            bool isMatch = false;
            int index = base.GetIndex(indArray, ind);

            entryPrice = -1;

            if (index > 2 && indArray[index].MACD.HasValue && indArray[index - 1].MACD.HasValue &&
                indArray[index].MACD.HasValue)
            {
                if ((Math.Abs(indArray[index - 1].WR.Value) >= WR_LEVEL || Math.Abs(indArray[index].WR.Value) >= WR_LEVEL) )
                {

                    isMatch = true;
                    entryPrice = (indArray[index].Open.Value + indArray[index].Close.Value) * 0.5;

                }
            }

            return isMatch;
        }


        public override string GetSetRefName()
        {
            return SET_Ref;
        }

        public override int GetFlag()
        {
            return 1;
        }

        public override double GetStopLevel()
        {
            return Stop_Level;
        }
        #endregion
    }
}
