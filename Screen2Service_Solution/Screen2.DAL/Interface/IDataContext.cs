using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.DAL.Interface
{
    /// <summary>
    /// Data Context Interface
    /// </summary>
    public interface IDataContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>()
           where TEntity : class;
        void SetModified<TEntity>(TEntity entity)
            where TEntity : class;
        void EnsureCollectionLoaded<TEntity, TElement>(TEntity entity, Expression<Func<TEntity, ICollection<TElement>>> collection)
            where TEntity : class
            where TElement : class;
        int SaveChanges();
    }
}
