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
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailsRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(OrderDetails orderDetails)
        {
            _db.Update(orderDetails);
        }
    }
}
