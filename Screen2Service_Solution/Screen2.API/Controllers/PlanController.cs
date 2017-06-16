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
    [RoutePrefix("api/plan")]
    [Authorize]
    public class PlanController : BaseApiController
    {

        #region Constructor
        public PlanController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            Plan j = null;

            try
            {
                PlanBLL bll = new PlanBLL(_unit);

                j = bll.GetByID(id);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(j);
        }

        [HttpGet]
        [Route("getbyzone")]
        public async Task<IHttpActionResult> Get(int? zoneId)
        {
            List<Plan> slist = null;

            try
            {
                var currentUser = await GetCurrentUser();

                PlanBLL bll = new PlanBLL(_unit);

                slist = bll.GetListByZone(currentUser.Id, zoneId).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(slist);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put(Plan Plan)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                Plan.Owner = currentUser.Id;
                Plan.Created = DateTime.Now;
                Plan.Modified = DateTime.Now;

                new PlanBLL(_unit).Create(Plan);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(Plan);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] Plan Plan)
        {
            try
            {
                if (Plan.Id > 0)
                {
                    var currentUser = await GetCurrentUser();
                    Plan.Owner = currentUser.Id;
                    Plan.Modified = DateTime.Now;

                    (new PlanBLL(_unit)).Update(Plan);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok(Plan);
        }


        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                PlanBLL bll = new PlanBLL(_unit);

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
                        BadRequest("You don't have permission to delete this plan.");
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