/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Position (version 1.0)
 */

using UnityEngine;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        /// <summary>
        /// Check if the position on left side or right side of the target.
        /// </summary>
        /// <returns>0: forward/backward</returns>
        /// <returns>1: right side</returns>
        /// <returns>-1: left side</returns>
        public static int GetLeftOrRightSide(Transform target, Vector3 checkingPosition)
        {
            Vector3 direction = checkingPosition - target.position;
            Vector3 crossProduct = Vector3.Cross(target.forward, direction);
            float dotProduct = Vector3.Dot(crossProduct, target.up);
            if (dotProduct > 0)
            {
                return 1;
            }
            else if (dotProduct < 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Check if the position on front side or back side of the target.
        /// </summary>
        /// <returns>0: left/right</returns>
        /// <returns>1: front side</returns>
        /// <returns>-1: back side</returns>
        public static int GetFrontOrBackSide(Transform target, Vector3 checkingPosition)
        {
            Vector3 direction = checkingPosition - target.position;
            Vector3 crossProduct = Vector3.Cross(target.right, direction);
            float dotProduct = Vector3.Dot(crossProduct, target.up);
            if (dotProduct > 0)
            {
                return -1;
            }
            else if (dotProduct < 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}