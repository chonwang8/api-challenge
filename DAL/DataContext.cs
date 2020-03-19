using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<Position>()
                .HasIndex(p => p.Name)
                .IsUnique();
            modelBuilder.Entity<Cv>().ToTable("Cvs");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Cv> Cvs { get; set; }
    }
}
