using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzPlanerApp.Models
{
    public class Kategorie
    {
        [Key]
        public int KategorieId { get; set; }
       
        
        public string Name { get; set; } = string.Empty;
        public string Ausgabengrenze { get; set; }

        public List<Ausgabe> Ausgaben {  get; set; }


    }
}
