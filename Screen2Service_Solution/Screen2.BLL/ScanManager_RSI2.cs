using Screen2.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screen2.Entity;
using Screen2.Indicator;

namespace Screen2.BLL
{
    public class ScanManager_RSI2 : BaseStatScan
    {

        #region Constructors
        public ScanManager_RSI2()
        {

        }

        public ScanManager_RSI2(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region
        public override void PopulateExtendedIndicators(Entity.Indicator[] indArray)
        {
            double[] close = new double[indArray.Length];
            double?[] rsiOutput = new double?[indArray.Length];
            double?[] rsiOutput_Acc = new double?[indArray.Length];
            for (int i = 0; i < indArray.Length; i++)
            {
                if (indArray[i].Close.HasValue)
                {
                    close[i] = indArray[i].Close.Value;
                }
                else
                {
                    close[i] = 0;
                }
            }

            RSI.Calculate(close, 2, rsiOutput);

            GenericHelper.GetAccumulate(rsiOutput, 2, 2, rsiOutput_Acc);

            for (int i = 0; i < indArray.Length; i++)
            {
                indArray[i].ExtendedIndicators = new Dictionary<string, double?>();

                indArray[i].ExtendedIndicators.Add("RSI2", rsiOutput[i]);
                indArray[i].ExtendedIndicators.Add("RSI2ACC", rsiOutput_Acc[i]);
            }
        }


        public override bool IsEntryCriteriaMatched(List<StatScanResultItem> resultList, Entity.Indicator[] indArray, Entity.Indicator ind, out double entryPrice)
        {
            bool isMatch = false;
            int targetLevel = 10;

            entryPrice = -1;

            if(ind.ExtendedIndicators["RSI2"].HasValue && ind.ExtendedIndicators["RSI2ACC"].HasValue)
            {
                if (ind.Close > ind.SMA200 &&
                    ind.ExtendedIndicators["RSI2"].Value < targetLevel)
                {
                    isMatch = true;
                    entryPrice = ind.Close.Value;
                }
            }

            return isMatch;
        }


        public override bool IsStopCriteriaMatched(StatScanResultItem resultItem, Entity.Indicator[] indArray, Entity.Indicator ind, out double stopPrice)
        {
            bool isMatch = false;
            stopPrice = -1;
            int index = base.GetIndex(indArray, ind);

            if (!resultItem.stopLevel.HasValue)
            {
                // Initial Stop setup
                double stopLevel = indArray[index - 1].Low.Value < indArray[index - 2].Low.Value ? indArray[index - 1].Low.Value : indArray[index - 2].Low.Value;

                if (ind.Low.Value < stopLevel)
                {
                    resultItem.stopLevel = ind.Low.Value * 0.98;
                    //stopPrice = stopLevel;
                    //isMatch = true;
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

                    if(stopLevel > resultItem.stopLevel.Value)
                    {
                        resultItem.stopLevel = stopLevel;
                    }
                }

            }

            return isMatch;
        }


        public override bool IsLimitCriteriaMatched(StatScanResultItem resultItem, Entity.Indicator[] indArray, Entity.Indicator ind, out double limitPrice)
        {
            bool isMatch = false;
            limitPrice = -1;


            if (!resultItem.LimitLevel.HasValue)
            {
                if (ind.High.Value > (ind.BB_High.Value + 0.01))
                {
                    limitPrice = ind.SMA5.Value;
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
                    (ind.High.Value > ind.SMA5.Value && ind.Low.Value <= ind.SMA5.Value))
                {
                    limitPrice = ind.SMA5.Value;
                    isMatch = true;
                }
                else
                {
                    resultItem.LimitLevel = ind.BB_High;
                }
            }

            return isMatch;
        }


        public override bool IsDailyScanMatched(List<StatScanResultItem> resultList, Entity.Indicator[] indArray, Entity.Indicator ind, out double entryPrice)
        {
            return IsEntryCriteriaMatched(resultList, indArray, ind, out entryPrice);
        }


        public override int GetFlag()
        {
            return 1;
        }

        public override string GetSetRefName()
        {
            return "RSI2_1";
        }

        public override double GetStopLevel()
        {
            return 0.99;
        }
        #endregion
    }
}
