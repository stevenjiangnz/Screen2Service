using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class Rule :BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Direction { get; set; }
        public string Type { get; set; }
        public string Assembly { get; set; }
        public string Formula { get; set; }
        public Boolean IsSystem { get; set; }
        public string Owner { get; set; }
        public string Note { get; set; }
        public DateTime UpdatedDT { get; set; }
    }
}
