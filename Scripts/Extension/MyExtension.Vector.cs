/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses.Vector (version 1.0)
 */

using UnityEngine;

namespace MyClasses
{
    public static partial class MyExtension
    {
        /// <summary>
        /// Rotate the specified point around another point.
        /// </summary>
        public static void RotatePointAroundAnother(this Vector2 point, Vector2 centerPoint, float angle)
        {
            point = MyUtilities.RotatePointAroundAnother(point, centerPoint, angle);
        }

        /// <summary>
        /// Rotate the specified point around a pivot.
        /// </summary>
        public static void RotateAroundPivot(this Vector3 point, Vector3 pivot, Euler euler)
        {
            point = MyUtilities.RotatePointAroundPivot(point, pivot, euler);
        }

        /// <summary>
        /// Rotate the specified point around a pivot.
        /// </summary>
        public static void RotateAroundPivot(this Vector3 point, Vector3 pivot, Quaternion quaternion)
        {
            point = MyUtilities.RotatePointAroundPivot(point, pivot, quaternion);
        }
    }
}