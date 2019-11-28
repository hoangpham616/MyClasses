/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIToggleButton (version 2.18)
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
        private Text mTitleTurnOn;
        [SerializeField]
        private Text mTitleTurnOff;
        [SerializeField]
        private bool mIsShowTitle = true;
        [SerializeField]
        private float mSlideTime = 0.1f;

        [SerializeField]
        private Transform mTurnOnPosition;
        [SerializeField]
        private Sprite mTurnOnSpriteBackground;
        [SerializeField]
        private Sprite mTurnOnSpriteToggle;

        [SerializeField]
        private Transform mTurnOffPosition;
        [SerializeField]
        private Sprite mTurnOffSpriteBackground;
        [SerializeField]
        private Sprite mTurnOffSpriteToggle;

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

        #region ----- MonoBehaviour Event -----

        void OnEnable()
        {
            mButton.onClick.AddListener(_OnClick);
        }

        void OnDisable()
        {
            mButton.onClick.RemoveAllListeners();
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
        public void SetToggle(bool isToggle, bool isShowAnim = true)
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
            mBackground.raycastTarget = isEnable;
            mToggle.raycastTarget = isEnable;
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

#if UNITY_EDITOR

        /// <summary>
        /// Create a template.
        /// </summary>
        public static void CreateTemplate()
        {
            GameObject canvas = MyUtilities.FindObjectInRoot("Canvas");

            GameObject obj = new GameObject("ToggleButton");
            if (canvas != null)
            {
                obj.transform.SetParent(canvas.transform, false);
            }

            RectTransform contentRectTransform = obj.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref contentRectTransform, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 200, 100, 0, 0);
            obj.AddComponent<Image>();

            GameObject background = new GameObject("Background");
            background.transform.SetParent(obj.transform, false);

            RectTransform backgroundRectTransform = background.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref backgroundRectTransform, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);
            Image backgroundImage = background.AddComponent<Image>();
            backgroundImage.color = Color.white;
            backgroundImage.raycastTarget = true;
            Button backgroundButton = background.AddComponent<Button>();

            GameObject toggle = new GameObject("Toggle");
            toggle.transform.SetParent(background.transform, false);

            RectTransform toggleRectTransform = toggle.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref toggleRectTransform, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 90, 90, 50, 0);
            Image toggleImage = toggle.AddComponent<Image>();
            toggleImage.color = Color.black;
            toggleImage.type = Image.Type.Simple;
            toggleImage.preserveAspect = true;
            toggleImage.raycastTarget = true;

            GameObject titleTurnOn = new GameObject("TitleTurnOn");
            titleTurnOn.transform.SetParent(background.transform, false);

            RectTransform titleTurnOnRectTransform = titleTurnOn.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref titleTurnOnRectTransform, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 50, 50, -50, 0);
            Text titleTurnOnText = titleTurnOn.AddComponent<Text>();
            titleTurnOnText.text = string.Empty;
            titleTurnOnText.fontSize = 40;
            titleTurnOnText.supportRichText = false;
            titleTurnOnText.alignment = TextAnchor.MiddleCenter;
            titleTurnOnText.horizontalOverflow = HorizontalWrapMode.Overflow;
            titleTurnOnText.verticalOverflow = VerticalWrapMode.Overflow;
            titleTurnOnText.color = Color.black;
            titleTurnOnText.raycastTarget = false;

            GameObject titleTurnOff = new GameObject("TitleTurnOff");
            titleTurnOff.transform.SetParent(background.transform, false);

            RectTransform titleTurnOffRectTransform = titleTurnOff.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref titleTurnOffRectTransform, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 50, 50, 50, 0);
            Text titleTurnOffText = titleTurnOff.AddComponent<Text>();
            titleTurnOffText.text = string.Empty;
            titleTurnOffText.fontSize = 40;
            titleTurnOffText.supportRichText = false;
            titleTurnOffText.alignment = TextAnchor.MiddleCenter;
            titleTurnOffText.horizontalOverflow = HorizontalWrapMode.Overflow;
            titleTurnOffText.verticalOverflow = VerticalWrapMode.Overflow;
            titleTurnOffText.color = Color.black;
            titleTurnOffText.raycastTarget = false;

            GameObject positionTurnOn = new GameObject("PositionTurnOn");
            positionTurnOn.SetActive(false);
            positionTurnOn.transform.SetParent(obj.transform, false);

            RectTransform positionTurnOnRectTransform = positionTurnOn.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref positionTurnOnRectTransform, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 10, 10, -50, 0);

            GameObject positionTurnOff = new GameObject("PositionTurnOff");
            positionTurnOff.SetActive(false);
            positionTurnOff.transform.SetParent(obj.transform, false);

            RectTransform positionTurnOffRectTransform = positionTurnOff.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref positionTurnOffRectTransform, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 10, 10, 50, 0);

            MyUGUIToggleButton objToggleButton = obj.AddComponent<MyUGUIToggleButton>();
            objToggleButton.mButton = backgroundButton;
            objToggleButton.mBackground = backgroundImage;
            objToggleButton.mToggle = toggleImage;
            objToggleButton.mTitleTurnOn = titleTurnOnText;
            objToggleButton.mTitleTurnOff = titleTurnOffText;
            objToggleButton.mTurnOnPosition = positionTurnOn.transform;
            objToggleButton.mTurnOffPosition = positionTurnOff.transform;

            EditorGUIUtility.PingObject(obj);
            Selection.activeGameObject = obj.gameObject;
        }

