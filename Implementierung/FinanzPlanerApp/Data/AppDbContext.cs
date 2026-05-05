using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanzPlanerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanzPlanerApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Kategorie> Kategorien => Set<Kategorie>();
        public DbSet<Ausgabe> Ausgaben => Set<Ausgabe>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "finanzplaner.db");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}
