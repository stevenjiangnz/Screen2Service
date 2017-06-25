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
    [RoutePrefix("api/indicator")]
    [Authorize]
    public class IndicatorController : BaseApiController
    {
        #region Constructor
        public IndicatorController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [Route("sma")]
        public IHttpActionResult GetSMA(int id, int period, int start = 0, int end = 0, bool byCalculate = true)
        {
            List<SMAViewModel> smaList = null;
            try
            {
                IndicatorBLL iBLL = new IndicatorBLL(_unit);

                smaList = iBLL.GetSMAByShareID(id, period, start, end);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(smaList);
        }


        [HttpGet]
        [Route("latesttradingdate")]
        public IHttpActionResult GetLatestTradingDate()
        {
            var latestTradingDate = new IndicatorBLL(_unit).GetLatestTradingDate();
            
            return Ok(latestTradingDate);
        }


        [HttpGet]
        [Route("latesttradingdatebyshare")]
        public IHttpActionResult GetLatestTradingDateByShare(int shareId)
        {
            var latestTradingDate = new IndicatorBLL(_unit).GetLatestTradingDateByShare(shareId);

            return Ok(latestTradingDate);
        }

        [HttpGet]
        [Route("indicator/{shareId:int}/{tradingDate:int}")]
        public IHttpActionResult GetIndicatorByShareDate(int shareId, int tradingDate)
        {
            Screen2.Entity.Indicator indicator;
            Ticker t;

            t = new TickerBLL(_unit).GetTickerByDate(shareId, tradingDate);

            indicator = new IndicatorBLL(_unit).GetIndicatorByShareDate(shareId, tradingDate);


            if ((t != null) && (indicator != null))
            {
                indicator.Open = t.Open;
                indicator.High = t.High;
                indicator.Low = t.Low;
                indicator.Volumn = t.Volumn;
                indicator.AdjustedClose = t.AdjustedClose;
            }

            return Ok(indicator);
        }

        #endregion
    }
}