using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class WatchList : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public bool IsSystem { get; set; }
        public int DisplayOrder { get; set; }
        public string Status { get; set; }
        [Index]
        public int? ZoneId { get; set; }
        public virtual Zone Zone { get; set; }

        [NotMapped]
        public int? MemberCount { get; set; }
    }
}
