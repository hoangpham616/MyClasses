/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyExtension.List (version 1.0)
 */

using UnityEngine;
using System.Collections.Generic;

namespace MyClasses
{
    public static partial class MyExtension
    {
        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void Log(this List<int> list)
        {
            Debug.Log(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogWarning(this List<int> list)
        {
            Debug.LogWarning(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogError(this List<int> list)
        {
            Debug.LogError(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void Log(this List<int[]> list)
        {
            Debug.Log(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogWarning(this List<int[]> list)
        {
            Debug.LogWarning(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogError(this List<int[]> list)
        {
            Debug.LogError(MyUtilities.ToString(list));
        }
    }
}