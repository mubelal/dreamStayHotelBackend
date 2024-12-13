using dreamStayHotel.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dreamStayHotel.Context
{
    public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
    {
       public DbSet<Room> Rooms { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Explicitly define the primary key for the 'Room' entity
            modelBuilder.Entity<Room>().HasKey(r => r.Id);
        }
    }
}
