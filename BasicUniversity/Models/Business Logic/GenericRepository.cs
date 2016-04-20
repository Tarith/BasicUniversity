using BasicUniversity.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BasicUniversity.Models
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        internal SchoolContext _context;
        internal DbSet<T> _db;

        public GenericRepository(SchoolContext context)
        {
            _context = context;
            _db = context.Set<T>();
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _db;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includedProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includedProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }

            return query;

        }

        public virtual T GetById(object id)
        {
            return _db.Find(id);
        }

        public virtual Task<T> GetByIdAsync(object id)
        {
            return _db.FindAsync(id);
        }

        public virtual void Insert(T entity)
        {
            _db.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            T entity = _db.Find(id);
            Delete(entity);
        }

        public virtual void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _db.Attach(entity);
            }

            _db.Remove(entity);
        }

    }
}
