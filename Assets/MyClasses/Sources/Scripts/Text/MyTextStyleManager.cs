/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyTextStyleManager (version 1.0)
 */

#pragma warning disable 0162
#pragma warning disable 0414

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;
using System.IO;
using TMPro;

namespace MyClasses
{
    public class MyTextStyleManager : MonoBehaviour
    {
        #region ----- Variable -----

        public static string CONFIG_DIRECTORY = "Configs/";

        [SerializeField]
        private MyTextStyleConfig mConfig;
        [SerializeField]
        private bool mIsAutoSaveOnChange = true;

        #endregion

        #region ----- Property -----

        public MyTextStyleConfig Config
        {
            get { return mConfig; }
        }

        public MyTextStyleInfo[] Infos
        {
            get { return mConfig.Infos; }
        }

        #endregion

        #region ----- Singleton -----

        private static object mSingletonLock = new object();
        private static MyTextStyleManager mInstance;

        public static MyTextStyleManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mSingletonLock)
                    {
                        mInstance = (MyTextStyleManager)FindObjectOfType(typeof(MyTextStyleManager));
                        if (mInstance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyTextStyleManager).Name);
                            mInstance = obj.AddComponent<MyTextStyleManager>();
                            if (Application.isPlaying)
                            {
                                DontDestroyOnLoad(obj);
                            }
                        }
                        else if (Application.isPlaying)
                        {
                            DontDestroyOnLoad(mInstance);
                        }
                        mInstance.LoadConfig();
                    }
                }
                return mInstance;
            }
        }

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            LoadConfig();
        }

        #endregion

        #region ----- Public Method -----

