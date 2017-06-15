using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Screen2.DAL.Interface
{
    /// <summary>
    /// Interface of repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        T Find(object id);
        void Add(T entity);
        void Delete(object id);
        void Delete(T entityToDelete);
        void Update(T entityToUpdate);
    }
}
