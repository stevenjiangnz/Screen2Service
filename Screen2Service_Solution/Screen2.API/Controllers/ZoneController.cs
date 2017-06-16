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
    [RoutePrefix("api/zone")]
    [Authorize]
    public class ZoneController : BaseApiController
    {
        #region Fields

        #endregion

        #region Constructor
        public ZoneController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            List<Zone> zlist = null;

            try
            {
                ZoneBLL bll = new ZoneBLL(_unit);

                zlist = bll.GetList().ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(zlist);
        }

        [HttpGet]
        [Route("nextdayzone")]
        public async Task<IHttpActionResult> GetNextDayZone(int zoneId)
        {
            Zone nextdayZone = null;
            TradeManager tm = new TradeManager(_unit);
            try
            {
                var currentUser = await GetCurrentUser();
                ZoneBLL bll = new ZoneBLL(_unit);

                nextdayZone = bll.SetZoneNextDay(zoneId);
                tm.ProcessAccountsByZone(currentUser.Id, zoneId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(nextdayZone);
        }


        [HttpPut]
        public async Task<IHttpActionResult> Put(Zone zone)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                zone.Owner = currentUser.Id;

                new ZoneBLL(_unit).Create(zone);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(zone);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(Zone zone)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                zone.Owner = currentUser.Id;

                (new ZoneBLL(_unit)).Update(zone);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok(zone);
        }
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                ZoneBLL bll = new ZoneBLL(_unit);

                bool isAdmin = await AppUserManager.IsInRoleAsync(currentUser.Id, "Admin");

                if (isAdmin)
                {
                    bll.Delete(id);
                }
                else
                {
                    var z = bll.GetByID(id);

                    if (z.Owner == currentUser.Id)
                    {
                        bll.Delete(id);
                    }
                    else
                    {
                        BadRequest("You don't have permission to delete this watch list.");
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