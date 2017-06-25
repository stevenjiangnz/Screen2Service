using Screen2.BLL;
using Screen2.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Screen2.Api.Controllers
{
    
    public class MarketController : BaseApiController
    {
        #region Constructor
        public MarketController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [Authorize]
        public IHttpActionResult Get()
        {


            var markets = new MarketBLL(_unit).GetList();
            return Ok(markets);
        }
        #endregion
    }
}