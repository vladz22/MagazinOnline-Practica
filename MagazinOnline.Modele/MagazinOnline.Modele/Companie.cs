using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.Modele
{
   public class Companie
    { 
        [Key]
        public int Id { get; set; }
        [Required]
        public string  Nume { get; set; }
        [Required]
        public string Adresa { get; set; }
        [Required]
        public string Oras { get; set; }
        public string Tara { get; set; }
        public string CodPostal { get; set; }
        [Required]
        public string NrTelefon { get; set; }
        public bool IsAutorizat { get; set; }
    }
}
