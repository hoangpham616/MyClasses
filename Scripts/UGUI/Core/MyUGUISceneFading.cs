/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUISceneFading (version 2.4)
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MyClasses.UI
{
    public class MyUGUISceneFading
    {
        #region ----- Variable -----

        public const string PREFAB_NAME = "SceneFading";

        private GameObject mRoot;
        private Image mImage;
        private EState mState;
        private float mBeginTime;
        private float mEndTime;
        private float mDuration;

        #endregion

        #region ----- Property -----

        public GameObject Root
        {
            get { return mRoot; }
            set { mRoot = value; }
        }

        public Transform Transform
        {
            get { return mRoot != null ? mRoot.transform : null; }
        }

        public bool IsFading
        {
            get { return mState != EState.Idle; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUISceneFading()
        {
#if UNITY_EDITOR
            if (!_CheckPrefab())
            {
                _CreatePrefab();
            }
#endif
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Turn on fading.
        /// </summary>
        public void TurnOnFading()
        {
            string key = typeof(MyUGUISceneFading).Name + "_HandleFading";
            MyCoroutiner.StopCoroutine(key);
            MyCoroutiner.StartCoroutine(key, _DoFading());
        }

        /// <summary>
        /// Fade in.
        /// </summary>
        /// <param name="duration">duration specified in seconds</param>
        public void FadeIn(float duration)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUISceneFading).Name + "] <color=#0000FFFF>FadeIn()</color>: duration=" + duration);
#endif

            if (duration > 0)
            {
                mBeginTime = Time.time;
                mEndTime = mBeginTime + duration;
                mDuration = duration;
                mState = EState.FadeIn;
            }
            else
            {
                mImage.enabled = false;
                mState = EState.Idle;
            }
        }

        /// <summary>
        /// Fade out.
        /// </summary>
        /// <param name="duration">duration specified in seconds</param>
        public void FadeOut(float duration)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUISceneFading).Name + "] <color=#0000FFFF>FadeOut()</color>: duration=" + duration);
#endif

            if (duration > 0)
            {
                mBeginTime = Time.time;
                mEndTime = mBeginTime + duration;
                mDuration = duration;
                mState = EState.FadeOut;
            }
            else
            {
                mImage.enabled = false;
                mState = EState.Idle;
            }
        }

        /// <summary>
        /// Create a template game object.
        /// </summary>
        public static GameObject CreateTemplate()
        {
            GameObject obj = new GameObject(PREFAB_NAME);

            obj = new GameObject("SceneFading");
            obj.AddComponent<RectTransform>();

            Image image = obj.AddComponent<Image>();
            image.color = Color.black;
            image.raycastTarget = true;
            image.enabled = false;

            RectTransform rect = obj.GetComponent<RectTransform>();
            MyUtilities.Anchor(ref rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

            return obj;
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Handle fading.
        /// </summary>
        private IEnumerator _DoFading()
        {
            if (mImage == null)
            {
                mImage = mRoot.GetComponent<Image>();
            }
            mImage.raycastTarget = true;
            mImage.enabled = false;

            Color color = mImage.color;

            while (true)
            {
                switch (mState)
                {
                    case EState.Idle:
                        {
                        }
                        break;
                    case EState.FadeIn:
                        {
                            color.a = (mEndTime - Time.time) / mDuration;
                            mImage.color = color;
                            mImage.enabled = true;

                            if (color.a <= 0)
                            {
                                mImage.enabled = false;
                                mState = EState.Idle;
                            }
                        }
                        break;
                    case EState.FadeOut:
                        {
                            color.a = (Time.time - mBeginTime) / mDuration;
                            mImage.color = color;
                            mImage.enabled = true;

                            if (color.a >= 1)
                            {
                                mState = EState.Idle;
                            }
                        }
                        break;
                }

                yield return null;
            }
        }

#if UNITY_EDITOR

        /// <summary>
        /// Check existence of prefab.
        /// </summary>
        private static bool _CheckPrefab()
        {
            string filePath = "Assets/Resources/" + MyUGUIManager.SPECIAL_DIRECTORY + PREFAB_NAME + ".prefab";
            return System.IO.File.Exists(filePath);
        }

        /// <summary>
        /// Create template prefab.
        /// </summary>
        private static void _CreatePrefab()
        {
            Debug.Log("[" + typeof(MyUGUISceneFading).Name + "] _CreatePrefab(): a template prefab was created.");

            GameObject prefab = CreateTemplate();

            string folderPath = "Assets/Resources/" + MyUGUIManager.SPECIAL_DIRECTORY;
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            string filePath = "Assets/Resources/" + MyUGUIManager.SPECIAL_DIRECTORY + PREFAB_NAME;
            UnityEditor.PrefabUtility.CreatePrefab(filePath + ".prefab", prefab, UnityEditor.ReplacePrefabOptions.ReplaceNameBased);
        }

#endif

        #endregion

        #region ----- Enumeration -----

        private enum EState
        {
            Idle,
            FadeIn,
            FadeOut
        }

        #endregion
    }
}