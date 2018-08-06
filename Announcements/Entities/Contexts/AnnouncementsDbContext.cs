using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Announcements.Entities.Contexts
{
    public class AnnouncementsDbContext : DbContext
    {
        public AnnouncementsDbContext(DbContextOptions<AnnouncementsDbContext> options) : base(options) { }

        public DbSet<Announcement> Announcements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Announcement>().ToTable(nameof(Announcement).ToLower());
        }
    }
}
