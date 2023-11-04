namespace Serialization.Services.Extensions
{
    public static class DateExtensions
    {
        public static long ToMillisecondPrecision(this DateTime d)
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public static long ToUnixTimestamp(this DateTime target)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
            var unixTimestamp = System.Convert.ToInt64((target - date).Ticks);

            return unixTimestamp;
        }
    }
}