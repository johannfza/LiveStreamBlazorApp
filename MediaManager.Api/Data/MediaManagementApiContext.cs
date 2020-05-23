using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MediaModels;

namespace MediaManager.Api.Data
{
    public class MediaManagementApiContext : DbContext
    {
        public MediaManagementApiContext(DbContextOptions<MediaManagementApiContext> options)
            : base(options)
        {

        }

        public DbSet<LiveStream> LiveStream { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Connection> Connections { get; set; }

        public DbSet<UserConnection> UserConnections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserConnection>()
                .HasKey(uc => new { uc.UserId, uc.ConnectionId });
            modelBuilder.Entity<UserConnection>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.Connections)
                .HasForeignKey(uc => uc.UserId);
            modelBuilder.Entity<UserConnection>()
                .HasOne(uc => uc.Connection)
                .WithMany(c => c.UserConnections)
                .HasForeignKey(uc => uc.ConnectionId);

            // Seed Data 
            for (int i = 1; i < 5; i++)
            {
                modelBuilder.Entity<LiveStream>().HasData(new LiveStream(i, $"LiveStream {i}", $"This is Live Stream {i}", $"http://www.golive.com/" + i.ToString(), "key" + i.ToString()));

                

            }



        }
    }
}
