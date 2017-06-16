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
    [RoutePrefix("api/alert")]
    [Authorize]
    public class AlertController : BaseApiController
    {

        #region Constructor
        public AlertController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [HttpGet]
        [Route("getbysharezone")]
        public async Task<IHttpActionResult> Get(int shareId, int? zoneId)
        {
            List<Alert> nlist = null;

            try
            {
                var currentUser = await base.GetCurrentUser();

                AlertBLL bll = new AlertBLL(_unit);

                nlist = bll.GetAlertListByShareZone(currentUser.Id, shareId, zoneId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(nlist);
        }

        [HttpGet]
        [Route("getbyzone")]
        public async Task<IHttpActionResult> Get(int? zoneId)
        {
            List<Alert> nlist = null;

            try
            {
                var currentUser = await base.GetCurrentUser();

                AlertBLL bll = new AlertBLL(_unit);

                nlist = bll.GetAlertListByZone(currentUser.Id, zoneId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(nlist);
        }


        [HttpPut]
        public async Task<IHttpActionResult> Put(Alert alert)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                alert.Owner = currentUser.Id;
                alert.ModifiedDate = DateTime.Now;

                new AlertBLL(_unit).Create(alert);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(alert);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(Alert alert)
        {
            try
            {
                if (alert.Id > 0)
                {
                    var currentUser = await GetCurrentUser();
                    alert.Owner = currentUser.Id;
                    alert.ModifiedDate = DateTime.Now;

                    (new AlertBLL(_unit)).Update(alert);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok(alert);
        }


        [HttpPost]
        [Route("verify")]
        public async Task<IHttpActionResult> Verify(Alert alert)
        {
            VerifyResult result = new VerifyResult();
            try
            {
                if (alert.Id > 0)
                {
                    result = (new AlertBLL(_unit)).VerifyAlert(alert);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok(result);
        }


        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                AlertBLL bll = new AlertBLL(_unit);

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
                        BadRequest("You don't have permission to delete this alert.");
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