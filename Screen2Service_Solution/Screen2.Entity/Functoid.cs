using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class Functoid : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Usage { get; set; }
        public string ReturnType { get; set; }
        public string DefaultValue { get; set; }
    }
}
