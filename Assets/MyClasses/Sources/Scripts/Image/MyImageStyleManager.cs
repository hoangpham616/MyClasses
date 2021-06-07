/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyImageStyleManager (version 1.0)
 */

#pragma warning disable 0162
#pragma warning disable 0414

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;
using System.IO;

namespace MyClasses
{
    public class MyImageStyleManager : MonoBehaviour
    {
        #region ----- Variable -----

        public static string CONFIG_DIRECTORY = "Configs/";

        [SerializeField]
        private MyImageStyleConfig mConfig;
        [SerializeField]
        private bool mIsAutoSaveOnChange = true;

        #endregion

        #region ----- Property -----
        
        public MyImageStyleConfig Config
        {
            get { return mConfig; }
        }

        public MyImageStyleInfo[] Infos
        {
            get { return mConfig.Infos; }
        }

        #endregion

        #region ----- Singleton -----

        private static object mSingletonLock = new object();
        private static MyImageStyleManager mInstance;

        public static MyImageStyleManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mSingletonLock)
                    {
                        mInstance = (MyImageStyleManager)FindObjectOfType(typeof(MyImageStyleManager));
                        if (mInstance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyImageStyleManager).Name);
                            mInstance = obj.AddComponent<MyImageStyleManager>();
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
            GameObject obj = new GameObject(typeof(MyImageStyleManager).Name);
            MyImageStyleManager script = obj.AddComponent<MyImageStyleManager>();
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
            if (!Directory.Exists("Assets/Resources/" + MyImageStyleManager.CONFIG_DIRECTORY))
            {
                Directory.CreateDirectory("Assets/Resources/" + MyImageStyleManager.CONFIG_DIRECTORY);
            }
#endif

