using Screen2.BLL;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Screen2.Api.Controllers
{
    [RoutePrefix("api/ticker")]
    [Authorize]
    public class TickerController : BaseApiController
    {
        #region Constructor
        public TickerController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [HttpGet]
        public IHttpActionResult Get(int id, int start = 0, int end = 0, string indicator="")
        {
            int MINI_TICKER = 30;
            TickerViewModel tickerVM = new TickerViewModel();
            List<Ticker> tickerList = null;

            IndicatorBLL iBLL = new IndicatorBLL(_unit);


            try
            {
                if (end == 0)
                {
                    end = DateHelper.DateToInt(DateTime.Now);
                }

                tickerList = new TickerBLL(_unit).GetTickerListByShareDB(id, start, end);

                // Load indicators
                tickerVM.TickerList = tickerList;

                if (!string.IsNullOrEmpty(indicator) && tickerList.Count >= MINI_TICKER)
                {
                    tickerVM.Indicators = iBLL.GetIndicators(tickerList, indicator);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(tickerVM);
        }

        [Route("getlatest/{shareId:int}")]
        [HttpGet]
        public IHttpActionResult GetLatest(int shareId)
        {
            Ticker t = null;

            try
            {
                t = new TickerBLL(_unit).GetLastTicker(shareId, null);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(t);
        }

        [Route("getlatestbyzone")]
        [HttpGet]
        public IHttpActionResult GetLatestByZone(int shareId, int? zoneId)
        {
            Ticker t = null;

            try
            {
                if(!zoneId.HasValue)
                {
                    t = new TickerBLL(_unit).GetLastTicker(shareId, null);
                }
                else
                {
                    var z = new ZoneBLL(_unit).GetByID(zoneId.Value);

                    t = new TickerBLL(_unit).GetLastTicker(shareId, z.TradingDate);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(t);
        }

        [Route("getnextbyzone")]
        [HttpGet]
        public IHttpActionResult GetNextByZone(int shareId, int? zoneId)
        {
            Ticker t = null;

            try
            {
                if (zoneId.HasValue)
                {
                    var z = new ZoneBLL(_unit).GetByID(zoneId.Value);
                    int tradingDate = (int)(new IndicatorBLL(_unit)).GetNextTradingDate(z.TradingDate);
                    t = new TickerBLL(_unit).GetTickerByDate(shareId, tradingDate);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(t);
        }



        [Route("gettickerbydate")]
        [HttpGet]
        public IHttpActionResult GetTickerByDate(int shareId, int tradingDate)
        {
            Ticker t = null;

            try
            {
                t = new TickerBLL(_unit).GetTickerByDate(shareId, tradingDate);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(t);
        }


        [Route("getverificationlist")]
        [HttpGet]
        public IHttpActionResult GetVerificationList(int shareId, int tradingDate, int count)
        {
            List<Ticker> t = null;

            try 
            {
                t = new TickerBLL(_unit).GetTickerListByDate(shareId, tradingDate, count);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(t);
        }

        [Route("getbyshareid/{id:int}/{startDate:int}/{endDate:int}")]
        public IHttpActionResult GetByShareID(int id, int startDate = 0, int endDate = 0)
        {
            List<Ticker> tickerList = null;

            try
            {
                if (endDate == 0)
                {
                    endDate = DateHelper.DateToInt(DateTime.Now);
                }

                tickerList = new TickerBLL(_unit).GetTickerListByShareDB(id, startDate, endDate);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(tickerList);
        }


        [HttpPost]
        [Route("refreshhistory/{id:int}")]
        public IHttpActionResult RefreshHistoryTickers(int id)
        {
            try
            {
                new TickerBLL(_unit).ReloadHistoryPriceTicker(id);
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

    public class TickerViewModel
    {
        public List<Ticker> TickerList
        {
            get; set;
        }

        public Dictionary<string, double?[]> Indicators
        {
            get;
            set;
        }

    }
}