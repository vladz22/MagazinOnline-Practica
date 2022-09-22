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
    public class ProdusRepository : Repository<Produs>, IProdusRepository
    {
        private readonly ApplicationDbContext _db;
        public ProdusRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(Produs produs)
        {
            var objfromdb = _db.Produse.FirstOrDefault(a => a.Id == produs.Id);
                if(objfromdb!=null)
            {
                if (produs.ImagineUrl != null)
                {
                    objfromdb.ImagineUrl = produs.ImagineUrl;
                }
                objfromdb.Nume = produs.Nume;
                objfromdb.Cod = produs.Cod;
                objfromdb.Descriere = produs.Descriere;
                objfromdb.Pret = produs.Pret;
                objfromdb.Pret5 = produs.Pret5;
                objfromdb.CategorieId = produs.CategorieId;
            }
        }
    }
}
