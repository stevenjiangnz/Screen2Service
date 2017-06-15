using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class DailyScan :BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public string Formula { get; set; }
        public bool UseRule { get; set; }
        public DateTime? Modified { get; set; }
        public string Status { get; set; }
        public DateTime? LastProcessed { get; set; }

        public string WatchListString { get; set; }

        [Index]
        public int? RuleId { get; set; }
        public virtual Rule Rule { get; set; }

        [Index]
        public int? ZoneId { get; set; }
        public virtual Zone Zone { get; set; }
    }
}
