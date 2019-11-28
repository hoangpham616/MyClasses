/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyLocalization (version 2.20)
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
        [SerializeField]
        private MyLocalizationManager.ELanguage[] mImageLanguages;
        [SerializeField]
        private GameObject[] mImageObjects;
        [SerializeField]
        private string[] mImageInvisibleTexts;

#if USE_MY_UI_TMPRO
        private TextMeshProUGUI mTextTMPro;
#endif

        private Text mText;
        private Color mColor;
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

        #region ----- MonoBehaviour Event -----

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
                    mColor = mText.color;
                }
#if USE_MY_UI_TMPRO
                else
                {
                    mTextTMPro = gameObject.GetComponent<TextMeshProUGUI>();
                    if (mTextTMPro != null)
                    {
                        mKey = mTextTMPro.text;
                        mColor = mTextTMPro.color;
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
#if USE_MY_UI_TMPRO
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
#endif

                return;
            }

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
                mText.color = mColor;
            }
#if USE_MY_UI_TMPRO
            else if (mTextTMPro != null)
            {
                mTextTMPro.text = text;
                mTextTMPro.color = mColor;
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
        private SerializedProperty mImageLanguages;
        private SerializedProperty mImageObjects;
        private SerializedProperty mImageInvisibleTexts;
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
            mImageLanguages = serializedObject.FindProperty("mImageLanguages");
            mImageObjects = serializedObject.FindProperty("mImageObjects");
            mImageInvisibleTexts = serializedObject.FindProperty("mImageInvisibleTexts");
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

            mIsImageLanguageVisible = EditorGUILayout.Foldout(mIsImageLanguageVisible, "Images");
            if (mIsImageLanguageVisible)
            {
                EditorGUI.indentLevel++;
                mImageLanguages.arraySize = EditorGUILayout.IntField("Size", mImageLanguages.arraySize);
                mImageObjects.arraySize = mImageLanguages.arraySize;
                mImageInvisibleTexts.arraySize = mImageLanguages.arraySize;
                for (int i = 0; i < mImageLanguages.arraySize; i++)
                {
                    SerializedProperty language = mImageLanguages.GetArrayElementAtIndex(i);
                    language.enumValueIndex = (int)(MyLocalizationManager.ELanguage)EditorGUILayout.EnumPopup("Language", (MyLocalizationManager.ELanguage)System.Enum.GetValues(typeof(MyLocalizationManager.ELanguage)).GetValue(language.enumValueIndex));

                    SerializedProperty image = mImageObjects.GetArrayElementAtIndex(i);
                    image.objectReferenceValue = (GameObject)EditorGUILayout.ObjectField("Image", image.objectReferenceValue, typeof(GameObject), true);

                    SerializedProperty invisibleText = mImageInvisibleTexts.GetArrayElementAtIndex(i);
                    invisibleText.stringValue = EditorGUILayout.TextField("Invisible Text", invisibleText.stringValue);
                }
                EditorGUI.indentLevel--;
            }
            mPrefix.stringValue = EditorGUILayout.TextField("Prefix", mPrefix.stringValue);
            mSuffix.stringValue = EditorGUILayout.TextField("Suffix", mSuffix.stringValue);
            mFormatText.enumValueIndex = (int)(MyLocalization.EFormatText)EditorGUILayout.EnumPopup("Format Text", (MyLocalization.EFormatText)System.Enum.GetValues(typeof(MyLocalization.EFormatText)).GetValue(mFormatText.enumValueIndex));

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}