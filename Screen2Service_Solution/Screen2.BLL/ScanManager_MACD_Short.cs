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
    public class ScanManager_MACD_Short : BaseStatScan
    {
        #region
        private double Stop_Level = 1.01;
        private string SET_Ref = "MACD_Short";
        private double WR_Level = 15;
        #endregion

        #region Constructors
        public ScanManager_MACD_Short()
        {

        }

        public ScanManager_MACD_Short(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public override bool IsEntryCriteriaMatched(List<StatScanResultItem> resultList, Entity.Indicator[] indArray, Entity.Indicator ind, out double entryPrice)
        {
            bool isMatch = false;
            int index = base.GetIndex(indArray, ind);

            entryPrice = -1;

            if (index > 1 && indArray[index].MACD.HasValue && indArray[index - 1].MACD.HasValue &&
                indArray[index - 2].MACD.HasValue
                )
            {
                if (Math.Abs(indArray[index - 2].MACD.Value) <= Math.Abs(indArray[index - 1].MACD.Value) &&
                    Math.Abs(indArray[index].MACD.Value) <= Math.Abs(indArray[index - 1].MACD.Value) &&
                   (Math.Abs(indArray[index - 1].WR.Value) <= WR_Level || Math.Abs(indArray[index - 1].WR.Value) <= WR_Level) &&
                   indArray[index].Close.Value < indArray[index].SMA50.Value)
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
                if (ind.Low.Value < (ind.BB_Low.Value - 0.01))
                {
                    if(ind.SMA5.Value > ind.Low.Value && ind.SMA5.Value < ind.High.Value)
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

            return isMatch;
        }

       

        public override bool IsDailyScanMatched(List<StatScanResultItem> resultList, Entity.Indicator[] indArray, Entity.Indicator ind, out double entryPrice)
        {
            bool isMatch = false;
            int index = base.GetIndex(indArray, ind);

            entryPrice = -1;

            if (index > 1 && indArray[index].MACD.HasValue && indArray[index - 1].MACD.HasValue &&
                indArray[index].MACD.HasValue
                )
            {
                if ((Math.Abs(indArray[index].WR.Value) <= WR_Level || Math.Abs(indArray[index - 1].WR.Value) <= WR_Level) )
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
            return -1;
        }

        public override double GetStopLevel()
        {
            return Stop_Level;
        }
        #endregion

    }
}
