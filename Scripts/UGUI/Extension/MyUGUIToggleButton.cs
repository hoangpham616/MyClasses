/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIToggleButton (version 2.0)
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Collections;

namespace MyClasses.UI
{
    public class MyUGUIToggleButton : MonoBehaviour
    {
        #region ----- Internal Class -----

        [Serializable]
        public class UnityEventBoolean : UnityEvent<bool> { }

        #endregion

        #region ----- Variable -----

        [SerializeField]
        private Button mButton;
        [SerializeField]
        private Image mBackground;
        [SerializeField]
        private Image mToggle;
        [SerializeField]
        private Text mTextTurnOn;
        [SerializeField]
        private Text mTextTurnOff;
        [SerializeField]
        private float mSlideTime = 0.1f;

        [SerializeField]
        private Transform mTurnOnPosition;
        [SerializeField]
        private Sprite mTurnOnSpriteBackground;
        [SerializeField]
        private Sprite mTurnOnSpriteToggle;
        [SerializeField]
        private string mTurnOnTitle;

        [SerializeField]
        private Transform mTurnOffPosition;
        [SerializeField]
        private Sprite mTurnOffSpriteBackground;
        [SerializeField]
        private Sprite mTurnOffSpriteToggle;
        [SerializeField]
        private string mTurnOffTitle;

        [SerializeField]
        private bool mIsEnableSoundClick = true;
        [SerializeField]
        private string mSFXClick = "Sounds/sfx_click";

        [SerializeField]
        public UnityEventBoolean OnValueChange;

        private EEffectType mEffectType = EEffectType.None;
        private bool mIsToggling;
        private bool mIsToggle;

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

        public float SlideTime
        {
            get { return mSlideTime; }
            set { mSlideTime = value; }
        }

        public bool IsToggle
        {
            get { return mIsToggle; }
        }

        public bool IsEnable
        {
            get { return mBackground.enabled; }
        }

        public bool IsDark
        {
            get { return mEffectType == EEffectType.Dark; }
        }

        public bool IsGray
        {
            get { return mEffectType == EEffectType.Gray; }
        }

        public EEffectType EffectType
        {
            get { return mEffectType; }
        }

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// Awake.
        /// </summary>
        void Awake()
        {
            mButton.onClick.RemoveAllListeners();
            mButton.onClick.AddListener(_OnClick);
        }

        #endregion

        #region ----- Button Event -----

