using Screen2.BLL;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Screen2.Api.Controllers
{
    [RoutePrefix("api/shareinfo")]
    public class ShareInfoController : BaseApiController
    {
        #region Constructor
        public ShareInfoController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [Route("getbyshareid/{id:int}")]
        public IHttpActionResult GetByShareID(int id)
        {

            var shareInfo = new ShareInfoBLL(_unit).GetByShareID(id);

            if(shareInfo == null)
            {
                shareInfo = new ShareInfo();
            }

            shareInfo.DataStartDate = new TickerBLL(_unit).GetDataStartDate(id);

            return Ok(shareInfo);
        }


        [HttpPost]
        public IHttpActionResult Post(ShareInfo shareInfo)
        {
            ShareInfo output = null;
            if (shareInfo.Id > 0)
            {
                output = new ShareInfoBLL(_unit).UpdateShareInfo(shareInfo);
            }

            return Ok(output);
        }


        [HttpPut]
        public IHttpActionResult Put(ShareInfo shareInfo)
        {
            try
            {
                shareInfo.LastProcessed = DateTime.Now;
                new ShareInfoBLL(_unit).Create(shareInfo);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(shareInfo);
        }

        #endregion
    }
}