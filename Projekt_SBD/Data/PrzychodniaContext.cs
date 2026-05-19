using Microsoft.EntityFrameworkCore;
using Projekt_SBD.Models;

namespace Projekt_SBD.Data
{
    public class PrzychodniaContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Diagnosis> Diagnosis { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Worker> Workers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle("User Id=Piotr;Password=Admin123;Data Source=localhost:1521/FREE;");
        }
    }
}