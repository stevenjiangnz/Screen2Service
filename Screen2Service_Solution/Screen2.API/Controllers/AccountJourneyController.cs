using Screen2.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Screen2.Api.Controllers
{
    [RoutePrefix("api/journey")]
    [Authorize]
    public class AccountJourneyController : BaseApiController
    {
        #region Constructor
        public AccountJourneyController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion


    }
}