using Microsoft.AspNet.Identity;
using Screen2.Api.Infrastructure;
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
    [RoutePrefix("api/watch")]
    [Authorize]
    public class WatchController : BaseApiController
    {
        #region Fields

        #endregion

        #region Constructor
        public WatchController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            List<WatchList> wlist = null;

            try
            {
                var currentUser = await base.GetCurrentUser();

                WatchListBLL bll = new WatchListBLL(_unit);

                wlist = bll.GetWatchList(currentUser.Id);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(wlist);
        }

        [HttpGet]
        [Route("getbyzone")]
        public async Task<IHttpActionResult> GetByZone(int? zoneId)
        {
            List<WatchList> wlist = null;

            try
            {
                var currentUser = await base.GetCurrentUser();

                WatchListBLL bll = new WatchListBLL(_unit);

                wlist = bll.GetWatchListByZone(currentUser.Id,zoneId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(wlist);
        }


        [HttpPost]
        [Route("addshare")]
        public async Task<IHttpActionResult> AddShareToWatch(int watchID, string shareList)
        {
            try
            {
                var currentUser = await base.GetCurrentUser();

                (new WatchListBLL(_unit)).AddShareToWatchList(watchID, currentUser.Id, shareList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok();
        }

        [HttpPost]
        [Route("addsharebatch")]
        public async Task<IHttpActionResult> AddShareToWatchBatch(int watchID, [FromBody] int[] inlist)
        {
            string shareList = "";

            shareList = ObjHelper.GetStringFromInts(inlist, ",");

            try
            {
                var currentUser = await base.GetCurrentUser();

                (new WatchListBLL(_unit)).AddShareToWatchList(watchID, currentUser.Id, shareList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok();
        }

        [HttpPost]
        [Route("removesharebatch")]
        public async Task<IHttpActionResult> RemoveShareFromWatchBatch(int watchID, [FromBody] int[] inlist)
        {
            string shareList = "";

            shareList = ObjHelper.GetStringFromInts(inlist, ",");

            try
            {
                var currentUser = await base.GetCurrentUser();

                (new WatchListBLL(_unit)).RemoveShareFromWatchList(watchID, currentUser.Id, shareList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok();
        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            WatchList w = null;

            try
            {
                WatchListBLL bll = new WatchListBLL(_unit);

                w = bll.GetByID(id);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(w);
        }

        [HttpPost]
        [Route("removeshare")]
        public async Task<IHttpActionResult> RemoveShareToWatch(int watchID, string shareList)
        {
            try
            {
                var currentUser = await base.GetCurrentUser();

                (new WatchListBLL(_unit)).RemoveShareFromWatchList(watchID, currentUser.Id, shareList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok();
        }

        [HttpGet]
        [Route("watchdetail/{watchId:int}")]
        public async Task<IHttpActionResult> GetWatchListDetail(int watchId)
        {
            List<WatchListDetail> watchDetailList = null;

            try
            {
                var currentUser = await base.GetCurrentUser();

                watchDetailList = (new WatchListBLL(_unit)).GetWatchListDetail(watchId, currentUser.Id);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok(watchDetailList);

        }


        [HttpGet]
        [Route("getbyshare/{shareId:int}")]
        public async Task<IHttpActionResult> GetWatchListByShare(int shareId)
        {
            List<int> watchDetailList = null;

            try
            {
                var currentUser = await base.GetCurrentUser();

                watchDetailList = (new WatchListBLL(_unit)).GetWatchListByShare(shareId, currentUser.Id);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok(watchDetailList);

        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(WatchList watchList)
        {
            try
            {
                if (watchList.Id > 0)
                {
                    if (!watchList.IsSystem)
                    {
                        var currentUser = await GetCurrentUser();
                        watchList.Owner = currentUser.Id;
                    }
                    else
                    {
                        watchList.Owner = "System";
                    }

                    (new WatchListBLL(_unit)).Update(watchList);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok(watchList);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put(WatchList watchList)
        {
            try
            {

                var currentUser = await GetCurrentUser();
                watchList.Owner = currentUser.Id;

                new WatchListBLL(_unit).Create(watchList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(watchList);
        }


        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                WatchListBLL bll = new WatchListBLL(_unit);

                bool isAdmin = await AppUserManager.IsInRoleAsync(currentUser.Id, "Admin");

                if (isAdmin)
                {
                    bll.Delete(id);
                }
                else
                {
                    var w = bll.GetByID(id);

                    if (!w.IsSystem && w.Owner == currentUser.Id)
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