using Screen2.BLL;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Screen2.Api.Controllers
{
    [RoutePrefix("api/tradeaccount")]
    [Authorize]
    public class TradeAccountController : BaseApiController
    {

        #region Constructor
        public TradeAccountController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            List<Account> slist = null;

            try
            {
                AccountBLL bll = new AccountBLL(_unit);

                slist = bll.GetList().ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(slist);
        }

        [HttpGet]
        [Route("getbyzone")]
        public async Task<IHttpActionResult> GetByZone(int? zoneId = null)
        {
            List<OutAccount> slist = null;

            try
            {
                var currentUser = await base.GetCurrentUser();

                AccountBLL bll = new AccountBLL(_unit);

                slist = bll.GetAccountFullListByUser(currentUser.Id, zoneId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(slist);
        }


        [HttpGet]
        [Route("accountbalancejourney")]
        public async Task<IHttpActionResult> GetAccountBalanceJourneyByAccount(int accountId, int size =50)
        {
            List<AccountBalanceJourney> slist = null;

            try
            {

                AccountBalanceJourneyBLL bll = new AccountBalanceJourneyBLL(_unit);

                slist = bll.GetListByAccount(accountId, size);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(slist);
        }


        [HttpGet]
        [Route("positionsummarylist")]
        public async Task<IHttpActionResult> GetPositionSummaryListByAccount(int accountId, int? size = 50)
        {
            List<OutPosition> slist = null;

            try
            {

                TradePositionBLL bll = new TradePositionBLL(_unit);

                slist = bll.GetPositionSummaryList(accountId, size);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(slist);
        }

        [HttpGet]
        [Route("accounttransaction")]
        public async Task<IHttpActionResult> GetAccountTransactionByAccount(int accountId, int? size =50)
        {
            List<OutTransaction> tlist = null;

            try
            {

                TransactionBLL bll = new TransactionBLL(_unit);

                tlist = bll.GetTransactionList(accountId, size);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(tlist);
        }

        [HttpGet]
        [Route("transaction")]
        public async Task<IHttpActionResult> GetTransactionById(int id)
        {
            Transaction tr = null;

            try
            {

                TransactionBLL bll = new TransactionBLL(_unit);

                tr = bll.GetByID(id);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(tr);
        }

        [HttpGet]
        [Route("accountposition")]
        public async Task<IHttpActionResult> GetTradePositionByAccount(int accountId)
        {
            List<TradePosition> tlist = null;

            try
            {

                TradePositionBLL bll = new TradePositionBLL(_unit);

                tlist = bll.GetListByAccount(accountId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(tlist);
        }


        [HttpGet]
        [Route("accountoutstandingposition")]
        public async Task<IHttpActionResult> GetOutstandingTradePositionByAccount(int accountId)
        {
            List<TradePosition> tlist = null;

            try
            {
                TradePositionBLL bll = new TradePositionBLL(_unit);

                tlist = bll.GetOutstandingPositions(accountId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(tlist);
        }


        [HttpGet]
        [Route("accountsummary")]
        public async Task<IHttpActionResult> GetAccountSummary(int accountId)
        {
            AccountSummary asummary = null;

            try
            {
                AccountBLL bll = new AccountBLL(_unit);
                asummary = bll.GetAccountSummary(accountId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(asummary);
        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            Account s = null;

            try
            {
                AccountBLL bll = new AccountBLL(_unit);

                s = bll.GetByID(id);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(s);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put(OutAccount account)
        {
            OutAccount createdAccount = null;
            try
            {
                var currentUser = await GetCurrentUser();

                account.CreateDate = DateTime.Now;
                account.CreatedBy = currentUser.Id;
                account.Owner = currentUser.Id;

                createdAccount = new AccountBLL(_unit).Create(account);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(createdAccount);
        }


        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] Account account)
        {
            try
            {
                if (account.Id > 0)
                {
                    var currentUser = await GetCurrentUser();
                    account.Owner = currentUser.Id;

                    (new AccountBLL(_unit)).Update(account);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok(account);
        }


        [HttpPost]
        [Route("transferfund")]
        public async Task<IHttpActionResult> TransferFund(int accountId, string operation, double amount)
        {
            OutAccount outAcc = null;
            try
            {
                var currentUser = await GetCurrentUser();
                var accBll = new AccountBLL(_unit);
                var accBalance = new AccountBalanceBLL(_unit).GetAccountBalanceByAccount(accountId);

                if (operation.Equals("Withdraw", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (accBalance != null && accBalance.AvailableFund < amount)
                    {
                        return BadRequest("Insufficient fund.");
                    }
                }

                outAcc = accBll.TransferFund(accountId, operation, amount);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok(outAcc);
        }


        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                AccountBLL bll = new AccountBLL(_unit);

                bool isAdmin = await AppUserManager.IsInRoleAsync(currentUser.Id, "Admin");

                if (isAdmin)
                {
                    bll.DeleteFull(id);
                }
                else
                {
                    var w = bll.GetByID(id);

                    if (w.Owner == currentUser.Id)
                    {
                        bll.DeleteFull(id);
                    }
                    else
                    {
                        BadRequest("You don't have permission to delete this account.");
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok();
        }
        #endregion
    }
}