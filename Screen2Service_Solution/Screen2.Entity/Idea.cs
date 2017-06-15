using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class Idea : BaseEntity
    {
        public string Topic{ get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Owner { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; } 

    }
}
