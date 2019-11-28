/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyOnlineTime (version 1.1)
 */

using System;

namespace MyClasses
{
    public class MyOnlineTime
    {
        #region ----- Variable -----

        private static long mOffsetUnixTime = 0;

        #endregion

        #region ----- Property -----

        /// <summary>
        /// Return the current unix timestamp in milliseconds.
        /// </summary>
        public static long CurrentUnixTime
        {
            get { return MyLocalTime.CurrentUnixTime + mOffsetUnixTime; }
        }

        /// <summary>
        /// Return the current DateTime.
        /// </summary>
        public static DateTime CurrentDateTime
        {
            get { return MyLocalTime.GetTimeSinceUnixEpoch(CurrentUnixTime); }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Sync with online time.
        /// </summary>
        /// <param name="onlineUnixTime">milliseconds</param>
        public static void SyncOnlineTime(long onlineUnixTime)
        {
            mOffsetUnixTime = onlineUnixTime - MyLocalTime.CurrentUnixTime;
        }

        #endregion
    }
}