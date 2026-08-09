using bartering.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bartering.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Item> Items => Set<Item>();
        public DbSet<SwapOffer> SwapOffers => Set<SwapOffer>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Item>(entity =>
            {
                entity.HasIndex(i => i.Status);
                entity.HasIndex(i => i.Category);
                entity.HasOne(i => i.Owner)
                    .WithMany(u => u.Items)
                    .HasForeignKey(i => i.OwnerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<SwapOffer>(entity =>
            {
                entity.HasOne(s => s.FromUser)
                    .WithMany(u => u.SentOffers)
                    .HasForeignKey(s => s.FromUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(s => s.ToUser)
                    .WithMany(u => u.ReceivedOffers)
                    .HasForeignKey(s => s.ToUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(s => s.OfferedItem)
                    .WithMany()
                    .HasForeignKey(s => s.OfferedItemId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(s => s.RequestedItem)
                    .WithMany()
                    .HasForeignKey(s => s.RequestedItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
