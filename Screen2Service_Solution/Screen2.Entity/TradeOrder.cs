using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Screen2.Entity
{
    public class TradeOrder : BaseEntity
    {
        public double LatestPrice { get; set; }
        public int LatestTradingDate { get; set; }
        public int? ProcessedTradingDate { get; set; }
        public int TradingOrderDate { get; set; }
        public double OrderPrice { get; set; }
        public int Size { get; set; }
        public string Direction { get; set; } //
        public string OrderType { get; set; }
        public double? Stop { get; set; }
        public double? Limit { get; set; }
        public double Reserve { get; set; }
        public double Fee { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedBy { get; set; }

        public string Source { get; set; }
        public string Reason { get; set; }

        [NotMapped]
        public double OrderValue
        {
            get
            {
                return OrderPrice * Size;
            }
        }

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

        [Required]
        [Index]
        public int AccountId { get; set; }
        [XmlIgnore]
        public virtual Account Account { get; set; }

        [Required]
        [Index]
        public int ShareId { get; set; }
        [XmlIgnore]
        public virtual Share Share { get; set; }
    }
}
