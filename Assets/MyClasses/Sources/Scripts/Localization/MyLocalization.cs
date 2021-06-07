/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyLocalization (version 3.2)
 */

#pragma warning disable 0414
#pragma warning disable 0649

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyClasses
{
    public class MyLocalization : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private string mKey = string.Empty;
        [SerializeField]
        private string mPrefix = string.Empty;
        [SerializeField]
        private string mSuffix = string.Empty;
        [SerializeField]
        private EFormat mFormatText = EFormat.None;
        [SerializeField]
        private MyLocalizationManager.ELanguage[] mImageLanguages;
        [SerializeField]
        private GameObject[] mImageObjects;
        [SerializeField]
        private string[] mImageInvisibleTexts;
        
        private Text mText;
        private TextMeshProUGUI mTextTMPro;
        private Color mColor;
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
            Initialize();
            Localize();
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Initialize.
        /// </summary>
        public void Initialize()
        {
            if (mText == null)
            {
                mText = gameObject.GetComponent<Text>();
                if (mText != null)
                {
                    mColor = mText.color;
                }
                else if (mTextTMPro == null)
                {
                    mTextTMPro = gameObject.GetComponent<TextMeshProUGUI>();
                    if (mTextTMPro != null)
                    {
                        mColor = mTextTMPro.color;
                    }
                }
                mIsHasFix = !string.IsNullOrEmpty(mPrefix) || !string.IsNullOrEmpty(mSuffix);
            }
        }

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
            // localize image
            int imageIndex = -1;
            for (int i = 0; i < mImageLanguages.Length; i++)
            {
                if (imageIndex == -1 && (mImageLanguages[i] + 1) == MyLocalizationManager.Instance.Language)
                {
                    imageIndex = i;
                }
                mImageObjects[i].SetActive(false);
            }

            if (imageIndex >= 0)
            {
                mImageObjects[imageIndex].SetActive(true);

                string invisibleText = mImageInvisibleTexts.Length > imageIndex && mImageInvisibleTexts[imageIndex] != null ? mImageInvisibleTexts[imageIndex] : string.Empty;

                if (mText != null)
                {
                    mText.text = invisibleText;
                    if (invisibleText.Length > 0)
                    {
                        Color color = mColor;
                        color.a = 0;
                        mText.color = color;
                    }
                }
                else if (mTextTMPro != null)
                {
                    mTextTMPro.text = invisibleText;
                    if (invisibleText.Length > 0)
                    {
                        Color color = mColor;
                        color.a = 0;
                        mTextTMPro.color = color;
                    }
                }
            }

            // localize text
            if (mKey == string.Empty)
            {
                return;
            }

            string text = MyLocalizationManager.Instance.LoadKey(mKey);
            if (mIsHasFix)
            {
                text = mPrefix + text + mSuffix;
            }

            switch (mFormatText)
            {
                case EFormat.Capitalization:
                    {
                        text = text[0].ToString().ToUpper() + text.Substring(1).ToLower();
                    }
                    break;
                case EFormat.Lowercase:
                    {
                        text = text.ToLower();
                    }
                    break;
                case EFormat.Uppercase:
                    {
                        text = text.ToUpper();
                    }
                    break;
            }

            if (mText != null)
            {
                mText.text = text;
                mText.color = mColor;
            }
            else if (mTextTMPro != null)
            {
                mTextTMPro.text = text;
                mTextTMPro.color = mColor;
            }
        }

        #endregion

        #region ----- Enumeration -----

        public enum EFormat
        {
            None = 0,
            Capitalization,
            Lowercase,
            Uppercase
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyLocalization)), CanEditMultipleObjects]
    public class MyLocalizationEditor : Editor
    {
        private MyLocalization mScript;
        private SerializedProperty mImageLanguages;
        private SerializedProperty mImageObjects;
        private SerializedProperty mImageInvisibleTexts;
        private SerializedProperty mKey;
        private SerializedProperty mPrefix;
        private SerializedProperty mSuffix;
        private SerializedProperty mFormatText;
        private bool mIsImageLanguageVisible = true;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyLocalization)target;
            mKey = serializedObject.FindProperty("mKey");
            mPrefix = serializedObject.FindProperty("mPrefix");
            mSuffix = serializedObject.FindProperty("mSuffix");
            mFormatText = serializedObject.FindProperty("mFormatText");
            mImageLanguages = serializedObject.FindProperty("mImageLanguages");
            mImageObjects = serializedObject.FindProperty("mImageObjects");
            mImageInvisibleTexts = serializedObject.FindProperty("mImageInvisibleTexts");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyLocalization), false);

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Text", EditorStyles.boldLabel);
            mKey.stringValue = EditorGUILayout.TextField("    Key", mKey.stringValue);
            mPrefix.stringValue = EditorGUILayout.TextField("    Prefix", mPrefix.stringValue);
            mSuffix.stringValue = EditorGUILayout.TextField("    Suffix", mSuffix.stringValue);
            mFormatText.enumValueIndex = (int)(MyLocalization.EFormat)EditorGUILayout.EnumPopup("    Format", (MyLocalization.EFormat)System.Enum.GetValues(typeof(MyLocalization.EFormat)).GetValue(mFormatText.enumValueIndex));

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if (!Application.isPlaying)
                {
                    MyLocalizationManager.Instance.LoadLanguage(MyLocalizationManager.Instance.Language, true);
                    mScript.Initialize();
                    mScript.Localize();
                }
            }

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Image", EditorStyles.boldLabel);
            mImageLanguages.arraySize = EditorGUILayout.IntField("    Size", mImageLanguages.arraySize);
            if (mImageLanguages.arraySize > 0)
            {
                mIsImageLanguageVisible = EditorGUILayout.Foldout(mIsImageLanguageVisible, "    Images");
                if (mIsImageLanguageVisible)
                {
                    EditorGUI.indentLevel++;
                    mImageObjects.arraySize = mImageLanguages.arraySize;
                    mImageInvisibleTexts.arraySize = mImageLanguages.arraySize;
                    for (int i = 0; i < mImageLanguages.arraySize; i++)
                    {
                        EditorGUILayout.LabelField((i + 1).ToString(), EditorStyles.boldLabel);

                        SerializedProperty language = mImageLanguages.GetArrayElementAtIndex(i);
                        language.enumValueIndex = (int)(MyLocalizationManager.ELanguage)EditorGUILayout.EnumPopup("    Language", (MyLocalizationManager.ELanguage)System.Enum.GetValues(typeof(MyLocalizationManager.ELanguage)).GetValue(language.enumValueIndex));

                        SerializedProperty image = mImageObjects.GetArrayElementAtIndex(i);
                        image.objectReferenceValue = (GameObject)EditorGUILayout.ObjectField("    Image", image.objectReferenceValue, typeof(GameObject), true);

                        SerializedProperty invisibleText = mImageInvisibleTexts.GetArrayElementAtIndex(i);
                        invisibleText.stringValue = EditorGUILayout.TextField("    Invisible Text", invisibleText.stringValue);
                    }
                    EditorGUI.indentLevel--;
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }

#endif
}