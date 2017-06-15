using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class Record :BaseEntity
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public int? ZoneId { get; set; }
        public int TradingDate { get; set; }
        public string OriginalFileName { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public string Owner { get; set; }
        public DateTime CreateDT { get; set; }
    }
}
