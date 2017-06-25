using Screen2.DAL;
using Screen2.DAL.Interface;
using Screen2.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Screen2.Api.Controllers
{
    //[RoutePrefix("api/order")]
    public class OrderController : BaseApiController
    {
        #region Constructor
        public OrderController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion


        #region Methods
        public IHttpActionResult Get()
        {
            UnitWork unit = new UnitWork(new DataContext());

            var orders = (List<TradeOrder>)unit.TradeOrder.Get();
            return Ok(orders);
        }
        #endregion
    }
}