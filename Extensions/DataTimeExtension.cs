namespace CampusFlow.Extensions
{
    public static class DataTimeExtension
    {
        public static DateTime DateByWeekDay(this DateTime dateTime, DayOfWeek dateByWeekDay)
        {
            int diff = (7 + (dateTime.DayOfWeek - dateByWeekDay)) % 7;
            return dateTime.AddDays(-1 * diff).Date;
        }
    }
}
