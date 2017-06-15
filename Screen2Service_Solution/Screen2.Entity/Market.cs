using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class Market :BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string SymbolSuffix { get; set; }


    }
}
