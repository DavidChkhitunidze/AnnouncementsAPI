using Announcements.Entities;
using Announcements.Entities.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Announcements.Helpers.Extensions
{
    public static class AnnouncementsDbContextExtensions
    {
        public static void SeedAnnouncementsData(this AnnouncementsDbContext context)
        {
            if (context.Announcements.Any())
                return;

            var announcementList = new List<Announcement>()
            {
                new Announcement
                {
                    ID = Guid.NewGuid(),
                    Title = "Title 1",
                    Description = "Description 1",
                    Phone = "111-111-111",
                    ImageName = "Image12345.jpg",
                    VipAnnouncement = true
                },

                new Announcement
                {
                    ID = Guid.NewGuid(),
                    Title = "Title 2",
                    Description = "Description 2",
                    Phone = "222-222-222",
                    ImageName = "Image12345.jpg",
                    VipAnnouncement = false
                },

                new Announcement
                {
                    ID = Guid.NewGuid(),
                    Title = "Title 3",
                    Description = "Description 3",
                    Phone = "333-333-333",
                    ImageName = "Image12345.jpg",
                    VipAnnouncement = false
                },

                new Announcement
                {
                    ID = Guid.NewGuid(),
                    Title = "Title 4",
                    Description = "Description 4",
                    Phone = "444-444-444",
                    ImageName = "Image12345.jpg",
                    VipAnnouncement = false
                },

                new Announcement
                {
                    ID = Guid.NewGuid(),
                    Title = "Title 5",
                    Description = "Description 5",
                    Phone = "555-555-555",
                    ImageName = "Image12345.jpg",
                    VipAnnouncement = false
                },

                new Announcement
                {
                    ID = Guid.NewGuid(),
                    Title = "Title 6",
                    Description = "Description 6",
                    Phone = "666-666-666",
                    ImageName = "Image12345.jpg",
                    VipAnnouncement = true
                },

                new Announcement
                {
                    ID = Guid.NewGuid(),
                    Title = "Title 7",
                    Description = "Description 7",
                    Phone = "777-777-777",
                    ImageName = "Image12345.jpg",
                    VipAnnouncement = false
                },

                new Announcement
                {
                    ID = Guid.NewGuid(),
                    Title = "Title 8",
                    Description = "Description 8",
                    Phone = "888-888-888",
                    ImageName = "Image12345.jpg",
                    VipAnnouncement = false
                },

                new Announcement
                {
                    ID = Guid.NewGuid(),
                    Title = "Title 9",
                    Description = "Description 9",
                    Phone = "999-999-999",
                    ImageName = "Image12345.jpg",
                    VipAnnouncement = false
                },

                new Announcement
                {
                    ID = Guid.NewGuid(),
                    Title = "Title 10",
                    Description = "Description 10",
                    Phone = "101-010-101",
                    ImageName = "Image12345.jpg",
                    VipAnnouncement = false
                },

                new Announcement
                {
                    ID = Guid.NewGuid(),
                    Title = "Title 11",
                    Description = "Description 11",
                    Phone = "010-101-010",
                    ImageName = "Image12345.jpg",
                    VipAnnouncement = false
                },

                new Announcement
                {
                    ID = Guid.NewGuid(),
                    Title = "Title 12",
                    Description = "Description 12",
                    Phone = "121-212-121",
                    ImageName = "Image12345.jpg",
                    VipAnnouncement = false
                },

                new Announcement
                {
                    ID = Guid.NewGuid(),
                    Title = "Title 13",
                    Description = "Description 13",
                    Phone = "131-313-131",
                    ImageName = "Image12345.jpg",
                    VipAnnouncement = false
                },

                new Announcement
                {
                    ID = Guid.NewGuid(),
                    Title = "Title 14",
                    Description = "Description 14",
                    Phone = "141-414-141",
                    ImageName = "Image12345.jpg",
                    VipAnnouncement = true
                },

                new Announcement
                {
                    ID = Guid.NewGuid(),
                    Title = "Title 15",
                    Description = "Description 15",
                    Phone = "151-515-151",
                    ImageName = "Image12345.jpg",
                    VipAnnouncement = false
                },
            };

            context.Announcements.AddRange(announcementList);
            context.SaveChanges();
        }
    }
}
