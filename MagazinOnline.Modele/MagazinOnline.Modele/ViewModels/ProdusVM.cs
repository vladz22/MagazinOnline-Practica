using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.Modele.ViewModels
{
     public class ProdusVM
    {
        public Produs Produs { get; set; }
        public IEnumerable<SelectListItem> CategorieLista { get; set; }
    }
}
