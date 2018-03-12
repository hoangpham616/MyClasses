/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Shuffle (version 1.0)
 */

using UnityEngine;
using System.Collections.Generic;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        /// <summary>
        /// Shuffle elements in an array.
        /// </summary>
        public static T[] Shuffle<T>(T[] array)
        {
            for (int i = 0; i < array.Length; i += 2)
            {
                Swap<T>(ref array[i], ref array[Random.Range(0, array.Length)]);
            }
            return array;
        }

        /// <summary>
        /// Shuffle elements in a list.
        /// </summary>
        public static List<T> Shuffle<T>(List<T> list)
        {
            int count = list.Count;
            for (int i = 0; i < count; i += 2)
            {
                Swap<T>(list, i, Random.Range(0, count));
            }
            return list;
        }
    }
}