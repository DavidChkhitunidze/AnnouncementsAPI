using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Announcements.Entities
{
    public class Announcement : BaseClass
    {
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Phone { get; set; }
        
        public string ImageName { get; set; }

        public bool VipAnnouncement { get; set; }
    }
}
