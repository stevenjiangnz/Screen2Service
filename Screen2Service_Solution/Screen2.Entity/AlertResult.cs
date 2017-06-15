using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class AlertResult : BaseEntity
    {
        [Index]
        public int TradingDate { get; set; }
        public bool IsMatch { get; set; }
        public string Message { get; set; }
        public DateTime ProcessDT { get; set; }
        public int? ZoneId { get; set; }

        [Required]
        [Index]
        public int ShareId { get; set; }
        //public virtual Share Share { get; set; }

        [Required]
        [Index]
        public int AlertId { get; set; }
        //public virtual Alert Alert { get; set; }
    }
}
