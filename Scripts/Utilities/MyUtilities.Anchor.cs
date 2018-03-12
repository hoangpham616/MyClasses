/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Anchor (version 1.0)
 */

using UnityEngine;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        #region ----- Public Method -----

        /// <summary>
        /// Anchor.
        /// </summary>
        /// <param name="minOffset">the offset of the lower left corner of the rectangle relative to the lower left anchor</param>
        /// <param name="maxOffset">the offset of the upper right corner of the rectangle relative to the upper right anchor</param>
        public static void Anchor(ref RectTransform rectTrans, EAnchorPreset anchorPreset, EAnchorPivot anchorPivot, Vector2 minOffset, Vector2 maxOffset)
        {
            _SetAnchorPreset(ref rectTrans, anchorPreset);
            _SetAnchorPivot(ref rectTrans, anchorPivot);
            
            rectTrans.offsetMin = minOffset;
            rectTrans.offsetMax = maxOffset;
        }

        /// <summary>
        /// Anchor.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="offsetX">anchored position x</param>
        /// <param name="offsetY">anchored position y</param>
        public static void Anchor(ref RectTransform rectTrans, EAnchorPreset anchorPreset, EAnchorPivot anchorPivot, int width, int height, int offsetX, int offsetY)
        {
            _SetAnchorPreset(ref rectTrans, anchorPreset);
            _SetAnchorPivot(ref rectTrans, anchorPivot);

            rectTrans.anchoredPosition = new Vector2(offsetX, offsetY);
            rectTrans.sizeDelta = new Vector2(width, height);
        }

        /// <summary>
        /// Anchor.
        /// </summary>
        /// <param name="offsetX">anchored position x</param>
        /// <param name="offsetY">anchored position y</param>
        public static void Anchor(ref RectTransform rectTrans, EAnchorPreset anchorPreset, EAnchorPivot anchorPivot, int offsetX, int offsetY)
        {
            _SetAnchorPreset(ref rectTrans, anchorPreset);
            _SetAnchorPivot(ref rectTrans, anchorPivot);

            rectTrans.anchoredPosition = new Vector2(offsetX, offsetY);
        }

        /// <summary>
        /// Anchor.
        /// </summary>
        public static void Anchor(ref RectTransform rectTrans, EAnchorPreset anchorPreset, EAnchorPivot anchorPivot)
        {
            _SetAnchorPreset(ref rectTrans, anchorPreset);
            _SetAnchorPivot(ref rectTrans, anchorPivot);
        }

        /// <summary>
        /// Anchor.
        /// </summary>
        public static void Anchor(ref RectTransform rectTrans, EAnchorPreset anchorPreset)
        {
            _SetAnchorPreset(ref rectTrans, anchorPreset);
        }

        /// <summary>
        /// Anchor.
        /// </summary>
        public static void Anchor(ref RectTransform rectTrans, EAnchorPivot anchorPivot)
        {
            _SetAnchorPivot(ref rectTrans, anchorPivot);
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Set anchor preset.
        /// </summary>
        private static void _SetAnchorPreset(ref RectTransform rectTrans, EAnchorPreset anchorPreset)
        {
            switch (anchorPreset)
            {
                case EAnchorPreset.TopLeft:
                    {
                        rectTrans.anchorMin = new Vector2(0, 1);
                        rectTrans.anchorMax = new Vector2(0, 1);
                    }
                    break;
                case EAnchorPreset.TopCenter:
                    {
                        rectTrans.anchorMin = new Vector2(0.5f, 1);
                        rectTrans.anchorMax = new Vector2(0.5f, 1);
                    }
                    break;
                case EAnchorPreset.TopRight:
                    {
                        rectTrans.anchorMin = Vector2.one;
                        rectTrans.anchorMax = Vector2.one;
                    }
                    break;
                case EAnchorPreset.MiddleLeft:
                    {
                        rectTrans.anchorMin = new Vector2(0, 0.5f);
                        rectTrans.anchorMax = new Vector2(0, 0.5f);
                    }
                    break;
                case EAnchorPreset.MiddleCenter:
                    {
                        rectTrans.anchorMin = new Vector2(0.5f, 0.5f);
                        rectTrans.anchorMax = new Vector2(0.5f, 0.5f);
                    }
                    break;
                case EAnchorPreset.MiddleRight:
                    {
                        rectTrans.anchorMin = new Vector2(1, 0.5f);
                        rectTrans.anchorMax = new Vector2(1, 0.5f);
                    }
                    break;
                case EAnchorPreset.BottomLeft:
                    {
                        rectTrans.anchorMin = Vector2.zero;
                        rectTrans.anchorMax = Vector2.zero;
                    }
                    break;
                case EAnchorPreset.BottomCenter:
                    {
                        rectTrans.anchorMin = new Vector2(0.5f, 0);
                        rectTrans.anchorMax = new Vector2(0.5f, 0);
                    }
                    break;
                case EAnchorPreset.BottomRight:
                    {
                        rectTrans.anchorMin = new Vector2(1, 0.5f);
                        rectTrans.anchorMax = new Vector2(1, 0.5f);
                    }
                    break;
                case EAnchorPreset.VerticalStretchLeft:
                    {
                        rectTrans.anchorMin = Vector2.zero;
                        rectTrans.anchorMax = new Vector2(0, 1);
                    }
                    break;
                case EAnchorPreset.VerticalStretchCenter:
                    {
                        rectTrans.anchorMin = new Vector2(0.5f, 0);
                        rectTrans.anchorMax = new Vector2(0.5f, 1);
                    }
                    break;
                case EAnchorPreset.VerticalStretchRight:
                    {
                        rectTrans.anchorMin = new Vector2(1, 0);
                        rectTrans.anchorMax = Vector2.one;
                    }
                    break;
                case EAnchorPreset.HorizontalStretchTop:
                    {
                        rectTrans.anchorMin = new Vector2(0, 1);
                        rectTrans.anchorMax = Vector2.one;
                    }
                    break;
                case EAnchorPreset.HorizontalStretchMiddle:
                    {
                        rectTrans.anchorMin = new Vector2(0, 0.5f);
                        rectTrans.anchorMax = new Vector2(1, 0.5f);
                    }
                    break;
                case EAnchorPreset.HorizontalStretchBottom:
                    {
                        rectTrans.anchorMin = Vector2.zero;
                        rectTrans.anchorMax = new Vector2(1, 0);
                    }
                    break;
                case EAnchorPreset.DualStretch:
                    {
                        rectTrans.anchorMin = Vector2.zero;
                        rectTrans.anchorMax = Vector2.one;
                    }
                    break;
            }
        }

        /// <summary>
        /// Set anchor pivot.
        /// </summary>
        private static void _SetAnchorPivot(ref RectTransform rectTrans, EAnchorPivot anchorPivot)
        {
            switch (anchorPivot)
            {
                case EAnchorPivot.TopLeft:
                    {
                        rectTrans.pivot = new Vector2(0, 1);
                    }
                    break;
                case EAnchorPivot.TopCenter:
                    {
                        rectTrans.pivot = new Vector2(0.5f, 1);
                    }
                    break;
                case EAnchorPivot.TopRight:
                    {
                        rectTrans.pivot = Vector2.one;
                    }
                    break;
                case EAnchorPivot.MiddleLeft:
                    {
                        rectTrans.pivot = new Vector2(0, 0.5f);
                    }
                    break;
                case EAnchorPivot.MiddleCenter:
                    {
                        rectTrans.pivot = new Vector2(0.5f, 0.5f);
                    }
                    break;
                case EAnchorPivot.MiddleRight:
                    {
                        rectTrans.pivot = new Vector2(1, 0.5f);
                    }
                    break;
                case EAnchorPivot.BottomLeft:
                    {
                        rectTrans.pivot = Vector2.zero;
                    }
                    break;
                case EAnchorPivot.BottomCenter:
                    {
                        rectTrans.pivot = new Vector2(0.5f, 0);
                    }
                    break;
                case EAnchorPivot.BottomRight:
                    {
                        rectTrans.pivot = new Vector2(1, 0);
                    }
                    break;
            }
        }

        #endregion

        #region ----- Enumeration -----

        public enum EAnchorPreset
        {
            TopLeft,
            TopCenter,
            TopRight,

            MiddleLeft,
            MiddleCenter,
            MiddleRight,

            BottomLeft,
            BottomCenter,
            BottomRight,

            VerticalStretchLeft,
            VerticalStretchCenter,
            VerticalStretchRight,

            HorizontalStretchTop,
            HorizontalStretchMiddle,
            HorizontalStretchBottom,

            DualStretch
        }

        public enum EAnchorPivot
        {
            TopLeft,
            TopCenter,
            TopRight,

            MiddleLeft,
            MiddleCenter,
            MiddleRight,

            BottomLeft,
            BottomCenter,
            BottomRight
        }

        #endregion
    }
}