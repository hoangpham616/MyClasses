/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Draw (version 1.0)
 */

using UnityEngine;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        /// <summary>
        /// Draw a line in editor for debug.
        /// </summary>
        /// <param name="from">start position</param>
        /// <param name="to">end position</param>
        /// <param name="color">color</param>
        /// <param name="duration">the number of second exists</param>
        public static void DrawDebugLine(Vector3 from, Vector3 to, Color color, float duration = 10f)
        {
#if UNITY_EDITOR
            Debug.DrawLine(from, to, color, duration, true);
#endif
        }

        /// <summary>
        /// Draw a line in editor for debug.
        /// </summary>
        /// <param name="from">start position</param>
        /// <param name="to">end position</param>
        /// <param name="distance">length</param>
        /// <param name="color">color</param>
        /// <param name="duration">the number of second exists</param>
        public static void DrawDebugLine(Vector3 from, Vector3 direction, float distance, Color color, float duration = 10f)
        {
#if UNITY_EDITOR
            Debug.DrawLine(from, from + (direction.normalized * distance), color, duration, false);
#endif
        }

        /// <summary>
        /// Draw a circle in editor for debug.
        /// </summary>
        /// <param name="position">center of circle</param>
        /// <param name="up">vector up</param>
        /// <param name="radius">radius</param>
        /// <param name="color">color</param>
        /// <param name="duration">the number of second exists</param>
        public static void DrawDebugCircle(Vector3 position, Vector3 up, float radius, Color color, float duration = 10f)
        {
#if UNITY_EDITOR
            up = up.normalized * radius;
            Vector3 forward = Vector3.Slerp(up, -up, 0.5f);

            Vector3 lastPoint = position + (Quaternion.AngleAxis(0, up) * forward).normalized * radius;
            Vector3 nextPoint = Vector3.zero;

            float offsetAngle = 360 / 30f;

            for (float i = offsetAngle; i <= 360; i += offsetAngle)
            {
                nextPoint = position + (Quaternion.AngleAxis(i, up) * forward).normalized * radius;

                Debug.DrawLine(lastPoint, nextPoint, color, duration);

                lastPoint = nextPoint;
            }
#endif
        }

        /// <summary>
        /// Draw an arc in editor for debug.
        /// </summary>
        /// <param name="position">center of arc</param>
        /// <param name="forward">vector forward</param>
        /// <param name="up">vector up</param>
        /// <param name="radius">radius</param>
        /// <param name="arcAngle">arc angle</param>
        /// <param name="color">color</param>
        /// <param name="duration">the number of second exists</param>
        public static void DrawDebugArc(Vector3 position, Vector3 forward, Vector3 up, float radius, float arcAngle, Color color, float duration = 10f)
        {
#if UNITY_EDITOR
            forward = forward.normalized * radius;
            up = up.normalized * radius;

            float halfArcAngle = arcAngle / 2f;
            float offsetAngle = arcAngle / 15f;

            Vector3 beginPos = position + (Quaternion.AngleAxis(-halfArcAngle, up) * forward).normalized * radius;
            Vector3 endPos = position + (Quaternion.AngleAxis(halfArcAngle, up) * forward).normalized * radius;

            Vector3 lastPoint = beginPos;
            Vector3 nextPoint = Vector3.zero;

            for (float i = -halfArcAngle; i <= halfArcAngle; i += offsetAngle)
            {
                nextPoint = position + (Quaternion.AngleAxis(i, up) * forward).normalized * radius;

                Debug.DrawLine(lastPoint, nextPoint, color, duration);

                lastPoint = nextPoint;
            }

            Debug.DrawLine(position, beginPos, color, duration);
            Debug.DrawLine(position, endPos, color, duration);
#endif
        }
    }
}