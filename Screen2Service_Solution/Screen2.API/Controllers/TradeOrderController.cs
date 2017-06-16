using Screen2.BLL;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Shared;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Screen2.Api.Controllers
{
    [RoutePrefix("api/tradeorder")]
    [Authorize]
    public class TradeOrderController : BaseApiController
    {
        #region Constructor
        public TradeOrderController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            List<TradeOrder> slist = null;

            try
            {
                TradeOrderBLL bll = new TradeOrderBLL(_unit);

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
        [Route("getbyaccount")]
        public async Task<IHttpActionResult> GetByAccount(int accountId, int size = 50)
        {
            List<TradeOrder> slist = null;

            try
            {
                TradeOrderBLL bll = new TradeOrderBLL(_unit);

                slist = bll.GetListByAccount(accountId, size);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(slist);
        }

        [HttpPost]
        [Route("updateposition")]
        public async Task<IHttpActionResult> UpdatePosition([FromBody] InUpdatePositionParams input)
        {
            TradePositionBLL tpBLL = new TradePositionBLL(_unit);
            TradePosition position = null;

            try
            {
                position = tpBLL.UpdatePosition(input);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(position);
        }

        [HttpGet]
        [Route("getbyaccountstatus")]
        public async Task<IHttpActionResult> GetByAccountStatus(int accountId, string status)
        {
            List<TradeOrder> slist = null;

            try
            {
                TradeOrderBLL bll = new TradeOrderBLL(_unit);

                slist = bll.GetListByAccountStatus(accountId, status);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(slist);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put(TradeOrder order)
        {
            try
            {
                var currentUser = await GetCurrentUser();

                order.UpdateDate = DateTime.Now;
                order.UpdatedBy = currentUser.Id;

                order.Status = OrderStatus.Open.ToString();
                order.Source = OrderSource.Entry.ToString();

                new TradeOrderBLL(_unit).CreateOrder(order);
            }
            catch(ApplicationException appEx)
            {
                return BadRequest(appEx.Message);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(order);
        }


        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] TradeOrder order)
        {
            try
            {
                if (order.Id > 0)
                {
                    var currentUser = await GetCurrentUser();
                    order.UpdatedBy = currentUser.Id;
                    order.UpdateDate = DateTime.Now;

                    (new TradeOrderBLL(_unit)).UpdateOrder(order);
                }
            }
            catch (ApplicationException appEx)
            {
                return BadRequest(appEx.Message);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok(order);
        }


        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                TradeOrderBLL bll = new TradeOrderBLL(_unit);

                bool isAdmin = await AppUserManager.IsInRoleAsync(currentUser.Id, "Admin");

                if (isAdmin)
                {
                    bll.RemoveOrder(id);
                }
                else
                {
                    var o = bll.GetByID(id);

                    if (o.UpdatedBy == currentUser.Id)
                    {
                        bll.RemoveOrder(id);
                    }
                    else
                    {
                        BadRequest("You don't have permission to delete this order.");
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

        [HttpPost]
        [Route("tradereview")]
        public async Task<IHttpActionResult> UpdateTradeReview(TradeReview review)
        {
            try
            {
                var currentUser = await GetCurrentUser();

                review.UpdatedBy = currentUser.Id;
                review.UpdatedDT = DateTime.Now;

                new TradeReviewBLL(_unit).Update(review);
            }
            catch (ApplicationException appEx)
            {
                return BadRequest(appEx.Message);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(review);
        }


        [HttpGet]
        [Route("tradereview")]
        public async Task<IHttpActionResult> GetTradeReviewByPosition(int positionId)
        {
            TradeReview review = null;
            try
            {
                review = new TradeReviewBLL(_unit).GetTradeReviewByPositionId(positionId);
            }
            catch (ApplicationException appEx)
            {
                return BadRequest(appEx.Message);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(review);
        }


        #endregion
    }
}