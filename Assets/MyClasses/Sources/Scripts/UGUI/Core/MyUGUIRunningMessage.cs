/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIRunningMessage (version 2.22)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0649

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MyClasses.UI
{
    public class MyUGUIRunningMessage
    {
        #region ----- Variable -----

        public const string PREFAB_NAME = "RunningMessage";

        private Text mText;

        private Animator mAnimator;
        private GameObject mGameObject;
        private GameObject mContainer;
        private RectTransform mMask;
        private Vector3 mCurPos;
        private EState mState;
        private EType mType;
        private Color mTextOriginalColor;
        private float mSpeed;
        private float mMinSpeed;
        private float mMaxSpeed;
        private float mEndX;

        private List<string> mContents = new List<string>();
        private int mMaxContentQueue = 3;

        #endregion

        #region ----- Property -----

        public GameObject GameObject
        {
            get { return mGameObject; }
            set { mGameObject = value; }
        }

        public Transform Transform
        {
            get { return mGameObject != null ? mGameObject.transform : null; }
        }

        public bool IsShow
        {
            get { return mGameObject != null && mGameObject.activeSelf; }
        }

        public EType Type
        {
            get { return mType; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIRunningMessage(EType type)
        {
            mType = type;
#if UNITY_EDITOR
            if (!_CheckPrefab(mType))
            {
                _CreatePrefab(mType);
            }
#endif
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set max content queue.
        /// </summary>
        public void SetMaxQueue(int maxQueue)
        {
            if (maxQueue < 0)
            {
                maxQueue = 999;
            }
            mMaxContentQueue = maxQueue;
        }

        /// <summary>
        /// Show.
        /// </summary>
        public void Show(string content, float minSpeed, float maxSpeed)
        {
            if (mGameObject != null)
            {
                mContents.Add(content);
                if (mContents.Count > mMaxContentQueue)
                {
                    mContents.RemoveAt(0);
                }

                mMinSpeed = minSpeed;
                mMaxSpeed = maxSpeed;

                if (!mGameObject.activeSelf)
                {
                    _Init();

                    mState = EState.PreShow;
                }
            }
        }

        /// <summary>
        /// Hide.
        /// </summary>
        public void Hide()
        {
            if (mGameObject != null)
            {
                _Init();

                mContents.Clear();

                mState = EState.Hide;
            }
        }

        /// <summary>
        /// Hide immedialy.
        /// </summary>
        public void HideImmedialy()
        {
            if (mGameObject != null)
            {
                _Init();

                mContents.Clear();
                mAnimator = null;

                mState = EState.Hide;

                Update(Time.deltaTime);
            }
        }

        /// <summary>
        /// Update state machine.
        /// </summary>
        public void Update(float dt)
        {
            if (mText == null)
            {
                return;
            }

            switch (mState)
            {
                case EState.PreShow:
                    {
                        if (mContents.Count < 0)
                        {
                            return;
                        }

                        Color colorAlpha0 = mText.color;
                        colorAlpha0.a = 0;
                        mText.color = colorAlpha0;
                        mText.text = mContents[0];
                        mContents.RemoveAt(0);

                        mGameObject.SetActive(true);
                        mAnimator = mGameObject.GetComponent<Animator>();
                        if (mAnimator != null)
                        {
                            mAnimator.Play("Show");
                        }

                        mState = EState.Show;
                    }
                    break;
                case EState.Show:
                    {
                        mSpeed = mMinSpeed * (mText.rectTransform.rect.width / mMask.rect.width);
                        mSpeed = Mathf.Clamp(mSpeed, mMinSpeed, mMaxSpeed);

                        float halfWidth = mText.rectTransform.rect.width / 2;
                        float beginX = (mMask.rect.width / 2) + halfWidth;
                        mEndX = mMask.rect.x - halfWidth;

                        mCurPos = Vector3.zero;
                        mCurPos.x = beginX + (mSpeed / 2);
                        mText.color = mTextOriginalColor;
                        mText.rectTransform.localPosition = mCurPos;

                        mState = EState.Update;
                    }
                    break;
                case EState.Update:
                    {
                        mCurPos.x -= mSpeed * dt;
                        mText.rectTransform.localPosition = mCurPos;

                        if (mCurPos.x < mEndX)
                        {
                            if (mContents.Count > 0)
                            {
                                mText.text = mContents[0];
                                mContents.RemoveAt(0);

                                mState = EState.Show;
                            }
                            else
                            {
                                mAnimator = mGameObject.GetComponent<Animator>();

                                mState = EState.Hide;
                            }
                        }
                    }
                    break;
                case EState.Hide:
                    {
                        mCurPos.x = (mMask.rect.width / 2) + mText.rectTransform.rect.width;
                        mText.rectTransform.localPosition = mCurPos;

                        if (mAnimator != null)
                        {
                            mAnimator.Play("Hide");
                            mState = EState.Hiding;
                        }
                        else
                        {
                            mGameObject.SetActive(false);
                            mState = EState.Idle;
                        }

                    }
                    break;
                case EState.Hiding:
                    {
                        if (mAnimator == null || mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                        {
                            mGameObject.SetActive(false);
                            mState = EState.Idle;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Return name of game object by type.
        /// </summary>
        public static string GetGameObjectName(EType type)
        {
            return PREFAB_NAME + (type == EType.Default ? string.Empty : "_" + type.ToString());
        }

        /// <summary>
        /// Create a template game object.
        /// </summary>
        public static GameObject CreateTemplate(EType type)
        {
            GameObject obj = new GameObject(GetGameObjectName(type));

            obj.transform.SetParent(MyUGUIManager.Instance.CanvasOnTop.transform, false);
            obj.layer = LayerMask.NameToLayer("UI");
            obj.AddComponent<CanvasRenderer>();
            obj.AddComponent<Canvas>();

            RectTransform root_rect = obj.GetComponent<RectTransform>();
            MyUtilities.Anchor(ref root_rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

            GameObject container = new GameObject("Container");
            container.transform.SetParent(obj.transform, false);
            container.layer = LayerMask.NameToLayer("UI");

            Image container_image = container.AddComponent<Image>();
            container_image.color = new Color(0, 0, 0, 0.5f);
            container_image.raycastTarget = false;

            RectTransform container_rect = container.GetComponent<RectTransform>();
            MyUtilities.Anchor(ref container_rect, MyUtilities.EAnchorPreset.HorizontalStretchTop, MyUtilities.EAnchorPivot.TopCenter, new Vector2(200, -160), new Vector2(-200, -80));

            GameObject mask = new GameObject("Mask");
            mask.transform.SetParent(container.transform, false);
            mask.layer = LayerMask.NameToLayer("UI");
            mask.AddComponent<CanvasRenderer>();
            mask.AddComponent<RectMask2D>();

            RectTransform mask_rect = mask.GetComponent<RectTransform>();
            MyUtilities.Anchor(ref mask_rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, new Vector2(100, 0), new Vector2(-100, 0));

            GameObject text = new GameObject("Text");
            text.transform.SetParent(mask.transform, false);
            text.layer = LayerMask.NameToLayer("UI");

            Text text_text = text.AddComponent<Text>();
            text_text.text = "This is running message";
            text_text.color = Color.white;
            text_text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text_text.fontSize = 50;
            text_text.alignment = TextAnchor.MiddleLeft;
            text_text.verticalOverflow = VerticalWrapMode.Overflow;
            text_text.raycastTarget = false;

            RectTransform text_rect = text.GetComponent<RectTransform>();
            MyUtilities.Anchor(ref text_rect, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 0, 0);

            ContentSizeFitter text_csf = text.AddComponent<ContentSizeFitter>();
            text_csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

            return obj;
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Init.
        /// </summary>
        private void _Init()
        {
            if (mGameObject != null)
            {
                if (mContainer == null || mMask == null || mText == null)
                {
                    mGameObject.SetActive(false);

                    mContainer = MyUtilities.FindObjectInFirstLayer(mGameObject, "Container");
                    mMask = MyUtilities.FindObjectInFirstLayer(mContainer.gameObject, "Mask").GetComponent<RectTransform>();
                    mText = MyUtilities.FindObjectInFirstLayer(mMask.gameObject, "Text").GetComponent<Text>();
                    mTextOriginalColor = mText.color;

                    mState = EState.Idle;
                }
            }
        }

#if UNITY_EDITOR

        /// <summary>
        /// Check existence of prefab.
        /// </summary>
        private static bool _CheckPrefab(EType type)
        {
            string filePath = "Assets/Resources/" + MyUGUIManager.SPECIAL_DIRECTORY + GetGameObjectName(type) + ".prefab";
            return System.IO.File.Exists(filePath);
        }

        /// <summary>
        /// Create template prefab.
        /// </summary>
        private static void _CreatePrefab(EType type)
        {
            Debug.Log("[" + typeof(MyUGUIToastMessage).Name + "] CreatePrefab(): a template prefab was created.");

            GameObject prefab = CreateTemplate(type);

            string folderPath = "Assets/Resources/" + MyUGUIManager.SPECIAL_DIRECTORY;
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            string filePath = "Assets/Resources/" + MyUGUIManager.SPECIAL_DIRECTORY + GetGameObjectName(type);
            UnityEditor.PrefabUtility.CreatePrefab(filePath + ".prefab", prefab, UnityEditor.ReplacePrefabOptions.ReplaceNameBased);
        }

#endif

        #endregion

        #region ----- Enumeration -----

        private enum EState
        {
            Idle,
            PreShow,
            Show,
            Update,
            Hide,
            Hiding,
        }

        public enum EType
        {
            Default,
            Custom,
            Custom2,
            Custom3
        }

        #endregion
    }
}