using Screen2.DAL.Interface;
using Screen2.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class ScanManager_BreakOut_Long : BaseStatScan
    {
        #region
        private double Stop_Level = 0.99;
        private string SET_Ref = "Breakout_Long";
        private double ADX_Narrow = 3;
        #endregion

        #region Constructors
        public ScanManager_BreakOut_Long()
        {

        }

        public ScanManager_BreakOut_Long(IUnitWork unit) : base(unit)
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
                if (indArray[index].ADX_Plus > indArray[index].ADX_Minus &&
                    indArray[index-2].ADX_Minus >= indArray[index - 2].ADX &&
                    indArray[index-1].ADX_Minus <= indArray[index -1].ADX)
                {

                    isMatch = true;
                    entryPrice = (indArray[index].Open.Value + indArray[index].Close.Value) * 0.5;

                }
            }

            return isMatch;
        }

        public override bool IsDailyScanMatched(List<StatScanResultItem> resultList, Entity.Indicator[] indArray, Entity.Indicator ind, out double entryPrice)
        {
            bool isMatch = false;
            int index = base.GetIndex(indArray, ind);

            entryPrice = -1;

            if (index > 2 && indArray[index].MACD.HasValue && indArray[index - 1].MACD.HasValue &&
                indArray[index].MACD.HasValue &&
                    indArray[index].ADX_Plus > indArray[index].ADX_Minus)
            {
                if (indArray[index - 1].ADX <= indArray[index - 2].ADX &&
                    indArray[index].ADX >= indArray[index - 1].ADX)
                {
                    isMatch = true;
                }

                if (!isMatch)
                {
                    if (((indArray[index - 1].ADX_Minus.Value - indArray[index - 1].ADX.Value) < (indArray[index].ADX_Minus.Value - indArray[index].ADX.Value)) &&
                       Math.Abs(indArray[index].ADX_Minus.Value - indArray[index].ADX.Value) < ADX_Narrow)
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

