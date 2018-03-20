/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIButton (version 2.0)
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MyClasses.UI
{
    public class MyUGUIButton : Button, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        #region ----- Variable -----

        [SerializeField]
        [HideInInspector]
        private bool mIsEnableSoundClick = true;
        [SerializeField]
        [HideInInspector]
        private string mSFXClick = "Sounds/sfx_click";

        private Image mImage;
        private Text mText;

        private MyPointerEvent mOnEventPointerClick;
        private MyPointerEvent mOnEventPointerDown;
        private MyPointerEvent mOnEventPointerPress;
        private MyPointerEvent mOnEventPointerUp;
        private PointerEventData mPointerEventDataPress;

        private EEffectType mEffectType = EEffectType.None;
        private EEffectType mGrayType = EEffectType.Gray;

        #endregion

        #region ----- Property -----

        public bool IsEnableSoundClick
        {
            get { return mIsEnableSoundClick; }
            set { mIsEnableSoundClick = value; }
        }

        public string SFXClick
        {
            get { return mSFXClick; }
            set { mSFXClick = value; }
        }

        public bool IsEnable
        {
            get { return enabled; }
        }

        public bool IsDark
        {
            get { return mEffectType == EEffectType.Dark; }
        }

        public bool IsGray
        {
            get { return mEffectType == EEffectType.Gray || mEffectType == EEffectType.GrayImageOnly || mEffectType == EEffectType.GrayTextOnly; }
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

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// Update.
        /// </summary>
        void Update()
        {
            if (mPointerEventDataPress != null)
            {
                mOnEventPointerPress.Invoke(mPointerEventDataPress);
            }
        }

        #endregion

        #region ----- Implement IPointerDownHandler, IPointerUpHandler, IPointerClickHandler -----

        /// <summary>
        /// OnPointerClick.
        /// </summary>
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (interactable)
            {
                base.OnSubmit(eventData);

                if (mOnEventPointerClick != null)
                {
                    mOnEventPointerClick.Invoke(eventData);
                }

                if (!string.IsNullOrEmpty(SFXClick))
                {
                    MySoundManager.Instance.PlaySFX(SFXClick);
                }
            }
        }

        /// <summary>
        /// OnPointerDown.
        /// </summary>
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (mOnEventPointerDown != null)
            {
                mOnEventPointerDown.Invoke(eventData);
            }

            DoStateTransition(SelectionState.Pressed, false);

            if (mOnEventPointerPress != null)
            {
                mPointerEventDataPress = eventData;
            }
        }

        /// <summary>
        /// OnPointerUp.
        /// </summary>
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            DoStateTransition(SelectionState.Normal, false);

            mPointerEventDataPress = null;

            if (mOnEventPointerUp != null)
            {
                mOnEventPointerUp.Invoke(eventData);
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
            enabled = isEnable;
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
        /// Set text.
        /// </summary>
        public void SetText(string text)
        {
            _InitText();

            if (mText != null)
            {
                mText.text = text;
            }
        }

        /// <summary>
        /// Hide current effect.
        /// </summary>
        public void Normalize()
        {
            SetEffect(EEffectType.None);
        }

        /// <summary>
        /// Darken.
        /// </summary>
        public void Darken()
        {
            SetEffect(EEffectType.Dark);
        }

        /// <summary>
        /// Set dark.
        /// </summary>
        public void SetDark(bool isDim)
        {
            if (isDim)
            {
                Darken();
            }
            else if (IsDark)
            {
                Normalize();
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
            _InitText();

            if (mImage != null)
            {
                if (effectType == EEffectType.Dark)
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
            if (mText != null)
            {
                if (effectType == EEffectType.Gray || effectType == EEffectType.GrayTextOnly)
                {
                    mText.material = MyResourceManager.GetMaterialGrayscale();
                }
                else
                {
                    mText.material = null;
                }
            }

            mEffectType = effectType;
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Init image.
        /// </summary>
        private void _InitImage()
        {
            if (mImage == null)
            {
                if (targetGraphic != null)
                {
                    mImage = targetGraphic.GetComponent<Image>();
                }
                else
                {
                    mImage = gameObject.GetComponent<Image>();
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
            }
        }

        #endregion

        #region ----- Internal Class -----

        public class MyPointerEvent : UnityEvent<PointerEventData>
        {
        }

        #endregion

        #region ----- Enumeration -----

        public enum EEffectType : byte
        {
            None = 0,
            Dark,
            Gray,
            GrayImageOnly,
            GrayTextOnly,
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUGUIButton))]
    public class MyUGUIButtonEditor : Editor
    {
        private MyUGUIButton mScript;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIButton)target;
        }
        
        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            mScript.IsEnableSoundClick = EditorGUILayout.Toggle("Play Sound On Click", mScript.IsEnableSoundClick);
            if (mScript.IsEnableSoundClick)
            {
                mScript.SFXClick = EditorGUILayout.TextField("Resources Path", mScript.SFXClick);
            }
        }
    }

#endif
}