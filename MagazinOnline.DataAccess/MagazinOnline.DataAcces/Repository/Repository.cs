using MagazinOnline.DataAcces.Data;
using MagazinOnline.DataAcces.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.DataAcces.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Adaugare(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(int id)
        {
           return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filtru = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filtru != null)
            {
                query = query.Where(filtru);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            if (orderby != null)
            {
                return orderby(query).ToList();
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filtru = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filtru != null)
            {
                query = query.Where(filtru);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }

        public void Stergere(T entity)
        {
            dbSet.Remove(entity);
        }
        public void StergereRange(IEnumerable<T> entity) 
        {
            dbSet.RemoveRange(entity);
        }
    }
}
