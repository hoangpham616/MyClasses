/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIButton (version 2.36)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0649

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;

#if USE_MY_UI_TMPRO
using TMPro;
#endif

namespace MyClasses.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Button))]
    public class MyUGUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        #region ----- Variable -----

        private const string DO_PRESS_SCALE_DOWN_ANIMATION = "_DoPressScaleDownAnimation_";
        private const string DO_PRESS_SCALE_UP_ANIMATION = "_DoPressScaleUpAnimation_";
        private const string DO_CLICK_SCALE_ANIMATION = "_DoClickScaleAnimation_";

#if USE_MY_UI_TMPRO
        [SerializeField]
        private TextMeshProUGUI mTextTMPro;
#endif

        [SerializeField]
        private Button mButton;
        [SerializeField]
        private Image mImage;
        [SerializeField]
        private Image mIcon;
        [SerializeField]
        private Text mText;

        [SerializeField]
        private Vector3 mOriginalScale;

        [SerializeField]
        private EAnimationType mPressAnimationType = EAnimationType.Scale;
        [SerializeField]
        private Vector3 mPressAnimationScaleBy = new Vector3(-0.1f, -0.1f, 0);
        [SerializeField]
        private float mPressAnimationDelayTime = 0;
        [SerializeField]
        private float mPressAnimationDownDuration = 0.07f;
        [SerializeField]
        private float mPressAnimationUpDuration = 0.05f;
        [SerializeField]
        private bool mPressAnimationUseExtraTouchZone = true;
        [SerializeField]
        private Transform mPressAnimationExtraTouchZone;

        [SerializeField]
        private EAnimationType mClickAnimationType = EAnimationType.None;
        [SerializeField]
        private Vector3 mClickAnimationScaleBy = new Vector3(-0.1f, -0.1f, 0);
        [SerializeField]
        private float mClickAnimationDelayTime = 0;
        [SerializeField]
        private float mClickAnimationDownDuration = 0.07f;
        [SerializeField]
        private float mClickAnimationUpDuration = 0.05f;
        [SerializeField]
        private ESoundType mClickSoundType = ESoundType.ResourcesPath;
        [SerializeField]
        private string mClickSoundResourcePath = "Sounds/sfx_click";
        [SerializeField]
        private AudioClip mClickSoundAudioClip = null;
        [SerializeField]
        private float mClickSoundDelayTime = 0;
        [SerializeField]
        private float mClickSoundLocalVolume = 1;

        [SerializeField]
        private bool mIsTriggerClickEventAfterAnimationComplete = false;

        private MyPointerEvent mOnEventPointerDown;
        private MyPointerEvent mOnEventPointerPress;
        private MyPointerEvent mOnEventPointerUp;
        private MyPointerEvent mOnEventPointerClick;
        private MyPointerEvent mOnEventPointerDoubleClick;
        private PointerEventData mPointerEventDataPress;
        private PointerEventData mPointerEventDataClick;

        private EEffectType mEffectType = EEffectType.None;
        private EEffectType mDarkType = EEffectType.Dark;
        private EEffectType mGrayType = EEffectType.Gray;

        private bool mIsPressAnimationScaleCompleted = false;
        private float mLastClickTime = 0;
        private long mId = 0;

        #endregion

        #region ----- Property -----

        public Button Button
        {
            get
            {
                if (mButton == null)
                {
                    mButton = gameObject.GetComponent<Button>();
                }
                return mButton;
            }
        }

        public Vector3 OriginalScale
        {
            get { return mOriginalScale; }
            set { mOriginalScale = value; }
        }

        public EAnimationType PressAnimationType
        {
            get { return mPressAnimationType; }
            set { mPressAnimationType = value; }
        }

        public Vector3 PressAnimationScaleBy
        {
            get { return mPressAnimationScaleBy; }
            set { mPressAnimationScaleBy = value; }
        }

        public float PressAnimationDelayTime
        {
            get { return mPressAnimationDelayTime; }
            set { mPressAnimationDelayTime = value; }
        }

        public float PressAnimationDownDuration
        {
            get { return mPressAnimationDownDuration; }
            set { mPressAnimationDownDuration = value; }
        }

        public float PressAnimationUpDuration
        {
            get { return mPressAnimationUpDuration; }
            set { mPressAnimationUpDuration = value; }
        }

        public bool PressAnimationUseExtraTouchZone
        {
            get { return mPressAnimationUseExtraTouchZone; }
            set { mPressAnimationUseExtraTouchZone = value; }
        }

        public EAnimationType ClickAnimationType
        {
            get { return mClickAnimationType; }
            set { mClickAnimationType = value; }
        }

        public Vector3 ClickAnimationScaleBy
        {
            get { return mClickAnimationScaleBy; }
            set { mClickAnimationScaleBy = value; }
        }

        public float ClickAnimationDelayTime
        {
            get { return mClickAnimationDelayTime; }
            set { mClickAnimationDelayTime = value; }
        }

        public float ClickAnimationDownDuration
        {
            get { return mClickAnimationDownDuration; }
            set { mClickAnimationDownDuration = value; }
        }

        public float ClickAnimationUpDuration
        {
            get { return mClickAnimationUpDuration; }
            set { mClickAnimationUpDuration = value; }
        }

        public ESoundType SoundType
        {
            get { return mClickSoundType; }
            set { mClickSoundType = value; }
        }

        public string SoundResourcePath
        {
            get { return mClickSoundResourcePath; }
            set { mClickSoundResourcePath = value; }
        }

        public AudioClip SoundAudioClip
        {
            get { return mClickSoundAudioClip; }
            set { mClickSoundAudioClip = value; }
        }

        public float SoundDelayTime
        {
            get { return mClickSoundDelayTime; }
            set { mClickSoundDelayTime = value; }
        }

        public float SoundLocalVolume
        {
            get { return mClickSoundLocalVolume; }
            set { mClickSoundLocalVolume = value; }
        }

        public bool IsTriggerEventAfterAnimationComplete
        {
            get { return mIsTriggerClickEventAfterAnimationComplete; }
            set { mIsTriggerClickEventAfterAnimationComplete = value; }
        }

        public bool IsEnable
        {
            get
            {
                if (!enabled)
                {
                    return false;
                }

                if (mImage != null)
                {
                    if (mImage.raycastTarget)
                    {
                        return true;
                    }
                }

                if (mIcon != null)
                {
                    if (mIcon.raycastTarget)
                    {
                        return true;
                    }
                }

                if (mText != null)
                {
                    if (mText.raycastTarget)
                    {
                        return true;
                    }
                }
#if USE_MY_UI_TMPRO
                else if (mTextTMPro != null)
                {
                    if (mTextTMPro.raycastTarget)
                    {
                        return true;
                    }
                }
#endif

                return false;
            }
        }

        public bool IsDark
        {
            get { return mEffectType == EEffectType.Dark || mEffectType == EEffectType.DarkImageOnly || mEffectType == EEffectType.DarkTextOnly; }
        }

        public bool IsGray
        {
            get { return mEffectType == EEffectType.Gray || mEffectType == EEffectType.GrayImageOnly || mEffectType == EEffectType.GrayTextOnly; }
        }

        public Image Image
        {
            get
            {
                _InitImage();
                return mImage;
            }
        }

        public Image Icon
        {
            get
            {
                _InitIcon();
                return mIcon;
            }
        }

        public Text Text
        {
            get
            {
                _InitText();
                return mText;
            }
        }

