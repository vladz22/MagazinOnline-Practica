using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.Modele
{
   public class ShoppingCart
    {
        public ShoppingCart()
        {
            Count = 1;
        }
        [Key]
        public int Id { get; set; }
        public string  ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public int ProdusId { get; set; }
        [ForeignKey("ProdusId")]
        public Produs Produs { get; set; }
        [Range(1,1000,ErrorMessage ="Te rog introdu o valoare intre 1-1000")]
        public int  Count { get; set; }
        [NotMapped]
        public double  Pret { get; set; }
    }
}
