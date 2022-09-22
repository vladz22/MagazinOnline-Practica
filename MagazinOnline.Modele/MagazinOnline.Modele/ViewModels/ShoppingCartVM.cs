using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.Modele.ViewModels
{
   public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ListaCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
