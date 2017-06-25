using NCalc;
using Screen2.Shared;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class ExpressionManager
    {
        public static Screen2.Entity.Indicator[] Indicators;
        public static int CurrentIndex;
        public static Screen2.Entity.Indicator CurrentIndicator;

        public ExpressionManager(Screen2.Entity.Indicator[] inds, int currentInd)
        {
            Indicators = inds;
            CurrentIndex = currentInd;
        }


        public static bool CheckIndicatorExpression(string formula)
        {
            bool isRuleMatched = false;
            Expression e = new Expression(formula, EvaluateOptions.IgnoreCase);

            e.EvaluateFunction += DelegateMethod;

            isRuleMatched = (bool)e.Evaluate();

            return isRuleMatched;
        }

        public static void DelegateMethod(string name, FunctionArgs args)
        {
            string indicatorName;
            Screen2.Entity.Indicator ind;
            int offset;
            switch (name.ToLower())
            {
                case "iv":
                case "ivalue":
                    indicatorName = args.Parameters[0].ParsedExpression.ToString();
                    ind = ExpressionManager.Indicators[ExpressionManager.CurrentIndex];

                    args.Result = ObjHelper.GetPropValue(ind, CalcHelper.PropName(indicatorName));
                    break;
                case "ivd":
                case "ivalue_d":
                    indicatorName = args.Parameters[0].ParsedExpression.ToString();
                    offset = int.Parse(args.Parameters[1].Evaluate().ToString());
                    ind = ExpressionManager.Indicators[ExpressionManager.CurrentIndex + offset];

                    args.Result = ObjHelper.GetPropValue(ind, CalcHelper.PropName(indicatorName));
                    break;
                //default:
                //    throw new ArgumentException(string.Format("custom function {0} is not defined", name));
            }
        }


    }
}
