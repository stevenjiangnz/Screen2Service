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
    [RoutePrefix("api/broker")]
    [Authorize]
    public class BrokerController : BaseApiController
    {

        #region Constructor
        public BrokerController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            List<Broker> slist = null;

            try
            {
                BrokerBLL bll = new BrokerBLL(_unit);

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
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            Broker s = null;

            try
            {
                BrokerBLL bll = new BrokerBLL(_unit);

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
        public async Task<IHttpActionResult> Put(Broker broker)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                broker.Owner = currentUser.Id;

                new BrokerBLL(_unit).Create(broker);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(broker);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(Broker broker)
        {
            try
            {
                if (broker.Id > 0)
                {
                    var currentUser = await GetCurrentUser();
                    broker.Owner = currentUser.Id;

                    (new BrokerBLL(_unit)).Update(broker);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok(broker);
        }


        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                BrokerBLL bll = new BrokerBLL(_unit);

                bool isAdmin = await AppUserManager.IsInRoleAsync(currentUser.Id, "Admin");

                if (isAdmin)
                {
                    bll.Delete(id);
                }
                else
                {
                    var w = bll.GetByID(id);

                    if (w.Owner == currentUser.Id)
                    {
                        bll.Delete(id);
                    }
                    else
                    {
                        BadRequest("You don't have permission to delete this broker.");
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