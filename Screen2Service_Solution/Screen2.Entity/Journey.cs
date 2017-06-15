using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class Journey : BaseEntity
    {
        public string StartDay { get; set; }
        public string Status { get; set; }
        public string Owner { get; set; }
        public string Content { get; set; }
        public DateTime Created{ get; set; }
        public DateTime Modified { get; set; }

        [Index]
        public int? ZoneId { get; set; }
        public virtual Zone Zone { get; set; }
    }
}
