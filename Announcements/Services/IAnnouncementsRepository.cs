using Announcements.Entities;
using AnnouncementsAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Announcements.Services
{
    public interface IAnnouncementsRepository
    {
        PagedList<Announcement> GetAnnouncements(AnnouncementsResourceParameters announcementsResourceParameters);
        Announcement GetAnnouncement(Guid id);
        void AddAnnouncement(Announcement announcement);
        void DeleteAnnouncement(Announcement announcement);
        void UpdateAnnouncement(Announcement announcement);
        bool AnnouncementExists(Guid id);
        bool Save();
    }
}
