/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyEasing (version 1.1)
 * Reference:   https://easings.net/
 */

using UnityEngine;

namespace MyClasses
{
    public class MyEasing
    {
        #region ----- Enumeration -----

        public enum EEase
        {
            InSine = 11,
            OutSine = 12,
            InOutSine = 13,
            OutInSine = 14,
            InQuad = 21,
            OutQuad = 22,
            InOutQuad = 23,
            OutInQuad = 24,
            InCubic = 31,
            OutCubic = 32,
            InOutCubic = 33,
            OutInCubic = 34,
            InQuart = 41,
            OutQuart = 42,
            InOutQuart = 43,
            OutInQuart = 44,
            InQuint = 51,
            OutQuint = 52,
            InOutQuint = 53,
            OutInQuint = 54,
            InExpo = 61,
            OutExpo = 62,
            InOutExpo = 63,
            OutInExpo = 64,
            InCirc = 71,
            OutCirc = 72,
            InOutCirc = 73,
            OutInCirc = 74,
            InBack = 81,
            OutBack = 82,
            InOutBack = 83,
            OutInBack = 84,
        }

        #endregion

        #region ----- Define -----

        public static readonly Vector2[] IN_SINE = new Vector2[] { new Vector2(0.12f, 0f), new Vector2(0.39f, 0f) };
        public static readonly Vector2[] OUT_SINE = new Vector2[] { new Vector2(0.61f, 1f), new Vector2(0.88f, 1f) };
        public static readonly Vector2[] IN_OUT_SINE = new Vector2[] { new Vector2(0.37f, 0f), new Vector2(0.63f, 1f) };

        public static readonly Vector2[] IN_QUAD = new Vector2[] { new Vector2(0.11f, 0f), new Vector2(0.5f, 0f) };
        public static readonly Vector2[] OUT_QUAD = new Vector2[] { new Vector2(0.5f, 1f), new Vector2(0.89f, 1f) };
        public static readonly Vector2[] IN_OUT_QUAD = new Vector2[] { new Vector2(0.45f, 0f), new Vector2(0.55f, 1f) };

        public static readonly Vector2[] IN_CUBIC = new Vector2[] { new Vector2(0.32f, 0f), new Vector2(0.67f, 0f) };
        public static readonly Vector2[] OUT_CUBIC = new Vector2[] { new Vector2(0.33f, 1f), new Vector2(0.68f, 1f) };
        public static readonly Vector2[] IN_OUT_CUBIC = new Vector2[] { new Vector2(0.65f, 0f), new Vector2(0.35f, 1f) };

        public static readonly Vector2[] IN_QUART = new Vector2[] { new Vector2(0.5f, 0f), new Vector2(0.75f, 0f) };
        public static readonly Vector2[] OUT_QUART = new Vector2[] { new Vector2(0.25f, 1f), new Vector2(0.5f, 1f) };
        public static readonly Vector2[] IN_OUT_QUART = new Vector2[] { new Vector2(0.76f, 0f), new Vector2(0.24f, 1f) };

        public static readonly Vector2[] IN_QUINT = new Vector2[] { new Vector2(0.64f, 0f), new Vector2(0.78f, 0f) };
        public static readonly Vector2[] OUT_QUINT = new Vector2[] { new Vector2(0.22f, 1f), new Vector2(0.36f, 1f) };
        public static readonly Vector2[] IN_OUT_QUINT = new Vector2[] { new Vector2(0.83f, 0f), new Vector2(0.17f, 1f) };

        public static readonly Vector2[] IN_EXPO = new Vector2[] { new Vector2(0.7f, 0f), new Vector2(0.84f, 0f) };
        public static readonly Vector2[] OUT_EXPO = new Vector2[] { new Vector2(0.16f, 1f), new Vector2(0.3f, 1f) };
        public static readonly Vector2[] IN_OUT_EXPO = new Vector2[] { new Vector2(0.87f, 0f), new Vector2(0.13f, 1f) };

