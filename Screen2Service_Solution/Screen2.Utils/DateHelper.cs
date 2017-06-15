using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Utils
{
    public class DateHelper
    {
        public static int DateToInt(DateTime date)
        {
            return (date.Year * 10000) + (date.Month * 100) + date.Day;
        }

        public static DateTime IntToDate(int dateInt)
        {
            String dateString = dateInt.ToString();
            DateTime dt = new DateTime(
                int.Parse(dateString.Substring(0, 4)),
                int.Parse(dateString.Substring(4, 2)),
                int.Parse(dateString.Substring(6, 2))
                );

            return dt;
        }


        public static long IntToJSTicks(int dateInt)
        {
            long jsTicks = 0;
            DateTime dt = IntToDate(dateInt);

            jsTicks = (long)dt.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;

            return jsTicks;
        }

        public static long DateToJSTicks(DateTime dt)
        {
            long jsTicks = 0;

            if (dt != null)
            {
                jsTicks = (long)dt.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            }
            return jsTicks;
        }


        public static int NextTradingDay(int currentDay)
        {
            int nextDay = 0;

            DateTime dt = IntToDate(currentDay).AddDays(1);

            if (dt.DayOfWeek == DayOfWeek.Saturday)
            {
                dt = dt.AddDays(2);
            };

            if (dt.DayOfWeek == DayOfWeek.Sunday)
            {
                dt = dt.AddDays(1);
            };

            nextDay = DateToInt(dt);

            return nextDay;
        }
    }
}