        /// <summary>
        /// Click on toggle.
        /// </summary>
        private void _OnClick()
        {
            if (!mIsToggling)
            {
                SetToggle(!mIsToggle, true);
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set toggle.
        /// </summary>
        public void SetToggle(bool isToggle, bool isShowAnim)
        {
            if (isShowAnim)
            {
                StartCoroutine(_DoToggling(isToggle));
            }
            else
            {
                _SetToggle(isToggle);
            }
        }

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
            mBackground.enabled = isEnable;
            mToggle.enabled = isEnable;
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
        /// Gray out.
        /// </summary>
        public void GrayOut()
        {
            SetEffect(EEffectType.Gray);
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
            Material material = null;

            if (effectType == EEffectType.Dark)
            {
                material = MyResourceManager.GetMaterialDarkening();
            }
            else if (effectType == EEffectType.Gray)
            {
                material = MyResourceManager.GetMaterialGrayscale();
            }

            mBackground.material = material;
            mToggle.material = material;

            mEffectType = effectType;
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Handle toggling.
        /// </summary>
        private IEnumerator _DoToggling(bool isToggle)
        {
            Vector3 fromPosition = isToggle ? mTurnOffPosition.position : mTurnOnPosition.position;
            Vector3 toPosition = isToggle ? mTurnOnPosition.position : mTurnOffPosition.position;
            Vector3 moveSpeed = (toPosition - fromPosition) / mSlideTime;

            mIsToggling = true;
            while (mIsToggling)
            {
                Vector3 moveOffset = moveSpeed * Time.deltaTime;

                mToggle.transform.position = mToggle.transform.position + moveOffset;
                if ((moveOffset.x > 0 && mToggle.transform.position.x >= toPosition.x) ||
                    (moveOffset.x < 0 && mToggle.transform.position.x <= toPosition.x) ||
                    (moveOffset.y > 0 && mToggle.transform.position.y >= toPosition.y) ||
                    (moveOffset.y < 0 && mToggle.transform.position.y <= toPosition.y))
                {
                    break;
                }

                yield return null;
            }

            _SetToggle(isToggle);
        }

        /// <summary>
        /// Set toggle.
        /// </summary>
        private void _SetToggle(bool isToggle)
        {
            mIsToggling = false;
            mIsToggle = isToggle;

            if (mIsToggle)
            {
                if (mTurnOnSpriteBackground != null)
                {
                    mBackground.sprite = mTurnOnSpriteBackground;
                }
                if (mTurnOnSpriteToggle != null)
                {
                    mToggle.sprite = mTurnOnSpriteToggle;
                }
                if (mTextTurnOn != null)
                {
                    if (!string.IsNullOrEmpty(mTurnOnTitle))
                    {
                        mTextTurnOn.gameObject.SetActive(true);
                        mTextTurnOn.text = mTurnOffTitle;
                    }
                    else
                    {
                        mTextTurnOn.gameObject.SetActive(false);
                    }
                }
                if (mTextTurnOff != null)
                {
                    mTextTurnOff.gameObject.SetActive(false);
                }
                mToggle.transform.position = mTurnOnPosition.position;
            }
            else
            {
                if (mTurnOffSpriteBackground != null)
                {
                    mBackground.sprite = mTurnOffSpriteBackground;
                }
                if (mTurnOffSpriteToggle != null)
                {
                    mToggle.sprite = mTurnOffSpriteToggle;
                }
                if (mTextTurnOff != null)
                {
                    if (!string.IsNullOrEmpty(mTurnOffTitle))
                    {
                        mTextTurnOff.gameObject.SetActive(true);
                        mTextTurnOff.text = mTurnOffTitle;
                    }
                    else
                    {
                        mTextTurnOff.gameObject.SetActive(false);
                    }
                }
                if (mTextTurnOn != null)
                {
                    mTextTurnOn.gameObject.SetActive(false);
                }
                mToggle.transform.position = mTurnOffPosition.position;
            }

            if (OnValueChange != null)
            {
                OnValueChange.Invoke(mIsToggle);
            }
        }

        #endregion

        #region ----- Enumeration -----

        public enum EEffectType : byte
        {
            None = 0,
            Dark,
            Gray
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUGUIToggleButton))]
    public class MyUGUIToggleButtonEditor : Editor
    {
        private MyUGUIToggleButton mScript;
        private SerializedProperty mButton;
        private SerializedProperty mBackground;
        private SerializedProperty mToggle;
        private SerializedProperty mTextTurnOn;
        private SerializedProperty mTextTurnOff;
        private SerializedProperty mTurnOnPosition;
        private SerializedProperty mTurnOnSpriteBackground;
        private SerializedProperty mTurnOnSpriteToggle;
        private SerializedProperty mTurnOnTitle;
        private SerializedProperty mTurnOffPosition;
        private SerializedProperty mTurnOffSpriteBackground;
        private SerializedProperty mTurnOffSpriteToggle;
        private SerializedProperty mTurnOffTitle;
        private SerializedProperty mOnValueChange;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIToggleButton)target;
            mButton = serializedObject.FindProperty("mButton");
            mBackground = serializedObject.FindProperty("mBackground");
            mToggle = serializedObject.FindProperty("mToggle");
            mTextTurnOn = serializedObject.FindProperty("mTextTurnOn");
            mTextTurnOff = serializedObject.FindProperty("mTextTurnOff");
            mTurnOnPosition = serializedObject.FindProperty("mTurnOnPosition");
            mTurnOnSpriteBackground = serializedObject.FindProperty("mTurnOnSpriteBackground");
            mTurnOnSpriteToggle = serializedObject.FindProperty("mTurnOnSpriteToggle");
            mTurnOnTitle = serializedObject.FindProperty("mTurnOnTitle");
            mTurnOffPosition = serializedObject.FindProperty("mTurnOffPosition");
            mTurnOffSpriteBackground = serializedObject.FindProperty("mTurnOffSpriteBackground");
            mTurnOffSpriteToggle = serializedObject.FindProperty("mTurnOffSpriteToggle");
            mTurnOffTitle = serializedObject.FindProperty("mTurnOffTitle");
            mOnValueChange = serializedObject.FindProperty("OnValueChange");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIBooter), false);

            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Toggle", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            mButton.objectReferenceValue = EditorGUILayout.ObjectField("Button", mButton.objectReferenceValue, typeof(Button), true);
            mBackground.objectReferenceValue = EditorGUILayout.ObjectField("Image Background", mBackground.objectReferenceValue, typeof(Image), true);
            mToggle.objectReferenceValue = EditorGUILayout.ObjectField("Image Toggle", mToggle.objectReferenceValue, typeof(Image), true);
            mTextTurnOn.objectReferenceValue = EditorGUILayout.ObjectField("Text Turn On (Nullable)", mTextTurnOn.objectReferenceValue, typeof(Text), true);
            mTextTurnOff.objectReferenceValue = EditorGUILayout.ObjectField("Text Turn Off (Nullable)", mTextTurnOff.objectReferenceValue, typeof(Text), true);
            mScript.SlideTime = EditorGUILayout.FloatField("Slide Time", mScript.SlideTime);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("On", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            mTurnOnPosition.objectReferenceValue = EditorGUILayout.ObjectField("Position", mTurnOnPosition.objectReferenceValue, typeof(Transform), true);
            mTurnOnSpriteBackground.objectReferenceValue = EditorGUILayout.ObjectField("Sprite Background", mTurnOnSpriteBackground.objectReferenceValue, typeof(Sprite), true);
            mTurnOnSpriteToggle.objectReferenceValue = EditorGUILayout.ObjectField("Sprite Toggle", mTurnOnSpriteToggle.objectReferenceValue, typeof(Sprite), true);
            mTurnOnTitle.stringValue = EditorGUILayout.TextField("Title", mTurnOnTitle.stringValue);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Off", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            mTurnOffPosition.objectReferenceValue = EditorGUILayout.ObjectField("Position", mTurnOffPosition.objectReferenceValue, typeof(Transform), true);
            mTurnOffSpriteBackground.objectReferenceValue = EditorGUILayout.ObjectField("Sprite Background", mTurnOffSpriteBackground.objectReferenceValue, typeof(Sprite), true);
            mTurnOffSpriteToggle.objectReferenceValue = EditorGUILayout.ObjectField("Sprite Toggle", mTurnOffSpriteToggle.objectReferenceValue, typeof(Sprite), true);
            mTurnOffTitle.stringValue = EditorGUILayout.TextField("Title", mTurnOffTitle.stringValue);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Sound", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            mScript.IsEnableSoundClick = EditorGUILayout.Toggle("Play Sound On Click", mScript.IsEnableSoundClick);
            if (mScript.IsEnableSoundClick)
            {
                mScript.SFXClick = EditorGUILayout.TextField("Resources Path", mScript.SFXClick);
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Event", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(mOnValueChange, new GUIContent("On Value Change"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
