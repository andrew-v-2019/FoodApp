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
    }
}
