using Screen2.BLL;
using Screen2.DAL;
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
    [Authorize]
    [RoutePrefix("api/share")]
    public class ShareController :BaseApiController
    {
        #region Constructor
        public ShareController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [Authorize]
        public IHttpActionResult Get()
        {
            ShareBLL sBLL = new ShareBLL(_unit);

            var shares = sBLL.GetList();
            return Ok(shares);
        }

        [HttpPost]
        public IHttpActionResult Post(Share share)
        {
            Share output= null;
            if (share.Id > 0)
            {
               output = new ShareBLL(_unit).UpdateShare(share);
            }

            return Ok(output);
        }

        [HttpPut]
        public IHttpActionResult Put(Share share)
        {

            try {
                new ShareBLL(_unit).Create(share);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(share);
        }

        [HttpGet]
        [Route("search/{tradingDate:int}")]
        public IHttpActionResult SearchStockDaily(int tradingDate)
        {
            List<OutStockSearchResult> sList = new List<OutStockSearchResult>();

            if (tradingDate <= 0)
            {
                return BadRequest("trading date must be provided...");
            }

            try
            {
                sList = new IndicatorBLL(_unit).SearchShareDayTicker(tradingDate, tradingDate);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(sList);
        }


        [HttpGet]
        [Route("search/latest")]
        public IHttpActionResult SearchStockDailyLatest()
        {
            List<OutStockSearchResult> sList = new List<OutStockSearchResult>();

            try
            {
                sList = new IndicatorBLL(_unit).GetShareDayTickerLatest();
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(sList);
        }


        [HttpGet]
        [Route("searchbywatch")]
        public IHttpActionResult SearchStockDailyByWatch(int watchId, int tradingDate, bool reverse)
        {
            List<OutStockSearchResult> sList = new List<OutStockSearchResult>();

            try
            {
                sList = new IndicatorBLL(_unit).SearchShareByWatch(watchId, tradingDate, reverse);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(sList);
        }

        [HttpGet]
        [Route("searchalert")]
        public async Task<IHttpActionResult> SearchAlert(int tradingDate, bool force, int? zoneId)
        {
            List<OutStockSearchResult> nlist = null;

            try
            {
                var currentUser = await base.GetCurrentUser();

                AlertBLL bll = new AlertBLL(_unit);

                nlist = bll.SearchAlert(currentUser.Id, tradingDate, force, zoneId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(nlist);
        }

        [HttpGet]
        [Route("searchbydailyscan")]
        public async Task<IHttpActionResult> SearchByDailyScan(int tradingDate, int dailyScanId, bool force)
        {
            List<OutStockSearchResult> nlist = null;

            try
            {
                var currentUser = await base.GetCurrentUser();

                DailyScanBLL bll = new DailyScanBLL(_unit);

                nlist = bll.SearchScan(tradingDate, dailyScanId, force);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(nlist);
        }



        [HttpGet]
        [Route("getstocklistbyzone")]
        public IHttpActionResult GetStockListByZone(int zoneId)
        {
            List<Share> sList = new List<Share>();

            try
            {
                sList = new ShareBLL(_unit).GetShareListByZone(zoneId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(sList);
        }
        #endregion
    }
}