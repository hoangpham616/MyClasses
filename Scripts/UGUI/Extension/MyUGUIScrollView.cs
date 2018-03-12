/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIScrollView (version 2.0)
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MyClasses.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class MyUGUIScrollView : MonoBehaviour
    {
        #region ----- Variable -----

        private ScrollRect mScrollRect;

        #endregion

        #region ----- Implement MonoBehaviour -----

        void Awake()
        {
            mScrollRect = gameObject.GetComponent<ScrollRect>();
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Move x-axis.
        /// </summary>
        /// <param name="targetPosition">0: left, 1: right</param>
        public void MoveX(float targetPosition, float second)
        {
            StartCoroutine(_MoveX(targetPosition, second));
        }

        /// <summary>
        /// Move y-axis.
        /// </summary>
        /// <param name="targetPosition">0: top, 1: bottom</param>
        public void MoveY(float targetPosition, float second)
        {
            StartCoroutine(_MoveY(1 - targetPosition, second));
        }

        /// <summary>
        /// Move to start of scrollview.
        /// </summary>
        public void MoveToStart(float second = 0)
        {
            if (mScrollRect.horizontal)
            {
                MoveX(0, second);
            }
            if (mScrollRect.vertical)
            {
                MoveY(0, second);
            }
        }

        /// <summary>
        /// Move to middle of scrollview.
        /// </summary>
        public void MoveToMiddle(float second = 0)
        {
            if (mScrollRect.horizontal)
            {
                MoveX(0.5f, second);
            }
            if (mScrollRect.vertical)
            {
                MoveY(0.5f, second);
            }
        }

        /// <summary>
        /// Move to end of scrollview.
        /// </summary>
        public void MoveToEnd(float second = 0)
        {
            if (mScrollRect.horizontal)
            {
                MoveX(1, second);
            }
            if (mScrollRect.vertical)
            {
                MoveY(1, second);
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Move x-axis.
        /// </summary>
        private IEnumerator _MoveX(float targetPosition, float second)
        {
            float startPosition = mScrollRect.horizontalNormalizedPosition;
            float velocity = 1 / second;
            float position = 0;

            while (position < 1)
            {
                position += velocity * Time.deltaTime;
                mScrollRect.horizontalNormalizedPosition = Mathf.Lerp(startPosition, targetPosition, position);
                
                yield return 0;
            }
        }

        /// <summary>
        /// Move y-axis.
        /// </summary>
        private IEnumerator _MoveY(float targetPosition, float second)
        {
            float startPosition = mScrollRect.verticalNormalizedPosition;
            float velocity = 1 / second;
            float position = 0;

            while (position < 1)
            {
                position += velocity * Time.deltaTime;
                mScrollRect.verticalNormalizedPosition = Mathf.Lerp(startPosition, targetPosition, position);
                
                yield return 0;
            }
        }

        #endregion
    }
}
