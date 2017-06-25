using Screen2.Entity;
using Screen2.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screen2.DAL;
using Screen2.Shared;
using log4net;
using Screen2.Utils;

namespace Screen2.Hisotry.Loader
{
    class Program
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static UnitWork unit = new UnitWork(new DataContext());
        static TickerBLL tBLL = new TickerBLL(unit);
        static IndicatorBLL iBLL = new IndicatorBLL(unit);
        static AlertBLL aBLL = new AlertBLL(unit);
        static DailyScanBLL dBLL = new DailyScanBLL(unit);
        static SettingBLL sBLL = new SettingBLL(unit);

        protected static ILog Log
        {
            get
            {
                return _log;
            }
        }

        static void Main(string[] args)
        {
            bool isValidInput = true;

            try
            {
                if (args.Length == 1)
                {
                    string mode = args[0].ToUpper();

                    switch (mode)
                    {
                        case "LOADTICKERS":
                            LoadTickers();
                            break;
                        case "PROCESSINDICATORS":
                            ProcessIndicators();
                            break;
                        case "PROCESSALERTS":
                            ProcessAlerts();
                            break;
                        case "PROCESSDAILYSCAN":
                            ProcessDailyScan();
                            break;
                        default:
                            isValidInput = false;
                            break;
                    }

                }
                else
                {
                    isValidInput = false;
                }

                if (!isValidInput)
                {
                    Console.WriteLine("Wrong parameter. \n" + 
                        "Eg. Screen2.Hisotry.Loader.exe LoadTickers|ProcessIndicators");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                LogHelper.Error(_log, ex.ToString());
            }
        }


        public static void LoadTickers()
        {
            Console.WriteLine("Start to load tickers");
            int tradingDate = DateHelper.DateToInt(DateTime.Now);
            tBLL.UpdateDailyShareTickerBatchToday(tradingDate.ToString());
            Console.WriteLine("Finish loading tickers");
        }

        public static void ProcessIndicators()
        {
            Console.WriteLine("Start to process indiactors");
            iBLL.ProcessIndicatorFull();
            Console.WriteLine("Start processing indiactors");
        }

        public static void ProcessAlerts()
        {
            Console.WriteLine("Start to process alerts");
            aBLL.ProcessAlertsFull(true);
            Console.WriteLine("Start processing alerts");
        }

        public static void ProcessDailyScan()
        {
            //Console.WriteLine("Start to process daily scan");
            //dBLL.ProcessDailyScanFull(true);
            //Console.WriteLine("Start processing daily scan");
        }
    }
}
