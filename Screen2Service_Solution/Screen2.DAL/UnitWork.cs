using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Utils;

namespace Screen2.DAL
{
    /// <summary>
    /// Unit of Work class
    /// </summary>
    public class UnitWork : IUnitWork
    {
        #region Fields
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IDataContext _context;
        private IRepository<TradeOrder> _order;
        private IRepository<Market> _market;
        private IRepository<Share> _share;
        private IRepository<Ticker> _ticker;
        private IRepository<AuditLog> _auditLog;
        private IRepository<Setting> _setting;
        private IRepository<ShareInfo> _shareInfo;
        private IRepository<ETFInfo> _etfInfo;
        private IRepository<Indicator> _indicator;
        private IRepository<WatchList> _watchList;
        private IRepository<WatchListDetail> _watchListDetail;
        private IRepository<Note> _note;
        private IRepository<Functoid> _functoid;
        private IRepository<Formula> _formula;
        private IRepository<Rule> _rule;
        private IRepository<Scan> _scan;
        private IRepository<ScanRun> _scanRun;
        private IRepository<Zone> _zone;
        private IRepository<Broker> _broker;
        private IRepository<Account> _account;
        private IRepository<Transaction> _transaction;
        private IRepository<Plan> _plan;
        private IRepository<Alert> _alert;
        private IRepository<AlertResult> _alertResult;
        private IRepository<Journey> _journey;
        private IRepository<Idea> _idea;
        private IRepository<DailyScan> _dailyScan;
        private IRepository<DailyScanResult> _dailyScanResult;
        private IRepository<AccountBalance> _accountBalance;
        private IRepository<AccountBalanceJourney> _accountBalanceJourney;
        private IRepository<TradeSet> _tradeSet;
        private IRepository<OrderReview> _orderReview;
        private IRepository<TradePosition> _tradePosition;
        private IRepository<TradeReview> _tradeReview;
        private IRepository<TradeSimulateOrder> _tradeSimulateOrder;
        private IRepository<StatScanSet> _statScanSet;
        private IRepository<Record> _record;
        private IRepository<AsxEod> _asxEod;

        #endregion

        #region Constructor
        public UnitWork(IDataContext context) 
        {
            LogHelper.LoadLog4NetConfiguration();

            if (context == null)
                throw new ArgumentException("An instance of IDataContext is required to use this class.", "context");

            _context = context;
        }

