using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.DataAcces.Repository.IRepository
{
   public interface IUnitOfWork:IDisposable
    {
        ICategorieRepository Categorie { get; }
        IProdusRepository Produs { get; }
        ICompanieRepository Companie { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IShoppingCartRepository ShoppingCart { get; }
        void Save();

    }
}
