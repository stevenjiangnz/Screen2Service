using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Screen2.Entity
{
    public class Account : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner {get;set;}
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsTrackingAccount { get; set; }

        [Index]
        public int BrokerId { get; set; }
        [XmlIgnore]
        public virtual Broker Broker { get; set; }

        [Index]
        public int? ZoneId { get; set; }
        [XmlIgnore]
        public virtual Zone Zone { get; set; }
    }
}
