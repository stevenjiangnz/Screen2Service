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
    [RoutePrefix("api/idea")]
    [Authorize]
    public class IdeaController : BaseApiController
    {

        #region Constructor
        public IdeaController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            Idea j = null;

            try
            {
                IdeaBLL bll = new IdeaBLL(_unit);

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
        [Route("getlist")]
        public async Task<IHttpActionResult> Get()
        {
            List<Idea> slist = null;

            try
            {
                var currentUser = await GetCurrentUser();

                IdeaBLL bll = new IdeaBLL(_unit);

                slist = bll.GetList(currentUser.Id).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(slist);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] Idea idea)
        {
            try
            {
                if (idea.Id > 0)
                {
                    var currentUser = await GetCurrentUser();
                    idea.Owner = currentUser.Id;
                    idea.Modified = DateTime.Now;

                    (new IdeaBLL(_unit)).Update(idea);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok(idea);
        }


        [HttpPut]
        public async Task<IHttpActionResult> Put(Idea idea)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                idea.Owner = currentUser.Id;
                idea.Created = DateTime.Now;
                idea.Modified = DateTime.Now;

                new IdeaBLL(_unit).Create(idea);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(idea);
        }


        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                IdeaBLL bll = new IdeaBLL(_unit);

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
                        BadRequest("You don't have permission to delete this idea.");
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