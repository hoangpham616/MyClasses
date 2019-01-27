/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyLocalization (version 2.13)
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;

#if USE_MY_UI_TMPRO
using TMPro;
#endif

namespace MyClasses
{
#if !USE_MY_UI_TMPRO
    [RequireComponent(typeof(Text))]
#endif
    public class MyLocalization : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private string mPrefix = string.Empty;
        [SerializeField]
        private string mSuffix = string.Empty;
        [SerializeField]
        private EFormatText mFormatText = EFormatText.None;

#if USE_MY_UI_TMPRO
        private TextMeshProUGUI mTextTMPro;
#endif

        private Text mText;
        private string mKey;
        private bool mIsHasFix;

        #endregion

        #region ----- Property -----

        public string Prefix
        {
            get { return mPrefix; }
            set
            {
                mPrefix = value;
                mIsHasFix = !string.IsNullOrEmpty(mPrefix) || !string.IsNullOrEmpty(mSuffix);
                Localize();
            }
        }

        public string Suffix
        {
            get { return mSuffix; }
            set
            {
                mSuffix = value;
                mIsHasFix = !string.IsNullOrEmpty(mPrefix) || !string.IsNullOrEmpty(mSuffix);
                Localize();
            }
        }

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// Start.
        /// </summary>
        void Start()
        {
            MyLocalizationManager.Instance.Register(this);
        }

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            if (mText == null)
            {
                mText = gameObject.GetComponent<Text>();
                if (mText != null)
                {
                    mKey = mText.text;
                }
#if USE_MY_UI_TMPRO
                else
                {
                    mTextTMPro = gameObject.GetComponent<TextMeshProUGUI>();
                    if (mTextTMPro != null)
                    {
                        mKey = mTextTMPro.text;
                    }
                }
#endif
                mIsHasFix = !string.IsNullOrEmpty(mPrefix) || !string.IsNullOrEmpty(mSuffix);
            }

            Localize();
        }

        /// <summary>
        /// OnDisable.
        /// </summary>
        void OnDisable()
        {
            if (mText != null)
            {
                mText.text = mKey;
            }
#if USE_MY_UI_TMPRO
            else if (mTextTMPro != null)
            {
                mTextTMPro.text = mKey;
            }
#endif
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set key.
        /// </summary>
        public void SetKey(string key)
        {
            mKey = key;
        }

        /// <summary>
        /// Localize.
        /// </summary>
        public void Localize()
        {
            string text = MyLocalizationManager.Instance.LoadKey(mKey);

            if (mIsHasFix)
            {
                text = mPrefix + text + mSuffix;
            }

            if (mFormatText == EFormatText.Lowercase)
            {
                text = text.ToLower();
            }
            else if (mFormatText == EFormatText.Uppercase)
            {
                text = text.ToUpper();
            }

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

        #endregion

        #region ----- Enumeration -----

        public enum EFormatText
        {
            None = 0,
            Lowercase = 1,
            Uppercase = 2
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyLocalization))]
    public class MyLocalizationEditor : Editor
    {
        private MyLocalization mScript;
        private SerializedProperty mPrefix;
        private SerializedProperty mSuffix;
        private SerializedProperty mFormatText;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyLocalization)target;
            mPrefix = serializedObject.FindProperty("mPrefix");
            mSuffix = serializedObject.FindProperty("mSuffix");
            mFormatText = serializedObject.FindProperty("mFormatText");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyLocalization), false);

            serializedObject.Update();

            mPrefix.stringValue = EditorGUILayout.TextField("Prefix", mPrefix.stringValue);
            mSuffix.stringValue = EditorGUILayout.TextField("Suffix", mSuffix.stringValue);
            mFormatText.enumValueIndex = (int)(MyLocalization.EFormatText)EditorGUILayout.EnumPopup("Format Text", (MyLocalization.EFormatText)System.Enum.GetValues(typeof(MyLocalization.EFormatText)).GetValue(mFormatText.enumValueIndex));

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}