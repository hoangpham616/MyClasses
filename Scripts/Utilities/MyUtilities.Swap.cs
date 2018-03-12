/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Swap (version 1.2)
 */

using UnityEngine;
using System.Collections.Generic;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        /// <summary>
        /// Swap 2 numbers.
        /// </summary>
        public static void Swap(ref int a, ref int b)
        {
            a = a ^ b;
            b = a ^ b;
            a = a ^ b;
        }

        /// <summary>
        /// Swap 2 elements.
        /// </summary>
        public static void Swap<T>(ref T a, ref T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// Swap 2 elements in a list.
        /// </summary>
        public static void Swap<T>(List<T> list, int a, int b)
        {
            var temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }

        /// <summary>
        /// Swap 2 game objects' position.
        /// </summary>
        public static void Swap(ref Transform a, ref Transform b)
        {
            Vector3 tmp = a.position;
            a.position = b.position;
            b.position = tmp;
        }

        /// <summary>
        /// Swap 2 game objects' position.
        /// </summary>
        public static void Swap(ref RectTransform a, ref RectTransform b)
        {
            Vector3 tmp = a.position;
            a.position = b.position;
            b.position = tmp;
        }
    }
}