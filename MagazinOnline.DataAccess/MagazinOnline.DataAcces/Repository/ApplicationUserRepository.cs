using MagazinOnline.DataAcces.Data;
using MagazinOnline.DataAcces.Repository.IRepository;
using MagazinOnline.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.DataAcces.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>,IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        //public void Update(Categorie categorie)
        //{
        //    var objfromdb = _db.Categoriile.FirstOrDefault(a => a.Id == categorie.Id);
        //    if(objfromdb!=null)
        //    {
        //        objfromdb.Nume = categorie.Nume;
        //    }
        //}
    }
}
