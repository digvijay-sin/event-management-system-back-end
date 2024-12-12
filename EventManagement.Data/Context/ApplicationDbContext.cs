using EventManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Rsvp> Rsvps { get; set; }
        public DbSet<User> Users{ get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Rsvps)
                .WithOne(r => r.Event)
                .HasForeignKey(r => r.EventId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<User>()
                .HasMany(u => u.Events)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Role>().
                HasMany(r => r.Users).
                WithOne(u => u.Role)
               .HasForeignKey(u => u.RoleId)
               .OnDelete(DeleteBehavior.Cascade);
                
        }
    }
}
