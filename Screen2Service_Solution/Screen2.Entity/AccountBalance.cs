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
    public class AccountBalance : BaseEntity
    {
        public double FundAmount { get; set; }
        public double AvailableFund { get; set; }
        public double FeeSum { get; set; }
        public int? TradingDate { get; set; }
        public DateTime UpdateDT { get; set; }

        [NotMapped]
        public double TotalBalance { get {
                return AvailableFund + Reserve + Margin;
            } }
        [NotMapped]
        public double Margin { get; set; }
        [NotMapped]
        public double Reserve { get; set; }
        [NotMapped]
        public double PositionValue { get; set; }

        public double PL
        {
            get
            {
                return TotalBalance - FundAmount;
            }
        }

        public double PLPercent
        {
            get
            {
                if (TotalBalance != 0)
                {
                    return 100 * (TotalBalance - FundAmount) / FundAmount;
                }
                else
                {
                    return 0;
                }
            }
        }

        [Required]
        [Index]
        public int AccountId { get; set; }
        [XmlIgnore]
        public virtual Account Account { get; set; }
    }
}
