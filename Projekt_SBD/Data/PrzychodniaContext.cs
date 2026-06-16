using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Projekt_SBD.Models;

namespace Projekt_SBD.Data
{
    public class PrzychodniaContext : DbContext
    {
        public PrzychodniaContext() { }
        
        public PrzychodniaContext(DbContextOptions<PrzychodniaContext> options) : base(options) { }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Diagnosis> Diagnosis { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<VisitHist> VisitHists { get; set; }
        public DbSet<SupplyHist> SupplyHists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle("User Id=ApplicationIdentity;Password=Admin123;Data Source=localhost:1521/FREE;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ADMINISTRATOR");
            base.OnModelCreating(modelBuilder);
        }
    }

    public class PrzychodniaContextFactory : IDesignTimeDbContextFactory<PrzychodniaContext>
    {
        public PrzychodniaContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PrzychodniaContext>();
            optionsBuilder.UseOracle("User Id=Administrator;Password=Admin123;Data Source=localhost:1521/FREE;");

            return new PrzychodniaContext(optionsBuilder.Options);
        }
    }
}