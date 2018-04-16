/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Rotation (version 1.0)
 */

using UnityEngine;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        /// <summary>
        /// Rotate a point around another point.
        /// </summary>
        public static Vector3 RotatePointAroundAnotherPoint(Vector2 point, Vector2 centerPoint, float angle)
        {
            Vector3 euler = Vector3.zero;
            euler.z = angle;
            return RotatePointAroundPivot(point, centerPoint, Quaternion.Euler(euler));
        }

        /// <summary>
        /// Rotate a point around pivot.
        /// </summary>
        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 euler)
        {
            return RotatePointAroundPivot(point, pivot, Quaternion.Euler(euler));
        }

        /// <summary>
        /// Rotate a point around pivot.
        /// </summary>
        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angle)
        {
            return angle * (point - pivot) + pivot;
        }
    }
}