/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyLocalizationManager (version 2.20)
 */

#pragma warning disable 0162

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

        [SerializeField]
        private string mPathPersistent = "/localization.csv";
        [SerializeField]
        private string mPathResources = "Configs/localization";
        [SerializeField]
        private EPath mPath = EPath.RESOURCES;
        [SerializeField]
        private EMode mMode = EMode.DEVICE_LANGUAGE_AND_CACHE;
        [SerializeField]
        private ELanguage mDefaultLanguage = ELanguage.Vietnamese;

        private ELanguage mLanguageType = ELanguage.None;
        private string[] mLanguageKeys;
        private int mLanguageIndex;
        private Dictionary<string, string[]> mDictionary = new Dictionary<string, string[]>();
        private List<MyLocalization> mListLocalization = new List<MyLocalization>();

        #endregion

        #region ----- Property -----

        public string PathPersistent
        {
            get { return mPathPersistent; }
            set { mPathPersistent = value; }
        }

        public string PathResources
        {
            get { return mPathResources; }
            set { mPathResources = value; }
        }

        public EPath Path
        {
            get { return mPath; }
            set { mPath = value; }
        }

        public EMode Mode
        {
            get { return mMode; }
            set { mMode = value; }
        }

        public ELanguage DefaultLanguage
        {
            get { return mDefaultLanguage; }
            set { mDefaultLanguage = value; }
        }

        public ELanguage Language
        {
            get
            {
                if (mLanguageType == ELanguage.None)
                {
                    switch (mMode)
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
                            DontDestroyOnLoad(obj);
                        }
                    }
                }
                return mInstance;
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Initalize.
        /// </summary>
        /// <param name="localized mode"></param>
        /// <param name="default language"></param>
        public void Init(EMode mode, ELanguage defaultLanguage)
        {
            mMode = mode;
            mDefaultLanguage = defaultLanguage;
            LoadLanguage(Language);
        }

        /// <summary>
        /// Reload localization file.
        /// </summary>
        public void Reload()
        {
            if (mPath == EPath.PERSISTENT)
            {
                if (!File.Exists(Application.persistentDataPath + mPathPersistent))
                {
                    Debug.LogError("[" + typeof(MyLocalizationManager).Name + "] Reload(): Could not find file \"" + (Application.persistentDataPath + mPathPersistent) + "\".");

                    TextAsset textAsset = Resources.Load(mPathResources) as TextAsset;
                    if (textAsset == null)
                    {
                        Debug.LogError("[" + typeof(MyLocalizationManager).Name + "] Reload(): Could not find file \"" + mPathResources + "\" too.");
                    }
                    else
                    {
                        mDictionary = MyCSV.DeserializeByRowAndRowName(textAsset.text);
                        mLanguageKeys = mDictionary.First().Value;
                    }
                }
                else
                {
                    string text = File.ReadAllText(Application.persistentDataPath + mPathPersistent);
                    mDictionary = MyCSV.DeserializeByRowAndRowName(text);
                    mLanguageKeys = mDictionary.First().Value;
                }
            }
            else
            {
                TextAsset textAsset = Resources.Load(mPathResources) as TextAsset;
                if (textAsset == null)
                {
                    Debug.LogError("[" + typeof(MyLocalizationManager).Name + "] Reload(): Could not find file \"" + mPathResources + "\".");
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
                mLanguageKeys[0] = mDefaultLanguage.ToString();
            }
        }

        /// <summary>
        /// Load language file.
        /// </summary>
        public void LoadLanguage(ELanguage language)
        {
            Language = language;

            if (mLanguageKeys == null)
            {
                Reload();
            }

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
                mLanguageType = mDefaultLanguage;
            }
        }

        /// <summary>
        /// Load language type based on device language.
        /// </summary>
        private void _LoadLanguageBasedOnDevice()
        {
            if (Application.systemLanguage != SystemLanguage.Unknown && Enum.IsDefined(typeof(ELanguage), (int)Application.systemLanguage))
            {
                mLanguageType = (ELanguage)Application.systemLanguage;
            }
            else
            {
                mLanguageType = mDefaultLanguage;
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

        public enum EPath
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
            Unknown = 42
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyLocalizationManager))]
    public class MyLocalizationManagerEditor : Editor
    {
        private MyLocalizationManager mScript;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyLocalizationManager)target;
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyLocalizationManager), false);

            mScript.PathPersistent = EditorGUILayout.TextField("Persistent Path", mScript.PathPersistent);
            mScript.PathResources = EditorGUILayout.TextField("Resources Path", mScript.PathResources);
            EditorGUILayout.LabelField(string.Empty);
            mScript.Path = (MyLocalizationManager.EPath)EditorGUILayout.EnumPopup("Path", mScript.Path);
            mScript.Mode = (MyLocalizationManager.EMode)EditorGUILayout.EnumPopup("Mode", mScript.Mode);
            mScript.DefaultLanguage = (MyLocalizationManager.ELanguage)EditorGUILayout.EnumPopup("Default Language", mScript.DefaultLanguage);
        }
    }

#endif
}