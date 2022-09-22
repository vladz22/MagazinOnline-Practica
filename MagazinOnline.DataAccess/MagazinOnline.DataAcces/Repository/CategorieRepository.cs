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
    public class CategorieRepository : Repository<Categorie>, ICategorieRepository
    {
        private readonly ApplicationDbContext _db;
        public CategorieRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(Categorie categorie)
        {
            var objfromdb = _db.Categoriile.FirstOrDefault(a => a.Id == categorie.Id);
            if(objfromdb!=null)
            {
                objfromdb.Nume = categorie.Nume;
            }
        }
    }
}
