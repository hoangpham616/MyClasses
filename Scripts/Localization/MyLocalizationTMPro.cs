/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyLocalizationTMPro (version 2.11)
 */

#if USE_MY_UI_TMPRO
using UnityEngine;
using TMPro;

namespace MyClasses
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class MyLocalizationTMPro : MyLocalization
    {
        #region ----- Variable -----
        
        private TextMeshProUGUI mTextTMPro;

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
            if (mTextTMPro == null)
            {
                mTextTMPro = gameObject.GetComponent<TextMeshProUGUI>();
                mKey = mTextTMPro.text;
                mIsHasFix = !string.IsNullOrEmpty(mPrefix) || !string.IsNullOrEmpty(mSuffix);
            }

            Localize();
        }

        /// <summary>
        /// OnDisable.
        /// </summary>
        void OnDisable()
        {
            mTextTMPro.text = mKey;
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Localize.
        /// </summary>
        public override void Localize()
        {
            if (mIsHasFix)
            {
                mTextTMPro.text = mPrefix + MyLocalizationManager.Instance.LoadKey(mKey) + mSuffix;
            }
            else
            {
                mTextTMPro.text = MyLocalizationManager.Instance.LoadKey(mKey);
            }
        }

        #endregion
    }

    //#if UNITY_EDITOR

    //    [CustomEditor(typeof(MyLocalizationTMPro))]
    //    public class MyLocalizationTMProEditor : Editor
    //    {
    //        private MyLocalization mScript;
    //        private SerializedProperty mPrefix;
    //        private SerializedProperty mSuffix;

    //        /// <summary>
    //        /// OnEnable.
    //        /// </summary>
    //        void OnEnable()
    //        {
    //            mScript = (MyLocalizationTMPro)target;
    //            mPrefix = serializedObject.FindProperty("mPrefix");
    //            mSuffix = serializedObject.FindProperty("mSuffix");
    //        }

    //        /// <summary>
    //        /// OnInspectorGUI.
    //        /// </summary>
    //        public override void OnInspectorGUI()
    //        {
    //            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyLocalizationTMPro), false);

    //            serializedObject.Update();

    //            mPrefix.stringValue = EditorGUILayout.TextField("Prefix", mPrefix.stringValue);
    //            mSuffix.stringValue = EditorGUILayout.TextField("Suffix", mSuffix.stringValue);

    //            serializedObject.ApplyModifiedProperties();
    //        }
    //    }

    //#endif
}
#endif