using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class ScanMatch
    {
        public int TradingDate { get; set; }

        public ScanVerification Verification { get; set; }

        [Required]
        [Index]
        public int ShareId { get; set; }

        public virtual Share Share { get; set; }
    }
}