#endif

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
                if (mTitleTurnOn != null)
                {
                    mTitleTurnOn.gameObject.SetActive(mIsShowTitle);
                }
                if (mTitleTurnOff != null)
                {
                    mTitleTurnOff.gameObject.SetActive(false);
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
                if (mTitleTurnOff != null)
                {
                    mTitleTurnOff.gameObject.SetActive(mIsShowTitle);
                }
                if (mTitleTurnOn != null)
                {
                    mTitleTurnOn.gameObject.SetActive(false);
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
        private SerializedProperty mIsShowTitle;
        private SerializedProperty mTurnOnPosition;
        private SerializedProperty mTurnOnSpriteBackground;
        private SerializedProperty mTurnOnSpriteToggle;
        private SerializedProperty mTurnOffPosition;
        private SerializedProperty mTurnOffSpriteBackground;
        private SerializedProperty mTurnOffSpriteToggle;
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
            mTextTurnOn = serializedObject.FindProperty("mTitleTurnOn");
            mTextTurnOff = serializedObject.FindProperty("mTitleTurnOff");
            mIsShowTitle = serializedObject.FindProperty("mIsShowTitle");
            mTurnOnPosition = serializedObject.FindProperty("mTurnOnPosition");
            mTurnOnSpriteBackground = serializedObject.FindProperty("mTurnOnSpriteBackground");
            mTurnOnSpriteToggle = serializedObject.FindProperty("mTurnOnSpriteToggle");
            mTurnOffPosition = serializedObject.FindProperty("mTurnOffPosition");
            mTurnOffSpriteBackground = serializedObject.FindProperty("mTurnOffSpriteBackground");
            mTurnOffSpriteToggle = serializedObject.FindProperty("mTurnOffSpriteToggle");
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
            mIsShowTitle.boolValue = EditorGUILayout.Toggle("Is Show Title", mIsShowTitle.boolValue);
            mScript.SlideTime = EditorGUILayout.FloatField("Slide Time", mScript.SlideTime);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("On", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            mTurnOnPosition.objectReferenceValue = EditorGUILayout.ObjectField("Position", mTurnOnPosition.objectReferenceValue, typeof(Transform), true);
            mTurnOnSpriteBackground.objectReferenceValue = EditorGUILayout.ObjectField("Sprite Background", mTurnOnSpriteBackground.objectReferenceValue, typeof(Sprite), true);
            mTurnOnSpriteToggle.objectReferenceValue = EditorGUILayout.ObjectField("Sprite Toggle", mTurnOnSpriteToggle.objectReferenceValue, typeof(Sprite), true);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Off", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            mTurnOffPosition.objectReferenceValue = EditorGUILayout.ObjectField("Position", mTurnOffPosition.objectReferenceValue, typeof(Transform), true);
            mTurnOffSpriteBackground.objectReferenceValue = EditorGUILayout.ObjectField("Sprite Background", mTurnOffSpriteBackground.objectReferenceValue, typeof(Sprite), true);
            mTurnOffSpriteToggle.objectReferenceValue = EditorGUILayout.ObjectField("Sprite Toggle", mTurnOffSpriteToggle.objectReferenceValue, typeof(Sprite), true);
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
