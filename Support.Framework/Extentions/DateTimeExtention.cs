using System;
using System.Globalization;

namespace Support.Framework.Extentions
{
    public static class DateTimeExtention
    {
        /// <summary>
        /// return ToString("yyyyMMdd");
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToStringUI(this DateTime date)
        {
            return date.ToString("yyyyMMdd");
        }

        /// <summary>
        /// Recebe o .Value de um EditText de Date e retorna DateTime
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string dateStr)
        {
            return DateTime.ParseExact(dateStr.Trim(), "yyyyMMdd", CultureInfo.InvariantCulture);
        }
    }
}
