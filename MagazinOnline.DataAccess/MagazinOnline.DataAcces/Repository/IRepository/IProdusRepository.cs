using MagazinOnline.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.DataAcces.Repository.IRepository
{
   public interface IProdusRepository:IRepository<Produs>
    {
        public void Update(Produs produs);

    }
}
