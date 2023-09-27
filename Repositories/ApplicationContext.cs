using Dal.Models;
using Dal.Models.HelperModel;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class ApplicationContext : DbContext
    {
        public DbSet<TokenModel> Tokens { get; set; }
        public DbSet<Experement> Experements { get; set; }
        public DbSet<TokenExperement> TokenExperements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) // Налаштовую зв'язок багато до багатьох 
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TokenExperement>()
                .HasKey(x => new { x.TokenId, x.ExperementId });

            modelBuilder.Entity<TokenExperement>()
                .HasOne(x => x.TokenModel)
                .WithMany(x => x.Experements)
                .HasForeignKey(x => x.TokenId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TokenExperement>()
                .HasOne(x => x.Experement)
                .WithMany(x => x.Tokens)
                .HasForeignKey(x => x.ExperementId)
                .OnDelete(DeleteBehavior.NoAction);
        }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.Migrate();
        }
    }
}