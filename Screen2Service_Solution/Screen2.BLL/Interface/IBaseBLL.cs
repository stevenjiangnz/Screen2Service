using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL.Interface
{
    public interface IBaseBLL<T> where T : class
    {
        void Create(T entity);
        void Delete(T entity);
        void Delete(object id);
        System.Collections.Generic.IEnumerable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, Func<System.Linq.IQueryable<T>, System.Linq.IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        T GetByID(int id);
        System.Collections.Generic.IEnumerable<T> GetList();
        void Update(T entity);
    }
}
