using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BasicUniversity.Models
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        T GetById(object id);
        Task<T> GetByIdAsync(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(object id);
        void Delete(T entity);
    }
}
