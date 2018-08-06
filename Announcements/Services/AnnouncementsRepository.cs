using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Announcements.Entities;
using Announcements.Entities.Contexts;
using AnnouncementsAPI.Helpers;

namespace Announcements.Services
{
    public class AnnouncementsRepository : IAnnouncementsRepository
    {
        private AnnouncementsDbContext _context;

        public AnnouncementsRepository(AnnouncementsDbContext context)
        {
            _context = context;
        }

        public void AddAnnouncement(Announcement announcement)
        {
            if (announcement.ID == Guid.Empty)
                announcement.ID = Guid.NewGuid();

            _context.Announcements.Add(announcement);
        }

        public bool AnnouncementExists(Guid id) => _context.Announcements.Any(a => a.ID == id);

        public void DeleteAnnouncement(Announcement announcement) => _context.Announcements.Remove(announcement);

        public Announcement GetAnnouncement(Guid id) => _context.Announcements.FirstOrDefault(a => a.ID == id);

        public PagedList<Announcement> GetAnnouncements(AnnouncementsResourceParameters announcementsResourceParameters)
        {
            var collectionFromDb = _context.Announcements
                .OrderByDescending(a => a.VipAnnouncement)
                .ThenBy(a => a.Title).AsQueryable();

            if (!string.IsNullOrEmpty(announcementsResourceParameters.SearchString))
            {
                var normalizedSearchString = announcementsResourceParameters.SearchString.Trim().ToLowerInvariant();

                collectionFromDb = collectionFromDb.Where(a => a.Title.ToLowerInvariant().Contains(normalizedSearchString));
            }

            return PagedList<Announcement>.Create(collectionFromDb, announcementsResourceParameters.PageNumber, announcementsResourceParameters.PageSize);
        }

        public bool Save() => _context.SaveChanges() >= 0;

        public void UpdateAnnouncement(Announcement announcement)
        {
            
        }
    }
}
