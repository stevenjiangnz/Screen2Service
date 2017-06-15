using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class Scan : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public int StartDate { get; set; }
        public int? EndDate { get; set; }

        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public bool IsScheduled { get; set; }
        public string ProfitFormula { get; set; }
        public string StopFormula { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }
        public string ScopeType { get; set; }
        [Required]
        [Index]
        public int RuleId { get; set; }
        public virtual Rule Rule { get; set; }

        public string ShareString { get; set; } 
        public string WatchString { get; set; } 
    }
}
