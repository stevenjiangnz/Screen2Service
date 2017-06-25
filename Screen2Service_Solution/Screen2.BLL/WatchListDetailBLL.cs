using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class WatchListDetailBLL : BaseBLL<WatchListDetail>, IBaseBLL<WatchListDetail>
    {
        #region Constructors
        public WatchListDetailBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the watch list list by watch identifier.
        /// </summary>
        /// <param name="watchID">The watch identifier.</param>
        /// <returns></returns>
        public List<WatchListDetail> GetWatchListListByWatchID(int watchID)
        {
            List<WatchListDetail> wdList = new List<WatchListDetail>();

            wdList = _unit.DataContext.WatchListDetail.Where(c => c.WatchListId == watchID).ToList();

            return wdList;
        }

        public List<int> GetShareIdsByWatchID(int watchId)
        {
            List<int> shareIdList = new List<int>(); ;
            List<WatchListDetail> watchList = this.GetWatchListListByWatchID(watchId);

            foreach (var w in watchList)
            {
                shareIdList.Add(w.ShareId);
            }

            return shareIdList;
        }
        #endregion
    }
}
