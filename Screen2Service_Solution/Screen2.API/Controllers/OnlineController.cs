using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Screen2.DAL.Interface;

namespace Screen2.Api.Controllers
{
    [RoutePrefix("api/online")]
    public class OnlineController : BaseApiController
    {
        #region Constructor
        public OnlineController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region methods
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok("OK. Service is online. on trigger when pull request. from testcity");
        }
        #endregion
    }
}