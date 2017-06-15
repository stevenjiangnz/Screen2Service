using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class ETFInfo : BaseEntity
    {
        public string Category { get; set; }
        public string FundFamily { get; set; }
        public Double? NetAssetM { get; set; }
        public Double? YieldPercentage { get; set; }
        public string InceptionDate { get; set; }

        [Required]
        [Index]
        public int ShareId { get; set; }

        public virtual Share Share { get; set; }
    }
}
