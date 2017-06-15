using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class Broker :BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Shortable { get; set; }
        public double FeeRate { get; set; }
        public double MinFee { get; set; }
        public string Owner { get; set; }
        public bool IsActive { get; set; }
    }
}
