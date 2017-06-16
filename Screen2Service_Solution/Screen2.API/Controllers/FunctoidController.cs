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
    [RoutePrefix("api/functoid")]
    [Authorize]
    public class FunctoidController : BaseApiController
    {
        #region Constructor
        public FunctoidController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            List<Functoid> flist = null;

            try
            {
                flist = new FunctoidBLL(_unit).GetList().ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(flist);
        }


        [HttpPut]
        public async Task<IHttpActionResult> Put(Functoid functoid)
        {
            try
            {
                new FunctoidBLL(_unit).Create(functoid);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(functoid);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(Functoid functoid)
        {
            try
            {
                new FunctoidBLL(_unit).Update(functoid);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(functoid);
        }


        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                FunctoidBLL bll = new FunctoidBLL(_unit);

                bll.Delete(id);
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