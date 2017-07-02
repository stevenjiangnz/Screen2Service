using Microsoft.AspNet.Identity.EntityFramework;
using Screen2.DAL.Interface;
using Screen2.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Screen2.DAL
{
    public class DataContext : IdentityDbContext<ApplicationUser>, IDataContext
    {
        public DataContext()
            : base("name=Screen2Connection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public IDbSet<TradeOrder> Orders { get; set; }
        public IDbSet<Market> Markets { get; set; }
        public IDbSet<Share> Shares { get; set; }
        public IDbSet<Ticker> Tickers { get; set; }
        public IDbSet<Setting> Settings { get; set; }
        public IDbSet<AuditLog> AuditLogs { get; set; }
        public IDbSet<ShareInfo> ShareInfo { get; set; }
        public IDbSet<ETFInfo> ETFInfo { get; set; }
        public IDbSet<Indicator> Indicator { get; set; }
        public IDbSet<WatchList> WatchList { get; set; }
        public IDbSet<WatchListDetail> WatchListDetail { get; set; }
        public IDbSet<Note> Note { get; set; }
        public IDbSet<Functoid> Functoid { get; set; }
        public IDbSet<Formula> Formula { get; set; }
        public IDbSet<Rule> Rule { get; set; }
        public IDbSet<Scan> Scan { get; set; }
        public IDbSet<ScanRun> ScanRun { get; set; }
        public IDbSet<Zone> Zone { get; set; }
        public IDbSet<Broker> Broker { get; set; }
        public IDbSet<Account> Account { get; set; }
        public IDbSet<Transaction> Transaction { get; set; }
        public IDbSet<Plan> Plan { get; set; }
        public IDbSet<Alert> Alert { get; set; }
        public IDbSet<AlertResult> AlertResult { get; set; }
        public IDbSet<Journey> Journey { get; set; }
        public IDbSet<Idea> Idea { get; set; }
        public IDbSet<DailyScan> DailyScan { get; set; }
        public IDbSet<DailyScanResult> DailyScanResult { get; set; }
        public IDbSet<AccountBalance> AccountBalance { get; set; }
        public IDbSet<AccountBalanceJourney> AccountBalanceJourney { get; set; }
        public IDbSet<TradeSet> TradeSet { get; set; }
        public IDbSet<OrderReview> OrderReview { get; set; }
        public IDbSet<TradePosition> TradePosition { get; set; }
        public IDbSet<TradeReview> TradeReview { get; set; }
        public IDbSet<TradeSimulateOrder> TradeSimulateOrder { get; set; }
        public IDbSet<StatScanSet> StatScanSet { get; set; }
        public IDbSet<Record> Record { get; set; }
        public IDbSet<AsxEod> AsxEod { get; set; }

        public static DataContext Create()
        {
            return new DataContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
        /// <summary>
        /// Sets the modified.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void SetModified<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }


        /// <summary>
        /// Ensures the collection loaded.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="property">The property.</param>
        public void EnsureCollectionLoaded<TEntity, TElement>(TEntity entity, Expression<Func<TEntity, ICollection<TElement>>> property)
            where TEntity : class
            where TElement : class
        {
            var collection = Entry(entity).Collection(property);
            if (!collection.IsLoaded)
            {
                collection.Load();
            }
        }

        /// <summary>
        /// Extension point allowing the userPrincipal to customize validation of an entity or filter out validation results.
        /// Called by <see cref="M:System.Data.Entity.DbContext.GetValidationErrors" />.
        /// </summary>
        /// <param name="entityEntry">DbEntityEntry instance to be validated.</param>
        /// <param name="items">User defined dictionary containing additional info for custom validation.
        /// It will be passed to <see cref="T:System.ComponentModel.DataAnnotations.ValidationContext" />
        /// and will be exposed as <see cref="P:System.ComponentModel.DataAnnotations.ValidationContext.Items" />.
        /// This parameter is optional and can be null.</param>
        /// <returns>
        /// Entity validation result. Possibly null when overridden.
        /// </returns>
        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            return null;
        }


    }
}