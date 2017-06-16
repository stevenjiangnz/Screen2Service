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
    [RoutePrefix("api/rule")]
    [Authorize]
    public class RuleController : BaseApiController
    {
        #region Constructor
        public RuleController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            List<Rule> rlist = null;

            try
            {
                var currentUser = await base.GetCurrentUser();

                RuleBLL bll = new RuleBLL(_unit);

                rlist = bll.GetListByCurrentUser(currentUser.Id);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(rlist);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            Rule s = null;

            try
            {
                var currentUser = await base.GetCurrentUser();

                RuleBLL bll = new RuleBLL(_unit);

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
        public async Task<IHttpActionResult> Put(Rule rule)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                rule.Owner = currentUser.Id;
                rule.UpdatedDT = DateTime.Now;

                new RuleBLL(_unit).Create(rule);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(rule);
        }


        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                RuleBLL bll = new RuleBLL(_unit);

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
                        BadRequest("You don't have permission to delete this rule.");
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
        public async Task<IHttpActionResult> Post(Rule rule)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                rule.Owner = currentUser.Id;
                rule.UpdatedDT = DateTime.Now;

                (new RuleBLL(_unit)).Update(rule);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok(rule);
        }

        #endregion

    }
}