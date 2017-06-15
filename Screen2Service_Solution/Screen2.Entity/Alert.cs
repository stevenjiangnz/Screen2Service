using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class Alert :BaseEntity
    {
        public string Owner { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Formula { get; set; }
        public string Message { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastProcessed { get; set; }

        [Index]
        public int? ZoneId { get; set; }
        public virtual Zone Zone { get; set; }


        [Required]
        [Index]
        public int ShareId { get; set; }
        public virtual Share Share { get; set; }
    }
}
