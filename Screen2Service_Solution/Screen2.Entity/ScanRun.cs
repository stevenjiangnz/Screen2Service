using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class ScanRun : BaseEntity
    {
        public DateTime RunStart { get; set; }
        public DateTime? RunEnd { get; set; }

        public string Status { get; set; }
        public string Owner { get; set; }
        public string Result { get; set; }

        [Required]
        [Index]
        public int ScanId { get; set; }

        public virtual Scan Scan { get; set; }
    }
}
