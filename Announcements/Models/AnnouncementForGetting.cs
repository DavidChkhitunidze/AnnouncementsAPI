using System;

namespace Announcements.Models
{
    public class AnnouncementForGetting
    {
        public Guid ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Phone { get; set; }

        public string ImageName { get; set; }

        public bool VipAnnouncement { get; set; }
    }
}
