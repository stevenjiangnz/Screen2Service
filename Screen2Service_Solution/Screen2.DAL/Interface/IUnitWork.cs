using Screen2.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Screen2.DAL.Interface
{
    public interface IUnitWork : IDisposable
    {
        DataContext DataContext { get; }
        //IRepository<WatchListDetail> WatchListDetail { get; }
        void SaveChanges();
    }
}
