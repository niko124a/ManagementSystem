using Common.Entities;
using DatabaseAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    public class AutoMechanicManagementSystemDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationType> ReservationTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ApiAuth> ApiAuth { get; set; }

        public AutoMechanicManagementSystemDbContext()
        {
        }
        public AutoMechanicManagementSystemDbContext(DbContextOptions<AutoMechanicManagementSystemDbContext> options) : base(options)
        {
        }

        // TODO: Remove this method later.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer("Server=db,1433;Database=AutoMechanicManagementSystemDb;User Id=sa;Password=Test1234!;TrustServerCertificate=true;"); // connectionstring for db when running in docker
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AutoMechanicManagementSystemDb;Trusted_Connection=True;"); // connectionstring for db when running locally
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Car>()
                .HasIndex(c => c.Registration)
                .IsUnique();

            modelBuilder.Entity<ReservationType>()
                .HasIndex(rt => rt.Name)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
