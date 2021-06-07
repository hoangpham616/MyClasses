/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUILoadingIndicator (version 2.9)
*/

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0649

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyClasses.UI
{
    public class MyUGUILoadingIndicator
    {
        #region ----- Variable -----

        public const string PREFAB_NAME = "LoadingIndicator";

        private GameObject mGameObject;
        private Text mTips;
        private Text mDescription;
        private MyUGUIButton mButtonCancel;

        private ELoadingType mLoadingType;
        private List<int> mListSimpleID = new List<int>();
        private int mCountSimple;
        private float mStartingTime;
        private Action mActionCancel;

        #endregion

        #region ----- Property -----

        public bool IsActive
        {
            get { return mGameObject != null && mGameObject.activeSelf; }
        }

        public GameObject GameObject
        {
            get { return mGameObject; }
        }

        public Transform Transform
        {
            get { return mGameObject.transform; }
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

        #region ----- Button Event -----

        /// <summary>
        /// Click on button cancel.
        /// </summary>
        private void _OnClickCancel(PointerEventData arg0)
        {
            Hide();

            if (mActionCancel != null)
            {
                mActionCancel();
                mActionCancel = null;
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Initialize.
        /// </summary>
        public void Initialize(GameObject gameObject)
        {
            mGameObject = gameObject;

            if (mGameObject != null)
            {
                GameObject tipsRoot = MyUtilities.FindObjectInFirstLayer(mGameObject, ELoadingType.Tips.ToString());
                if (tipsRoot != null)
                {
                    GameObject description = MyUtilities.FindObject(tipsRoot, "Description");
                    if (description != null)
                    {
                        mDescription = description.GetComponent<Text>();
                    }

                    GameObject tips = MyUtilities.FindObject(tipsRoot, "Tips");
                    if (tips != null)
                    {
                        mTips = tips.GetComponent<Text>();
                    }

                    GameObject cancel = MyUtilities.FindObject(tipsRoot, "ButtonCancel");
                    if (cancel != null)
                    {
                        mButtonCancel = cancel.GetComponent<MyUGUIButton>();
                        mButtonCancel.OnEventPointerClick.RemoveAllListeners();
                        mButtonCancel.OnEventPointerClick.AddListener(_OnClickCancel);
                    }
                }
            }
        }

        /// <summary>
        /// Show tips loading indicator.
        /// </summary>
        /// <param name="isThreeDots">show three dots effect for description</param>
        /// <param name="timeOut">-1: forever loading</param>
        public void ShowTips(string tips, string description, bool isThreeDots, float timeOut = -1, Action timeOutCallback = null, Action cancelCallback = null)
        {
            if (mLoadingType != ELoadingType.Tips)
            {
                mListSimpleID.Clear();
            }
            mLoadingType = ELoadingType.Tips;

            if (mGameObject != null)
            {
                if (mTips != null)
                {
                    mTips.text = tips;
                }

                if (mDescription != null)
                {
                    mDescription.text = description;
                }

                _Show();

                if (isThreeDots && mDescription != null)
                {
                    string[] descriptions = new string[] { description + ".", description + "..", description + "..." };
                    MyCoroutiner.Start(_DoChangeDescription(descriptions, 0.2f));
                }

                if (timeOut > 0)
                {
                    string coroutineKey = typeof(MyUGUILoadingIndicator).Name + "_HideTips";
                    MyCoroutiner.Start(coroutineKey, _DoHideTips(timeOut, timeOutCallback));
                }
            }
        }

        /// <summary>
        /// Show simple loading indicator and return loading id.
        /// </summary>
        /// <param name="timeOut">-1: forever loading</param>
        public int ShowSimple(float timeOut = -1, Action timeOutCallback = null)
        {
            if (mLoadingType != ELoadingType.Simple)
            {
                mListSimpleID.Clear();
            }
            mLoadingType = ELoadingType.Simple;

            if (mGameObject != null)
            {
                int loadingID = ++mCountSimple;
                mListSimpleID.Add(loadingID);

                _Show();

                if (timeOut > 0)
                {
                    string coroutineKey = typeof(MyUGUILoadingIndicator).Name + "_HideSimple" + loadingID;
                    MyCoroutiner.Start(coroutineKey, _DoHideSimple(loadingID, timeOut, timeOutCallback));
                }

                return loadingID;
            }

            return -1;
        }

        /// <summary>
        /// Hide simple loading indicator by loading id.
        /// </summary>
        /// <param name="minLiveTime">minimum seconds have to show before hiding</param>
        public void HideSimple(int loadingID, float minLiveTime = 0)
        {
            if (mGameObject != null)
            {
                if (!mListSimpleID.Contains(loadingID))
                {
                    return;
                }

                string coroutineKey = typeof(MyUGUILoadingIndicator).Name + "_HideSimple" + loadingID;
                MyCoroutiner.Stop(coroutineKey);

                if (minLiveTime > 0)
                {
                    float displayedTime = Time.time - mStartingTime;
                    if (displayedTime < minLiveTime)
                    {
                        float delayTime = minLiveTime - displayedTime;
                        MyCoroutiner.Start(coroutineKey, _DoHideSimple(loadingID, delayTime));
                        return;
                    }
                }

                mListSimpleID.Remove(loadingID);
                mListSimpleID.ToString();
                if (mListSimpleID.Count == 0)
                {
                    _Hide();
                }
            }
        }

        /// <summary>
        /// Hide loading indicator.
        /// </summary>
        /// <param name="minLiveTime">minimum seconds have to show before hiding</param>
        public void Hide(float minLiveTime = 0)
        {
            if (mGameObject != null)
            {
                if (minLiveTime > 0)
                {
                    float displayedTime = Time.time - mStartingTime;
                    if (displayedTime < minLiveTime)
                    {
                        float delayTime = minLiveTime - displayedTime;
                        string coroutineKey = typeof(MyUGUILoadingIndicator).Name + "_Hide";
                        MyCoroutiner.Stop(coroutineKey);
                        MyCoroutiner.Start(coroutineKey, _DoHide(delayTime));
                        return;
                    }
                }

                _Hide();
            }
        }

        /// <summary>
        /// Create a template game object.
        /// </summary>
        public static GameObject CreateTemplate()
        {
            GameObject obj = new GameObject(PREFAB_NAME);

            RectTransform obj_rect = obj.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref obj_rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

            int countWaitingPopupID = Enum.GetNames(typeof(ELoadingType)).Length;
            for (int i = 0; i < countWaitingPopupID; i++)
            {
                ELoadingType type = (ELoadingType)Enum.GetValues(typeof(ELoadingType)).GetValue(i);
                if (type == ELoadingType.None)
                {
                    continue;
                }

                GameObject child = new GameObject(type.ToString());
                child.transform.SetParent(obj.transform, false);
                child.SetActive(false);

                RectTransform child_rect = child.AddComponent<RectTransform>();
                MyUtilities.Anchor(ref child_rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

                if (type == ELoadingType.Simple)
                {
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
                        if (System.IO.File.Exists(paths[j] + "/Sources/Animations/my_animator_loading_indicator_circle.controller"))
                        {
                            Animator root_animator = child.AddComponent<Animator>();
                            root_animator.runtimeAnimatorController = (RuntimeAnimatorController)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[j] + "/Sources/Animations/my_animator_loading_indicator_circle.controller", typeof(RuntimeAnimatorController));
                            if (System.IO.File.Exists(paths[j] + "/Sources/Images/my_loading_indicator_circle_bg.png"))
                            {
                                background_image.sprite = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[j] + "/Sources/Images/my_loading_indicator_circle_bg.png", typeof(Sprite));
                            }
                            if (System.IO.File.Exists(paths[j] + "/Sources/Images/my_loading_indicator_circle.png"))
                            {
                                image_image.sprite = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[j] + "/Sources/Images/my_loading_indicator_circle.png", typeof(Sprite));
                            }
                            Debug.LogError("[" + typeof(MyUGUILoadingIndicator).Name + "] CreateTemplate(): please setup \"my_animator_loading_indicator_circle\" controller.");
                            Debug.LogError("[" + typeof(MyUGUILoadingIndicator).Name + "] CreateTemplate(): mapping \"my_animation_loading_indicator_circle\" motion for \"Circle\" state.");
                            break;
                        }
                    }
#endif
                }
                else if (type == ELoadingType.Tips)
                {
                    GameObject loading = new GameObject("Loading");
                    loading.transform.SetParent(child.transform, false);

                    RectTransform loadingd_rect = loading.AddComponent<RectTransform>();
                    MyUtilities.Anchor(ref loadingd_rect, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 200, 200, 0, 150);

                    GameObject imageBG = new GameObject("ImageBackground");
                    imageBG.transform.SetParent(loading.transform, false);

                    RectTransform background_rect = imageBG.AddComponent<RectTransform>();
                    MyUtilities.Anchor(ref background_rect, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 200, 200, 0, 0);

                    Image background_image = imageBG.AddComponent<Image>();
                    background_image.raycastTarget = false;
                    background_image.color = Color.black;

                    GameObject image = new GameObject("Image");
                    image.transform.SetParent(loading.transform, false);

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
                            Animator root_animator = loading.AddComponent<Animator>();
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

                    GameObject description = new GameObject("Description");
                    description.transform.SetParent(child.transform, false);

                    RectTransform description_rect = description.AddComponent<RectTransform>();
                    MyUtilities.Anchor(ref description_rect, MyUtilities.EAnchorPreset.HorizontalStretchMiddle, MyUtilities.EAnchorPivot.MiddleCenter, new Vector2(50, -50), new Vector2(-50, 50));

                    Text description_text = description.AddComponent<Text>();
                    description_text.text = "description";
                    description_text.color = Color.white;
                    description_text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                    description_text.fontSize = 40;
                    description_text.alignment = TextAnchor.MiddleCenter;
                    description_text.horizontalOverflow = HorizontalWrapMode.Wrap;
                    description_text.verticalOverflow = VerticalWrapMode.Overflow;
                    description_text.raycastTarget = false;

                    GameObject cancel = new GameObject("ButtonCancel");
                    cancel.transform.SetParent(child.transform, false);

                    RectTransform cancel_rect = cancel.AddComponent<RectTransform>();
                    MyUtilities.Anchor(ref cancel_rect, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 240, 80, 0, -120);

                    cancel.AddComponent<Image>();
                    cancel.AddComponent<MyUGUIButton>();

                    GameObject cancel_text = new GameObject("Text");
                    cancel_text.transform.SetParent(cancel.transform, false);

                    RectTransform cancel_text_rect = cancel_text.AddComponent<RectTransform>();
                    MyUtilities.Anchor(ref cancel_text_rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

                    Text cancel_text_text = cancel_text.AddComponent<Text>();
                    cancel_text_text.text = "Cancel";
                    cancel_text_text.color = Color.black;
                    cancel_text_text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                    cancel_text_text.fontSize = 40;
                    cancel_text_text.supportRichText = false;
                    cancel_text_text.alignment = TextAnchor.MiddleCenter;
                    cancel_text_text.horizontalOverflow = HorizontalWrapMode.Wrap;
                    cancel_text_text.verticalOverflow = VerticalWrapMode.Overflow;
                    cancel_text_text.raycastTarget = false;

                    GameObject tips = new GameObject("Tips");
                    tips.transform.SetParent(child.transform, false);

                    RectTransform tips_rect = tips.AddComponent<RectTransform>();
                    MyUtilities.Anchor(ref tips_rect, MyUtilities.EAnchorPreset.HorizontalStretchBottom, MyUtilities.EAnchorPivot.BottomCenter, new Vector2(50, 0), new Vector2(-50, 100));

                    Text tips_text = tips.AddComponent<Text>();
                    tips_text.text = "tips";
                    tips_text.color = Color.white;
                    tips_text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                    tips_text.fontSize = 40;
                    tips_text.alignment = TextAnchor.MiddleCenter;
                    tips_text.horizontalOverflow = HorizontalWrapMode.Wrap;
                    tips_text.verticalOverflow = VerticalWrapMode.Overflow;
                    tips_text.raycastTarget = false;
                }
            }

            return obj;
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Show.
        /// </summary>
        private void _Show()
        {
            if (mStartingTime < 0)
            {
                mStartingTime = Time.time;
            }

            GameObject child = null;
            int countChild = mGameObject.transform.childCount;
            for (int i = 0; i < countChild; i++)
            {
                child = mGameObject.transform.GetChild(i).gameObject;
                child.SetActive(child.name.Equals(mLoadingType.ToString()));
            }

            mGameObject.transform.SetAsLastSibling();
            mGameObject.SetActive(true);

            MyUGUIManager.Instance.UpdatePopupOverlay();
        }

        /// <summary>
        /// Hide.
        /// </summary>
        private void _Hide()
        {
            if (mGameObject != null)
            {
                mStartingTime = -1;
                mListSimpleID.Clear();
                mGameObject.SetActive(false);

                MyUGUIManager.Instance.UpdatePopupOverlay();
            }
        }

        /// <summary>
        /// Handle hiding.
        /// </summary>
        private IEnumerator _DoHide(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            _Hide();
        }

        /// <summary>
        /// Handle hiding tips loading indicator.
        /// </summary>
        private IEnumerator _DoHideTips(float delayTime, Action callback = null)
        {
            yield return new WaitForSeconds(delayTime);

            if (mLoadingType == ELoadingType.Tips)
            {
                _Hide();

                if (callback != null && callback.Target != null)
                {
                    callback();
                }
            }
        }

        /// <summary>
        /// Handle hiding simple loading indicator by loading id.
        /// </summary>
        private IEnumerator _DoHideSimple(int loadingID, float delayTime, Action callback = null)
        {
            yield return new WaitForSeconds(delayTime);

            if (mLoadingType == ELoadingType.Simple && mListSimpleID.Contains(loadingID))
            {
                mListSimpleID.Remove(loadingID);
                if (mListSimpleID.Count == 0)
                {
                    _Hide();
                }

                if (callback != null && callback.Target != null)
                {
                    callback();
                }
            }
        }

        /// <summary>
        /// Handle changing description.
        /// </summary>
        private IEnumerator _DoChangeDescription(string[] descriptions, float delayTime)
        {
            int index = 0;
            while (mLoadingType == ELoadingType.Tips && mDescription != null && descriptions != null)
            {
                mDescription.text = descriptions[index];
                index = (index + 1) % descriptions.Length;
                yield return new WaitForSeconds(delayTime);
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

        #region ----- Enumeration -----

        private enum ELoadingType
        {
            None,
            Simple,
            Tips
        }

        #endregion
    }
}