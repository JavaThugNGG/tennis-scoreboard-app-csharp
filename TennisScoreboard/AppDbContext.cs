using Microsoft.EntityFrameworkCore;

namespace TennisScoreboard
{
    public class AppDbContext : DbContext
    {
        public DbSet<PlayerEntity> Players { get; set; }
        public DbSet<MatchEntity> Matches { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlayerEntity>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<MatchEntity>()
                .HasOne(m => m.Player1)
                .WithMany(p => p.MatchesAsPlayer1)
                .HasForeignKey(m => m.Player1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MatchEntity>()
                .HasOne(m => m.Player2)
                .WithMany(p => p.MatchesAsPlayer2)
                .HasForeignKey(m => m.Player2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MatchEntity>()
                .HasOne(m => m.Winner)
                .WithMany(p => p.MatchesWon)
                .HasForeignKey(m => m.WinnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
