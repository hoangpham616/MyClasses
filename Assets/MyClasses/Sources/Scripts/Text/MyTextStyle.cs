/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyTextStyle (version 1.0)
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
    public class MyTextStyle : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private MyTextStyleManager.EStyle mStyle = MyTextStyleManager.EStyle.UNDEFINED;

#if USE_MY_UI_TMPRO
        private TextMeshProUGUI mTextTMPro;
#endif

        private Text mText;

        #endregion

        #region ----- Property -----

        public MyTextStyleManager.EStyle Style
        {
            get { return mStyle; }
            set
            {
                mStyle = value;
                Refresh();
            }
        }

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// Start.
        /// </summary>
        void Start()
        {
            Refresh();
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Refresh.
        /// </summary>
        public void Refresh()
        {
            MyTextStyleManager.MyTextStyleInfo info = MyTextStyleManager.Instance.GetInfo(mStyle);
            if (info == null)
            {
                return;
            }

            if (mText == null)
            {
                mText = gameObject.GetComponent<Text>();
#if USE_MY_UI_TMPRO
                if (mText == null)
                {
                    mTextTMPro = gameObject.GetComponent<TextMeshProUGUI>();
                }
#endif
            }

            if (mText != null)
            {
                if (info.Font != null)
                {
                    mText.font = info.Font;
                }
                mText.fontSize = info.FontSizeMin;
                mText.color = info.Color;
            }
#if USE_MY_UI_TMPRO
            else if (mTextTMPro != null)
            {
                if (info.TMPFontAsset != null)
                {
                    mTextTMPro.font = info.TMPFontAsset;
                }
                mTextTMPro.fontSize = info.FontSizeMin;
                mTextTMPro.fontSizeMin = info.FontSizeMin;
                mTextTMPro.fontSizeMax = info.FontSizeMax;
                mTextTMPro.enableAutoSizing = info.FontSizeMin < info.FontSizeMax;
                mTextTMPro.color = info.Color;
            }
#endif
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyTextStyle)), CanEditMultipleObjects]
    public class MyTextEditor : Editor
    {
        private MyTextStyle mScript;
        private SerializedProperty mStyle;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyTextStyle)target;
            mStyle = serializedObject.FindProperty("mStyle");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyTextStyle), false);

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            mStyle.enumValueIndex = (int)(MyTextStyleManager.EStyle)EditorGUILayout.EnumPopup("Style", (MyTextStyleManager.EStyle)mStyle.enumValueIndex);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                mScript.Refresh();
            }

            MyTextStyleManager.MyTextStyleInfo info = MyTextStyleManager.Instance.GetInfo((MyTextStyleManager.EStyle)mStyle.enumValueIndex);
            EditorGUILayout.LabelField("Your Note", info != null ? info.Note : string.Empty);
        }
    }

#endif
}