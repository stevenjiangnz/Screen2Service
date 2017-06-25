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
    [RoutePrefix("api/dailyscan")]
    [Authorize]
    public class DailyScanController : BaseApiController
    {

        #region Constructor
        public DailyScanController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [HttpGet]
        [Route("getbyzone")]
        public async Task<IHttpActionResult> GetByZone(int? zoneId = null)
        {
            List<DailyScan> slist = null;

            try
            {
                var currentUser = await base.GetCurrentUser();

                DailyScanBLL bll = new DailyScanBLL(_unit);

                slist = bll.GetDailyScanListByUser(currentUser.Id, zoneId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(slist);
        }


        [HttpPut]
        public async Task<IHttpActionResult> Put(DailyScan dailyScan)
        {
            try
            {
                var currentUser = await GetCurrentUser();

                dailyScan.Modified = DateTime.Now;
                dailyScan.Owner = currentUser.Id;

                if(dailyScan.UseRule)
                {
                    dailyScan.Formula = null;
                }
                else
                {
                    dailyScan.RuleId = null;
                }

                new DailyScanBLL(_unit).Create(dailyScan);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(dailyScan);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(DailyScan dailyScan)
        {
            try
            {
                if (dailyScan.Id > 0)
                {
                    var currentUser = await GetCurrentUser();
                    dailyScan.Owner = currentUser.Id;
                    dailyScan.Modified = DateTime.Now;

                    (new DailyScanBLL(_unit)).Update(dailyScan);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok(dailyScan);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                DailyScanBLL bll = new DailyScanBLL(_unit);

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
                        BadRequest("You don't have permission to delete this dail scan.");
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