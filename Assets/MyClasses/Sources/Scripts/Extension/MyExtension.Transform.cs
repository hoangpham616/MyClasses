/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyExtension.Transform (version 1.1)
 */

using UnityEngine;

namespace MyClasses
{
    public static partial class MyExtension
    {
        /// <summary>
        /// Return the first child of the specified object.
        /// </summary>
        public static Transform GetFirstChild(this Transform transform)
        {
            if (transform != null)
            {
                int countChild = transform.childCount;
                if (countChild > 0)
                {
                    return transform.GetChild(0);
                }
            }

            return null;
        }

        /// <summary>
        /// Return the last child of the specified object.
        /// </summary>
        public static Transform GetLastChild(this Transform transform)
        {
            if (transform != null)
            {
                int countChild = transform.childCount;
                if (countChild > 0)
                {
                    return transform.GetChild(countChild - 1);
                }
            }

            return null;
        }

        /// <summary>
        /// Rotate the specified object around a pivot.
        /// </summary>
        public static void RotateAroundPivot(this Transform transform, Vector3 pivot, Vector3 euler)
        {
            transform.position = MyUtilities.RotatePointAroundPivot(transform.position, pivot, euler);
        }

        /// <summary>
        /// Rotate the specified object around a pivot.
        /// </summary>
        public static void RotateAroundPivot(this Transform transform, Vector3 pivot, Quaternion angle)
        {
            transform.position = MyUtilities.RotatePointAroundPivot(transform.position, pivot, angle);
        }
    }
}