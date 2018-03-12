/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyEventEmitter (version 1.0)
 */

using System;
using System.Collections.Generic;

namespace MyClasses
{
    public static class MyEventEmitter
    {
        #region ----- Variable -----

        private static Dictionary<int, List<Action<object>>> mEventData = new Dictionary<int, List<Action<object>>>();

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Register an event.
        /// </summary>
        public static void Register(int eventID, Action<object> callback)
        {
            if (!mEventData.ContainsKey(eventID))
            {
                mEventData[eventID] = new List<Action<object>>();
            }
            mEventData[eventID].Add(callback);
        }

        /// <summary>
        /// Unregister an event.
        /// </summary>
        public static void Unregister(int eventID)
        {
            if (mEventData.ContainsKey(eventID))
            {
                mEventData.Remove(eventID);
            }
        }

        /// <summary>
        /// Unregister a callback.
        /// </summary>
        public static void Unregister(int eventID, Action<object> callback)
        {
            if (mEventData.ContainsKey(eventID))
            {
                mEventData[eventID].Remove(callback);
            }
        }

        /// <summary>
        /// Unregister all events.
        /// </summary>
        public static void UnregisterAll()
        {
            mEventData.Clear();
        }

        /// <summary>
        /// Call all callback functions of an event.
        /// </summary>
        public static void Emit(int eventID, object param)
        {
            if (mEventData.ContainsKey(eventID))
            {
                List<Action<object>> actions = mEventData[eventID];
                int length = actions.Count;
                for (int i = 0; i < length; i++)
                {
                    actions[i](param);
                }
            }
        }

        #endregion
    }
}