#if USE_MY_UI_TMPRO
        public TextMeshProUGUI TextTMPro
        {
            get
            {
                _InitText();
                return mTextTMPro;
            }
        }
#endif

        public MyPointerEvent OnEventPointerDown
        {
            get
            {
                if (mOnEventPointerDown == null)
                {
                    mOnEventPointerDown = new MyPointerEvent();
                }
                return mOnEventPointerDown;
            }
        }

        public MyPointerEvent OnEventPointerPress
        {
            get
            {
                if (mOnEventPointerPress == null)
                {
                    mOnEventPointerPress = new MyPointerEvent();
                }
                return mOnEventPointerPress;
            }
        }

        public MyPointerEvent OnEventPointerUp
        {
            get
            {
                if (mOnEventPointerUp == null)
                {
                    mOnEventPointerUp = new MyPointerEvent();
                }
                return mOnEventPointerUp;
            }
        }

        public MyPointerEvent OnEventPointerClick
        {
            get
            {
                if (mOnEventPointerClick == null)
                {
                    mOnEventPointerClick = new MyPointerEvent();
                }
                return mOnEventPointerClick;
            }
        }

        public MyPointerEvent OnEventPointerDoubleClick
        {
            get
            {
                if (mOnEventPointerDoubleClick == null)
                {
                    mOnEventPointerDoubleClick = new MyPointerEvent();
                }
                return mOnEventPointerDoubleClick;
            }
        }

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// Awake.
        /// </summary>
        void Awake()
        {
            mOriginalScale = transform.localScale;
        }

        /// <summary>
        /// Update.
        /// </summary>
        void Update()
        {
            if (mOnEventPointerPress != null && mPointerEventDataPress != null)
            {
                mOnEventPointerPress.Invoke(mPointerEventDataPress);
            }
        }

        #endregion

        #region ----- IPointerDownHandler, IPointerUpHandler, IPointerClickHandler Implementation -----

        /// <summary>
        /// OnPointerDown.
        /// </summary>
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            mPointerEventDataPress = eventData;
            mPointerEventDataClick = null;
            mIsPressAnimationScaleCompleted = false;

            if (mOnEventPointerDown != null)
            {
                mOnEventPointerDown.Invoke(eventData);
            }

            if (mPressAnimationType == EAnimationType.Scale)
            {
                mId = MyLocalTime.CurrentUnixTime;
                MyCoroutiner.Start(DO_PRESS_SCALE_DOWN_ANIMATION + mId, _DoPressScaleDownAnimation());
            }
        }

        /// <summary>
        /// OnPointerUp.
        /// </summary>
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            mPointerEventDataPress = null;

            if (mOnEventPointerUp != null)
            {
                mOnEventPointerUp.Invoke(eventData);
            }

            if (mPressAnimationType == EAnimationType.Scale)
            {
                MyCoroutiner.Stop(DO_PRESS_SCALE_DOWN_ANIMATION + mId);
                MyCoroutiner.Start(DO_PRESS_SCALE_UP_ANIMATION + mId, _DoPressScaleUpAnimation(() =>
                {
                    mIsPressAnimationScaleCompleted = true;
                    if (mClickAnimationType == EAnimationType.None)
                    {
                        if (mOnEventPointerClick != null && mPointerEventDataClick != null)
                        {
                            mPointerEventDataClick.pointerPress = gameObject;
                            mOnEventPointerClick.Invoke(mPointerEventDataClick);
                            mPointerEventDataClick = null;
                        }
                    }
                }));
            }
        }

        /// <summary>
        /// OnPointerClick.
        /// </summary>
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            mPointerEventDataClick = eventData;

            if (Button.interactable)
            {
                if (mOnEventPointerDoubleClick != null)
                {
                    if (mLastClickTime == 0 || Time.time - mLastClickTime > 1)
                    {
                        mLastClickTime = Time.time;
                        _ProcessPointerClickEvent();
                    }
                    else
                    {
                        mLastClickTime = 0;
                        _ProcessPointerDoubleClickEvent();
                    }
                }
                else
                {
                    _ProcessPointerClickEvent();
                }
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Active.
        /// </summary>
        public void Active()
        {
            SetActive(true);
        }

        /// <summary>
        /// Deactive.
        /// </summary>
        public void Deactive()
        {
            SetActive(false);
        }

        /// <summary>
        /// Active/deactive.
        /// </summary>
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        /// <summary>
        /// Enable.
        /// </summary>
        public void Enable()
        {
            SetEnable(true);
        }

        /// <summary>
        /// Disable.
        /// </summary>
        public void Disable()
        {
            SetEnable(false);
        }

        /// <summary>
        /// Enable/disable.
        /// </summary>
        public void SetEnable(bool isEnable)
        {
            _InitImage();
            _InitIcon();
            _InitText();

            enabled = isEnable;
            if (mImage != null)
            {
                mImage.raycastTarget = isEnable;
            }
            if (mIcon != null)
            {
                mIcon.raycastTarget = isEnable;
            }
            if (mText != null)
            {
                mText.raycastTarget = isEnable;
            }
#if USE_MY_UI_TMPRO
            else if (mTextTMPro != null)
            {
                mTextTMPro.raycastTarget = isEnable;
            }
#endif
        }

        /// <summary>
        /// Set image.
        /// </summary>
        public void SetImage(Sprite sprite)
        {
            _InitImage();

            if (mImage != null)
            {
                mImage.sprite = sprite;
            }
        }

        /// <summary>
        /// Set icon.
        /// </summary>
        public void SetIcon(Sprite sprite)
        {
            _InitIcon();

            if (mIcon != null)
            {
                mIcon.sprite = sprite;
            }
        }

        /// <summary>
        /// Set text.
        /// </summary>
        public void SetText(string text)
        {
            _InitText();

            if (mText != null)
            {
                mText.text = text;
            }
#if USE_MY_UI_TMPRO
            else if (mTextTMPro != null)
            {
                mTextTMPro.text = text;
            }
#endif
        }

        /// <summary>
        /// Hide current effect.
        /// </summary>
        public void Normalize()
        {
            SetEffect(EEffectType.None);
            SetOpacity(1);
        }

        /// <summary>
        /// Opacity.
        /// </summary>
        public void Opacity()
        {
            SetOpacity(0.6f);
        }

        /// <summary>
        /// Set opacity.
        /// </summary>
        public void SetOpacity(float alpha)
        {
            _InitImage();
            _InitIcon();
            _InitText();

            if (mImage != null)
            {
                Color color = mImage.color;
                color.a = alpha;
                mImage.color = color;
            }
            if (mIcon != null)
            {
                Color color = mIcon.color;
                color.a = alpha;
                mIcon.color = color;
            }
            if (mText != null)
            {
                Color color = mText.color;
                color.a = alpha;
                mText.color = color;
            }
#if USE_MY_UI_TMPRO
            else if (mTextTMPro != null)
            {
                Color color = mTextTMPro.color;
                color.a = alpha;
                mTextTMPro.color = color;
            }
#endif
        }

        /// <summary>
        /// Darken.
        /// </summary>
        public void Darken()
        {
            SetEffect(mDarkType);
        }

        /// <summary>
        /// Set dark.
        /// </summary>
        public void SetDark(bool isDark)
        {
            if (isDark)
            {
                Darken();
            }
            else if (IsDark)
            {
                Normalize();
            }
        }

        /// <summary>
        /// Set dark mode.
        /// </summary>
        public void SetDarkMode(bool isDarkImage = true, bool isDarkText = true)
        {
            if (isDarkImage && isDarkText)
            {
                mDarkType = EEffectType.Dark;
            }
            else if (isDarkImage)
            {
                mDarkType = EEffectType.DarkImageOnly;
            }
            else if (isDarkText)
            {
                mDarkType = EEffectType.DarkTextOnly;
            }
            else
            {
                mDarkType = EEffectType.None;
            }
        }

        /// <summary>
        /// Gray out.
        /// </summary>
        public void GrayOut()
        {
            SetEffect(mGrayType);
        }

        /// <summary>
        /// Set gray mode.
        /// </summary>
        public void SetGrayMode(bool isGrayImage = true, bool isGrayText = true)
        {
            if (isGrayImage && isGrayText)
            {
                mGrayType = EEffectType.Gray;
            }
            else if (isGrayImage)
            {
                mGrayType = EEffectType.GrayImageOnly;
            }
            else if (isGrayText)
            {
                mGrayType = EEffectType.GrayTextOnly;
            }
            else
            {
                mGrayType = EEffectType.None;
            }
        }

        /// <summary>
        /// Set grayscale.
        /// </summary>
        public void SetGray(bool isGray)
        {
            if (isGray)
            {
                GrayOut();
            }
            else if (IsGray)
            {
                Normalize();
            }
        }

        /// <summary>
        /// Set effect.
        /// </summary>
        public void SetEffect(EEffectType effectType)
        {
            if (mEffectType == effectType)
            {
                return;
            }

            _InitImage();
            _InitIcon();
            _InitText();

            if (mImage != null)
            {
                if (effectType == EEffectType.Dark || effectType == EEffectType.DarkImageOnly)
                {
                    mImage.material = MyResourceManager.GetMaterialDarkening();
                }
                else if (effectType == EEffectType.Gray || effectType == EEffectType.GrayImageOnly)
                {
                    mImage.material = MyResourceManager.GetMaterialGrayscale();
                }
                else
                {
                    mImage.material = null;
                }
            }
            if (mIcon != null)
            {
                if (effectType == EEffectType.Dark || effectType == EEffectType.DarkImageOnly)
                {
                    mIcon.material = MyResourceManager.GetMaterialDarkening();
                }
                else if (effectType == EEffectType.Gray || effectType == EEffectType.GrayImageOnly)
                {
                    mIcon.material = MyResourceManager.GetMaterialGrayscale();
                }
                else
                {
                    mIcon.material = null;
                }
            }
            if (mText != null)
            {
                if (effectType == EEffectType.Dark || effectType == EEffectType.DarkTextOnly)
                {
                    mText.material = MyResourceManager.GetMaterialDarkening();
                }
                else if (effectType == EEffectType.Gray || effectType == EEffectType.GrayTextOnly)
                {
                    mText.material = MyResourceManager.GetMaterialGrayscale();
                }
                else
                {
                    mText.material = null;
                }
            }
#if USE_MY_UI_TMPRO
            else if (mTextTMPro != null)
            {
                if (effectType == EEffectType.Dark || effectType == EEffectType.DarkTextOnly)
                {
                    mTextTMPro.material = MyResourceManager.GetMaterialDarkening();
                }
                else if (effectType == EEffectType.Gray || effectType == EEffectType.GrayTextOnly)
                {
                    mTextTMPro.material = MyResourceManager.GetMaterialGrayscale();
                }
                else
                {
                    mTextTMPro.material = null;
                }
            }
#endif

            mEffectType = effectType;
        }

        /// <summary>
        /// Invoke click event.
        /// </summary>
        public void SelfClick()
        {
            if (Button.interactable)
            {
                mPointerEventDataClick = new PointerEventData(EventSystem.current);
                mPointerEventDataClick.pointerPress = gameObject;

                _ProcessPointerClickEvent();
            }
        }

#if UNITY_EDITOR

        /// <summary>
        /// Create a template.
        /// </summary>
        public static GameObject CreateTextTemplate()
        {
            GameObject obj = new GameObject("Button");
            if (Selection.activeTransform != null)
            {
                obj.transform.parent = Selection.activeTransform;
            }

            RectTransform contentRectTransform = obj.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref contentRectTransform, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 300, 100, 0, 0);
            obj.AddComponent<Image>();
            obj.AddComponent<MyUGUIButton>();

            GameObject text = new GameObject("Text");
            text.transform.SetParent(obj.transform, false);

            RectTransform textRectTransform = text.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref textRectTransform, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);
            Text textText = text.AddComponent<Text>();
            textText.text = "Button";
            textText.fontSize = 40;
            textText.supportRichText = false;
            textText.alignment = TextAnchor.MiddleCenter;
            textText.color = Color.black;
            textText.raycastTarget = false;

            EditorGUIUtility.PingObject(obj);
            Selection.activeGameObject = obj.gameObject;

            return obj;
        }

        /// <summary>
        /// Create a template.
        /// </summary>
        public static GameObject CreateTextMeshProTemplate()
        {
            GameObject obj = new GameObject("Button");
            if (Selection.activeTransform != null)
            {
                obj.transform.parent = Selection.activeTransform;
            }

            RectTransform contentRectTransform = obj.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref contentRectTransform, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 300, 100, 0, 0);
            obj.AddComponent<Image>();
            obj.AddComponent<MyUGUIButton>();

            GameObject text = new GameObject("Text");
            text.transform.SetParent(obj.transform, false);

            RectTransform textRectTransform = text.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref textRectTransform, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);
