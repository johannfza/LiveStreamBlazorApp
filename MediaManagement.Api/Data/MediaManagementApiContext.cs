using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MediaModelLibrary;

namespace MediaManagement.Api.Data
{
    public class MediaManagementApiContext : DbContext
    {
        public MediaManagementApiContext (DbContextOptions<MediaManagementApiContext> options)
            : base(options)
        {
        }

        public DbSet<LiveStream> LiveStream { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            for (int i = 1; i < 5; i++)
            {
                modelBuilder.Entity<LiveStream>().HasData(new LiveStream(i, $"LiveStream {i}", $"This is Live Stream {i}", "http://www.golive.com/" + i));

            }
        }


    }
}
