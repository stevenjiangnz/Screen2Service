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
    [RoutePrefix("api/journey")]
    [Authorize]
    public class JourneyController : BaseApiController
    {

        #region Constructor
        public JourneyController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            Journey j = null;

            try
            {
                JourneyBLL bll = new JourneyBLL(_unit);

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
            List<Journey> slist = null;

            try
            {
                var currentUser = await GetCurrentUser();

                JourneyBLL bll = new JourneyBLL(_unit);

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
        public async Task<IHttpActionResult> Put(Journey journey)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                journey.Owner = currentUser.Id;
                journey.Created = DateTime.Now;
                journey.Modified = DateTime.Now;

                new JourneyBLL(_unit).Create(journey);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(journey);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] Journey journey)
        {
            try
            {
                if (journey.Id > 0)
                {
                    var currentUser = await GetCurrentUser();
                    journey.Owner = currentUser.Id;
                    journey.Modified = DateTime.Now;

                    (new JourneyBLL(_unit)).Update(journey);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok(journey);
        }


        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                JourneyBLL bll = new JourneyBLL(_unit);

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
                        BadRequest("You don't have permission to delete this journey.");
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