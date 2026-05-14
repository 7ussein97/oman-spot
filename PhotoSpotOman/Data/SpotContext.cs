using Microsoft.EntityFrameworkCore;
using PhotoSpotOman.Models;

namespace PhotoSpotOman.Data
{
    public class SpotContext : DbContext
    {
        public SpotContext(DbContextOptions<SpotContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Spot> Spots { get; set; }
        public DbSet<SpotImage> SpotImages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationships
            modelBuilder.Entity<Spot>()
                .HasOne(s => s.User)
                .WithMany(u => u.Spots)
                .HasForeignKey(s => s.AddedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Spot)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.SpotId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Spot)
                .WithMany(s => s.Likes)
                .HasForeignKey(l => l.SpotId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SpotImage>()
                .HasOne(i => i.Spot)
                .WithMany(s => s.Images)
                .HasForeignKey(i => i.SpotId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SpotImage>()
                .HasOne(i => i.User)
                .WithMany()
                .HasForeignKey(i => i.UploadedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
