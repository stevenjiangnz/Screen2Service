using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class OutPosition
    {
        public int Id { get; set; }
        public int ShareId { get; set; }
        public int Size { get; set; }
        public int EntryDate { get; set; }
        public double EntryPrice { get; set; }
        public double EntryFee { get; set; }
        public int EntryTransactionId { get; set; }
        public int? ExitDate { get; set; }
        public double? ExitPrice { get; set; }
        public double? ExitFee { get; set; }
        public int? ExitTransactionId { get; set; }
        public int? Days { get; set; }

        public int Flag
        {
            get
            {
                if (Size > 0)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }

        public double? Diff
        {
            get
            {
                double? di = null;

                if (ExitDate.HasValue && ExitPrice.HasValue)
                {
                    di = (ExitPrice - EntryPrice) * Size - EntryFee - ExitFee;
                }

                return di;

            }
        }

        public double? Diff_Per
        {
            get
            {
                return 100 * Diff / (Math.Abs(Size) * EntryPrice);
            }
        }
    }
}
