using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.Modele
{
   public class OrderHeader
    {
        [Key]
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public DateTime ShippingDate { get; set; }
        [Required]
        public double OrderTotal { get; set; }
        public string AWB { get; set; }
        public string  Curier { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime  PaymentDate { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public string TranzactionId { get; set; }
        [Required]
        public string Adresa { get; set; }
        [Required]
        public string Oras { get; set; }
        [Required]
        public string Tara { get; set; }
        [Required]
        public string CodPostal { get; set; }
        [Required]
        public string Nume { get; set; }
    }
}
