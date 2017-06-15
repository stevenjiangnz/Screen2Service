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
    public class TradePosition:BaseEntity
    {
        public int Size { get; set; }
        public double EntryPrice { get; set; }
        public double EntryFee { get; set; }
        public double ExistFee { get; set; }
        public double ExistPrice { get; set; }
        public double CurrentPrice { get; set; }
        public double Margin { get; set; }
        public int CurrentTradingDate { get; set; } 
        public int? LastProcessedDate { get; set; }
        public double? Stop { get; set; }
        public double? Limit { get; set; }
        public int EntryTransactionId { get; set; }
        public int? ExitTransactionId { get; set; }
        public int? days { get; set; }
        public double? OtherCost { get; set; }

        [NotMapped]
        public double CurrentValue
        {
            get
            {
                return CurrentPrice * Size * Flag;
            }
        }


        [NotMapped]
        public double EntryCost
        {
            get
            {
                return Size * EntryPrice * Flag + EntryFee * Flag;
            }
        }

        [NotMapped]
        public int Flag { get
            {
                return Size > 0 ? 1 : -1;
            } }

        public string Source { get; set; }

        [NotMapped]
        public double Diff_V
        {
            get
            {
                return (CurrentValue - EntryCost) * Flag;
            }
        }

        [NotMapped]
        public double Diff
        {
            get
            {
                return 100 * Diff_V / EntryCost;
            }
        }

        public DateTime UpdateDT { get; set; }

        [Index]
        [Required]
        public int ShareId { get; set; }
        [XmlIgnore]
        public virtual Share Share { get; set; }


        [Required]
        [Index]
        public int AccountId { get; set; }
        [XmlIgnore]
        public virtual Account Account { get; set; }
        
    }
}
