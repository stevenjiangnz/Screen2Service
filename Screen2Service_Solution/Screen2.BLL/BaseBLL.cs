using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    /// <summary>
    /// Base BLL class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseBLL<T> : IBaseBLL<T>, IDisposable where T : class
    {
        #region Fields
        protected readonly IUnitWork _unit;
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected string _connectionString;
        protected int _CommandTimeout = 600;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBLL{T}"/> class.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public BaseBLL(IUnitWork unit)
        {
            _unit = unit;

            _connectionString = ConfigurationManager.ConnectionStrings["Screen2Connection"].ConnectionString;
        }
        #endregion

        #region
        /// <summary>
        /// Gets the disposal method list.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetList()
        {
            IEnumerable<T> result = null;
            try
            {
                IRepository<T> repository = this.GetRepository();

                result = repository.Get();
            }
            catch (Exception ex)
            {
                _log.Error("Error in GetList", ex);
                throw;
            }

            return result;
        }


        /// <summary>
        /// Creates the specified disposal method.
        /// </summary>
        /// <param name="disposalMethod">The disposal method.</param>
        public virtual void Create(T entity)
        {
            try
            {
                IRepository<T> repository = this.GetRepository();

                repository.Add(entity);

                _unit.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.Error("Error in Create", ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual T GetByID(int id)
        {
            try
            {
                IRepository<T> repository = this.GetRepository();

                return repository.Find(id);
            }
            catch (Exception ex)
            {
                _log.Error("Error in GetByID", ex);
                throw;
            }
        }


        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Update(T entity)
        {
            try
            {
                IRepository<T> repository = this.GetRepository();

                repository.Update(entity);

                _unit.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.Error("Error in Update Entity", ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the specified status.
        /// </summary>
        /// <param name="status">The status.</param>
        public virtual void Delete(T entity)
        {
            try
            {
                IRepository<T> repository = this.GetRepository();

                repository.Delete(entity);

                _unit.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.Error("Error in Delete entity", ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the specified status.
        /// </summary>
        /// <param name="status">The status.</param>
        public virtual void Delete(object id)
        {
            try
            {
                IRepository<T> repository = this.GetRepository();

                repository.Delete(id);

                _unit.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.Error("Error in Delete by ID", ex);
                throw;
            }
        }

        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            try
            {
                IRepository<T> repository = this.GetRepository();

                return repository.Get(filter, orderBy, includeProperties);
            }
            catch (Exception ex)
            {
                _log.Error("Error in Get entity", ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <returns></returns>
        protected IRepository<T> GetRepository()
        {
            IRepository<T> repository = null;

            string typeName = typeof(T).Name;

            repository = (IRepository<T>)_unit.GetType().GetProperty(typeName).GetValue(_unit, null);

            return repository;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_unit != null)
                {
                    _unit.Dispose();
                }
            }
        }
        #endregion

    }
}