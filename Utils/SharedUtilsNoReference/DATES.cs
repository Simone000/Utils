using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtilsNoReference
{
    public static class DATES
    {
        //TimeZoneInfo romeZone = TimeZoneInfo. FindSystemTimeZoneById(@"W. Europe Standard Time");
        //DateTime DateRome = TimeZoneInfo.ConvertTime(Data, romeZone);


        public static List<DateTime> GetGiorni(DateTime From, int Quanti)
        {
            var ris = Enumerable.Range(0, Quanti).Select(n => From.AddDays(n)).ToList();
            return ris;
        }

        /// <summary>
        /// tutti i giorni nel mese corrente
        /// </summary>
        /// <param name="Anno"></param>
        /// <param name="Mese"></param>
        /// <returns></returns>
        public static List<DateTime> GetDaysInMonth(int Anno, int Mese) //todo: da testare (ultima settimana forse non c'è)
        {
            //tutti i giorni nel mese corrente
            var dates = Enumerable.Range(1, DateTime.DaysInMonth(Anno, Mese))
                                  .Select(n => new DateTime(Anno, Mese, n))
                                  .ToList();
            return dates;
        }


        public static double ToJSData(this DateTime Data)
        {
            return Data.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local))
               .TotalMilliseconds;
        }

        public static double? ToJSData(this DateTime? Data)
        {
            if (Data == null)
                return null;
            return Data.Value.ToJSData();
        }

        /// <summary>
        /// da usare quando la data che mi arriva dal db non è utc
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="SourceTimeZoneID">W.Europe Standard Time</param>
        /// <param name="DestinationTimeZoneID">UTC</param>
        /// <returns></returns>
        public static double ToJSData(this DateTime Data,
                                      string SourceTimeZoneID, string DestinationTimeZoneID = "UTC")
        {
            var DateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Data, SourceTimeZoneID, DestinationTimeZoneID);

            return DateUtc.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
               .TotalMilliseconds;

        }

        /// <summary>
        /// da usare quando la data che mi arriva dal db non è utc
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="SourceTimeZoneID">W.Europe Standard Time</param>
        /// <param name="DestinationTimeZoneID">UTC</param>
        /// <returns></returns>
        public static double? ToJSData(this DateTime? Data,
                                       string SourceTimeZoneID, string DestinationTimeZoneID = "UTC")
        {
            if (Data == null)
                return null;

            return Data.Value.ToJSData(SourceTimeZoneID, DestinationTimeZoneID);
        }


        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)DayOfWeek.Monday - (int)jan1.DayOfWeek;

            DateTime firstMonday = jan1.AddDays(daysOffset);
            if (firstMonday.Year < year)
            {
                firstMonday = firstMonday.AddDays(7);
            }

            //int firstWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(jan1, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
            /*int firstWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(jan1, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

            if(firstWeek > 1)
            {
                weekOfYear -= 1;
            }*/

            weekOfYear -= 1;

            var ris = firstMonday.AddDays(weekOfYear * 7);
            return ris;
        }


        public static int GetWeekOfMonth(this DateTime time)
        {
            DateTime first = new DateTime(time.Year, time.Month, 1);
            return time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }

        public static int GetWeekOfYear(this DateTime time)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            //static GregorianCalendar _gc = new GregorianCalendar();
            //return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(time, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
            //return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            //return _gc.GetWeekOfYear(time, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
        }


        //public static int GetEpochTime()
        //{
        //    return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        //}
    }
}
