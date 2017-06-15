using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class OutAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public int BrokerId { get; set; }
        public int? ZoneId { get; set; }
        public int? AccountId { get; set; }
        public int? AccountBalanceId { get; set; }
        public bool? IsTrackingAccount { get; set; }
        public double TotalBalance { get; set; }
        public double FundAmount { get; set; }
        public double FeeSum { get; set; }
        public double Reserve { get; set; }
        public double AvailableFund { get; set; }
        public double Margin { get; set; }
        public double PositionValue { get; set; }
        public double EntryValue { get; set; }
        public int? TradingDate { get; set; }
        public DateTime BalanceUpdateDT { get; set; }
    }
}
