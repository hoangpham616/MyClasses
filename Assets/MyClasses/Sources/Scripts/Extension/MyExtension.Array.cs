/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyExtension.Array (version 1.2)
 */

using UnityEngine;

namespace MyClasses
{
    public static partial class MyExtension
    {
        /// <summary>
        /// Print items in array.
        /// </summary>
        public static void Log(this bool[] array)
        {
            Debug.Log(MyUtilities.ToString(array));
        }

        /// <summary>
        /// Print items in array.
        /// </summary>
        public static void LogWarning(this bool[] array)
        {
            Debug.LogWarning(MyUtilities.ToString(array));
        }

        /// <summary>
        /// Print items in array.
        /// </summary>
        public static void LogError(this bool[] array)
        {
            Debug.LogError(MyUtilities.ToString(array));
        }

        /// <summary>
        /// Print items in array.
        /// </summary>
        public static void Log(this int[] array)
        {
            Debug.Log(MyUtilities.ToString(array));
        }

        /// <summary>
        /// Print items in array.
        /// </summary>
        public static void LogWarning(this int[] array)
        {
            Debug.LogWarning(MyUtilities.ToString(array));
        }

        /// <summary>
        /// Print items in array.
        /// </summary>
        public static void LogError(this int[] array)
        {
            Debug.LogError(MyUtilities.ToString(array));
        }

        /// <summary>
        /// Print items in array.
        /// </summary>
        public static void Log(this int[][] arrays)
        {
            Debug.Log(MyUtilities.ToString(arrays));
        }

        /// <summary>
        /// Print items in array.
        /// </summary>
        public static void LogWarning(this int[][] arrays)
        {
            Debug.LogWarning(MyUtilities.ToString(arrays));
        }

        /// <summary>
        /// Print items in array.
        /// </summary>
        public static void LogError(this int[][] arrays)
        {
            Debug.LogError(MyUtilities.ToString(arrays));
        }

        /// <summary>
        /// Print items in array.
        /// </summary>
        public static void Log(this float[] array)
        {
            Debug.Log(MyUtilities.ToString(array));
        }

        /// <summary>
        /// Print items in array.
        /// </summary>
        public static void LogWarning(this float[] array)
        {
            Debug.LogWarning(MyUtilities.ToString(array));
        }

        /// <summary>
        /// Print items in array.
        /// </summary>
        public static void LogError(this float[] array)
        {
            Debug.LogError(MyUtilities.ToString(array));
        }

        /// <summary>
        /// Print items in array.
        /// </summary>
        public static void Log(this float[][] arrays)
        {
            Debug.Log(MyUtilities.ToString(arrays));
        }

        /// <summary>
        /// Print items in array.
        /// </summary>
        public static void LogWarning(this float[][] arrays)
        {
            Debug.LogWarning(MyUtilities.ToString(arrays));
        }

        /// <summary>
        /// Print items in array.
        /// </summary>
        public static void LogError(this float[][] arrays)
        {
            Debug.LogError(MyUtilities.ToString(arrays));
        }
    }
}
