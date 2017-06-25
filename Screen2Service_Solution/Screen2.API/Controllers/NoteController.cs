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
    [RoutePrefix("api/note")]
    [Authorize]
    public class NoteController : BaseApiController
    {
        #region Constructor
        public NoteController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IHttpActionResult> Get(int shareId = 0, int tradingDate =0)
        {
            List<Note> nlist = null;

            try
            {
                var currentUser = await base.GetCurrentUser();

                NoteBLL bll = new NoteBLL(_unit);

                nlist = bll.GetNotesByShareAndUser(shareId, tradingDate, currentUser.Id);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(nlist);
        }

        [HttpGet]
        [Route("getbysharezone")]
        public async Task<IHttpActionResult> GetByShareZone(int shareId, int? zoneId)
        {
            List<Note> nlist = null;

            try
            {
                var currentUser = await base.GetCurrentUser();

                NoteBLL bll = new NoteBLL(_unit);

                nlist = bll.GetNoteListByZone(shareId, zoneId, currentUser.Id);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(nlist);
        }


        [HttpPut]
        public async Task<IHttpActionResult> Put(Note note)
        {
            try
            {
                var currentUser = await GetCurrentUser();

                note.CreatedBy = currentUser.Id;

                note.Create = DateTime.Now;

                new NoteBLL(_unit).Create(note);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(note);
        }


        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                NoteBLL bll = new NoteBLL(_unit);

                bool isAdmin = await AppUserManager.IsInRoleAsync(currentUser.Id, "Admin");

                if (isAdmin)
                {
                    bll.Delete(id);
                }
                else
                {
                    var w = bll.GetByID(id);

                    if ( w.CreatedBy == currentUser.Id)
                    {
                        bll.Delete(id);
                    }
                    else
                    {
                        BadRequest("You don't have permission to delete this note.");
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