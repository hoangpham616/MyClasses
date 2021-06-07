/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyImageStyle (version 1.0)
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;

namespace MyClasses
{
    [RequireComponent(typeof(Image))]
    public class MyImageStyle : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private MyImageStyleManager.EStyle mStyle = MyImageStyleManager.EStyle.UNDEFINED;
        [SerializeField]
        private MyImageStyleManager.MyImageStyleInfo mInfo;

        private Image mImage;
        private RectTransform mRectTransform;

        #endregion

        #region ----- Property -----

        public MyImageStyleManager.EStyle Style
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
            MyImageStyleManager.MyImageStyleInfo info = MyImageStyleManager.Instance.GetInfo(mStyle);
            if (info == null)
            {
                return;
            }

            if (mImage == null)
            {
                mImage = gameObject.GetComponent<Image>();
            }

            if (info.Image != null)
            {
                mImage.sprite = info.Image;
            }

            if (info.Material != null)
            {
                mImage.material = info.Material;
            }
            
            mImage.color = info.Color;

            if (info.Size.x > 0 && info.Size.y > 0)
            {
                if (mRectTransform == null)
                {
                    mRectTransform = gameObject.GetComponent<RectTransform>();
                }
                if (mRectTransform != null)
                {
                    mRectTransform.sizeDelta = info.Size;
                }
            }

            switch (info.Type)
            {
                case MyImageStyleManager.EImageType.SIMPLE:
                    {
                        mImage.type = Image.Type.Simple;
                        mImage.preserveAspect = false;
                    }
                    break;
                case MyImageStyleManager.EImageType.SIMPLE_PRESERVE_ASPECT:
                    {
                        mImage.type = Image.Type.Simple;
                        mImage.preserveAspect = true;
                    }
                    break;
                case MyImageStyleManager.EImageType.SLICED:
                    {
                        mImage.type = Image.Type.Sliced;
                        mImage.fillCenter = false;
                    }
                    break;
                case MyImageStyleManager.EImageType.SLICED_FILL_CENTER:
                    {
                        mImage.type = Image.Type.Sliced;
                        mImage.fillCenter = true;
                    }
                    break;
            }
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyImageStyle)), CanEditMultipleObjects]
    public class MyImageEditor : Editor
    {
        private MyImageStyle mScript;
        private SerializedProperty mStyle;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyImageStyle)target;
            mStyle = serializedObject.FindProperty("mStyle");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyImageStyle), false);

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            mStyle.enumValueIndex = (int)(MyImageStyleManager.EStyle)EditorGUILayout.EnumPopup("Style", (MyImageStyleManager.EStyle)mStyle.enumValueIndex);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                mScript.Refresh();
            }

            MyImageStyleManager.MyImageStyleInfo info = MyImageStyleManager.Instance.GetInfo((MyImageStyleManager.EStyle)mStyle.enumValueIndex);
            EditorGUILayout.LabelField("Your Note", info != null ? info.Note : string.Empty);
        }
    }

#endif
}