/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyLocalTime (version 1.1)
 */

using System;

namespace MyClasses
{
    public class MyLocalTime
    {
        #region ----- Variable -----

        private static readonly DateTime mEpochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        #endregion

        #region ----- Property -----

        /// <summary>
        /// Return the epoch time.
        /// </summary>
        public static DateTime EpochTime
        {
            get { return mEpochTime; }
        }

        /// <summary>
        /// Return the current unix timestamp in milliseconds.
        /// </summary>
        public static long CurrentUnixTime
        {
            get { return (long)(DateTime.UtcNow - mEpochTime).TotalMilliseconds; }
        }

        /// <summary>
        /// Return the today unix timestamp in milliseconds.
        /// </summary>
        public static long TodayUnixTime
        {
            get
            {
                DateTime today = GetTimeSinceToday(0);
                return ConvertDateTimeToUnixTime(today);
            }
        }

        /// <summary>
        /// Return the tomorrow unix timestamp in milliseconds.
        /// </summary>
        public static long TomorrowUnixTime
        {
            get
            {
                DateTime tomorrow = GetTimeSinceToday(86400000);
                return ConvertDateTimeToUnixTime(tomorrow);
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Return time since epoch time.
        /// </summary>
        /// <param name="milliseconds">number of milliseconds add into epoch time</param>
        public static DateTime GetTimeSinceUnixEpoch(long milliseconds)
        {
            return mEpochTime.AddMilliseconds(milliseconds);
        }

        /// <summary>
        /// Return time since today.
        /// </summary>
        /// <param name="milliseconds">number of milliseconds add into today</param>
        public static DateTime GetTimeSinceToday(long milliseconds)
        {
            return DateTime.UtcNow.Date.AddMilliseconds(milliseconds);
        }

        /// <summary>
        /// Return time since Monday.
        /// </summary>
        /// <param name="milliseconds">number of milliseconds add into this Monday</param>
        public static DateTime GetTimeSinceMonday(long milliseconds)
        {
            DateTime monday = DateTime.UtcNow;

            int diff = monday.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0)
            {
                diff += 7;
            }

            monday = monday.AddDays(-1 * diff).Date;

            return monday.AddMilliseconds(milliseconds);
        }

        /// <summary>
        /// Convert DateTime to unix time in milliseconds.
        /// </summary>
        public static long ConvertDateTimeToUnixTime(DateTime date)
        {
            return (long)(date - mEpochTime).TotalMilliseconds;
        }

        /// <summary>
        /// Convert local DateTime to unix time in milliseconds.
        /// </summary>
        public static long ConvertDateLocalTimeToUnixTime(DateTime date)
        {
            return (long)(date - mEpochTime.ToLocalTime()).TotalMilliseconds;
        }

        /// <summary>
        /// Check this moment in time range of today.
        /// </summary>
        public static bool IsThisMomentInTimeRangeOfToday(long beginMilliseconds, long endMilliseconds)
        {
            DateTime utcNow = DateTime.UtcNow;
            DateTime today = utcNow.Date;

            DateTime begin = today.AddMilliseconds(beginMilliseconds);
            DateTime end = today.AddMilliseconds(endMilliseconds);

            return begin <= utcNow && utcNow <= end;
        }

        #endregion
    }
}