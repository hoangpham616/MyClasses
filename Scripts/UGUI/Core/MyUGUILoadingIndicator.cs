/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUILoadingIndicator (version 2.1)
*/

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyClasses.UI
{
    public class MyUGUILoadingIndicator
    {
        #region ----- Variable -----

        public const string PREFAB_NAME = "LoadingIndicator";

        private GameObject mRoot;

        private List<int> mListID = new List<int>();
        private int mCount;
        private float mStartingTime;

        #endregion

        #region ----- Property -----

        public bool IsActive
        {
            get { return mRoot != null && mRoot.activeSelf; }
        }

        public GameObject Root
        {
            get { return mRoot; }
            set { mRoot = value; }
        }

        public Transform Transform
        {
            get { return mRoot.transform; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUILoadingIndicator()
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
        /// Show loading indicator and return loading id.
        /// </summary>
        /// <param name="timeOut">-1: forever loading</param>
        public int Show(ELoadingIndicatorID loadingIndicatorID = ELoadingIndicatorID.Circle, float timeOut = -1, Action timeOutCallback = null)
        {
            if (mRoot != null)
            {
                int loadingID = ++mCount;

                _Show(loadingID, loadingIndicatorID);

                if (timeOut > 0)
                {
                    string coroutineKey = typeof(MyUGUILoadingIndicator).Name + "_Hide" + loadingID;
                    MyCoroutiner.StartCoroutine(coroutineKey, _ProcessHide(loadingID, timeOut, timeOutCallback));
                }

                return loadingID;
            }

            return -1;
        }

        /// <summary>
        /// Hide loading indicator.
        /// </summary>
        /// <param name="minLiveTime">minimum seconds have to show before hiding</param>
        public void Hide(float minLiveTime = 0)
        {
            if (mRoot != null)
            {
                if (minLiveTime > 0)
                {
                    float displayedTime = mStartingTime - Time.time;
                    if (displayedTime < minLiveTime)
                    {
                        float delayTime = minLiveTime - displayedTime;
                        string coroutineKey = typeof(MyUGUILoadingIndicator).Name + "_Hide";
                        MyCoroutiner.StopCoroutine(coroutineKey);
                        MyCoroutiner.StartCoroutine(coroutineKey, _ProcessHide(delayTime));
                        return;
                    }
                }

                _Hide();
            }
        }

        /// <summary>
        /// Hide loading indicator by loading id.
        /// </summary>
        /// <param name="minLiveTime">minimum seconds have to show before hiding</param>
        public void Hide(int loadingID, float minLiveTime = 0)
        {
            if (mRoot != null)
            {
                if (!mListID.Contains(loadingID))
                {
                    return;
                }

                string coroutineKey = typeof(MyUGUILoadingIndicator).Name + "_Hide" + loadingID;
                MyCoroutiner.StopCoroutine(coroutineKey);

                if (minLiveTime > 0)
                {
                    float displayedTime = Time.time - mStartingTime;
                    if (displayedTime < minLiveTime)
                    {
                        float delayTime = minLiveTime - displayedTime;
                        MyCoroutiner.StartCoroutine(coroutineKey, _ProcessHide(loadingID, delayTime));
                        return;
                    }
                }

                mListID.Remove(loadingID);
                mListID.ToString();
                if (mListID.Count == 0)
                {
                    _Hide();
                }
            }
        }

        /// <summary>
        /// Create a template game object.
        /// </summary>
        public static GameObject CreateTemplate()
        {
            GameObject obj = new GameObject(PREFAB_NAME);

            int countWaitingPopupID = Enum.GetNames(typeof(ELoadingIndicatorID)).Length;
            for (int i = 0; i < countWaitingPopupID; i++)
            {
                GameObject child = new GameObject(Enum.GetValues(typeof(ELoadingIndicatorID)).GetValue(i).ToString());
                child.transform.SetParent(obj.transform, false);
                child.SetActive(false);

                if (child.name == ELoadingIndicatorID.Circle.ToString())
                {
                    Animator root_animator = child.AddComponent<Animator>();

                    GameObject imageBG = new GameObject("ImageBackground");
                    imageBG.transform.SetParent(child.transform, false);

                    RectTransform background_rect = imageBG.AddComponent<RectTransform>();
                    MyUtilities.Anchor(ref background_rect, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 200, 200, 0, 0);

                    Image background_image = imageBG.AddComponent<Image>();
                    background_image.raycastTarget = false;
                    background_image.color = Color.black;

                    GameObject image = new GameObject("Image");
                    image.transform.SetParent(child.transform, false);

                    RectTransform image_rect = image.AddComponent<RectTransform>();
                    MyUtilities.Anchor(ref image_rect, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 200, 200, 0, 0);

                    Image image_image = image.AddComponent<Image>();
                    image_image.raycastTarget = false;
                    image_image.color = Color.white;

#if UNITY_EDITOR
                    string[] paths = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
                    for (int j = 0; j < paths.Length; j++)
                    {
                        if (System.IO.File.Exists(paths[j] + "/Animations/my_animator_loading_indicator_circle.controller"))
                        {
                            root_animator.runtimeAnimatorController = (RuntimeAnimatorController)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[j] + "/Animations/my_animator_loading_indicator_circle.controller", typeof(RuntimeAnimatorController));
                            if (System.IO.File.Exists(paths[j] + "/Images/my_loading_indicator_circle_bg.png"))
                            {
                                background_image.sprite = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[j] + "/Images/my_loading_indicator_circle_bg.png", typeof(Sprite));
                            }
                            if (System.IO.File.Exists(paths[j] + "/Images/my_loading_indicator_circle.png"))
                            {
                                image_image.sprite = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[j] + "/Images/my_loading_indicator_circle.png", typeof(Sprite));
                            }
                            break;
                        }
                    }
#endif
                }
            }

            return obj;
        }

#endregion

#region ----- Private Method -----

        /// <summary>
        /// Show.
        /// </summary>
        private void _Show(int id, ELoadingIndicatorID loadingIndicatorID)
        {
            if (mStartingTime < 0)
            {
                mStartingTime = Time.time;
            }

            if (mListID == null)
            {
                mListID = new List<int>();
            }
            mListID.Add(id);

            GameObject child = null;
            int countChild = mRoot.transform.childCount;
            for (int i = 0; i < countChild; i++)
            {
                child = mRoot.transform.GetChild(i).gameObject;
                child.SetActive(child.name.Equals(loadingIndicatorID.ToString()));
            }

            mRoot.transform.SetAsLastSibling();
            mRoot.SetActive(true);

            MyUGUIManager.Instance.UpdatePopupOverlay();
        }

        /// <summary>
        /// Hide.
        /// </summary>
        private void _Hide()
        {
            mStartingTime = -1;

            if (mListID == null)
            {
                mListID = new List<int>();
            }
            mListID.Clear();

            mRoot.SetActive(false);

            MyUGUIManager.Instance.UpdatePopupOverlay();
        }

        /// <summary>
        /// Handle hiding.
        /// </summary>
        private IEnumerator _ProcessHide(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            _Hide();
        }

        /// <summary>
        /// Handle hiding by loading id.
        /// </summary>
        private IEnumerator _ProcessHide(int loadingID, float delayTime, Action callback = null)
        {
            Debug.Log("_ProcessHide " + loadingID + " " + delayTime);
            yield return new WaitForSeconds(delayTime);

            if (mListID.Contains(loadingID))
            {
                mListID.Remove(loadingID);
                mListID.ToString();
                if (mListID.Count == 0)
                {
                    _Hide();
                }

                if (callback != null && callback.Target != null)
                {
                    callback();
                }
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
            Debug.Log("[" + typeof(MyUGUILoadingIndicator).Name + "] CreatePrefab(): a template prefab was created.");

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
    }
}