using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.Modele
{
   public class Produs
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nume { get; set; }
        [Required]
        public string Cod { get; set; }
        public string Descriere { get; set; }
        [Required]
        [Range(1,10000)]
        public double Pret { get; set; }
        [Range(1,50000)]
        public double  Pret5 { get; set; }
        public string ImagineUrl { get; set; }
        public int CategorieId { get; set; }
        [ForeignKey("CategorieId")]
        public Categorie Categorie { get; set; }
    }
}
