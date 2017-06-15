using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Entity
{
    public class WatchListDetail : BaseEntity
    {
        [Required]
        [Index]
        [MaxLength(128)]
        public string UserId { get; set; }

        [Required]
        [Index]
        public int WatchListId { get; set; }
        public virtual WatchList WatchList { get; set; }

        [Required]
        [Index]
        public int ShareId { get; set; }
        public virtual Share Share { get; set; }

    }
}
