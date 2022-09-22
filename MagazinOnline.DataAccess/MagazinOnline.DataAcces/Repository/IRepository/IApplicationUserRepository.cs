using MagazinOnline.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.DataAcces.Repository.IRepository
{
   public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
       // void Update(ApplicationUser applicationUser);nu folosim inca
    }
}
