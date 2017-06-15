using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class Note: BaseEntity
    {
        public string Comment { get; set; }
        [MaxLength(128)]
        public string Type { get; set; }

        [Index]
        public int? ShareId { get; set; }
        [Index]
        public int? TradingDate { get; set; }
        [Index]
        public int? ZoneId { get; set; }

        [Index]
        [MaxLength(128)]
        public string CreatedBy { get; set; }
        public DateTime Create { get; set; }

        [NotMapped]
        public string FirstName { get; set; }
        [NotMapped]
        public string LastName { get; set; }
        [NotMapped]
        public string UserName { get; set; }

    }
}
