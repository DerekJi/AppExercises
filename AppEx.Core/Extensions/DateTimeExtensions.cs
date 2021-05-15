using System;

namespace AppEx.Core.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long ToUnixTime(this DateTime dt)
        {
            var unixTime = ((DateTimeOffset)dt).ToUnixTimeSeconds();
            return unixTime;
        }
    }
}
