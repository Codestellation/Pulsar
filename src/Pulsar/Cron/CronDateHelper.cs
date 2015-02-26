using System;

namespace Codestellation.Pulsar.Cron
{
    public class CronDateHelper
    {
        public static int GetLastDayOfMonth(DateTime date)
        {
            return GetLastDayOfMonth(date.Year, date.Month);
        }

        public static int GetLastDayOfMonth(int year, int month)
        {
            int maxValue = 0;
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    maxValue = 31;
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    maxValue = 30;
                    break;
                case 2:
                    maxValue = DateTime.IsLeapYear(year) ? 29 : 28;
                    break;
            }
            return maxValue;
        }

        public static DateTime First(DayOfWeek dayOfWeek, DateTime date)
        {
            for (int day = 1; day <= 7; day++)
            {
                var candidate = new DateTime(date.Year, date.Month, day); //first day of month
                if (candidate.DayOfWeek == dayOfWeek)
                {
                    return candidate;
                }
            }
            throw new InvalidOperationException("This is impossible case");
        }

        public static DayOfWeek ToDayOfWeek(int dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 1:
                    return DayOfWeek.Sunday;
                case 2:
                    return DayOfWeek.Monday;
                case 3:
                    return DayOfWeek.Tuesday;
                case 4:
                    return DayOfWeek.Wednesday;
                case 5:
                    return DayOfWeek.Thursday;
                case 6:
                    return DayOfWeek.Friday;
                case 7:
                    return DayOfWeek.Saturday;
            }
            var message = string.Format("Day of week should be digit between 1 and 7, but was {0}", dayOfWeek);
            throw new FormatException(message);
        }

        public static int ToCronValue(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return 1;
                case DayOfWeek.Monday:
                    return 2;
                case DayOfWeek.Tuesday:
                    return 3;
                case DayOfWeek.Wednesday:
                    return 4;
                case DayOfWeek.Thursday:
                    return 5;
                case DayOfWeek.Friday:
                    return 6;
                case DayOfWeek.Saturday:
                    return 7;
            }
            throw new InvalidOperationException("It's impossible");
        }

        public static DateTime BeginOfNextYear(DateTime currentPoint)
        {
            return new DateTime(currentPoint.Year+1, 1,1);
        }
    }
}