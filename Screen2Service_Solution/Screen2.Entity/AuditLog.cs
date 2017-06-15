using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class AuditLog : BaseEntity
    {
        public AuditLog()
        {
            this.ActionTime = DateTime.Now;
        }

        [StringLength(50)]
        public string ActionType { get; set; }
       
        public string ActionMessage { get; set; }

        public string ActionResult { get; set; }

        public string ExtraData { get; set; }

        public DateTime ActionTime { get; set; }

        public int? ShareId { get; set; }

        public virtual Share Share { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
