using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class OutTransaction
    {
        public int Id { get; set; }
        public string Direction { get; set; }
        public double Price { get; set; }
        public int Size { get; set; }
        public double TotalValue { get; set; }
        public string Message { get; set; }
        public double Fee { get; set; }
        public int TradingDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int ShareId { get; set; }
        public string Symbol { get; set; }
        public int OrderId { get; set; }
        public int TradeSetId { get; set; }
    }
}
