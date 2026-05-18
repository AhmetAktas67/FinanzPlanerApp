using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanzPlanerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanzPlanerApp.Data
{
    public partial class AppDbContext : DbContext
    {
        public DbSet<Kategorie> Kategorien => Set<Kategorie>();
        public DbSet<Ausgabe> Ausgaben => Set<Ausgabe>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "finanzplaner.db");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }

        public void InitializeDatabase()
        {
            Database.EnsureCreated();

            if (!Kategorien.Any())
            {
                Kategorien.AddRange(
                    new Kategorie { Name = "Essen", Farbe = "#FFB300" },
                    new Kategorie { Name = "Tank", Farbe = "#1E88E5" },
                    new Kategorie { Name = "Einkauf", Farbe = "#43A047" },
                    new Kategorie { Name = "Freizeit", Farbe = "#D81B60" }
                );

                SaveChanges();
            }


            if (!Ausgaben.Any())
            {
                Kategorie? essen = Kategorien.FirstOrDefault(k => k.Name == "Essen");
                Kategorie? tank = Kategorien.FirstOrDefault(k => k.Name == "Tank");

                if (essen != null)
                {
                    Ausgaben.Add(new Ausgabe
                    {
                        Betrag = 24.99m,
                        Datum = DateTime.Now,
                        Beschreibung = "McDonalds",
                        KategorieID = essen.KategorieId
                    });
                }

                if (tank != null)
                {
                    Ausgaben.Add(new Ausgabe
                    {
                        Betrag = 65.40m,
                        Datum = DateTime.Now,
                        Beschreibung = "Shell Tankstelle",
                        KategorieID = tank.KategorieId
                    });
                }

                SaveChanges();
            }
        }
    }
}
