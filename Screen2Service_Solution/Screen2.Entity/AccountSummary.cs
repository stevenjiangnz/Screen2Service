using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class AccountSummary
    {
        public Account Account { get; set; }
        public AccountBalance Balance { get; set; }
        public List<TradeOrder> OpenOrders { get; set; }
        public List<TradePosition> CurrentPositions { get; set; }

        public double OrderFeeSum
        {
            get
            {
                double v = 0;
                if (OpenOrders != null)
                {
                    foreach (var o in OpenOrders)
                    {
                        v += o.Fee;
                    }
                }

                return v;
            }
        }

        public double OrderValueSum
        {
            get
            {
                double v = 0;
                if (OpenOrders != null)
                {
                    foreach (var o in OpenOrders)
                    {
                        v += o.OrderValue;
                    }
                }

                return v;
            }
        }

        public double OrderReserveSum
        {
            get
            {
                double v = 0;
                if (OpenOrders != null)
                {
                    foreach (var o in OpenOrders)
                    {
                        v += o.Reserve;
                    }
                }

                return v;
            }
        }


        public double PositionEntrySum
        {
            get
            {
                double v = 0;
                if (CurrentPositions != null)
                {
                    foreach (var o in CurrentPositions)
                    {
                        v += o.EntryCost;
                    }
                }

                return v;
            }
        }


        public double PositionValueSum
        {
            get
            {
                double v = 0;
                if (CurrentPositions != null)
                {
                    foreach (var o in CurrentPositions)
                    {
                        v += o.CurrentValue;
                    }
                }

                return v;
            }
        }

        public double PositionMarginSum
        {
            get
            {
                double v = 0;
                if (CurrentPositions != null)
                {
                    foreach (var o in CurrentPositions)
                    {
                        v += o.Margin;
                    }
                }

                return v;
            }
        }

        public double PositionDiffSum
        {
            get
            {
                double v = 0;
                if (CurrentPositions != null)
                {
                    foreach (var o in CurrentPositions)
                    {
                        v += (o.Diff_V);
                    }
                }

                return v;
            }
        }

        public double PositionDiffSumPercent
        {
            get
            {
                if(PositionEntrySum != 0)
                {
                    return 100 * PositionDiffSum / PositionEntrySum;
                }
                else
                {
                    return 0;
                }
            }
        }

    }
}
