using Microsoft.EntityFrameworkCore;
using Projekt_SBD.Models;

namespace Projekt_SBD.Data
{
    public class PrzychodniaContext : DbContext
    {
        public DbSet<Pacjent> Pacjenci { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle("User Id=Michal;Password=Admin123;Data Source=localhost:1521/FREE;");
        }
    }
}