#if UNITY_EDITOR

        /// <summary>
        /// Create a template.
        /// </summary>
        public static void CreateTemplate()
        {
            GameObject obj = new GameObject(typeof(MyTextStyleManager).Name);
            MyTextStyleManager script = obj.AddComponent<MyTextStyleManager>();
            script.LoadConfig();

            EditorGUIUtility.PingObject(obj);
            Selection.activeGameObject = obj.gameObject;
        }

        /// <summary>
        /// Save config to asset file.
        /// </summary>
        public void SaveConfig()
        {
            EditorUtility.SetDirty(mConfig);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

#endif

        /// <summary>
        /// Load config from asset file.
        /// </summary>
        public void LoadConfig()
        {
            if (mConfig != null)
            {
                return;
            }

#if UNITY_EDITOR
            if (!Directory.Exists("Assets/Resources/" + MyTextStyleManager.CONFIG_DIRECTORY))
            {
                Directory.CreateDirectory("Assets/Resources/" + MyTextStyleManager.CONFIG_DIRECTORY);
            }
#endif

            string filePath = MyTextStyleManager.CONFIG_DIRECTORY + typeof(MyTextStyleConfig).Name + ".asset";
            mConfig = Resources.Load(filePath, typeof(MyTextStyleConfig)) as MyTextStyleConfig;
#if UNITY_EDITOR
            if (mConfig == null)
            {
                mConfig = ScriptableObject.CreateInstance<MyTextStyleConfig>();
                mConfig.Infos = new MyTextStyleInfo[(int)EStyle.Length];
                for (int i = 0; i < mConfig.Infos.Length; ++i)
                {
                    mConfig.Infos[i] = new MyTextStyleInfo();
                    mConfig.Infos[i].Type = (EStyle)i;
                }
                AssetDatabase.CreateAsset(mConfig, filePath);
                AssetDatabase.SaveAssets();
            }
#endif
        }

        /// <summary>
        /// Return info by type.
        /// </summary>
        public MyTextStyleInfo GetInfo(EStyle type)
        {
            if (type <= EStyle.UNDEFINED || EStyle.Length <= type)
            {
                return null;
            }
            return mConfig.Infos[(int)type];
        }

        #endregion

        #region ----- Internal Class -----

        [Serializable]
        public class MyTextStyleInfo
        {
            public EStyle Type = EStyle.UNDEFINED;
            public Font Font = null;
            public TMP_FontAsset TMPFontAsset = null;
            public int FontSizeMin = 0;
            public int FontSizeMax = 0;
            public Color Color = Color.white;
            public string Note = "";
        }

        #endregion

        #region ----- Enumeration -----

        public enum EStyle
        {
            UNDEFINED = 0,

            SCENE_TITLE_1,
            SCENE_TITLE_2,
            SCENE_TITLE_3,
            SCENE_TITLE_4,
            SCENE_TITLE_5,

            SCENE_BUTTON_1,
            SCENE_BUTTON_2,
            SCENE_BUTTON_3,
            SCENE_BUTTON_4,
            SCENE_BUTTON_5,

            POPUP_TITLE_1,
            POPUP_TITLE_2,
            POPUP_TITLE_3,
            POPUP_TITLE_4,
            POPUP_TITLE_5,

            POPUP_BUTTON_1,
            POPUP_BUTTON_2,
            POPUP_BUTTON_3,
            POPUP_BUTTON_4,
            POPUP_BUTTON_5,

            SMALL_BUTTON_1,
            SMALL_BUTTON_2,
            SMALL_BUTTON_3,
            SMALL_BUTTON_4,
            SMALL_BUTTON_5,

            MEDIUM_BUTTON_1,
            MEDIUM_BUTTON_2,
            MEDIUM_BUTTON_3,
            MEDIUM_BUTTON_4,
            MEDIUM_BUTTON_5,

            LARGE_BUTTON_1,
            LARGE_BUTTON_2,
            LARGE_BUTTON_3,
            LARGE_BUTTON_4,
            LARGE_BUTTON_5,

            CUSTOM_BUTTON_1,
            CUSTOM_BUTTON_2,
            CUSTOM_BUTTON_3,
            CUSTOM_BUTTON_4,
            CUSTOM_BUTTON_5,
            CUSTOM_BUTTON_6,
            CUSTOM_BUTTON_7,
            CUSTOM_BUTTON_8,
            CUSTOM_BUTTON_9,
            CUSTOM_BUTTON_10,

            MONEY_TEXT_1,
            MONEY_TEXT_2,
            MONEY_TEXT_3,
            MONEY_TEXT_4,
            MONEY_TEXT_5,

            TINY_TEXT_1,
            TINY_TEXT_2,
            TINY_TEXT_3,
            TINY_TEXT_4,

            SMALL_TEXT_1,
            SMALL_TEXT_2,
            SMALL_TEXT_3,
            SMALL_TEXT_4,

            MEDIUM_TEXT_1,
            MEDIUM_TEXT_2,
            MEDIUM_TEXT_3,
            MEDIUM_TEXT_4,

            LARGE_TEXT_1,
            LARGE_TEXT_2,
            LARGE_TEXT_3,
            LARGE_TEXT_4,

            HUGE_TEXT_1,
            HUGE_TEXT_2,
            HUGE_TEXT_3,
            HUGE_TEXT_4,

            GIGANTIC_TEXT_1,
            GIGANTIC_TEXT_2,
            GIGANTIC_TEXT_3,
            GIGANTIC_TEXT_4,

            CUSTOM_1,
            CUSTOM_2,
            CUSTOM_3,
            CUSTOM_4,
            CUSTOM_5,
            CUSTOM_6,
            CUSTOM_7,
            CUSTOM_8,
            CUSTOM_9,
            CUSTOM_10,
            CUSTOM_11,
            CUSTOM_12,
            CUSTOM_13,
            CUSTOM_14,
            CUSTOM_15,
            CUSTOM_16,
            CUSTOM_17,
            CUSTOM_18,
            CUSTOM_19,
            CUSTOM_20,

            Length,
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyTextStyleManager))]
    public class MyTextStyleManagerEditor : Editor
    {
        private MyTextStyleManager mScript;
        private SerializedProperty mConfig;
        private SerializedProperty mIsAutoSaveOnChange;
        private bool mIsSceneTitlesVisible = false;
        private bool mIsSceneButtonsVisible = false;
        private bool mIsPopupTitlesVisible = false;
        private bool mIsPopupButtonsVisible = false;
        private bool mIsSmallButtonsVisible = false;
        private bool mIsMediumButtonsVisible = false;
        private bool mIsLargeButtonsVisible = false;
        private bool mIsCustomButtonsVisible = false;
        private bool mIsMoneyTextsVisible = false;
        private bool mIsTinyTextsVisible = false;
        private bool mIsSmallTextsVisible = false;
        private bool mIsMediumTextsVisible = false;
        private bool mIsLargeTextsVisible = false;
        private bool mIsHugeTextsVisible = false;
        private bool mIsGiganticTextsVisible = false;
        private bool mIsCustomsVisible = false;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyTextStyleManager)target;
            mConfig = serializedObject.FindProperty("mConfig");
            mIsAutoSaveOnChange = serializedObject.FindProperty("mIsAutoSaveOnChange");

            mScript.LoadConfig();
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyTextStyleManager), false);

            serializedObject.Update();

            EditorGUILayout.ObjectField(mConfig, new GUIContent("Config File"));

            EditorGUI.BeginChangeCheck();

            mIsAutoSaveOnChange.boolValue = EditorGUILayout.Toggle("Auto Save On Change", mIsAutoSaveOnChange.boolValue);

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Show All"))
            {
                mIsSceneTitlesVisible = true;
                mIsSceneButtonsVisible = true;
                mIsPopupTitlesVisible = true;
                mIsPopupButtonsVisible = true;
                mIsSmallButtonsVisible = true;
                mIsMediumButtonsVisible = true;
                mIsLargeButtonsVisible = true;
                mIsCustomButtonsVisible = true;
                mIsMoneyTextsVisible = true;
                mIsTinyTextsVisible = true;
                mIsSmallTextsVisible = true;
                mIsMediumTextsVisible = true;
                mIsLargeTextsVisible = true;
                mIsHugeTextsVisible = true;
                mIsGiganticTextsVisible = true;
                mIsCustomsVisible = true;
            }
            if (GUILayout.Button("Hide All"))
            {
                mIsSceneTitlesVisible = false;
                mIsSceneButtonsVisible = false;
                mIsPopupTitlesVisible = false;
                mIsPopupButtonsVisible = false;
                mIsSmallButtonsVisible = false;
                mIsMediumButtonsVisible = false;
                mIsLargeButtonsVisible = false;
                mIsCustomButtonsVisible = false;
                mIsMoneyTextsVisible = false;
                mIsTinyTextsVisible = false;
                mIsSmallTextsVisible = false;
                mIsMediumTextsVisible = false;
                mIsLargeTextsVisible = false;
                mIsHugeTextsVisible = false;
                mIsGiganticTextsVisible = false;
                mIsCustomsVisible = false;
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Save"))
            {
                mScript.SaveConfig();
            }

            bool isVisible = false;
            for (int i = 1; i < mScript.Infos.Length; ++i)
            {
                switch ((MyTextStyleManager.EStyle)i)
                {
                    case MyTextStyleManager.EStyle.SCENE_TITLE_1:
                        {
                            EditorGUILayout.LabelField(string.Empty);
                            mIsSceneTitlesVisible = EditorGUILayout.Foldout(mIsSceneTitlesVisible, "Scene Titles");
                            if (mIsSceneTitlesVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsSceneTitlesVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.SCENE_BUTTON_1:
                        {
                            if (mIsSceneTitlesVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsSceneButtonsVisible = EditorGUILayout.Foldout(mIsSceneButtonsVisible, "Scene Buttons");
                            if (mIsSceneButtonsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsSceneButtonsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.POPUP_TITLE_1:
                        {
                            if (mIsSceneButtonsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsPopupTitlesVisible = EditorGUILayout.Foldout(mIsPopupTitlesVisible, "Popup Titles");
                            if (mIsPopupTitlesVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsPopupTitlesVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.POPUP_BUTTON_1:
                        {
                            if (mIsPopupTitlesVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsPopupButtonsVisible = EditorGUILayout.Foldout(mIsPopupButtonsVisible, "Popup Buttons");
                            if (mIsPopupButtonsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsPopupButtonsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.SMALL_BUTTON_1:
                        {
                            if (mIsPopupButtonsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsSmallButtonsVisible = EditorGUILayout.Foldout(mIsSmallButtonsVisible, "Small Buttons");
                            if (mIsSmallButtonsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsSmallButtonsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.MEDIUM_BUTTON_1:
                        {
                            if (mIsSmallButtonsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsMediumButtonsVisible = EditorGUILayout.Foldout(mIsMediumButtonsVisible, "Medium Buttons");
                            if (mIsMediumButtonsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsMediumButtonsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.LARGE_BUTTON_1:
                        {
                            if (mIsMediumButtonsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsLargeButtonsVisible = EditorGUILayout.Foldout(mIsLargeButtonsVisible, "Large Buttons");
                            if (mIsLargeButtonsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsLargeButtonsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.CUSTOM_BUTTON_1:
                        {
                            if (mIsLargeButtonsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsCustomButtonsVisible = EditorGUILayout.Foldout(mIsCustomButtonsVisible, "Custom Buttons");
                            if (mIsCustomButtonsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsCustomButtonsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.MONEY_TEXT_1:
                        {
                            if (mIsPopupButtonsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsMoneyTextsVisible = EditorGUILayout.Foldout(mIsMoneyTextsVisible, "Money Texts");
                            if (mIsMoneyTextsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsMoneyTextsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.TINY_TEXT_1:
                        {
                            if (mIsMoneyTextsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsTinyTextsVisible = EditorGUILayout.Foldout(mIsTinyTextsVisible, "Tiny Texts");
                            if (mIsTinyTextsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsTinyTextsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.SMALL_TEXT_1:
                        {
                            if (mIsTinyTextsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsSmallTextsVisible = EditorGUILayout.Foldout(mIsSmallTextsVisible, "Small Texts");
                            if (mIsSmallTextsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsSmallTextsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.MEDIUM_TEXT_1:
                        {
                            if (mIsSmallTextsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsMediumTextsVisible = EditorGUILayout.Foldout(mIsMediumTextsVisible, "Medium Texts");
                            if (mIsMediumTextsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsMediumTextsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.LARGE_TEXT_1:
                        {
                            if (mIsMediumTextsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsLargeTextsVisible = EditorGUILayout.Foldout(mIsLargeTextsVisible, "Large Texts");
                            if (mIsLargeTextsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsLargeTextsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.HUGE_TEXT_1:
                        {
                            if (mIsLargeTextsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsHugeTextsVisible = EditorGUILayout.Foldout(mIsHugeTextsVisible, "Huge Texts");
                            if (mIsHugeTextsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsHugeTextsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.GIGANTIC_TEXT_1:
                        {
                            if (mIsHugeTextsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsGiganticTextsVisible = EditorGUILayout.Foldout(mIsGiganticTextsVisible, "Gigantic Texts");
                            if (mIsGiganticTextsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsGiganticTextsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.CUSTOM_1:
                        {
                            if (mIsGiganticTextsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsCustomsVisible = EditorGUILayout.Foldout(mIsCustomsVisible, "Customs");
                            if (mIsCustomsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsCustomsVisible;
                        }
                        break;
                }
                if (isVisible)
                {
                    EditorGUILayout.LabelField(mScript.Infos[i].Type.ToString(), EditorStyles.foldoutHeader);
                    mScript.Infos[i].Font = (Font)EditorGUILayout.ObjectField("Font For Text", mScript.Infos[i].Font, typeof(Font), true);
                    mScript.Infos[i].TMPFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("Font For TMPro", mScript.Infos[i].TMPFontAsset, typeof(TMP_FontAsset), true);
                    mScript.Infos[i].FontSizeMin = EditorGUILayout.IntField("Min Font Size", mScript.Infos[i].FontSizeMin);
                    mScript.Infos[i].FontSizeMax = EditorGUILayout.IntField("Max Font Size", mScript.Infos[i].FontSizeMax);
                    mScript.Infos[i].Color = EditorGUILayout.ColorField("Color", mScript.Infos[i].Color);
                    mScript.Infos[i].Note = EditorGUILayout.TextField("Your Note", mScript.Infos[i].Note);
                    EditorGUILayout.LabelField(string.Empty);
                }
                if (i == mScript.Infos.Length - 1)
                {
                    if (mIsCustomsVisible)
                    {
                        EditorGUI.indentLevel--;
                    }
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if (mIsAutoSaveOnChange.boolValue)
                {
                    mScript.SaveConfig();
                }
            }

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Show All"))
            {
                mIsSceneTitlesVisible = true;
                mIsSceneButtonsVisible = true;
                mIsPopupTitlesVisible = true;
                mIsPopupButtonsVisible = true;
                mIsSmallButtonsVisible = true;
                mIsMediumButtonsVisible = true;
                mIsLargeButtonsVisible = true;
                mIsCustomButtonsVisible = true;
                mIsMoneyTextsVisible = true;
                mIsTinyTextsVisible = true;
                mIsSmallTextsVisible = true;
                mIsMediumTextsVisible = true;
                mIsLargeTextsVisible = true;
                mIsHugeTextsVisible = true;
                mIsGiganticTextsVisible = true;
                mIsCustomsVisible = true;
            }
            if (GUILayout.Button("Hide All"))
            {
                mIsSceneTitlesVisible = false;
                mIsSceneButtonsVisible = false;
                mIsPopupTitlesVisible = false;
                mIsPopupButtonsVisible = false;
                mIsSmallButtonsVisible = false;
                mIsMediumButtonsVisible = false;
                mIsLargeButtonsVisible = false;
                mIsCustomButtonsVisible = false;
                mIsMoneyTextsVisible = false;
                mIsTinyTextsVisible = false;
                mIsSmallTextsVisible = false;
                mIsMediumTextsVisible = false;
                mIsLargeTextsVisible = false;
                mIsHugeTextsVisible = false;
                mIsGiganticTextsVisible = false;
                mIsCustomsVisible = false;
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Save"))
            {
                mScript.SaveConfig();
            }
        }
    }

#endif
}