#if USE_MY_UI_TMPRO
            TextMeshProUGUI textText = text.AddComponent<TextMeshProUGUI>();
            textText.text = "Button";
            textText.fontSize = 40;
            textText.alignment = TMPro.TextAlignmentOptions.Center;
            textText.color = Color.black;
            textText.raycastTarget = false;
#else
            Debug.LogError("[" + typeof(MyUGUIButton).Name + "] CreateTextMeshProTemplate(): Could not find define symbol \"USE_MY_UI_TMPRO\".");
#endif

            EditorGUIUtility.PingObject(obj);
            Selection.activeGameObject = obj.gameObject;

            return obj;
        }

#endif

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Init image.
        /// </summary>
        private void _InitImage()
        {
            if (mImage == null)
            {
                if (Button.targetGraphic != null)
                {
                    mImage = Button.targetGraphic.GetComponent<Image>();
                }
                else
                {
                    mImage = gameObject.GetComponent<Image>();
                }
            }
        }

        /// <summary>
        /// Init icon.
        /// </summary>
        private void _InitIcon()
        {
            if (mIcon == null)
            {
                GameObject icon = MyUtilities.FindObjectInFirstLayer(gameObject, "Icon");
                if (icon != null)
                {
                    mIcon = icon.GetComponent<Image>();
                }
            }
        }

        /// <summary>
        /// Init text.
        /// </summary>
        private void _InitText()
        {
            if (mText == null)
            {
                GameObject text = MyUtilities.FindObjectInFirstLayer(gameObject, "Text");
                if (text != null)
                {
                    mText = text.GetComponent<Text>();
                }
                else
                {
                    mText = gameObject.GetComponent<Text>();
                }
            }
#if USE_MY_UI_TMPRO
            if (mText == null && mTextTMPro == null)
            {
                GameObject text = MyUtilities.FindObjectInFirstLayer(gameObject, "Text");
                if (text != null)
                {
                    mTextTMPro = text.GetComponent<TextMeshProUGUI>();
                }
                else
                {
                    mTextTMPro = gameObject.GetComponent<TextMeshProUGUI>();
                }
            }
#endif
        }

        /// <summary>
        /// Init extra touch zone.
        /// </summary>
        private void _InitExtraTouchZone()
        {
            if (mPressAnimationExtraTouchZone == null)
            {
                GameObject touchZone = new GameObject("ExtraTouchZone");
                touchZone.AddComponent<CanvasRenderer>();
                touchZone.AddComponent<MyUGUITouchZone>();
                mPressAnimationExtraTouchZone = touchZone.transform;
                mPressAnimationExtraTouchZone.SetParent(transform);
                mPressAnimationExtraTouchZone.GetComponent<RectTransform>().sizeDelta = transform.GetComponent<RectTransform>().sizeDelta;
                mPressAnimationExtraTouchZone.localPosition = Vector3.zero;
                mPressAnimationExtraTouchZone.localScale = Vector3.one;
            }
        }

        /// <summary>
        /// Process OnPointerClick event.
        /// </summary>
        private void _ProcessPointerClickEvent()
        {
            Button.OnSubmit(mPointerEventDataClick);

            switch (mClickAnimationType)
            {
                case EAnimationType.Scale:
                    {
                        if (!mIsTriggerClickEventAfterAnimationComplete)
                        {
                            if (mOnEventPointerClick != null && mPointerEventDataClick != null)
                            {
                                mOnEventPointerClick.Invoke(mPointerEventDataClick);
                            }
                            mPointerEventDataClick = null;
                        }

                        MyCoroutiner.Start(DO_CLICK_SCALE_ANIMATION, _DoClickScaleAnimation(() =>
                        {
                            if (mIsTriggerClickEventAfterAnimationComplete)
                            {
                                if (mOnEventPointerClick != null && mPointerEventDataClick != null)
                                {
                                    mOnEventPointerClick.Invoke(mPointerEventDataClick);
                                }
                            }
                            mPointerEventDataClick = null;
                        }));
                    }
                    break;

                default:
                    {
                        if (mPressAnimationType == EAnimationType.None || !mIsTriggerClickEventAfterAnimationComplete || mIsPressAnimationScaleCompleted)
                        {
                            if (mOnEventPointerClick != null && mPointerEventDataClick != null)
                            {
                                mOnEventPointerClick.Invoke(mPointerEventDataClick);
                            }
                            mPointerEventDataClick = null;
                        }
                    }
                    break;
            }

            switch (mClickSoundType)
            {
                case ESoundType.ResourcesPath:
                    {
                        if (!string.IsNullOrEmpty(mClickSoundResourcePath))
                        {
                            MySoundManager.Instance.PlaySFX(mClickSoundResourcePath, mClickSoundDelayTime, mClickSoundLocalVolume);
                        }
                    }
                    break;

                case ESoundType.AudioClip:
                    {
                        if (mClickSoundAudioClip != null)
                        {
                            MySoundManager.Instance.PlaySFX(mClickSoundAudioClip, mClickSoundDelayTime, mClickSoundLocalVolume);
                        }
                    }
                    break;
            }

            if (mPressAnimationExtraTouchZone != null)
            {
                mPressAnimationExtraTouchZone.gameObject.SetActive(false);
            }
        }


        /// <summary>
        /// Process OnPointerDoubleClick event.
        /// </summary>
        private void _ProcessPointerDoubleClickEvent()
        {
            Button.OnSubmit(mPointerEventDataClick);

            switch (mClickAnimationType)
            {
                case EAnimationType.Scale:
                    {
                        if (!mIsTriggerClickEventAfterAnimationComplete)
                        {
                            if (mOnEventPointerDoubleClick != null && mPointerEventDataClick != null)
                            {
                                mOnEventPointerDoubleClick.Invoke(mPointerEventDataClick);
                            }
                            mPointerEventDataClick = null;
                        }

                        MyCoroutiner.Start(DO_CLICK_SCALE_ANIMATION, _DoClickScaleAnimation(() =>
                        {
                            if (mIsTriggerClickEventAfterAnimationComplete)
                            {
                                if (mOnEventPointerDoubleClick != null && mPointerEventDataClick != null)
                                {
                                    mOnEventPointerDoubleClick.Invoke(mPointerEventDataClick);
                                }
                            }
                            mPointerEventDataClick = null;
                        }));
                    }
                    break;

                default:
                    {
                        if (mPressAnimationType == EAnimationType.None || !mIsTriggerClickEventAfterAnimationComplete || mIsPressAnimationScaleCompleted)
                        {
                            if (mOnEventPointerDoubleClick != null && mPointerEventDataClick != null)
                            {
                                mOnEventPointerDoubleClick.Invoke(mPointerEventDataClick);
                            }
                            mPointerEventDataClick = null;
                        }
                    }
                    break;
            }

            switch (mClickSoundType)
            {
                case ESoundType.ResourcesPath:
                    {
                        if (!string.IsNullOrEmpty(mClickSoundResourcePath))
                        {
                            MySoundManager.Instance.PlaySFX(mClickSoundResourcePath, mClickSoundDelayTime, mClickSoundLocalVolume);
                        }
                    }
                    break;

                case ESoundType.AudioClip:
                    {
                        if (mClickSoundAudioClip != null)
                        {
                            MySoundManager.Instance.PlaySFX(mClickSoundAudioClip, mClickSoundDelayTime, mClickSoundLocalVolume);
                        }
                    }
                    break;
            }

            if (mPressAnimationExtraTouchZone != null)
            {
                mPressAnimationExtraTouchZone.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Play scale animation for down event.
        /// </summary>
        private IEnumerator _DoPressScaleDownAnimation()
        {
            _InitExtraTouchZone();
            mPressAnimationExtraTouchZone.gameObject.SetActive(mPressAnimationUseExtraTouchZone);

            yield return new WaitForSeconds(mPressAnimationDelayTime);

            float speed = 1f / mPressAnimationDownDuration;

            float deadline = Time.time + mPressAnimationDownDuration;
            while (Time.time < deadline)
            {
                if (this != null)
                {
                    transform.localScale += mPressAnimationScaleBy * Time.deltaTime * speed;
                    Vector3 touchZoneScale = mOriginalScale;
                    touchZoneScale.x /= transform.localScale.x;
                    touchZoneScale.y /= transform.localScale.y;
                    touchZoneScale.z /= transform.localScale.z;
                    mPressAnimationExtraTouchZone.localScale = touchZoneScale;
                    yield return null;
                }
                else
                {
                    deadline = 0;
                }
            }

            if (this != null)
            {
                transform.localScale = mOriginalScale + mPressAnimationScaleBy;
                Vector3 touchZoneScale = mOriginalScale;
                touchZoneScale.x /= transform.localScale.x;
                touchZoneScale.y /= transform.localScale.y;
                touchZoneScale.z /= transform.localScale.z;
                mPressAnimationExtraTouchZone.localScale = touchZoneScale;
            }
        }

        /// <summary>
        /// Play scale animation for up event.
        /// </summary>
        private IEnumerator _DoPressScaleUpAnimation(Action onComplete)
        {
            _InitExtraTouchZone();
            mPressAnimationExtraTouchZone.gameObject.SetActive(mPressAnimationUseExtraTouchZone);

            float speed = 1f / mPressAnimationUpDuration;

            float deadline = Time.time + mPressAnimationUpDuration;
            while (Time.time < deadline)
            {
                if (this != null)
                {
                    transform.localScale -= mPressAnimationScaleBy * Time.deltaTime * speed;
                    if ((mPressAnimationScaleBy.x < 0 && transform.localScale.x >= mOriginalScale.x) || (mPressAnimationScaleBy.x > 0 && transform.localScale.x <= mOriginalScale.x) || (mPressAnimationScaleBy.y < 0 && transform.localScale.y >= mOriginalScale.y) || (mPressAnimationScaleBy.y > 0 && transform.localScale.y <= mOriginalScale.y))
                    {
                        deadline = 0;
                        transform.localScale = mOriginalScale;
                        mPressAnimationExtraTouchZone.localScale = Vector3.one;
                    }
                    else
                    {
                        Vector3 touchZoneScale = mOriginalScale;
                        touchZoneScale.x /= transform.localScale.x;
                        touchZoneScale.y /= transform.localScale.y;
                        touchZoneScale.z /= transform.localScale.z;
                        mPressAnimationExtraTouchZone.localScale = touchZoneScale;
                        yield return null;
                    }
                }
                else
                {
                    deadline = 0;
                }
            }

            if (this != null)
            {
                transform.localScale = mOriginalScale;
                mPressAnimationExtraTouchZone.localScale = Vector3.one;
                mPressAnimationExtraTouchZone.gameObject.SetActive(false);
            }

            if (onComplete != null)
            {
                onComplete();
            }
        }

        /// <summary>
        /// Play scale animation for click event.
        /// </summary>
        private IEnumerator _DoClickScaleAnimation(Action onComplete)
        {
            yield return new WaitForSeconds(mClickAnimationDelayTime);

            transform.localScale = mOriginalScale;

            float speed1 = 1f / mClickAnimationDownDuration;
            float deadline1 = Time.time + mClickAnimationDownDuration;
            while (Time.time < deadline1)
            {
                if (this != null)
                {
                    transform.localScale += mClickAnimationScaleBy * Time.deltaTime * speed1;
                    yield return null;
                }
                else
                {
                    deadline1 = 0;
                }
            }

            float speed2 = 1f / mClickAnimationUpDuration;
            float deadline2 = Time.time + mClickAnimationUpDuration;
            while (Time.time < deadline2)
            {
                if (this != null)
                {
                    transform.localScale -= mClickAnimationScaleBy * Time.deltaTime * speed2;
                    yield return null;
                }
                else
                {
                    deadline2 = 0;
                }
            }

            if (this != null)
            {
                transform.localScale = mOriginalScale;
            }

            if (onComplete != null)
            {
                onComplete();
            }
        }

        #endregion

        #region ----- Internal Class -----

        public class MyPointerEvent : UnityEvent<PointerEventData>
        {
        }

        #endregion

        #region ----- Enumeration -----

        public enum EAnimationType
        {
            None = 0,
            Scale,
        }

        public enum ESoundType
        {
            None = 0,
            ResourcesPath,
            AudioClip,
        }

        public enum EEffectType : byte
        {
            None = 0,
            Dark,
            DarkImageOnly,
            DarkTextOnly,
            Gray,
            GrayImageOnly,
            GrayTextOnly
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUGUIButton)), CanEditMultipleObjects]
    public class MyUGUIButtonEditor : Editor
    {
        private MyUGUIButton mScript;
        private SerializedProperty mPressAnimationType;
        private SerializedProperty mPressAnimationScaleBy;
        private SerializedProperty mPressAnimationDelayTime;
        private SerializedProperty mPressAnimationDownDuration;
        private SerializedProperty mPressAnimationUpDuration;
        private SerializedProperty mPressAnimationUseExtraTouchZone;
        private SerializedProperty mClickAnimationType;
        private SerializedProperty mClickAnimationScaleBy;
        private SerializedProperty mClickAnimationDelayTime;
        private SerializedProperty mClickAnimationDownDuration;
        private SerializedProperty mClickAnimationUpDuration;
        private SerializedProperty mClickSoundType;
        private SerializedProperty mClickSoundResourcePath;
        private SerializedProperty mClickSoundAudioClip;
        private SerializedProperty mClickSoundDelayTime;
        private SerializedProperty mClickSoundLocalVolume;
        private SerializedProperty mIsTriggerClickEventAfterAnimationComplete;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIButton)target;
            mPressAnimationType = serializedObject.FindProperty("mPressAnimationType");
            mPressAnimationScaleBy = serializedObject.FindProperty("mPressAnimationScaleBy");
            mPressAnimationDelayTime = serializedObject.FindProperty("mPressAnimationDelayTime");
            mPressAnimationDownDuration = serializedObject.FindProperty("mPressAnimationDownDuration");
            mPressAnimationUpDuration = serializedObject.FindProperty("mPressAnimationUpDuration");
            mPressAnimationUseExtraTouchZone = serializedObject.FindProperty("mPressAnimationUseExtraTouchZone");
            mClickAnimationType = serializedObject.FindProperty("mClickAnimationType");
            mClickAnimationScaleBy = serializedObject.FindProperty("mClickAnimationScaleBy");
            mClickAnimationDelayTime = serializedObject.FindProperty("mClickAnimationDelayTime");
            mClickAnimationDownDuration = serializedObject.FindProperty("mClickAnimationDownDuration");
            mClickAnimationUpDuration = serializedObject.FindProperty("mClickAnimationUpDuration");
            mClickSoundType = serializedObject.FindProperty("mClickSoundType");
            mClickSoundResourcePath = serializedObject.FindProperty("mClickSoundResourcePath");
            mClickSoundAudioClip = serializedObject.FindProperty("mClickSoundAudioClip");
            mClickSoundDelayTime = serializedObject.FindProperty("mClickSoundDelayTime");
            mClickSoundLocalVolume = serializedObject.FindProperty("mClickSoundLocalVolume");
            mIsTriggerClickEventAfterAnimationComplete = serializedObject.FindProperty("mIsTriggerClickEventAfterAnimationComplete");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIButton), false);

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Press Event", EditorStyles.boldLabel);
            mPressAnimationType.enumValueIndex = (int)(MyUGUIButton.EAnimationType)EditorGUILayout.EnumPopup("   Animation", (MyUGUIButton.EAnimationType)mPressAnimationType.enumValueIndex);
            switch ((MyUGUIButton.EAnimationType)mPressAnimationType.enumValueIndex)
            {
                case MyUGUIButton.EAnimationType.Scale:
                    {
                        mPressAnimationScaleBy.vector3Value = EditorGUILayout.Vector3Field("   Scale By", mPressAnimationScaleBy.vector3Value);
                        mPressAnimationDelayTime.floatValue = EditorGUILayout.FloatField("   Start Delay", mPressAnimationDelayTime.floatValue);
                        mPressAnimationDownDuration.floatValue = EditorGUILayout.FloatField("   Down Duration", mPressAnimationDownDuration.floatValue);
                        mPressAnimationUpDuration.floatValue = EditorGUILayout.FloatField("   Up Duration", mPressAnimationUpDuration.floatValue);
                        mPressAnimationUseExtraTouchZone.boolValue = EditorGUILayout.Toggle("   Use Extra Touch Zone", mPressAnimationUseExtraTouchZone.boolValue);
                    }
                    break;
            }

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Click Event", EditorStyles.boldLabel);
            mClickAnimationType.enumValueIndex = (int)(MyUGUIButton.EAnimationType)EditorGUILayout.EnumPopup("   Animation", (MyUGUIButton.EAnimationType)mClickAnimationType.enumValueIndex);
            switch ((MyUGUIButton.EAnimationType)mClickAnimationType.enumValueIndex)
            {
                case MyUGUIButton.EAnimationType.Scale:
                    {
                        mClickAnimationScaleBy.vector3Value = EditorGUILayout.Vector3Field("      Scale By", mClickAnimationScaleBy.vector3Value);
                        mClickAnimationDelayTime.floatValue = EditorGUILayout.FloatField("      Start Delay", mClickAnimationDelayTime.floatValue);
                        mClickAnimationDownDuration.floatValue = EditorGUILayout.FloatField("   Down Duration", mClickAnimationDownDuration.floatValue);
                        mClickAnimationUpDuration.floatValue = EditorGUILayout.FloatField("   Up Duration", mClickAnimationUpDuration.floatValue);
                    }
                    break;
            }

            mClickSoundType.enumValueIndex = (int)(MyUGUIButton.ESoundType)EditorGUILayout.EnumPopup("   Play Sound On Click", (MyUGUIButton.ESoundType)mClickSoundType.enumValueIndex);
            switch ((MyUGUIButton.ESoundType)mClickSoundType.enumValueIndex)
            {
                case MyUGUIButton.ESoundType.ResourcesPath:
                    {
                        mClickSoundResourcePath.stringValue = EditorGUILayout.TextField("      Path", mClickSoundResourcePath.stringValue);
                    }
                    break;

                case MyUGUIButton.ESoundType.AudioClip:
                    {
                        mClickSoundAudioClip.objectReferenceValue = (AudioClip)EditorGUILayout.ObjectField("      Audio Clip", mClickSoundAudioClip.objectReferenceValue, typeof(AudioClip), false);
                    }
                    break;
            }
            if ((MyUGUIButton.ESoundType)mClickSoundType.enumValueIndex != MyUGUIButton.ESoundType.None)
            {
                mClickSoundDelayTime.floatValue = EditorGUILayout.FloatField("      Start Delay", mClickSoundDelayTime.floatValue);
                mClickSoundLocalVolume.floatValue = EditorGUILayout.FloatField("      Local Volume", mClickSoundLocalVolume.floatValue);
            }

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Other", EditorStyles.boldLabel);
            mIsTriggerClickEventAfterAnimationComplete.boolValue = EditorGUILayout.Toggle("   Trigger Click Event After Animation", mIsTriggerClickEventAfterAnimationComplete.boolValue);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }

#endif
}