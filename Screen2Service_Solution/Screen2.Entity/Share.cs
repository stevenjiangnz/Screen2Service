using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class Share:BaseEntity
    {


        [StringLength(30)]
        [Index("Symbol_Index")]
        public string Symbol { get; set; }


        [StringLength(100)]
        public string Name { get; set; }


        //[StringLength(35)]
        //[Index("YahooSymbol_Index")]
        //public string YahooSymbol { get; set; }

        [StringLength(100)]
        public string Industry { get; set; }

        [StringLength(100)]
        public string Sector { get; set; }

        [Index("IsActive_Index")]
        public bool IsActive { get; set; }

        public bool IsYahooVerified { get; set; }

        public DateTime? LastProcessed { get; set; }

        public string ProcessComment { get; set; }

        public bool IsCFD { get; set; }

        [StringLength(20)]
        [Required]
        public string ShareType { get; set; }

        [Required]
        public int MarketId { get; set; }

        public virtual Market Market { get; set; }
    }
}