            string filePath = MyImageStyleManager.CONFIG_DIRECTORY + typeof(MyImageStyleConfig).Name + ".asset";
            mConfig = Resources.Load(filePath, typeof(MyImageStyleConfig)) as MyImageStyleConfig;
#if UNITY_EDITOR
            if (mConfig == null)
            {
                mConfig = ScriptableObject.CreateInstance<MyImageStyleConfig>();
                mConfig.Infos = new MyImageStyleInfo[(int)EStyle.Length];
                for (int i = 0; i < mConfig.Infos.Length; ++i)
                {
                    mConfig.Infos[i] = new MyImageStyleInfo();
                    mConfig.Infos[i].Style = (EStyle)i;
                    switch (mConfig.Infos[i].Style)
                    {
                        case EStyle.COLOR_AQUA:
                            mConfig.Infos[i].Color = new Color(0, 255 / 255f, 255 / 255f);
                            break;

                        case EStyle.COLOR_BLACK:
                            mConfig.Infos[i].Color = new Color(0, 0, 0);
                            break;

                        case EStyle.COLOR_BLUE:
                        case EStyle.CUSTOM_COLOR_BLUE:
                            mConfig.Infos[i].Color = new Color(0, 0, 255 / 255f);
                            break;

                        case EStyle.COLOR_BROWN:
                        case EStyle.CUSTOM_COLOR_BROWN:
                            mConfig.Infos[i].Color = new Color(165 / 255f, 42 / 255f, 42 / 255f);
                            break;

                        case EStyle.COLOR_FUCHSIA:
                            mConfig.Infos[i].Color = new Color(255 / 255f, 0, 255 / 255f);
                            break;

                        case EStyle.COLOR_GRAY:
                        case EStyle.CUSTOM_COLOR_GRAY:
                            mConfig.Infos[i].Color = new Color(128 / 255f, 128 / 255f, 128 / 255f);
                            break;

                        case EStyle.COLOR_GOLD:
                        case EStyle.CUSTOM_COLOR_GOLD:
                            mConfig.Infos[i].Color = new Color(255 / 255f, 215 / 255f, 0);
                            break;

                        case EStyle.COLOR_GREEN:
                        case EStyle.CUSTOM_COLOR_GREEN:
                            mConfig.Infos[i].Color = new Color(0, 128 / 255f, 0);
                            break;

                        case EStyle.COLOR_MAROON:
                            mConfig.Infos[i].Color = new Color(128 / 255f, 0, 0);
                            break;

                        case EStyle.COLOR_NAVY:
                            mConfig.Infos[i].Color = new Color(0, 0, 128 / 255f);
                            break;

                        case EStyle.COLOR_OLIVE:
                            mConfig.Infos[i].Color = new Color(128 / 255f, 128 / 255f, 0);
                            break;

                        case EStyle.COLOR_ORANGE:
                        case EStyle.CUSTOM_COLOR_ORANGE:
                            mConfig.Infos[i].Color = new Color(255 / 255f, 165 / 255f, 0);
                            break;

                        case EStyle.COLOR_PURPLE:
                        case EStyle.CUSTOM_COLOR_PURPLE:
                            mConfig.Infos[i].Color = new Color(128 / 255f, 0, 128 / 255f);
                            break;

                        case EStyle.COLOR_RED:
                        case EStyle.CUSTOM_COLOR_RED:
                            mConfig.Infos[i].Color = new Color(255 / 255f, 0, 0);
                            break;

                        case EStyle.COLOR_SILVER:
                        case EStyle.CUSTOM_COLOR_SILVER:
                            mConfig.Infos[i].Color = new Color(192 / 255f, 192 / 255f, 192 / 255f);
                            break;

                        case EStyle.COLOR_TEAL:
                            mConfig.Infos[i].Color = new Color(0, 128 / 255f, 128 / 255f);
                            break;

                        case EStyle.COLOR_YELLOW:
                        case EStyle.CUSTOM_COLOR_YELLOW:
                            mConfig.Infos[i].Color = new Color(255 / 255f, 255 / 255f, 0);
                            break;

                        case EStyle.COLOR_WHITE:
                            mConfig.Infos[i].Color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                            break;
                    }
                }
                AssetDatabase.CreateAsset(mConfig, filePath);
                AssetDatabase.SaveAssets();
            }
#endif
        }

        /// <summary>
        /// Return info by type.
        /// </summary>
        public MyImageStyleInfo GetInfo(EStyle type)
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
        public class MyImageStyleInfo
        {
            public EStyle Style = EStyle.UNDEFINED;
            public EImageType Type = EImageType.NO_OVERRIDE;
            public Sprite Image = null;
            public Material Material = null;
            public Color Color = Color.white;
            public Vector2 Size = Vector2.zero;
            public string Note = "";
        }

        #endregion

        #region ----- Enumeration -----

        public enum EStyle
        {
            UNDEFINED = 0,

            COLOR_AQUA,
            COLOR_BLACK,
            COLOR_BLUE,
            COLOR_BROWN,
            COLOR_FUCHSIA,
            COLOR_GOLD,
            COLOR_GRAY,
            COLOR_GREEN,
            COLOR_MAROON,
            COLOR_NAVY,
            COLOR_OLIVE,
            COLOR_ORANGE,
            COLOR_PURPLE,
            COLOR_RED,
            COLOR_SILVER,
            COLOR_TEAL,
            COLOR_YELLOW,
            COLOR_WHITE,

            CUSTOM_COLOR_RED,
            CUSTOM_COLOR_GREEN,
            CUSTOM_COLOR_BLUE,
            CUSTOM_COLOR_GOLD,
            CUSTOM_COLOR_SILVER,
            CUSTOM_COLOR_BROWN,
            CUSTOM_COLOR_YELLOW,
            CUSTOM_COLOR_ORANGE,
            CUSTOM_COLOR_PURPLE,
            CUSTOM_COLOR_GRAY,

            SCENE_BACKGROUND_1,
            SCENE_BACKGROUND_2,
            SCENE_BACKGROUND_3,
            SCENE_BACKGROUND_4,
            SCENE_BACKGROUND_5,

            SCENE_HEADER_1,
            SCENE_HEADER_2,
            SCENE_HEADER_3,
            SCENE_HEADER_4,
            SCENE_HEADER_5,

            SCENE_BUTTON_1,
            SCENE_BUTTON_2,
            SCENE_BUTTON_3,
            SCENE_BUTTON_4,
            SCENE_BUTTON_5,

            POPUP_BACKGROUND_1,
            POPUP_BACKGROUND_2,
            POPUP_BACKGROUND_3,
            POPUP_BACKGROUND_4,
            POPUP_BACKGROUND_5,

            POPUP_HEADER_1,
            POPUP_HEADER_2,
            POPUP_HEADER_3,
            POPUP_HEADER_4,
            POPUP_HEADER_5,

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

            Length
        }

        public enum EImageType
        {
            NO_OVERRIDE = 0,
            SIMPLE,
            SIMPLE_PRESERVE_ASPECT,
            SLICED,
            SLICED_FILL_CENTER
        }

        #endregion
        [SerializeField]
        private Color x = new Color(88, 188, 8);
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyImageStyleManager))]
    public class MyImageStyleManagerEditor : Editor
    {
        private MyImageStyleManager mScript;
        private SerializedProperty mConfig;
        private SerializedProperty mIsAutoSaveOnChange;
        private SerializedProperty x;
        private bool mIsColorsVisible = false;
        private bool mIsCustomColorsVisible = false;
        private bool mIsSceneBackgroundsVisible = false;
        private bool mIsSceneHeadersVisible = false;
        private bool mIsSceneButtonsVisible = false;
        private bool mIsPopupBackgroundsVisible = false;
        private bool mIsPopupHeadersVisible = false;
        private bool mIsPopupButtonsVisible = false;
        private bool mIsSmallButtonsVisible = false;
        private bool mIsMediumButtonsVisible = false;
        private bool mIsLargeButtonsVisible = false;
        private bool mIsCustomButtonsVisible = false;
        private bool mIsCustomsVisible = false;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyImageStyleManager)target;
            mConfig = serializedObject.FindProperty("mConfig");
            mIsAutoSaveOnChange = serializedObject.FindProperty("mIsAutoSaveOnChange");
            x = serializedObject.FindProperty("x");

            mScript.LoadConfig();
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyImageStyleManager), false);

            serializedObject.Update();

            EditorGUILayout.ObjectField(mConfig, new GUIContent("Config File"));

            EditorGUI.BeginChangeCheck();

            mIsAutoSaveOnChange.boolValue = EditorGUILayout.Toggle("Auto Save On Change", mIsAutoSaveOnChange.boolValue);

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Show All"))
            {
                mIsColorsVisible = true;
                mIsCustomColorsVisible = true;
                mIsSceneBackgroundsVisible = true;
                mIsSceneHeadersVisible = true;
                mIsSceneButtonsVisible = true;
                mIsPopupBackgroundsVisible = true;
                mIsPopupHeadersVisible = true;
                mIsPopupButtonsVisible = true;
                mIsSmallButtonsVisible = true;
                mIsMediumButtonsVisible = true;
                mIsLargeButtonsVisible = true;
                mIsCustomButtonsVisible = true;
                mIsCustomsVisible = true;
            }
            if (GUILayout.Button("Hide All"))
            {
                mIsColorsVisible = false;
                mIsCustomColorsVisible = false;
                mIsSceneBackgroundsVisible = false;
                mIsSceneHeadersVisible = false;
                mIsSceneButtonsVisible = false;
                mIsPopupBackgroundsVisible = false;
                mIsPopupHeadersVisible = false;
                mIsPopupButtonsVisible = false;
                mIsSmallButtonsVisible = false;
                mIsMediumButtonsVisible = false;
                mIsLargeButtonsVisible = false;
                mIsCustomButtonsVisible = false;
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
                switch ((MyImageStyleManager.EStyle)i)
                {
                    case MyImageStyleManager.EStyle.COLOR_AQUA:
                        {
                            EditorGUILayout.LabelField(string.Empty);
                            mIsColorsVisible = EditorGUILayout.Foldout(mIsColorsVisible, "Standard Colors");
                            if (mIsColorsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsColorsVisible;
                        }
                        break;
                    case MyImageStyleManager.EStyle.CUSTOM_COLOR_RED:
                        {
                            if (mIsColorsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsCustomColorsVisible = EditorGUILayout.Foldout(mIsCustomColorsVisible, "Custom Colors");
                            if (mIsCustomColorsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsCustomColorsVisible;
                        }
                        break;
                    case MyImageStyleManager.EStyle.SCENE_BACKGROUND_1:
                        {
                            if (mIsCustomColorsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsSceneBackgroundsVisible = EditorGUILayout.Foldout(mIsSceneBackgroundsVisible, "Scene Backgrounds");
                            if (mIsSceneBackgroundsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsSceneBackgroundsVisible;
                        }
                        break;
                    case MyImageStyleManager.EStyle.SCENE_HEADER_1:
                        {
                            if (mIsSceneBackgroundsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsSceneHeadersVisible = EditorGUILayout.Foldout(mIsSceneHeadersVisible, "Scene Headers");
                            if (mIsSceneHeadersVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsSceneHeadersVisible;
                        }
                        break;
                    case MyImageStyleManager.EStyle.SCENE_BUTTON_1:
                        {
                            if (mIsSceneHeadersVisible)
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
                    case MyImageStyleManager.EStyle.POPUP_BACKGROUND_1:
                        {
                            if (mIsSceneButtonsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsPopupBackgroundsVisible = EditorGUILayout.Foldout(mIsPopupBackgroundsVisible, "Popup Backgrounds");
                            if (mIsPopupBackgroundsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsPopupBackgroundsVisible;
                        }
                        break;
                    case MyImageStyleManager.EStyle.POPUP_HEADER_1:
                        {
                            if (mIsPopupBackgroundsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            mIsPopupHeadersVisible = EditorGUILayout.Foldout(mIsPopupHeadersVisible, "Popup Headers");
                            if (mIsPopupHeadersVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = mIsPopupHeadersVisible;
                        }
                        break;
                    case MyImageStyleManager.EStyle.POPUP_BUTTON_1:
                        {
                            if (mIsPopupHeadersVisible)
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
                    case MyImageStyleManager.EStyle.SMALL_BUTTON_1:
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
                    case MyImageStyleManager.EStyle.MEDIUM_BUTTON_1:
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
                    case MyImageStyleManager.EStyle.LARGE_BUTTON_1:
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
                    case MyImageStyleManager.EStyle.CUSTOM_BUTTON_1:
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
                    case MyImageStyleManager.EStyle.CUSTOM_1:
                        {
                            if (mIsCustomButtonsVisible)
                            {
                                EditorGUI.indentLevel--;
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
                    EditorGUILayout.LabelField(mScript.Infos[i].Style.ToString(), EditorStyles.foldoutHeader);
                    mScript.Infos[i].Type = (MyImageStyleManager.EImageType)EditorGUILayout.EnumPopup("Type", mScript.Infos[i].Type);
                    mScript.Infos[i].Image = (Sprite)EditorGUILayout.ObjectField("Image", mScript.Infos[i].Image, typeof(Sprite), false);
                    mScript.Infos[i].Material = (Material)EditorGUILayout.ObjectField("Material", mScript.Infos[i].Material, typeof(Material), false);
                    mScript.Infos[i].Color = EditorGUILayout.ColorField("Color", mScript.Infos[i].Color);
                    mScript.Infos[i].Size = EditorGUILayout.Vector2Field("Size", mScript.Infos[i].Size);
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
                mIsColorsVisible = true;
                mIsCustomColorsVisible = true;
                mIsSceneBackgroundsVisible = true;
                mIsSceneHeadersVisible = true;
                mIsSceneButtonsVisible = true;
                mIsPopupBackgroundsVisible = true;
                mIsPopupHeadersVisible = true;
                mIsPopupButtonsVisible = true;
                mIsSmallButtonsVisible = true;
                mIsMediumButtonsVisible = true;
                mIsLargeButtonsVisible = true;
                mIsCustomButtonsVisible = true;
                mIsCustomsVisible = true;
            }
            if (GUILayout.Button("Hide All"))
            {
                mIsColorsVisible = false;
                mIsCustomColorsVisible = false;
                mIsSceneBackgroundsVisible = false;
                mIsSceneHeadersVisible = false;
                mIsSceneButtonsVisible = false;
                mIsPopupBackgroundsVisible = false;
                mIsPopupHeadersVisible = false;
                mIsPopupButtonsVisible = false;
                mIsSmallButtonsVisible = false;
                mIsMediumButtonsVisible = false;
                mIsLargeButtonsVisible = false;
                mIsCustomButtonsVisible = false;
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