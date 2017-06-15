using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class AccountBalanceJourney : BaseEntity
    {
        public double FundAmount { get; set; }
        public double TotalBalance { get; set; }
        public double AvailableFund { get; set; }
        public double Margin { get; set; }
        public double Reserve { get; set; }
        public double PositionValue { get; set; }
        public double FeeSum { get; set; }
        public int? TradingDate { get; set; }
        public DateTime UpdateDT { get; set; }
        public string Action { get; set; }
        public string AccountSummaryXML { get; set; }

        public int? TradeSetId { get; set; }

        [Index]
        public int? TransactionId { get; set; }

        [Index]
        public int? OrderId { get; set; }

        [Required]
        [Index]
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}
