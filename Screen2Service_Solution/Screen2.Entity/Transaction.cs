using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class Transaction :BaseEntity
    {
        // long or short
        public string Direction { get; set; }
        public double Price { get; set; }
        public int Size { get; set; }
        public string Message { get; set; }
        public double Fee { get; set; }
        public string ModifiedBy { get; set; }
        public int TradingDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        [NotMapped]
        public int Flag
        {
            get
            {
                if (Direction.Equals("Short", StringComparison.InvariantCultureIgnoreCase))
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }

        [Index]
        public int TradeOrderId { get; set; }
        public virtual TradeOrder TradeOrder { get; set; }

        //[Index]
        //public int TradeSetID { get; set; }
        //public virtual TradeSet TradeSet { get; set; }
    }
}