        public static readonly Vector2[] IN_CIRC = new Vector2[] { new Vector2(0.55f, 0f), new Vector2(1f, 0.45f) };
        public static readonly Vector2[] OUT_CIRC = new Vector2[] { new Vector2(0f, 0.55f), new Vector2(0.45f, 1f) };
        public static readonly Vector2[] IN_OUT_CIRC = new Vector2[] { new Vector2(0.85f, 0f), new Vector2(0.15f, 1f) };

        public static readonly Vector2[] IN_BACK = new Vector2[] { new Vector2(0.36f, 0f), new Vector2(0.66f, -0.56f) };
        public static readonly Vector2[] OUT_BACK = new Vector2[] { new Vector2(0.34f, 1.56f), new Vector2(0.64f, 1f) };
        public static readonly Vector2[] IN_OUT_BACK = new Vector2[] { new Vector2(0.68f, -0.6f), new Vector2(0.32f, 1.6f) };

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Returns bezier point by curve type.
        /// </summary>
        public static Vector2[] GetBezierPoints(EEase ease, Vector2 fromPoint, Vector2 toPoint)
        {
            Vector2[] points = new Vector2[4];

            points[0] = fromPoint;
            points[3] = toPoint;

            Vector2 direction = toPoint - fromPoint;
            switch (ease)
            {
                case EEase.InSine:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_SINE[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_SINE[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_SINE[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_SINE[1].y * direction.y);
                    }
                    break;

                case EEase.OutSine:
                    {
                        points[1].x = points[0].x + (MyEasing.OUT_SINE[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.OUT_SINE[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.OUT_SINE[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.OUT_SINE[1].y * direction.y);
                    }
                    break;

                case EEase.InOutSine:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_OUT_SINE[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_OUT_SINE[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_OUT_SINE[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_OUT_SINE[1].y * direction.y);
                    }
                    break;

                case EEase.OutInSine:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_OUT_SINE[1].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_OUT_SINE[1].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_OUT_SINE[0].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_OUT_SINE[0].y * direction.y);
                    }
                    break;

                case EEase.InQuad:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_QUAD[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_QUAD[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_QUAD[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_QUAD[1].y * direction.y);
                    }
                    break;

                case EEase.OutQuad:
                    {
                        points[1].x = points[0].x + (MyEasing.OUT_QUAD[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.OUT_QUAD[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.OUT_QUAD[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.OUT_QUAD[1].y * direction.y);
                    }
                    break;

                case EEase.InOutQuad:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_OUT_QUAD[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_OUT_QUAD[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_OUT_QUAD[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_OUT_QUAD[1].y * direction.y);
                    }
                    break;

                case EEase.OutInQuad:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_OUT_QUAD[1].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_OUT_QUAD[1].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_OUT_QUAD[0].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_OUT_QUAD[0].y * direction.y);
                    }
                    break;

                case EEase.InCubic:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_CUBIC[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_CUBIC[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_CUBIC[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_CUBIC[1].y * direction.y);
                    }
                    break;

                case EEase.OutCubic:
                    {
                        points[1].x = points[0].x + (MyEasing.OUT_CUBIC[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.OUT_CUBIC[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.OUT_CUBIC[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.OUT_CUBIC[1].y * direction.y);
                    }
                    break;

                case EEase.InOutCubic:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_OUT_CUBIC[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_OUT_CUBIC[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_OUT_CUBIC[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_OUT_CUBIC[1].y * direction.y);
                    }
                    break;

                case EEase.OutInCubic:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_OUT_CUBIC[1].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_OUT_CUBIC[1].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_OUT_CUBIC[0].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_OUT_CUBIC[0].y * direction.y);
                    }
                    break;

                case EEase.InQuart:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_QUART[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_QUART[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_QUART[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_QUART[1].y * direction.y);
                    }
                    break;

                case EEase.OutQuart:
                    {
                        points[1].x = points[0].x + (MyEasing.OUT_QUART[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.OUT_QUART[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.OUT_QUART[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.OUT_QUART[1].y * direction.y);
                    }
                    break;

                case EEase.InOutQuart:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_OUT_QUART[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_OUT_QUART[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_OUT_QUART[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_OUT_QUART[1].y * direction.y);
                    }
                    break;

                case EEase.OutInQuart:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_OUT_QUART[1].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_OUT_QUART[1].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_OUT_QUART[0].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_OUT_QUART[0].y * direction.y);
                    }
                    break;

                case EEase.InQuint:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_QUINT[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_QUINT[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_QUINT[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_QUINT[1].y * direction.y);
                    }
                    break;

                case EEase.OutQuint:
                    {
                        points[1].x = points[0].x + (MyEasing.OUT_QUINT[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.OUT_QUINT[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.OUT_QUINT[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.OUT_QUINT[1].y * direction.y);
                    }
                    break;

                case EEase.InOutQuint:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_OUT_QUINT[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_OUT_QUINT[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_OUT_QUINT[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_OUT_QUINT[1].y * direction.y);
                    }
                    break;

                case EEase.OutInQuint:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_OUT_QUINT[1].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_OUT_QUINT[1].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_OUT_QUINT[0].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_OUT_QUINT[0].y * direction.y);
                    }
                    break;

                case EEase.InExpo:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_EXPO[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_EXPO[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_EXPO[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_EXPO[1].y * direction.y);
                    }
                    break;

                case EEase.OutExpo:
                    {
                        points[1].x = points[0].x + (MyEasing.OUT_EXPO[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.OUT_EXPO[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.OUT_EXPO[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.OUT_EXPO[1].y * direction.y);
                    }
                    break;

                case EEase.InOutExpo:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_OUT_EXPO[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_OUT_EXPO[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_OUT_EXPO[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_OUT_EXPO[1].y * direction.y);
                    }
                    break;

                case EEase.OutInExpo:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_OUT_EXPO[1].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_OUT_EXPO[1].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_OUT_EXPO[0].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_OUT_EXPO[0].y * direction.y);
                    }
                    break;

                case EEase.InCirc:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_CIRC[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_CIRC[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_CIRC[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_CIRC[1].y * direction.y);
                    }
                    break;

                case EEase.OutCirc:
                    {
                        points[1].x = points[0].x + (MyEasing.OUT_CIRC[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.OUT_CIRC[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.OUT_CIRC[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.OUT_CIRC[1].y * direction.y);
                    }
                    break;

                case EEase.InOutCirc:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_OUT_CIRC[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_OUT_CIRC[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_OUT_CIRC[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_OUT_CIRC[1].y * direction.y);
                    }
                    break;

                case EEase.OutInCirc:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_OUT_CIRC[1].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_OUT_CIRC[1].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_OUT_CIRC[0].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_OUT_CIRC[0].y * direction.y);
                    }
                    break;

                case EEase.InBack:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_BACK[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_BACK[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_BACK[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_BACK[1].y * direction.y);
                    }
                    break;

                case EEase.OutBack:
                    {
                        points[1].x = points[0].x + (MyEasing.OUT_BACK[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.OUT_BACK[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.OUT_BACK[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.OUT_BACK[1].y * direction.y);
                    }
                    break;

                case EEase.InOutBack:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_OUT_BACK[0].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_OUT_BACK[0].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_OUT_BACK[1].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_OUT_BACK[1].y * direction.y);
                    }
                    break;

                case EEase.OutInBack:
                    {
                        points[1].x = points[0].x + (MyEasing.IN_OUT_BACK[1].x * direction.x);
                        points[1].y = points[0].y + (MyEasing.IN_OUT_BACK[1].y * direction.y);
                        points[2].x = points[0].x + (MyEasing.IN_OUT_BACK[0].x * direction.x);
                        points[2].y = points[0].y + (MyEasing.IN_OUT_BACK[0].y * direction.y);
                    }
                    break;
            }

            return points;
        }

        /// <summary>
        /// Returns time of a cubic bezier curve by distance.
        /// </summary>
        /// <param name="distance">0 -> 1</param>
        public static float GetTimeByDistance(EEase ease, float distance)
        {
            switch (ease)
            {
                case EEase.InSine:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, IN_SINE[0], IN_SINE[1], Vector2.one, distance).magnitude;
                    }

                case EEase.OutSine:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, OUT_SINE[0], OUT_SINE[1], Vector2.one, distance).magnitude;
                    }

                case EEase.InOutSine:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, IN_OUT_SINE[0], IN_OUT_SINE[1], Vector2.one, distance).magnitude;
                    }

                case EEase.InQuad:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, IN_QUAD[0], IN_QUAD[1], Vector2.one, distance).magnitude;
                    }

                case EEase.OutQuad:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, OUT_QUAD[0], OUT_QUAD[1], Vector2.one, distance).magnitude;
                    }

                case EEase.InOutQuad:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, IN_OUT_QUAD[0], IN_OUT_QUAD[1], Vector2.one, distance).magnitude;
                    }

                case EEase.InCubic:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, IN_CUBIC[0], IN_CUBIC[1], Vector2.one, distance).magnitude;
                    }

                case EEase.OutCubic:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, OUT_CUBIC[0], OUT_CUBIC[1], Vector2.one, distance).magnitude;
                    }

                case EEase.InOutCubic:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, IN_OUT_CUBIC[0], IN_OUT_CUBIC[1], Vector2.one, distance).magnitude;
                    }

                case EEase.InQuart:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, IN_QUART[0], IN_QUART[1], Vector2.one, distance).magnitude;
                    }

                case EEase.OutQuart:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, OUT_QUART[0], OUT_QUART[1], Vector2.one, distance).magnitude;
                    }

                case EEase.InOutQuart:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, IN_OUT_QUART[0], IN_OUT_QUART[1], Vector2.one, distance).magnitude;
                    }

                case EEase.InQuint:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, IN_QUINT[0], IN_QUINT[1], Vector2.one, distance).magnitude;
                    }

