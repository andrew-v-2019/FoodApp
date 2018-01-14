using System;

namespace Services.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime NextFriday(this DateTime dt)
        {
            var friday = GetNextWeekday(dt, DayOfWeek.Friday);
            return friday;
        }

        private static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        public static DateTime ParseDate(this string dtString)
        {
            var fr = DateTime.Now.NextFriday();
            if (string.IsNullOrWhiteSpace(dtString)) return fr;
            return DateTime.TryParse(dtString, out DateTime dateValue) ? dateValue : fr;
        }
    }

}
