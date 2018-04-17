/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Contains (version 1.0)
 */

using UnityEngine;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        /// <summary>
        /// Check if the triangle contains a point.
        /// </summary>
        public static bool CheckTriangleContainsPoint(Vector2 left, Vector2 mid, Vector2 right, Vector2 point)
        {
            Vector2 nAB = Vector2.zero;
            nAB.x = -(mid.y - left.y);
            nAB.y = mid.x - left.x;
            if (nAB.x * (point.x - left.x) + nAB.y * (point.y - left.y) > 0)
            {
                return false;
            }

            Vector2 nBC = Vector2.zero;
            nBC.x = -(right.y - mid.y);
            nBC.y = right.x - mid.x;
            if (nBC.x * (point.x - mid.x) + nBC.y * (point.y - mid.y) > 0)
            {
                return false;
            }

            Vector2 nCA = Vector2.zero;
            nCA.x = -(left.y - right.y);
            nCA.y = left.x - right.x;
            if (nCA.x * (point.x - right.x) + nCA.y * (point.y - right.y) > 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check if the quadrilateral contains a point.
        /// </summary>
        public static bool CheckQuadrilateralContainsPoint(Vector2 upperLeft, Vector2 upperRight, Vector2 lowerRight, Vector2 lowerLeft, Vector2 point)
        {
            Vector2 nAB = Vector2.zero;
            nAB.x = -(upperRight.y - upperLeft.y);
            nAB.y = upperRight.x - upperLeft.x;
            if (nAB.x * (point.x - upperLeft.x) + nAB.y * (point.y - upperLeft.y) > 0)
            {
                return false;
            }

            Vector2 nBC = Vector2.zero;
            nBC.x = -(lowerRight.y - upperRight.y);
            nBC.y = lowerRight.x - upperRight.x;
            if (nBC.x * (point.x - upperRight.x) + nBC.y * (point.y - upperRight.y) > 0)
            {
                return false;
            }

            Vector2 nCD = Vector2.zero;
            nCD.x = -(lowerLeft.y - lowerRight.y);
            nCD.y = lowerLeft.x - lowerRight.x;
            if (nCD.x * (point.x - lowerRight.x) + nCD.y * (point.y - lowerRight.y) > 0)
            {
                return false;
            }

            Vector2 nDA = Vector2.zero;
            nDA.x = -(upperLeft.y - lowerLeft.y);
            nDA.y = upperLeft.x - lowerLeft.x;
            if (nDA.x * (point.x - lowerLeft.x) + nDA.y * (point.y - lowerLeft.y) > 0)
            {
                return false;
            }

            return true;
        }
    }
}