                case EEase.OutQuint:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, OUT_QUINT[0], OUT_QUINT[1], Vector2.one, distance).magnitude;
                    }

                case EEase.InOutQuint:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, IN_OUT_QUINT[0], IN_OUT_QUINT[1], Vector2.one, distance).magnitude;
                    }

                case EEase.InExpo:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, IN_EXPO[0], IN_EXPO[1], Vector2.one, distance).magnitude;
                    }

                case EEase.OutExpo:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, OUT_EXPO[0], OUT_EXPO[1], Vector2.one, distance).magnitude;
                    }

                case EEase.InOutExpo:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, IN_OUT_EXPO[0], IN_OUT_EXPO[1], Vector2.one, distance).magnitude;
                    }

                case EEase.InCirc:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, IN_CIRC[0], IN_CIRC[1], Vector2.one, distance).magnitude;
                    }

                case EEase.OutCirc:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, OUT_CIRC[0], OUT_CIRC[1], Vector2.one, distance).magnitude;
                    }

                case EEase.InOutCirc:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, IN_OUT_CIRC[0], IN_OUT_CIRC[1], Vector2.one, distance).magnitude;
                    }

                case EEase.InBack:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, IN_BACK[0], IN_BACK[1], Vector2.one, distance).magnitude;
                    }

                case EEase.OutBack:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, OUT_BACK[0], OUT_BACK[1], Vector2.one, distance).magnitude;
                    }

                case EEase.InOutBack:
                    {
                        return MyBezier.GetCubicBezierPoint(Vector2.zero, IN_OUT_BACK[0], IN_OUT_BACK[1], Vector2.one, distance).magnitude;
                    }
            }

            return distance;
        }

        #endregion
    }
}
