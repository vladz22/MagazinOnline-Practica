using MagazinOnline.DataAcces.Data;
using MagazinOnline.DataAcces.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.DataAcces.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Categorie = new CategorieRepository(_db);
            Produs = new ProdusRepository(_db);
            Companie = new CompanieRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
        }
        public ICategorieRepository Categorie { get; private set; }

        public IProdusRepository Produs { get; private set; }
        public ICompanieRepository Companie { get; set; }
        public IApplicationUserRepository ApplicationUser { get; set; }
        public IOrderHeaderRepository OrderHeader { get; set; }
        public IOrderDetailsRepository OrderDetails { get; set; }
        public IShoppingCartRepository ShoppingCart { get; set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
