using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnnouncementsAPI.Models
{
    public abstract class AnnouncementManipulation
    {
        [Required]
        [MaxLength(100)]
        public virtual string Title { get; set; }

        [Required]
        [MaxLength(250)]
        public virtual string Description { get; set; }

        [Required]
        [MaxLength(20)]
        [RegularExpression(@"^(1-)?\d{3}-\d{3}-\d{3}$", ErrorMessage = "Number must match pattern: 000-000-000")]
        public virtual string Phone { get; set; }
        
        public virtual string ImageName { get; set; }

        public virtual bool VipAnnouncement { get; set; }
    }
}
