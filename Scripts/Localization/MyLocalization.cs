/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyLocalization (version 2.11)
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;

namespace MyClasses
{
#if !USE_MY_UI_TMPRO
    [RequireComponent(typeof(Text))]
#endif
    public class MyLocalization : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        protected string mPrefix = string.Empty;
        [SerializeField]
        protected string mSuffix = string.Empty;

        private Text mText;
        protected string mKey;
        protected bool mIsHasFix;

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
                mKey = mText.text;
                mIsHasFix = !string.IsNullOrEmpty(mPrefix) || !string.IsNullOrEmpty(mSuffix);
            }

            Localize();
        }

        /// <summary>
        /// OnDisable.
        /// </summary>
        void OnDisable()
        {
            mText.text = mKey;
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
        public virtual void Localize()
        {
            if (mIsHasFix)
            {
                mText.text = mPrefix + MyLocalizationManager.Instance.LoadKey(mKey) + mSuffix;
            }
            else
            {
                mText.text = MyLocalizationManager.Instance.LoadKey(mKey);
            }
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

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyLocalization)target;
            mPrefix = serializedObject.FindProperty("mPrefix");
            mSuffix = serializedObject.FindProperty("mSuffix");
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

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}