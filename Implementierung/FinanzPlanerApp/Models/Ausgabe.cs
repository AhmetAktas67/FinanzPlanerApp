using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzPlanerApp.Models
{
    public class Ausgabe
    {
        
        [Key]
        public int AusgabenID { get; set; }
       
        
        
        public decimal Betrag { get; set; }

        public DateTime Datum { get; set; }

        public string Beschreibung {  get; set; }= string.Empty;

        public int KategorieID { get; set; }
        public Kategorie? Kategorie { get; set; }
    }
}
