using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Announcements.Entities
{
    public class BaseClass
    {
        [Key]
        public Guid ID { get; set; }
    }
}
