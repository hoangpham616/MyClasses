/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIRunningText (version 2.8)
 */

using UnityEngine;
using UnityEngine.UI;

namespace MyClasses.UI
{
    public class MyUGUIRunningText
    {
        #region ----- Variable -----

        public const string PREFAB_NAME = "RunningText";

        private Text mText;

        private GameObject mGameObject;
        private GameObject mContainer;
        private RectTransform mMask;
        private Vector3 mCurPos;
        private EState mState;
        private Color mTextOriginalColor;
        private float mSpeed;
        private float mMinSpeed;
        private float mMaxSpeed;
        private float mEndX;

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

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIRunningText()
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
        /// Show running text.
        /// </summary>
        public void Show(string content, float minSpeed, float maxSpeed)
        {
            if (mGameObject != null)
            {
                _Init();

                mText.text = content;
                mMinSpeed = minSpeed;
                mMaxSpeed = maxSpeed;

                mState = EState.Show;
            }
        }

        /// <summary>
        /// Hide running text.
        /// </summary>
        public void Hide()
        {
            if (mGameObject != null)
            {
                _Init();

                mState = EState.Hide;
            }
        }

        /// <summary>
        /// Update state machine.
        /// </summary>
        public void LateUpdate(float dt)
        {
            if (mText == null)
            {
                return;
            }

            switch (mState)
            {
                case EState.Show:
                    {
                        if (!mGameObject.activeSelf)
                        {
                            Color colorAlpha0 = mText.color;
                            colorAlpha0.a = 0;
                            mText.color = colorAlpha0;

                            mGameObject.SetActive(true);
                        }
                        else
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
                    }
                    break;
                case EState.Update:
                    {
                        mCurPos.x -= mSpeed * dt;
                        mText.rectTransform.localPosition = mCurPos;

                        if (mCurPos.x < mEndX)
                        {
                            mState = EState.Hide;
                        }
                    }
                    break;
                case EState.Hide:
                    {
                        mCurPos.x = (mMask.rect.width / 2) + mText.rectTransform.rect.width;
                        mText.rectTransform.localPosition = mCurPos;
                        mGameObject.SetActive(false);

                        mState = EState.Idle;
                    }
                    break;
            }
        }

        /// <summary>
        /// Create a template game object.
        /// </summary>
        public static GameObject CreateTemplate()
        {
            GameObject obj = new GameObject(PREFAB_NAME);

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
            text_text.color = Color.white;
            text_text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text_text.fontSize = 40;
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
            Debug.Log("[" + typeof(MyUGUIToast).Name + "] CreatePrefab(): a template prefab was created.");

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
            Show,
            Update,
            Hide
        }

        #endregion
    }
}