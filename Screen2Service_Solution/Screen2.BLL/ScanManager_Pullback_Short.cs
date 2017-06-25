using Screen2.DAL.Interface;
using Screen2.Entity;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class ScanManager_Pullback_Short : BaseStatScan
    {
        #region
        private double Stop_Level = 1.015;
        private string SET_Ref = "PullBack_Short";

        #endregion

        #region Constructors
        public ScanManager_Pullback_Short()
        {
        }

        public ScanManager_Pullback_Short(IUnitWork unit) : base(unit)
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
                indArray[index - 1].ADX_Plus.Value < indArray[index - 1].ADX.Value &&
                indArray[index - 1].ADX.Value < indArray[index - 1].ADX_Minus.Value
                &&
                indArray[index - 2].ADX < indArray[index - 1].ADX)
            {
                int days = 0;
                if ((IsAdxFlappedWithinDays(indArray, index, out days) && days > 3) &&
                                (indArray[index - 1].MACD < indArray[index - 1].MACD_Signal))
                {
                    double? entry = (indArray[index - 1].SMA10 + indArray[index - 1].EMA20) * 0.502;
                    if (entry.HasValue &&
                            entry.Value <= indArray[index].High.Value && entry.Value >= indArray[index].Low.Value)
                    {
                        isMatch = true;
                        entryPrice = entry.Value;
                    }
                }
            }

            return isMatch;
        }


        public bool IsAdxBreakoutWithinDays(Entity.Indicator[] indArray, int index, out int days)
        {
            bool matched = false;
            days = index - 1;

            for (int i = 1; i < index; i++)
            {
                if(indArray[index - i].ADX.Value > indArray[index - i].ADX_Plus.Value &&
                    indArray[index - i -1].ADX.Value < indArray[index - i -1].ADX_Plus.Value)
                { 
                    matched = true;
                    days = i;
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

            if (index > 4 && indArray[index - 4].ADX.HasValue &&
                indArray[index].ADX_Plus.Value < indArray[index].ADX_Minus.Value)
            {
                int days = 0;

                if (IsAdxFlappedWithinDays(indArray, index, out days) && days > 3)
                {
                    double? entry = (indArray[index].SMA10 + indArray[index].EMA20) * 0.5;
                    if (entry.HasValue &&
                            entry.Value <= indArray[index].High.Value && entry.Value >= indArray[index].Low.Value)
                    {
                        isMatch = true;
                        entryPrice = entry.Value;
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
            return -1;
        }

        public override double GetStopLevel()
        {
            return Stop_Level;
        }
        #endregion
    }
}