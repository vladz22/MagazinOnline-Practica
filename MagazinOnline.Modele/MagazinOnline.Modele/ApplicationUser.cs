using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.Modele
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string Nume { get; set; }
        public string Adresa { get; set; }
        public string Oras { get; set; }
        public string  Tara { get; set; }
        public string CodPostal { get; set; }

        public int? CompanieId { get; set; } //int? pentru a accepta si cazul de null
        [ForeignKey("CompanieId")]
        public Companie Companie { get; set; }

        [NotMapped]//nu este adaugat in database
        public string Rol { get; set; }
    }
}
