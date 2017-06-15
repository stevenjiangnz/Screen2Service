using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class ShareInfo :BaseEntity
    {
        [StringLength(150)]
        public string CompanyUrl { get; set; }
        [StringLength(100)]
        public string YSector { get; set; }
        [StringLength(100)]
        public string YIndustry { get; set; }
       
        public string CompanySummary { get; set; }
        public int? EmployeeNumber { get; set; }

        public Double? MarketCapM { get; set; }

        public bool IsTop50 { get; set; }

        public bool IsTop100 { get; set; }

        public bool IsTop200 { get; set; }
        public bool IsTop300 { get; set; }
        public DateTime? LastProcessed { get; set; }

        [NotMapped]
        public string ShareType { get; set; }
        [NotMapped]
        public int DataStartDate { get; set; }

        [Required]
        [Index]
        public int ShareId { get; set; }

        public virtual Share Share { get; set; }
    }
}
