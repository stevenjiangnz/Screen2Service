using Screen2.DAL.Interface;
using Screen2.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class ScanManager_ADX_Long : BaseStatScan
    {
        #region
        private double Stop_Level = 0.99;
        private string SET_Ref = "ADX_Long";
        private int WR_LEVEL = 80;
        private double ADX_Low_Level = 20;
        private double Narrow_Level = 2;
        #endregion

        #region Constructors
        public ScanManager_ADX_Long()
        {
        }

        public ScanManager_ADX_Long(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public override bool IsEntryCriteriaMatched(List<StatScanResultItem> resultList, Entity.Indicator[] indArray, Entity.Indicator ind, out double entryPrice)
        {
            bool isMatch = false;
            int index = base.GetIndex(indArray, ind);

            entryPrice = -1;

            if (index > 4 && indArray[index - 4].ADX.HasValue &&
                indArray[index].ADX_Plus.Value > indArray[index].ADX_Minus.Value)
            {
                int days = 0;
                int bottomDays = 0;
                int wrDays = 0;

                if(Check_ADX_Below_Both_2days(indArray, index))
                {
                    if(((IsAdxFlappedWithinDays(indArray, index, out days) && days<= 3) ||
                        (IsAdxNarrowedWithinDays(indArray, index, out days) && days <= 3)) &&
                        (IsMacdBottomWithinDays(indArray, index, out bottomDays) && bottomDays < 4) ) 
                    {
                        double? latestItem = GetLastMatchDays(resultList, ind.ShareId, ind.TradingDate, 1);

                        Entity.Indicator indexInd = GetIndexIndicator(indArray[index - 1].TradingDate);

                        if (!(latestItem.HasValue && latestItem.Value <= 3) && 
                            (indexInd != null && indexInd.Heikin_Close >= indexInd.Heikin_Open))
                        {
                            if (ind.Low.Value <= ind.EMA20 && ind.High.Value >= ind.EMA20)
                            {
                                isMatch = true;
                                entryPrice = ind.EMA20.Value;
                            }
                        }
                    }
                }
            }

            return isMatch;
        }


        public bool Check_ADX_Below_Both_2days(Entity.Indicator[] indArray, int index)
        {
            bool matched = false;

            if(indArray[index].ADX.HasValue && indArray[index].ADX_Plus.HasValue && indArray[index].ADX_Minus.HasValue &&
                indArray[index-1].ADX.HasValue && indArray[index-1].ADX_Plus.HasValue && indArray[index-1].ADX_Minus.HasValue)
            {
                if(indArray[index].ADX.Value < indArray[index].ADX_Plus.Value && indArray[index].ADX.Value < indArray[index].ADX_Minus.Value &&
                   indArray[index-1].ADX.Value < indArray[index-1].ADX_Plus.Value && indArray[index].ADX.Value < indArray[index-1].ADX_Minus.Value &&
                   indArray[index].ADX.Value < ADX_Low_Level && indArray[index-1].ADX.Value < ADX_Low_Level)
                {
                    matched = true;
                }
            }

            return matched;
        }

        public bool IsAdxNarrowedWithinDays(Entity.Indicator[] indArray, int index, out int days)
        {
            bool matched = false;

            days = index - 1;

            for (int i = 0; i < index; i++)
            {
                if (Math.Abs(indArray[index -i].ADX_Plus.Value - indArray[index - i].ADX_Minus.Value) <= Narrow_Level)
                {
                    matched = true;
                    days = i;
                    break;
                }
            }

            return matched;
        }

        public bool IsMacdBottomWithinDays(Entity.Indicator[] indArray, int index, out int days)
        {
            bool matched = false;

            days = index - 1;

            for (int i = 0; i < index; i++)
            {
                if (indArray[index - i].MACD_Hist.Value <0 &&
                    Math.Abs(indArray[index - i -2].MACD.Value) < Math.Abs(indArray[index - i - 1].MACD.Value) &&
                    Math.Abs(indArray[index - i].MACD.Value) < Math.Abs(indArray[index - i - 1].MACD.Value))
                {
                    days = i-1;
                    matched = true;
                    break;
                }
            }

            return matched;
        }

        public bool IsWRWithinDays(Entity.Indicator[] indArray, int index, out int days)
        {
            bool matched = false;

            days = index - 1;

            for (int i = 0; i < index; i++)
            {
                if (Math.Abs(indArray[index-i].WR.Value)>WR_LEVEL)
                {
                    days = i;
                    matched = true;
                    break;
                }
            }

            return matched;
        }

        public override bool IsDailyScanMatched(List<StatScanResultItem> resultList, Entity.Indicator[] indArray, Entity.Indicator ind, out double entryPrice)
        {
            bool isMatch = false;
            int index = base.GetIndex(indArray, ind);

            entryPrice = -1;

            if (index > 4 && indArray[index - 4].ADX.HasValue)
            {
                int days = 0;

                if(IsAdxFlappedWithinDays(indArray, index, out days) )
                {
                    int maxIndex = 0;
                    double? maxADXDiff = 0;

                    for (int i = 0; i<days; i++)
                    {
                        if ((indArray[index -i].ADX_Plus - indArray[index -i].ADX_Minus)> maxADXDiff)
                        {
                            maxADXDiff = indArray[index-i].ADX_Plus - indArray[index-i].ADX_Minus;
                            maxIndex = index - i;
                        }
                    }

                    if (maxIndex != index) 
                    {
                        double diff = indArray[index].ADX_Plus.Value - indArray[index].ADX_Minus.Value;

                        if(diff >0 && diff < Narrow_Level)
                        {
                            isMatch = true;
                        }
                    }
                }

                if (!isMatch)
                {
                    if ((indArray[index - 1].ADX_Plus <= indArray[index - 1].ADX_Minus) &&
                        (indArray[index].ADX_Plus >= indArray[index].ADX_Minus))
                    {
                        isMatch = true;
                    }
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
