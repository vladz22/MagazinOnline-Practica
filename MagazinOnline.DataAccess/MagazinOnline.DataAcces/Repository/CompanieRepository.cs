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
    public class CompanieRepository : Repository<Companie>, ICompanieRepository
    {
        private readonly ApplicationDbContext _db;
        public CompanieRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(Companie companie)
        {
            _db.Update(companie);
        }
    }
}
