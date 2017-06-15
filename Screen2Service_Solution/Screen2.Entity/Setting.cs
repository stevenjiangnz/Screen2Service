using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class Setting:BaseEntity
    {
        [StringLength(100)]
        public string Key { get; set; }
        [MaxLength(2000)]
        public string Value { get; set; }
    }
}
