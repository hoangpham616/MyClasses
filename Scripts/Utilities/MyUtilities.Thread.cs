/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Thread (version 1.1)
 */

using System;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        /// <summary>
        /// Start a thread.
        /// </summary>
        public static void StartThread(Action doJob, Action callback)
        {
#if !UNITY_WEBGL
            Action threadJob = () =>
            {
                doJob();
                if (callback != null)
                {
                    callback();
                }
            };

            new System.Threading.Thread(new System.Threading.ThreadStart(threadJob)).Start();
#else
            doJob();
            if (callback != null)
            {
                callback();
            }
#endif
        }
    }
}