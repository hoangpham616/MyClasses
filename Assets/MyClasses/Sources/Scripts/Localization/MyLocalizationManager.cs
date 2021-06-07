/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyLocalizationManager (version 3.3)
 */

#pragma warning disable 0162
#pragma warning disable 0414

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyClasses
{
    public class MyLocalizationManager : MonoBehaviour
    {
        #region ----- Variable -----

#if USE_MY_LOCALIZATION_ARABIC
        public readonly string[] ARABIC_SYMBOLS = new string[] { ".", "؟", "(" };
#endif

        public static string CONFIG_DIRECTORY = "Configs/";

        [SerializeField]
        private MyLocalizationConfig mConfig;
        [SerializeField]
        private bool mIsAutoSaveOnChange = true;

        private ELanguage mLanguageType = ELanguage.None;
        private string[] mLanguageKeys;
        private int mLanguageIndex;
        private Dictionary<string, string[]> mDictionary = new Dictionary<string, string[]>();
        private List<MyLocalization> mListLocalization = new List<MyLocalization>();

        #endregion

        #region ----- Property -----

        public MyLocalizationConfig Config
        {
            get { return mConfig; }
        }

        public ELanguage Language
        {
            get
            {
                if (mLanguageType == ELanguage.None)
                {
                    switch (Config.Mode)
                    {
                        case EMode.CACHE_ONLY:
                            {
                                _LoadLanguageFromCache();
                            }
                            break;

                        case EMode.DEVICE_LANGUAGE_ONLY:
                            {
                                _LoadLanguageBasedOnDevice();
                            }
                            break;

                        case EMode.DEVICE_LANGUAGE_AND_CACHE:
                            {
                                _LoadLanguagBasedOnDeviceAndCache();
                            }
                            break;
                    }
                }
                return mLanguageType;
            }
            set
            {
                mLanguageType = value;

                PlayerPrefs.SetInt("MyLocalizationManager_Language", (int)mLanguageType);
                PlayerPrefs.Save();
            }
        }

        #endregion

        #region ----- Singleton -----

        private static object mSingletonLock = new object();
        private static MyLocalizationManager mInstance;

        public static MyLocalizationManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mSingletonLock)
                    {
                        mInstance = (MyLocalizationManager)FindObjectOfType(typeof(MyLocalizationManager));
                        if (mInstance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyLocalizationManager).Name);
                            mInstance = obj.AddComponent<MyLocalizationManager>();
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

        #region ----- Public Method -----

#if UNITY_EDITOR

        /// <summary>
        /// Create a template.
        /// </summary>
        public static void CreateTemplate()
        {
            GameObject obj = new GameObject(typeof(MyLocalizationManager).Name);
            MyLocalizationManager script = obj.AddComponent<MyLocalizationManager>();
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
            if (!Directory.Exists("Assets/Resources/" + MyLocalizationManager.CONFIG_DIRECTORY))
            {
                Directory.CreateDirectory("Assets/Resources/" + MyLocalizationManager.CONFIG_DIRECTORY);
            }
#endif

            string filePath = MyLocalizationManager.CONFIG_DIRECTORY + typeof(MyLocalizationConfig).Name + ".asset";
            mConfig = Resources.Load(filePath, typeof(MyLocalizationConfig)) as MyLocalizationConfig;
#if UNITY_EDITOR
            if (mConfig == null)
            {
                mConfig = ScriptableObject.CreateInstance<MyLocalizationConfig>();
                AssetDatabase.CreateAsset(mConfig, "Assets/Resources/" + filePath);
                AssetDatabase.SaveAssets();
            }
#endif
        }

        /// <summary>
        /// Load language.
        /// </summary>
        public void LoadLanguage(ELanguage language, bool isForce = false)
        {
            Language = language;

            if (isForce)
            {
                mLanguageKeys = null;
            }

            if (mLanguageKeys == null)
            {
                _LoadLocalization();
            }

#if USE_MY_LOCALIZATION_KHMER
            MyFontKhmerConverter.Initialize();
#endif

            for (int i = 0; i < mLanguageKeys.Length; i++)
            {
                if (mLanguageKeys[i].Equals(Language.ToString()))
                {
                    mLanguageIndex = i;
                    break;
                }
            }
        }

        /// <summary>
        /// Load text by key.
        /// </summary>
        public string LoadKey(string key)
        {
            if (mLanguageType == ELanguage.None)
            {
                LoadLanguage(Language);
            }

            if (mDictionary.ContainsKey(key))
            {
#if USE_MY_LOCALIZATION_ARABIC
                // please import "Arabic Support" package
                if (mLanguageType == ELanguage.Arabic)
                {
                    string value = mDictionary[key][mLanguageIndex];
                    string arabic = ArabicSupport.ArabicFixer.Fix(value, false, false);
                    for (int i = 0; i < 10; ++i)
                    {
                        string format = "{" + i + "}";
                        if (value.Contains(format))
                        {
                            arabic = arabic.Replace("}{" + i, format);
                            for (int j = 0; j < ARABIC_SYMBOLS.Length; ++j)
                            {
                                arabic = arabic.Replace(format + ARABIC_SYMBOLS[j], ARABIC_SYMBOLS[j] + format);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    return arabic;
                }
#endif
#if USE_MY_LOCALIZATION_KHMER
                if (mLanguageType == ELanguage.Unknown)
                {
                    return MyFontKhmerConverter.Convert(mDictionary[key][mLanguageIndex]);
                }
#endif
                return mDictionary[key][mLanguageIndex];
            }

            Debug.LogWarning("[" + typeof(MyLocalizationManager).Name + "] LoadKey(): Key \"" + key + "\" missing or null");

            return key;
        }

        /// <summary>
        /// Register an object which uses localization.
        /// </summary>
        public void Register(MyLocalization localization)
        {
            mListLocalization.Add(localization);
        }

        /// <summary>
        /// Unregister an object which uses localization.
        /// </summary>
        public void Unregister(MyLocalization localization)
        {
            mListLocalization.Remove(localization);
        }

        /// <summary>
        /// Localize all objects which active in hierarchy.
        /// </summary>
        public void Refresh()
        {
            for (int i = mListLocalization.Count - 1; i >= 0; i--)
            {
                if (mListLocalization[i] == null)
                {
                    mListLocalization.RemoveAt(i);
                }
                else if (mListLocalization[i].gameObject.activeInHierarchy)
                {
                    mListLocalization[i].Localize();
                }
            }
        }

        /// <summary>
        /// Localize all keys in string.
        /// </summary>
        /// <param name="prefixLoc">prefix string to define strings which need localize</param>
        public string LocalizeKeys(string input, string prefixLoc = "[loc]")
        {
            StringBuilder stringBuilder = new StringBuilder();

            string tmp, tmp2;

            string[] values = input.Split(' ');
            for (int i = 0; i < values.Length; i++)
            {
                tmp = values[i];
                int indexLoc = tmp.IndexOf(prefixLoc);

                if (indexLoc >= 0)
                {
                    tmp = tmp.Substring(0, indexLoc) + tmp.Substring(indexLoc + 5);

                    int indexEnd = tmp.IndexOf("</color>");
                    if (indexEnd > 0)
                    {
                        tmp2 = tmp.Substring(indexLoc, indexEnd - indexLoc);
                    }
                    else if (tmp.EndsWith(",") || tmp.EndsWith("."))
                    {
                        tmp2 = tmp.Substring(indexLoc);
                        tmp2 = tmp2.Substring(indexLoc, tmp2.Length - 1);
                    }
                    else
                    {
                        tmp2 = tmp.Substring(indexLoc);
                    }

                    tmp = tmp.Replace(tmp2, LoadKey(tmp2));
                }
                stringBuilder.Append(tmp + " ");
            }

            return stringBuilder.ToString();
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Load localization from csv file.
        /// </summary>
        private void _LoadLocalization()
        {
            if (Config.Location == ELocation.PERSISTENT)
            {
                if (!File.Exists(Application.persistentDataPath + Config.PersistentPath))
                {
                    Debug.LogError("[" + typeof(MyLocalizationManager).Name + "] Reload(): Could not find file \"" + (Application.persistentDataPath + Config.PersistentPath) + "\".");

                    TextAsset textAsset = Resources.Load(Config.ResourcesPath) as TextAsset;
                    if (textAsset == null)
                    {
                        Debug.LogError("[" + typeof(MyLocalizationManager).Name + "] Reload(): Could not find file \"" + Config.ResourcesPath + "\" too.");
                    }
                    else
                    {
                        mDictionary = MyCSV.DeserializeByRowAndRowName(textAsset.text);
                        mLanguageKeys = mDictionary.First().Value;
                    }
                }
                else
                {
                    string text = File.ReadAllText(Application.persistentDataPath + Config.PersistentPath);
                    mDictionary = MyCSV.DeserializeByRowAndRowName(text);
                    mLanguageKeys = mDictionary.First().Value;
                }
            }
            else
            {
                TextAsset textAsset = Resources.Load(Config.ResourcesPath) as TextAsset;
                if (textAsset == null)
                {
                    Debug.LogError("[" + typeof(MyLocalizationManager).Name + "] Reload(): Could not find file \"" + Config.ResourcesPath + "\".");
                }
                else
                {
                    mDictionary = MyCSV.DeserializeByRowAndRowName(textAsset.text);
                    mLanguageKeys = mDictionary.First().Value;
                }
            }

            if (mLanguageKeys == null)
            {
                mLanguageKeys = new string[1];
                mLanguageKeys[0] = Config.DefaultLanguage.ToString();
            }
        }

        /// <summary>
        /// Load language type from cache.
        /// </summary>
        private void _LoadLanguageFromCache()
        {
            int languageValue = PlayerPrefs.GetInt("MyLocalizationManager_Language", (int)ELanguage.None);
            if (languageValue != (int)ELanguage.None && Enum.IsDefined(typeof(ELanguage), languageValue))
            {
                mLanguageType = (ELanguage)languageValue;
            }
            else
            {
                mLanguageType = Config.DefaultLanguage;
            }
        }

        /// <summary>
        /// Load language type based on device language.
        /// </summary>
        private void _LoadLanguageBasedOnDevice()
        {
            if (Application.systemLanguage < SystemLanguage.Unknown && Enum.IsDefined(typeof(ELanguage), (int)Application.systemLanguage))
            {
                mLanguageType = (ELanguage)Application.systemLanguage;
            }
            else
            {
                mLanguageType = Config.DefaultLanguage;
            }
        }

        /// <summary>
        /// Load language type based on device language and cache.
        /// </summary>
        private void _LoadLanguagBasedOnDeviceAndCache()
        {
            int languageValue = PlayerPrefs.GetInt("MyLocalizationManager_Language", (int)ELanguage.None);
            if (languageValue != (int)ELanguage.None && Enum.IsDefined(typeof(ELanguage), languageValue))
            {
                mLanguageType = (ELanguage)languageValue;
            }
            else
            {
                _LoadLanguageBasedOnDevice();
            }
        }

        #endregion

        #region ----- Enumeration -----

        public enum ELocation
        {
            RESOURCES,
            PERSISTENT
        }

        public enum EMode
        {
            CACHE_ONLY,
            DEVICE_LANGUAGE_ONLY,
            DEVICE_LANGUAGE_AND_CACHE
        }

        public enum ELanguage
        {
            None = -1,
            Afrikaans = 0,
            Arabic = 1,
            Basque = 2,
            Belarusian = 3,
            Bulgarian = 4,
            Catalan = 5,
            Chinese = 6,
            Czech = 7,
            Danish = 8,
            Dutch = 9,
            English = 10,
            Estonian = 11,
            Faroese = 12,
            Finnish = 13,
            French = 14,
            German = 15,
            Greek = 16,
            Hebrew = 17,
            Hugarian = 18,
            Hungarian = 18,
            Icelandic = 19,
            Indonesian = 20,
            Italian = 21,
            Japanese = 22,
            Korean = 23,
            Lithuanian = 25,
            Norwegian = 26,
            Polish = 27,
            Portuguese = 28,
            Romanian = 29,
            Russian = 30,
            SerboCroatian = 31,
            Slovak = 32,
            Slovenian = 33,
            Spanish = 34,
            Swedish = 35,
            Thai = 36,
            Turkish = 37,
            Ukrainian = 38,
            Vietnamese = 39,
            ChineseSimplified = 40,
            ChineseTraditional = 41,
            Unknown = 42,
            Hindi = 43,
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyLocalizationManager))]
    public class MyLocalizationManagerEditor : Editor
    {
        private MyLocalizationManager mScript;
        private SerializedProperty mConfig;
        private SerializedProperty mIsAutoSaveOnChange;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyLocalizationManager)target;
            mConfig = serializedObject.FindProperty("mConfig");
            mIsAutoSaveOnChange = serializedObject.FindProperty("mIsAutoSaveOnChange");

            if (mScript.Config == null)
            {
                mScript.LoadConfig();
            }
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyLocalizationManager), false);

            serializedObject.Update();

            EditorGUILayout.ObjectField(mConfig, new GUIContent("Config File"));

            EditorGUI.BeginChangeCheck();

            mIsAutoSaveOnChange.boolValue = EditorGUILayout.Toggle("Auto Save On Change", mIsAutoSaveOnChange.boolValue);

            EditorGUILayout.LabelField(string.Empty);
            if (GUILayout.Button("Save"))
            {
                mScript.SaveConfig();
            }

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Location", EditorStyles.boldLabel);
            mScript.Config.Location = (MyLocalizationManager.ELocation)EditorGUILayout.EnumPopup("   Location", mScript.Config.Location);
            if (mScript.Config.Location == MyLocalizationManager.ELocation.RESOURCES)
            {
                mScript.Config.ResourcesPath = EditorGUILayout.TextField("   Path", mScript.Config.ResourcesPath);
            }
            else
            {
                mScript.Config.PersistentPath = EditorGUILayout.TextField("   Path", mScript.Config.PersistentPath);
            }

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Other", EditorStyles.boldLabel);
            mScript.Config.Mode = (MyLocalizationManager.EMode)EditorGUILayout.EnumPopup("   Mode", mScript.Config.Mode);
            mScript.Config.DefaultLanguage = (MyLocalizationManager.ELanguage)EditorGUILayout.EnumPopup("   Default Language", mScript.Config.DefaultLanguage);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if (mIsAutoSaveOnChange.boolValue)
                {
                    mScript.SaveConfig();
                }
            }
        }
    }

#endif
}