        public UnitWork()
        {
            if (_context == null)
            {
                _context = new DataContext();
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the data context.
        /// </summary>
        /// <value>
        /// The data context.
        /// </value>
        public DataContext DataContext
        {
            get
            {
                return (DataContext)_context;
            }
        }

        public IRepository<TradeOrder> TradeOrder
        {
            get
            {
                return _order ??
                       (_order = new GenericRepository<TradeOrder>(_context));
            }
        }

        public IRepository<Market> Market
        {
            get
            {
                return _market ??
                       (_market = new GenericRepository<Market>(_context));
            }
        }

        public IRepository<Share> Share
        {
            get
            {
                return _share ??
                       (_share = new GenericRepository<Share>(_context));
            }
        }

        public IRepository<Ticker> Ticker
        {
            get
            {
                return _ticker ??
                       (_ticker = new GenericRepository<Ticker>(_context));
            }
        }

        public IRepository<AuditLog> AuditLog
        {
            get
            {
                return _auditLog ??
                       (_auditLog = new GenericRepository<AuditLog>(_context));
            }
        }

        public IRepository<Setting> Setting
        {
            get
            {
                return _setting ??
                       (_setting = new GenericRepository<Setting>(_context));
            }
        }

        public IRepository<ShareInfo> ShareInfo
        {
            get
            {
                return _shareInfo ??
                       (_shareInfo = new GenericRepository<ShareInfo>(_context));
            }
        }

        public IRepository<ETFInfo> ETFInfo
        {
            get
            {
                return _etfInfo ??
                       (_etfInfo = new GenericRepository<ETFInfo>(_context));
            }
        }

        public IRepository<Indicator> Indicator
        {
            get
            {
                return _indicator ??
                       (_indicator = new GenericRepository<Indicator>(_context));
            }
        }

        public IRepository<WatchList> WatchList
        {
            get
            {
                return _watchList ??
                       (_watchList = new GenericRepository<WatchList>(_context));
            }
        }

        public IRepository<WatchListDetail> WatchListDetail
        {
            get
            {
                return _watchListDetail ??
                       (_watchListDetail = new GenericRepository<WatchListDetail>(_context));
            }
        }

        public IRepository<Note> Note
        {
            get
            {
                return _note ??
                       (_note = new GenericRepository<Note>(_context));
            }
        }

        public IRepository<Functoid> Functoid
        {
            get
            {
                return _functoid ??
                       (_functoid = new GenericRepository<Functoid>(_context));
            }
        }

        public IRepository<Formula> Formula
        {
            get
            {
                return _formula ??
                       (_formula = new GenericRepository<Formula>(_context));
            }
        }

        public IRepository<Rule> Rule
        {
            get
            {
                return _rule ??
                       (_rule = new GenericRepository<Rule>(_context));
            }
        }

        public IRepository<Scan> Scan
        {
            get
            {
                return _scan ??
                       (_scan = new GenericRepository<Scan>(_context));
            }
        }

        public IRepository<ScanRun> ScanRun
        {
            get
            {
                return _scanRun ??
                       (_scanRun = new GenericRepository<ScanRun>(_context));
            }
        }

        public IRepository<Zone> Zone
        {
            get
            {
                return _zone ??
                       (_zone = new GenericRepository<Zone>(_context));
            }
        }

        public IRepository<Broker> Broker
        {
            get
            {
                return _broker ??
                       (_broker = new GenericRepository<Broker>(_context));
            }
        }

        public IRepository<Account> Account
        {
            get
            {
                return _account ??
                       (_account = new GenericRepository<Account>(_context));
            }
        }

        public IRepository<Transaction> Transaction
        {
            get
            {
                return _transaction ??
                       (_transaction = new GenericRepository<Transaction>(_context));
            }
        }

        public IRepository<Plan> Plan
        {
            get
            {
                return _plan ??
                       (_plan = new GenericRepository<Plan>(_context));
            }
        }

        public IRepository<Alert> Alert
        {
            get
            {
                return _alert ??
                       (_alert = new GenericRepository<Alert>(_context));
            }
        }

        public IRepository<AlertResult> AlertResult
        {
            get
            {
                return _alertResult ??
                       (_alertResult = new GenericRepository<AlertResult>(_context));
            }
        }

        public IRepository<Journey> Journey
        {
            get
            {
                return _journey ??
                       (_journey = new GenericRepository<Journey>(_context));
            }
        }

        public IRepository<Idea> Idea
        {
            get
            {
                return _idea ??
                       (_idea = new GenericRepository<Idea>(_context));
            }
        }

        public IRepository<DailyScan> DailyScan
        {
            get
            {
                return _dailyScan ??
                       (_dailyScan = new GenericRepository<DailyScan>(_context));
            }
        }

        public IRepository<DailyScanResult> DailyScanResult
        {
            get
            {
                return _dailyScanResult ??
                       (_dailyScanResult = new GenericRepository<DailyScanResult>(_context));
            }
        }

        public IRepository<AccountBalance> AccountBalance
        {
            get
            {
                return _accountBalance ??
                       (_accountBalance = new GenericRepository<AccountBalance>(_context));
            }
        }

        public IRepository<AccountBalanceJourney> AccountBalanceJourney
        {
            get
            {
                return _accountBalanceJourney ??
                       (_accountBalanceJourney = new GenericRepository<AccountBalanceJourney>(_context));
            }
        }

        public IRepository<TradeSet> TradeSet
        {
            get
            {
                return _tradeSet ??
                       (_tradeSet = new GenericRepository<TradeSet>(_context));
            }
        }


        public IRepository<OrderReview> OrderReview
        {
            get
            {
                return _orderReview ??
                       (_orderReview = new GenericRepository<OrderReview>(_context));
            }
        }

        public IRepository<TradePosition> TradePosition
        {
            get
            {
                return _tradePosition ??
                       (_tradePosition = new GenericRepository<TradePosition>(_context));
            }
        }

        public IRepository<TradeReview> TradeReview
        {
            get
            {
                return _tradeReview ??
                       (_tradeReview = new GenericRepository<TradeReview>(_context));
            }
        }

        public IRepository<TradeSimulateOrder> TradeSimulateOrder
        {
            get
            {
                return _tradeSimulateOrder ??
                       (_tradeSimulateOrder = new GenericRepository<TradeSimulateOrder>(_context));
            }
        }

        public IRepository<StatScanSet> StatScanSet
        {
            get
            {
                return _statScanSet ??
                       (_statScanSet = new GenericRepository<StatScanSet>(_context));
            }
        }

        public IRepository<Record> Record
        {
            get
            {
                return _record ??
                       (_record = new GenericRepository<Record>(_context));
            }
        }

        public IRepository<AsxEod> AsxEod
        {
            get
            {
                return _asxEod ??
                       (_asxEod = new GenericRepository<AsxEod>(_context));
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Saves the changes.
        /// </summary>
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
        }
        #endregion
    }
}
