/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Time (version 1.5)
 */

using System;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        #region ----- Public Method -----

        /// <summary>
        /// Convert seconds to time string.
        /// </summary>
        public static string ConvertSecondsToTimeString(int seconds, ETimeFormat format = ETimeFormat.Flexibility)
        {
            if (seconds < 0)
            {
                seconds = 0;
            }

            string timeString = string.Empty;

            switch (format)
            {
                case ETimeFormat.Flexibility:
                    {
                        int sec = seconds % 60;
                        int min = seconds / 60;
                        int hour = min / 60;
                        int day = hour / 24;
                        if (day > 0)
                        {
                            timeString = (day < 10 ? "0" + day : day.ToString());
                        }
                        if (hour > 0)
                        {
                            if (day > 0)
                            {
                                hour %= 24;
                                timeString += ":" + (hour < 10 ? "0" + hour : hour.ToString());
                            }
                            else
                            {
                                timeString += (hour < 10 ? "0" + hour : hour.ToString());
                            }
                            min %= 60;
                            timeString += ":" + (min < 10 ? "0" + min : min.ToString());
                        }
                        else
                        {
                            timeString = (min < 10 ? "0" + min : min.ToString());
                        }
                        timeString += ":" + (sec < 10 ? "0" + sec : sec.ToString());
                    }
                    break;
                case ETimeFormat.DD_HH_MM_SS:
                    {
                        int sec = seconds % 60;
                        int min = (seconds % 3600) / 60;
                        int hour = (seconds % 86400) / 3600;
                        int day = seconds / 86400;
                        timeString = (day < 10 ? "0" + day : day.ToString()) + ":" + (hour < 10 ? "0" + hour : hour.ToString()) + ":" + (min < 10 ? "0" + min : min.ToString()) + ":" + (sec < 10 ? "0" + sec : sec.ToString());
                    }
                    break;
                case ETimeFormat.HH_MM_SS:
                    {
                        int sec = seconds % 60;
                        int min = (seconds % 3600) / 60;
                        int hour = seconds / 3600;
                        timeString = (hour < 10 ? "0" + hour : hour.ToString()) + ":" + (min < 10 ? "0" + min : min.ToString()) + ":" + (sec < 10 ? "0" + sec : sec.ToString());
                    }
                    break;
                case ETimeFormat.MM_SS:
                    {
                        int sec = seconds % 60;
                        int min = seconds / 60;
                        timeString = (min < 10 ? "0" + min : min.ToString()) + ":" + (sec < 10 ? "0" + sec : sec.ToString());
                    }
                    break;
            }

            return timeString;
        }

        /// <summary>
        /// Convert TimeSpan to time string.
        /// </summary>
        /// <param name="highestUnitTime">highest unit of time can be shown</param>
        /// <param name="maxUnitQuantity">maximum number of units can be shown</param>
        /// <param name="isShowZeroValue">always show even it is zero value</param>
        public static string ConvertTimeSpanToFullTimeString(TimeSpan timeSpan, EUnitTime highestUnitTime = EUnitTime.Second, int maxUnitQuantity = 2, bool isShowZeroValue = false,
            string keyDay = "_TEXT_DAY", string keyDays = "_TEXT_DAYS", string keyHour = "_TEXT_HOUR", string keyHours = "_TEXT_HOURS",
            string keyMinute = "_TEXT_MINUTE", string keyMinutes = "_TEXT_MINUTES", string keySecond = "_TEXT_SECOND", string keySeconds = "_TEXT_SECONDS")
        {
            return _ConvertTimeSpanToTimeString(timeSpan, highestUnitTime, maxUnitQuantity, isShowZeroValue, keyDay, keyDays, keyHour, keyHours, keyMinute, keyMinutes, keySecond, keySeconds);
        }

        /// <summary>
        /// Convert TimeSpan to time string.
        /// </summary>
        /// <param name="highestUnitTime">highest unit of time can be shown</param>
        /// <param name="maxUnitQuantity">maximum number of units can be shown</param>
        /// <param name="isShowZeroValue">always show even it is zero value</param>
        public static string ConvertTimeSpanToShortTimeString(TimeSpan timeSpan, EUnitTime highestUnitTime = EUnitTime.Second, int maxUnitQuantity = 2, bool isShowZeroValue = false,
            string keyDay = "_TEXT_SHORT_DAY", string keyDays = "_TEXT_SHORT_DAYS", string keyHour = "_TEXT_SHORT_HOUR", string keyHours = "_TEXT_SHORT_HOURS",
            string keyMinute = "_TEXT_SHORT_MINUTE", string keyMinutes = "_TEXT_SHORT_MINUTES", string keySecond = "_TEXT_SHORT_SECOND", string keySeconds = "_TEXT_SHORT_SECONDS")
        {
            return _ConvertTimeSpanToTimeString(timeSpan, highestUnitTime, maxUnitQuantity, isShowZeroValue, keyDay, keyDays, keyHour, keyHours, keyMinute, keyMinutes, keySecond, keySeconds);
        }

        /// <summary>
        /// Convert TimeSpan to time string.
        /// </summary>
        /// <param name="smallestUnitTime">smallest unit of time can be shown</param>
        /// <param name="isShowZeroValue">always show even it is zero value</param>
        public static string ConvertTimeSpanToFullTimeString(TimeSpan timeSpan, EUnitTime smallestUnitTime = EUnitTime.Second, bool isShowZeroValue = false,
            string keyDay = "_TEXT_DAY", string keyDays = "_TEXT_DAYS", string keyHour = "_TEXT_HOUR", string keyHours = "_TEXT_HOURS",
            string keyMinute = "_TEXT_MINUTE", string keyMinutes = "_TEXT_MINUTES", string keySecond = "_TEXT_SECOND", string keySeconds = "_TEXT_SECONDS")
        {
            return _ConvertTimeSpanToTimeString(timeSpan, smallestUnitTime, isShowZeroValue, keyDay, keyDays, keyHour, keyHours, keyMinute, keyMinutes, keySecond, keySeconds);
        }

        /// <summary>
        /// Convert TimeSpan to time string.
        /// </summary>
        /// <param name="smallestUnitTime">smallest unit of time can be shown</param>
        /// <param name="isShowZeroValue">always show even it is zero value</param>
        public static string ConvertTimeSpanToShortTimeString(TimeSpan timeSpan, EUnitTime smallestUnitTime = EUnitTime.Second, bool isShowZeroValue = false,
            string keyDay = "_TEXT_SHORT_DAY", string keyDays = "_TEXT_SHORT_DAYS", string keyHour = "_TEXT_SHORT_HOUR", string keyHours = "_TEXT_SHORT_HOURS",
            string keyMinute = "_TEXT_SHORT_MINUTE", string keyMinutes = "_TEXT_SHORT_MINUTES", string keySecond = "_TEXT_SHORT_SECOND", string keySeconds = "_TEXT_SHORT_SECONDS")
        {
            return _ConvertTimeSpanToTimeString(timeSpan, smallestUnitTime, isShowZeroValue, keyDay, keyDays, keyHour, keyHours, keyMinute, keyMinutes, keySecond, keySeconds);
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Convert TimeSpan to time string.
        /// </summary>
        private static string _ConvertTimeSpanToTimeString(TimeSpan timeSpan, EUnitTime highestUnitTime, int maxUnitQuantity, bool isShowZeroValue,
            string keyDay, string keyDays, string keyHour, string keyHours, string keyMinute, string keyMinutes, string keySecond, string keySeconds)
        {
            string time = string.Empty;

            if (highestUnitTime >= EUnitTime.Day && maxUnitQuantity > 0)
            {
                if (timeSpan.Days >= 2)
                {
                    time = timeSpan.Days + " " + MyLocalizationManager.Instance.LoadKey(keyDays);
                }
                else if (timeSpan.Days >= 1)
                {
                    time = "1 " + MyLocalizationManager.Instance.LoadKey(keyDay);
                }
                else if (isShowZeroValue)
                {
                    time = "0 " + MyLocalizationManager.Instance.LoadKey(keyDay);
                }
            }

            if (time != string.Empty)
            {
                maxUnitQuantity--;
            }

            if (highestUnitTime >= EUnitTime.Hour && maxUnitQuantity > 0)
            {
                if (timeSpan.Hours >= 2)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + timeSpan.Hours + " " + MyLocalizationManager.Instance.LoadKey(keyHours);
                }
                else if (timeSpan.Hours >= 1)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + "1 " + MyLocalizationManager.Instance.LoadKey(keyHour);
                }
                else if (isShowZeroValue)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + "0 " + MyLocalizationManager.Instance.LoadKey(keyHour);
                }
            }

            if (time != string.Empty)
            {
                maxUnitQuantity--;
            }

            if (highestUnitTime >= EUnitTime.Minute && maxUnitQuantity > 0)
            {
                if (timeSpan.Minutes >= 2)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + timeSpan.Minutes + " " + MyLocalizationManager.Instance.LoadKey(keyMinutes);
                }
                else if (timeSpan.Minutes >= 1)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + "1 " + MyLocalizationManager.Instance.LoadKey(keyMinute);
                }
                else if (isShowZeroValue)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + "0 " + MyLocalizationManager.Instance.LoadKey(keyMinute);
                }
            }

            if (time != string.Empty)
            {
                maxUnitQuantity--;
            }

            if (highestUnitTime >= EUnitTime.Second && maxUnitQuantity > 0)
            {
                if (timeSpan.Seconds >= 2)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + timeSpan.Seconds + " " + MyLocalizationManager.Instance.LoadKey(keySeconds);
                }
                else if (timeSpan.Seconds >= 1)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + "1 " + MyLocalizationManager.Instance.LoadKey(keySecond);
                }
                else if (isShowZeroValue)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + "0 " + MyLocalizationManager.Instance.LoadKey(keySecond);
                }
            }

            return time;
        }

        /// <summary>
        /// Convert TimeSpan to time string.
        /// </summary>
        private static string _ConvertTimeSpanToTimeString(TimeSpan timeSpan, EUnitTime smallestUnitTime, bool isShowZeroValue,
            string keyDay, string keyDays, string keyHour, string keyHours, string keyMinute, string keyMinutes, string keySecond, string keySeconds)
        {
            string time = string.Empty;

            if (smallestUnitTime <= EUnitTime.Day)
            {
                if (timeSpan.Days >= 2)
                {
                    time = timeSpan.Days + " " + MyLocalizationManager.Instance.LoadKey(keyDays);
                }
                else if (timeSpan.Days >= 1)
                {
                    time = "1 " + MyLocalizationManager.Instance.LoadKey(keyDay);
                }
                else if (isShowZeroValue)
                {
                    time = "0 " + MyLocalizationManager.Instance.LoadKey(keyDay);
                }
            }

            if (smallestUnitTime <= EUnitTime.Hour)
            {
                if (timeSpan.Hours >= 2)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + timeSpan.Hours + " " + MyLocalizationManager.Instance.LoadKey(keyHours);
                }
                else if (timeSpan.Hours >= 1)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + "1 " + MyLocalizationManager.Instance.LoadKey(keyHour);
                }
                else if (isShowZeroValue)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + "0 " + MyLocalizationManager.Instance.LoadKey(keyHour);
                }
            }

            if (smallestUnitTime <= EUnitTime.Minute)
            {
                if (timeSpan.Minutes >= 2)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + timeSpan.Minutes + " " + MyLocalizationManager.Instance.LoadKey(keyMinutes);
                }
                else if (timeSpan.Minutes >= 1)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + "1 " + MyLocalizationManager.Instance.LoadKey(keyMinute);
                }
                else if (isShowZeroValue)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + "0 " + MyLocalizationManager.Instance.LoadKey(keyMinute);
                }
            }

            if (smallestUnitTime <= EUnitTime.Second)
            {
                if (timeSpan.Seconds >= 2)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + timeSpan.Seconds + " " + MyLocalizationManager.Instance.LoadKey(keySeconds);
                }
                else if (timeSpan.Seconds >= 1)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + "1 " + MyLocalizationManager.Instance.LoadKey(keySecond);
                }
                else if (isShowZeroValue)
                {
                    time += (time.Length > 0 ? " " : string.Empty) + "0 " + MyLocalizationManager.Instance.LoadKey(keySecond);
                }
            }

            return time;
        }

        #endregion

        #region ----- Enumeration -----

        public enum ETimeFormat
        {
            Flexibility,
            DD_HH_MM_SS,
            HH_MM_SS,
            MM_SS
        }

        public enum EUnitTime
        {
            Second,
            Minute,
            Hour,
            Day
        }

        #endregion
    }
}