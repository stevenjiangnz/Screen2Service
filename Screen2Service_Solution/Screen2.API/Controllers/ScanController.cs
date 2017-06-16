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
    [RoutePrefix("api/scan")]
    [Authorize]
    public class ScanController : BaseApiController
    {
        #region Constructor
        public ScanController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            List<Scan> slist = null;

            try
            {
                var currentUser = await base.GetCurrentUser();

                ScanBLL bll = new ScanBLL(_unit);

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
            Scan s = null;

            try
            {
                var currentUser = await base.GetCurrentUser();

                ScanBLL bll = new ScanBLL(_unit);

                s = bll.GetByID(id);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(s);
        }


        [HttpGet]
        [Route("{id:int}/rule")]
        public async Task<IHttpActionResult> GetRuleByScan(int id)
        {
            Scan s = null;
            Rule r = null;
            try
            {
                var currentUser = await base.GetCurrentUser();

                ScanBLL bll = new ScanBLL(_unit);

                s = bll.GetByID(id);

                r = new RuleBLL(_unit).GetByID(s.RuleId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(r);
        }

        [HttpGet]
        [Route("run")]
        public async Task<IHttpActionResult> Run(int id)
        {
            List<ScanResult> mlist = null;

            try
            {
                ScanBLL bll = new ScanBLL(_unit);

                var scan = bll.GetByID(id);

                mlist = new ScanCalculator(_unit).RunScan(scan);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(mlist);
        }



        [HttpPut]
        public async Task<IHttpActionResult> Put(Scan scan)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                scan.Owner = currentUser.Id;

                new ScanBLL(_unit).Create(scan);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(scan);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(Scan scan)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                scan.Owner = currentUser.Id;

                (new ScanBLL(_unit)).Update(scan);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok(scan);
        }


        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                ScanBLL bll = new ScanBLL(_unit);

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
                        BadRequest("You don't have permission to delete this scan.");
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