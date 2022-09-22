using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.DataAcces.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filtru = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
            string includeProperties = null
            );
        T GetFirstOrDefault(
             Expression<Func<T, bool>> filtru = null,
            string includeProperties = null
            );
        void Adaugare(T entity);
        void Stergere(T entity);
        void StergereRange(IEnumerable<T> entity);

    }
}
