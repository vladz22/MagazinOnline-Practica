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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(OrderHeader orderHeader)
        {   
                _db.Update(orderHeader);
        }
    }
}
