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
    }
}