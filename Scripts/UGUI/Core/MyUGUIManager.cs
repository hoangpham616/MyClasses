﻿/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIManager (version 2.22)
 */

#pragma warning disable 0162
#pragma warning disable 0429

#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace MyClasses.UI
{
    public class MyUGUIManager : MonoBehaviour
    {
        #region ----- Internal Class -----

        private class AssetBundleConfig
        {
            public string URL;
            public int Version;
        }

        #endregion

        #region ----- Variable -----

        public static string SETTING_DIRECTORY = "Settings/UGUI/";
        public static string SCENE_DIRECTORY = "Prefabs/UGUI/Scenes/";
        public static string POPUP_DIRECTORY = "Prefabs/UGUI/Popups/";
        public static string HUD_DIRECTORY = "Prefabs/UGUI/HUDs/";
        public static string SPECIAL_DIRECTORY = "Prefabs/UGUI/Specials/";

        private Camera mCameraUI;

        private GameObject mCanvas;
        private GameObject mCanvasOnTop;
        private GameObject mCanvasOnTopHUD;
        private GameObject mCanvasOnTopPopup;
        private GameObject mCanvasOnTopFloatPopup;
        private GameObject mCanvasOnTopLoadingIndicator;
        private GameObject mCanvasSceneFading;

        private Dictionary<EPopupID, MyUGUIConfigPopup> mDictPopupConfig = new Dictionary<EPopupID, MyUGUIConfigPopup>();
        private AssetBundleConfig mCoreAssetBundleConfig;

        private List<MyUGUIUnityScene> mListUnityScene = new List<MyUGUIUnityScene>();
        private List<MyUGUIScene> mListScene = new List<MyUGUIScene>();
        private List<MyUGUIPopup> mListPopup = new List<MyUGUIPopup>();
        private List<MyUGUIPopup> mListFloatPopup = new List<MyUGUIPopup>();
        private List<MyUGUIRunningText> mListRunningText = new List<MyUGUIRunningText>();

        private MyUGUIUnityScene mCurrentUnityScene;
        private MyUGUIUnityScene mNextUnityScene;

        private MyUGUIScene mPreviousScene;
        private MyUGUIScene mNextScene;
        private MyUGUIScene mCurrentScene;
        private MyUGUISceneFading mCurrentSceneFading;

        private MyUGUIPopupOverlay mCurrentPopupOverlay;
        private MyUGUIPopup mCurrentPopup;
        private MyUGUIPopup mCurrentFloatPopup;

        private MyUGUILoadingIndicator mCurrentLoadingIndicator;
        private MyUGUIRunningText mCurrentRunningText;
        private MyUGUIToast mCurrentToast;

        private AsyncOperation mUnitySceneUnloadUnusedAsset;
        private AsyncOperation mUnitySceneLoad;

        private Action mOnScenePreEnterCallback;
        private Action mOnScenePostEnterCallback;
        private Action mOnScenePostVisibleCallback;

        private bool mIsClosePopupByClickingOutside;
        private bool mIsHideRunningTextWhenSwitchingScene;
        private bool mIsHideToastWhenSwitchingScene;

        private int mPreviousInitSceneIndex;

        #endregion

        #region ----- Property -----

        public Camera Camera
        {
            get { return mCameraUI; }
        }

        public GameObject Canvas
        {
            get { return mCanvas; }
        }

        public GameObject CanvasOnTop
        {
            get { return mCanvasOnTop; }
        }

        public GameObject CanvasOnTopHUD
        {
            get { return mCanvasOnTopHUD; }
        }

        public GameObject CanvasOnTopPopup
        {
            get { return mCanvasOnTopPopup; }
        }

        public GameObject CanvasOnTopFloatPopup
        {
            get { return mCanvasOnTopFloatPopup; }
        }

        public MyUGUIUnityScene CurrentUnityScene
        {
            get { return mCurrentUnityScene; }
        }

        public MyUGUIScene CurrentScene
        {
            get { return mCurrentScene; }
        }

        public MyUGUIScene PreviousScene
        {
            get { return mPreviousScene; }
        }

        public MyUGUISceneFading CurrentSceneFading
        {
            get { return mCurrentSceneFading; }
        }

        public MyUGUIPopup CurrentPopup
        {
            get { return mCurrentPopup; }
        }

        public MyUGUIPopup CurrentFloatPopup
        {
            get { return mCurrentFloatPopup; }
        }

        public MyUGUILoadingIndicator CurrentLoadingIndicator
        {
            get { return mCurrentLoadingIndicator; }
        }

        public bool IsShowingLoadingIndicator
        {
            get { return mCurrentLoadingIndicator != null && mCurrentLoadingIndicator.IsActive; }
        }

        public bool IsClosePopupByClickingOutside
        {
            get { return mIsClosePopupByClickingOutside; }
            set { mIsClosePopupByClickingOutside = value; }
        }

        public bool IsTouchingOnUI
        {
            get
            {
                if (EventSystem.current == null)
                {
                    return false;
                }
                return EventSystem.current.IsPointerOverGameObject();
            }
        }

        #endregion

        #region ----- Singleton -----

        private static object mSingletonLock = new object();
        private static MyUGUIManager mInstance;

        public static MyUGUIManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mSingletonLock)
                    {
                        mInstance = (MyUGUIManager)FindObjectOfType(typeof(MyUGUIManager));
                        if (mInstance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyUGUIManager).Name);
                            mInstance = obj.AddComponent<MyUGUIManager>();
                            DontDestroyOnLoad(obj);
                        }
                    }
                }
                return mInstance;
            }
        }

        #endregion

        #region ----- MonoBehaviour Implemention -----

        /// <summary>
        /// Awake.
        /// </summary>
        void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            _InitConfig();
        }

        /// <summary>
        /// Update.
        /// </summary>
        void Update()
        {
            _UpdateUnityScene();
            _UpdateScene(Time.deltaTime);
            _UpdateHUD(Time.deltaTime);

            for (int i = 0, count = mListPopup.Count; i < count; i++)
            {
                MyUGUIPopup popup = mListPopup[i];
                _UpdatePopup(ref popup, Time.deltaTime);
                mListPopup[i] = popup;
            }
            for (int i = 0, count = mListFloatPopup.Count; i < count; i++)
            {
                MyUGUIPopup popup = mListFloatPopup[i];
                _UpdateFloatPopup(ref popup, Time.deltaTime);
                mListFloatPopup[i] = popup;
            }

            if (mCurrentRunningText != null)
            {
                mCurrentRunningText.Update(Time.deltaTime);
            }
        }

        /// <summary>
        /// LateUpdate.
        /// </summary>
        void LateUpdate()
        {
            if (mCurrentToast != null)
            {
                mCurrentToast.LateUpdate(Time.deltaTime);
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set asset bundle for core UI (scene fading, popup overlay, toast, running text, loading indicator).
        /// </summary>
        public void SetAssetBundleForCore(string url, int version)
        {
            mCoreAssetBundleConfig = new AssetBundleConfig()
            {
                URL = url,
                Version = version
            };
        }

        /// <summary>
        /// Set asset bundle for HUDs.
        /// </summary>
        public void SetAssetBundleForHUDs(string url, int version)
        {
            foreach (MyUGUIUnityScene unityScene in mListUnityScene)
            {
                if (unityScene.HUD != null)
                {
                    unityScene.HUD.SetAssetBundle(url, version);
                }
            }
        }

        /// <summary>
        /// Set asset bundle for scene.
        /// </summary>
        public void SetAssetBundleForScene(ESceneID sceneID, string url, int version)
        {
            foreach (MyUGUIUnityScene unityScene in mListUnityScene)
            {
                foreach (MyUGUIScene scene in unityScene.ListScene)
                {
                    if (scene.ID == sceneID)
                    {
                        scene.SetAssetBundle(url, version);
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Set asset bundle for popup.
        /// </summary>
        public void SetAssetBundleForPopup(EPopupID popupID, string url, int version)
        {
            foreach (var item in mDictPopupConfig)
            {
                MyUGUIConfigPopup configPopup = mDictPopupConfig[item.Key];
                if (configPopup.ID == popupID)
                {
                    configPopup.AssetBundleURL = url;
                    configPopup.AssetBundleVersion = version;
                    return;
                }
            }
        }

        /// <summary>
        /// Back to previous scene.
        /// </summary>
        public bool Back()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>Back()</color>");
#endif

            if (mCurrentLoadingIndicator != null && mCurrentLoadingIndicator.IsActive)
            {
                return false;
            }

            if (mCurrentPopup != null && mCurrentPopup.IsActive)
            {
                HideCurrentPopup();
                return true;
            }

            int countScene = mListScene.Count;
            if (countScene >= 2)
            {
                MyUGUIScene scene = mListScene[countScene - 2];
                if (scene.UnitySceneID == mCurrentUnityScene.ID)
                {
                    ShowScene(scene.ID);
                }
                else
                {
                    ShowUnityScene(scene.UnitySceneID, scene.ID);
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Show a scene.
        /// </summary>
        /// <param name="unitySceneID">Empty: without scene</param>
        public void ShowUnityScene(EUnitySceneID unitySceneID, ESceneID sceneID)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowUnityScene()</color>: " + unitySceneID + " - " + sceneID);
#endif

            int countUnityScene = mListUnityScene.Count;
            for (int i = 0; i < countUnityScene; i++)
            {
                if (mListUnityScene[i].ID == unitySceneID)
                {
                    if (mCurrentUnityScene == null)
                    {
                        mCurrentUnityScene = mListUnityScene[i];
                        mCurrentUnityScene.State = EUnitySceneState.Unload;
                    }

                    mNextUnityScene = mListUnityScene[i];

                    int countScene = mNextUnityScene.ListScene.Count;
                    for (int j = 0; j < countScene; j++)
                    {
                        if (mNextUnityScene.ListScene[j].ID == sceneID)
                        {
                            if (mCurrentScene != null && mCurrentScene.IsLoaded)
                            {
                                mCurrentScene.OnUGUIExit();
                                mCurrentScene.OnUGUIInvisible();
                            }
                            mCurrentScene = null;
                            mNextScene = mNextUnityScene.ListScene[j];
                            mNextScene.UnitySceneID = mNextUnityScene.ID;
                            _AddSceneIntoSceneStack(mNextScene);
                            break;
                        }
                    }

                    return;
                }
            }
        }

        /// <summary>
        /// Return a scene which showed before in current Unity scene.
        /// </summary>
        public MyUGUIScene GetScene(ESceneID sceneID)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>GetScene()</color>: " + sceneID);
#endif

            int countScene = mCurrentUnityScene.ListScene.Count;
            for (int i = 0; i < countScene; i++)
            {
                if (mCurrentUnityScene.ListScene[i].ID == sceneID)
                {
                    return mCurrentUnityScene.ListScene[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Show a scene.
        /// </summary>
        public void ShowScene(ESceneID sceneID, bool isHideRunningTextWhenSwitchingScene = false, bool isHideToastWhenSwitchingScene = true, Action onPreEnterCallback = null, Action onPostEnterCallback = null, Action onPostVisibleCallback = null)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowScene()</color>: " + sceneID);
#endif

            int countScene = mCurrentUnityScene.ListScene.Count;
            for (int i = 0; i < countScene; i++)
            {
                if (mCurrentUnityScene.ListScene[i].ID == sceneID)
                {
                    mIsHideRunningTextWhenSwitchingScene = isHideRunningTextWhenSwitchingScene;
                    mIsHideToastWhenSwitchingScene = isHideToastWhenSwitchingScene;

                    mOnScenePreEnterCallback = onPreEnterCallback;
                    mOnScenePostEnterCallback = onPostEnterCallback;
                    mOnScenePostVisibleCallback = onPostVisibleCallback;

                    if (mCurrentScene == null)
                    {
                        mCurrentScene = mCurrentUnityScene.ListScene[i];
                        mCurrentScene.State = EBaseState.LoadAssetBundle;
                    }

                    mNextScene = mCurrentUnityScene.ListScene[i];
                    mNextScene.UnitySceneID = mCurrentUnityScene.ID;
                    _AddSceneIntoSceneStack(mNextScene);

                    return;
                }
            }
        }

        /// <summary>
        /// Update popup overlay.
        /// </summary>
        public void UpdatePopupOverlay()
        {
            if (mCurrentPopupOverlay != null)
            {
                StartCoroutine(_DoUpdatePopupOverlay());
            }
        }

        /// <summary>
        /// Show a popup.
        /// </summary>
        public MyUGUIPopup ShowPopup(EPopupID popupID, object attachedData = null, Action onCloseCallback = null)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowPopup()</color>: " + popupID);
#endif

            bool isRepeatable = popupID == EPopupID.Dialog0ButtonPopup || popupID == EPopupID.Dialog1ButtonPopup || popupID == EPopupID.Dialog2ButtonsPopup;

            return _ShowPopup(popupID, isRepeatable, attachedData, onCloseCallback);
        }

        /// <summary>
        /// Show a repeatable popup.
        /// </summary>
        public MyUGUIPopup ShowRepeatablePopup(EPopupID popupID, object attachedData = null, Action onCloseCallback = null)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowRepeatablePopup()</color>: " + popupID);
#endif

            return _ShowPopup(popupID, true, attachedData, onCloseCallback);
        }

        /// <summary>
        /// Show a float popup.
        /// </summary>
        public MyUGUIPopup ShowFloatPopup(EPopupID popupID, object attachedData = null)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowFloatPopup()</color>: " + popupID);
#endif

            return _ShowFloatPopup(popupID, false, attachedData);
        }

        /// <summary>
        /// Show a repeatable float popup.
        /// </summary>
        public MyUGUIPopup ShowRepeatableFloatPopup(EPopupID popupID, object attachedData = null)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowRepeatableFloatPopup()</color>: " + popupID);
#endif

            return _ShowFloatPopup(popupID, true, attachedData);
        }

        /// <summary>
        /// Hide current popup.
        /// </summary>
        public void HideCurrentPopup()
        {
            int countPopup = mListPopup.Count;
            for (int i = countPopup - 1; i >= 0; i--)
            {
                if (mListPopup[i] != null)
                {
                    mListPopup[i].Hide();
                    return;
                }
            }
        }

        /// <summary>
        /// Hide current float popup.
        /// </summary>
        public void HideCurrentFloatPopup()
        {
            int countPopup = mListFloatPopup.Count;
            for (int i = countPopup - 1; i >= 0; i--)
            {
                if (mListFloatPopup[i] != null)
                {
                    mListFloatPopup[i].Hide();
                    return;
                }
            }
        }

        /// <summary>
        /// Hide all popups.
        /// </summary>
        public void HideAllPopups(bool isHidePopup = true, bool isHideFloatPopup = true)
        {
            if (isHidePopup)
            {
                foreach (MyUGUIPopup popup in mListPopup)
                {
                    if (popup != null)
                    {
                        popup.Hide();
                    }
                }
            }

            if (isHideFloatPopup)
            {
                foreach (MyUGUIPopup popup in mListFloatPopup)
                {
                    if (popup != null)
                    {
                        popup.Hide();
                    }
                }
            }
        }

        /// <summary>
        /// Hide current tips loading indicator, show simple loading indicator and return loading id.
        /// </summary>
        public int ShowLoadingIndicator()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowLoadingIndicator()</color>");
#endif

            _InitLoadingIndicator();

            return mCurrentLoadingIndicator.ShowSimple();
        }

        /// <summary>
        /// Hide current tips loading indicator, show simple loading indicator and return loading id.
        /// </summary>
        public int ShowLoadingIndicator(float timeOut, Action timeOutCallback = null)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowLoadingIndicator()</color>: timeOut=" + timeOut);
#endif

            _InitLoadingIndicator();

            return mCurrentLoadingIndicator.ShowSimple(timeOut, timeOutCallback);
        }

        /// <summary>
        /// Hide current simple loading indicator and show tips loading indicator.
        /// </summary>
        public void ShowLoadingIndicatorWithTips(string tips, string description, bool isThreeDots, Action cancelCallback = null)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowLoadingIndicatorWithTips()</color>: tips=" + tips + " | description=" + description + " | isThreeDots=" + isThreeDots);
#endif

            _InitLoadingIndicator();

            mCurrentLoadingIndicator.ShowTips(tips, description, isThreeDots, -1, null, cancelCallback);
        }

        /// <summary>
        /// Hide current simple loading indicator and show tips loading indicator.
        /// </summary>
        public void ShowLoadingIndicatorWithTips(string tips, string description, bool isThreeDots, float timeOut, Action timeOutCallback = null, Action cancelCallback = null)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowLoadingIndicatorWithTips()</color>: tips=" + tips + " | description=" + description + " | isThreeDots=" + isThreeDots + " | timeOut=" + timeOut);
#endif

            _InitLoadingIndicator();

            mCurrentLoadingIndicator.ShowTips(tips, description, isThreeDots, timeOut, timeOutCallback, cancelCallback);
        }

        /// <summary>
        /// Hide loading indicator.
        /// </summary>
        /// <param name="minLiveTime">minimum seconds have to show before hiding</param>
        public void HideLoadingIndicator(float minLiveTime = 0.15f)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>HideLoadingIndicator()</color>: minLiveTime=" + minLiveTime);
#endif

            if (mCurrentLoadingIndicator != null)
            {
                mCurrentLoadingIndicator.Hide(minLiveTime);
            }
        }

        /// <summary>
        /// Hide loading indicator by loading id.
        /// </summary>
        /// <param name="minLiveTime">minimum seconds have to show before hiding</param>
        public void HideLoadingIndicator(int loadingID, float minLiveTime = 0.15f)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>HideLoadingIndicator()</color>: loadingID=" + loadingID + " minLiveTime=" + minLiveTime);
#endif

            if (mCurrentLoadingIndicator != null)
            {
                mCurrentLoadingIndicator.HideSimple(loadingID, minLiveTime);
            }
        }

        /// <summary>
        /// Show running text.
        /// </summary>
        public void ShowRunningText(string content, ERunningTextSpeed speed = ERunningTextSpeed.Normal, MyUGUIRunningText.EType type = MyUGUIRunningText.EType.Default, int maxQueue = -1)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowRunningText()</color>");
#endif

            _InitRunningText(type);

            if (maxQueue > 0)
            {
                mCurrentRunningText.SetMaxQueue(maxQueue);
            }
            mCurrentRunningText.Show(content, (int)speed, (int)speed * 1.2f);
        }

        /// <summary>
        /// Hide running text.
        /// </summary>
        public void HideRunningText()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>HideRunningText()</color>");
#endif

            if (mCurrentRunningText != null)
            {
                mCurrentRunningText.Hide();
            }
        }

        /// <summary>
        /// Show toast.
        /// </summary>
        public void ShowToast(string content, EToastDuration duration = EToastDuration.Medium)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowToast()</color>: duration=" + duration.ToString());
#endif

            _InitToast();

            mCurrentToast.Show(content, (int)duration);
        }

        /// <summary>
        /// Show toast.
        /// </summary>
        public void ShowToast(string content, float duration)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowToast()</color>: duration=" + duration);
#endif

            _InitToast();

            mCurrentToast.Show(content, duration);
        }

        /// <summary>
        /// Hide toast.
        /// </summary>
        public void HideToast()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>HideToast()</color>");
#endif

            if (mCurrentToast != null)
            {
                mCurrentToast.Hide();
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Init scenes and popups config.
        /// </summary>
        private void _InitConfig()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF0000FF>_InitConfig()</color>");
#endif

#if UNITY_EDITOR
            if (!System.IO.File.Exists("Assets/Resources/" + SETTING_DIRECTORY + typeof(MyUGUIConfigUnityScenes).Name + ".asset"))
            {
                Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitConfig(): Could not find scene config file. Please setup it on Menu Bar first.");
                return;
            }

            if (!System.IO.File.Exists("Assets/Resources/" + SETTING_DIRECTORY + typeof(MyUGUIConfigPopups).Name + ".asset"))
            {
                Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitConfig(): Could not find popup config file. Please setup it on Menu Bar first.");
                return;
            }
#endif

            mListUnityScene.Clear();
            MyUGUIConfigUnityScenes unityScenesConfig = Resources.Load<MyUGUIConfigUnityScenes>(SETTING_DIRECTORY + typeof(MyUGUIConfigUnityScenes).Name);
            if (unityScenesConfig != null)
            {
                foreach (MyUGUIConfigUnityScene unitySceneConfig in unityScenesConfig.ListUnityScene)
                {
                    MyUGUIUnityScene unityScene = new MyUGUIUnityScene(unitySceneConfig.ID, unitySceneConfig.SceneName);
                    if (!string.IsNullOrEmpty(unitySceneConfig.HUDScriptName))
                    {
                        if (string.IsNullOrEmpty(unitySceneConfig.HUDPrefabName))
                        {
                            Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitConfig(): HUD prefab of " + unitySceneConfig.HUDScriptName + " is empty. Please setup it on Menu Bar.");
                            return;
                        }
                        var hud = Activator.CreateInstance(Type.GetType(unitySceneConfig.HUDScriptName), unitySceneConfig.HUDPrefabName);
                        unityScene.SetHUD((MyUGUIHUD)hud);
                    }
                    foreach (MyUGUIConfigScene sceneConfig in unitySceneConfig.ListScene)
                    {
                        var scene = Activator.CreateInstance(Type.GetType(sceneConfig.ScriptName), sceneConfig.ID, sceneConfig.PrefabName, sceneConfig.IsInitWhenLoadUnityScene, sceneConfig.IsHideHUD, sceneConfig.FadeInDuration, sceneConfig.FadeOutDuration);
                        unityScene.AddScene((MyUGUIScene)scene);
                    }
                    mListUnityScene.Add(unityScene);
                }
            }
            else
            {
                Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitConfig(): Could not scene config file.");
            }

            mDictPopupConfig.Clear();
            MyUGUIConfigPopups popupsConfig = Resources.Load<MyUGUIConfigPopups>(SETTING_DIRECTORY + typeof(MyUGUIConfigPopups).Name);
            if (popupsConfig != null)
            {
                foreach (MyUGUIConfigPopup popupConfig in popupsConfig.ListPopup)
                {
                    mDictPopupConfig[popupConfig.ID] = popupConfig;
                }
            }
            else
            {
                Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitConfig(): Could not load popup config file.");
            }
        }

        /// <summary>
        /// Init canvases.
        /// </summary>
        private void _InitCanvas()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF7777FF>_InitCanvas()</color>");
#endif

            mCanvas = GameObject.Find("Canvas");
            if (mCanvas == null)
            {
                Debug.LogError("[" + typeof(MyUGUIManager).Name + "] OnUGUIInit(): Could not find \"Canvas\" in scene \"" + SceneManager.GetActiveScene().name + "\". Please create \"Canvas\" first.");
            }
            if (mCanvas != null)
            {
                mCanvas.GetComponent<Canvas>().sortingOrder = -1000;
            }

            mCanvasOnTop = GameObject.Find("CanvasOnTop");
            if (mCanvasOnTop == null)
            {
                Debug.LogError("[" + typeof(MyUGUIManager).Name + "] OnUGUIInit(): Could not find \"CanvasOnTop\" in scene \"" + SceneManager.GetActiveScene().name + "\". Please create \"CanvasOnTop\" first.");
            }
            if (mCanvasOnTop != null)
            {
                mCanvasOnTop.GetComponent<Canvas>().sortingOrder = 1000;

                if (mCurrentUnityScene.HUD != null)
                {
                    mCanvasOnTopHUD = MyUtilities.FindObjectInFirstLayer(mCanvasOnTop, "HUD");
                    if (mCanvasOnTopHUD == null)
                    {
                        mCanvasOnTopHUD = new GameObject("HUD");

                        RectTransform rect = mCanvasOnTopHUD.AddComponent<RectTransform>();
                        MyUtilities.Anchor(ref rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

                        mCanvasOnTopHUD.AddComponent<CanvasRenderer>();

                        mCanvasOnTopHUD.transform.SetParent(mCanvasOnTop.transform, false);
                    }
                    if (mCanvasOnTopHUD != null)
                    {
                        mCanvasOnTopHUD.transform.SetAsFirstSibling();
                    }
                }

                mCanvasOnTopPopup = MyUtilities.FindObjectInFirstLayer(mCanvasOnTop, "Popups");
                if (mCanvasOnTopPopup == null)
                {
                    mCanvasOnTopPopup = new GameObject("Popups");

                    RectTransform rect = mCanvasOnTopPopup.AddComponent<RectTransform>();
                    MyUtilities.Anchor(ref rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

                    mCanvasOnTopPopup.AddComponent<CanvasRenderer>();

                    mCanvasOnTopPopup.transform.SetParent(mCanvasOnTop.transform, false);
                }
                if (mCanvasOnTopPopup != null)
                {
                    mCanvasOnTopPopup.transform.SetAsLastSibling();
                }

                mCanvasOnTopFloatPopup = MyUtilities.FindObjectInFirstLayer(mCanvasOnTop, "FloatPopups");
                if (mCanvasOnTopFloatPopup == null)
                {
                    mCanvasOnTopFloatPopup = new GameObject("FloatPopups");

                    RectTransform rect = mCanvasOnTopFloatPopup.AddComponent<RectTransform>();
                    MyUtilities.Anchor(ref rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

                    mCanvasOnTopFloatPopup.AddComponent<CanvasRenderer>();

                    mCanvasOnTopFloatPopup.transform.SetParent(mCanvasOnTop.transform, false);
                }
                if (mCanvasOnTopFloatPopup != null)
                {
                    mCanvasOnTopFloatPopup.transform.SetAsLastSibling();
                }

                mCanvasOnTopLoadingIndicator = MyUtilities.FindObjectInFirstLayer(mCanvasOnTop, "LoadingIndicator");
                if (mCanvasOnTopLoadingIndicator == null)
                {
                    mCanvasOnTopLoadingIndicator = new GameObject("LoadingIndicator");

                    RectTransform rect = mCanvasOnTopLoadingIndicator.AddComponent<RectTransform>();
                    MyUtilities.Anchor(ref rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

                    mCanvasOnTopLoadingIndicator.AddComponent<CanvasRenderer>();

                    mCanvasOnTopLoadingIndicator.transform.SetParent(mCanvasOnTop.transform, false);
                }
                if (mCanvasOnTopLoadingIndicator != null)
                {
                    mCanvasOnTopLoadingIndicator.transform.SetAsLastSibling();
                }
            }

            if (mCanvasSceneFading == null)
            {
                mCanvasSceneFading = GameObject.Find("CanvasSceneFading");
                if (mCanvasSceneFading == null)
                {
                    Debug.LogError("[" + typeof(MyUGUIManager).Name + "] OnUGUIInit(): Could not find \"CanvasSceneFading\" in scene \"" + SceneManager.GetActiveScene().name + "\". A template was created instead.");

                    mCanvasSceneFading = new GameObject("CanvasSceneFading");
                    mCanvasSceneFading.AddComponent<Canvas>();
                    mCanvasSceneFading.AddComponent<CanvasScaler>();
                    mCanvasSceneFading.AddComponent<GraphicRaycaster>();
                }
                Canvas canvas = mCanvasSceneFading.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 10000;
                DontDestroyOnLoad(mCanvasSceneFading);
            }
        }

        /// <summary>
        /// Init camera.
        /// </summary>
        private void _InitCamera()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF7777FF>_InitCamera()</color>");
#endif

            if (mCameraUI == null)
            {
                GameObject uiCamera = MyUtilities.FindObjectInRoot("UICamera");
                if (uiCamera == null)
                {
                    uiCamera = new GameObject("UICamera");
                    uiCamera.AddComponent<Camera>();
                    uiCamera.transform.localPosition = new Vector3(0, 1, -10);

                    mCameraUI = uiCamera.GetComponent<Camera>();
                    mCameraUI.clearFlags = CameraClearFlags.Nothing;
                    mCameraUI.cullingMask |= LayerMask.GetMask("UI");
                }
                else
                {
                    mCameraUI = uiCamera.GetComponent<Camera>();
                }
            }
        }

        /// <summary>
        /// Init scene fading.
        /// </summary>
        private void _InitSceneFading()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF7777FF>_InitSceneFading()</color>");
#endif

            if (mCurrentSceneFading == null || mCurrentSceneFading.GameObject == null)
            {
                mCurrentSceneFading = new MyUGUISceneFading();
                mCurrentSceneFading.GameObject = MyUtilities.FindObjectInFirstLayer(mCanvasOnTop, MyUGUISceneFading.PREFAB_NAME);

                if (mCurrentSceneFading.GameObject == null)
                {
                    if (mCoreAssetBundleConfig != null && !string.IsNullOrEmpty(mCoreAssetBundleConfig.URL))
                    {
                        AssetBundle bundle = MyAssetBundleManager.Get(mCoreAssetBundleConfig.URL, mCoreAssetBundleConfig.Version);
                        if (bundle == null)
                        {
                            Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitSceneFading(): Could not get asset bundle which contains Scene Fading. A template was created instead.");
                            mCurrentSceneFading.GameObject = MyUGUISceneFading.CreateTemplate();
                        }
                        else
                        {
                            mCurrentSceneFading.GameObject = Instantiate(bundle.LoadAsset(MyUGUISceneFading.PREFAB_NAME) as GameObject);
                        }
                    }
                    else
                    {
                        mCurrentSceneFading.GameObject = Instantiate(Resources.Load(SPECIAL_DIRECTORY + MyUGUISceneFading.PREFAB_NAME) as GameObject);
                    }
                }

                mCurrentSceneFading.GameObject.name = MyUGUISceneFading.PREFAB_NAME;
                mCurrentSceneFading.Transform.SetParent(mCanvasSceneFading.transform, false);
                mCurrentSceneFading.TurnOnFading();
            }
        }

        /// <summary>
        /// Init popup overlay.
        /// </summary>
        private void _InitPopupOverlay()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF7777FF>_InitPopupOverlay()</color>");
#endif

            if (mCurrentPopupOverlay == null || mCurrentPopupOverlay.GameObject == null)
            {
                mCurrentPopupOverlay = new MyUGUIPopupOverlay();
                mCurrentPopupOverlay.GameObject = MyUtilities.FindObjectInFirstLayer(mCanvasOnTopPopup, MyUGUIPopupOverlay.PREFAB_NAME);

                if (mCurrentPopupOverlay.GameObject == null)
                {
                    if (mCoreAssetBundleConfig != null && !string.IsNullOrEmpty(mCoreAssetBundleConfig.URL))
                    {
                        AssetBundle bundle = MyAssetBundleManager.Get(mCoreAssetBundleConfig.URL, mCoreAssetBundleConfig.Version);
                        if (bundle == null)
                        {
                            Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitPopupOverlay(): Could not get asset bundle which contains Popup Overlay. A template was created instead.");
                            mCurrentPopupOverlay.GameObject = MyUGUIPopupOverlay.CreateTemplate();
                        }
                        else
                        {
                            mCurrentPopupOverlay.GameObject = Instantiate(bundle.LoadAsset(MyUGUIPopupOverlay.PREFAB_NAME) as GameObject);
                        }
                    }
                    else
                    {
                        mCurrentPopupOverlay.GameObject = Instantiate(Resources.Load(SPECIAL_DIRECTORY + MyUGUIPopupOverlay.PREFAB_NAME) as GameObject);
                    }
                }

                mCurrentPopupOverlay.GameObject.name = MyUGUIPopupOverlay.PREFAB_NAME;
                mCurrentPopupOverlay.GameObject.SetActive(false);
                mCurrentPopupOverlay.Transform.SetParent(mCanvasOnTopPopup.transform, false);
            }
        }

        /// <summary>
        /// Init loading indicator.
        /// </summary>
        private void _InitLoadingIndicator()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF7777FF>_InitLoadingIndicator()</color>");
#endif

            if (mCurrentLoadingIndicator == null || mCurrentLoadingIndicator.GameObject == null)
            {
                mCurrentLoadingIndicator = new MyUGUILoadingIndicator();
                GameObject go = MyUtilities.FindObjectInFirstLayer(mCanvasOnTopLoadingIndicator, MyUGUILoadingIndicator.PREFAB_NAME);

                if (mCurrentLoadingIndicator.GameObject == null)
                {
                    if (mCoreAssetBundleConfig != null && !string.IsNullOrEmpty(mCoreAssetBundleConfig.URL))
                    {
                        AssetBundle bundle = MyAssetBundleManager.Get(mCoreAssetBundleConfig.URL, mCoreAssetBundleConfig.Version);
                        if (bundle == null)
                        {
                            Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitLoadingIndicator(): Could not get asset bundle which contains Loading Indicator. A template was created instead.");
                            go = MyUGUILoadingIndicator.CreateTemplate();
                        }
                        else
                        {
                            go = Instantiate(bundle.LoadAsset(MyUGUILoadingIndicator.PREFAB_NAME) as GameObject);
                        }
                    }
                    else
                    {
                        go = Instantiate(Resources.Load(SPECIAL_DIRECTORY + MyUGUILoadingIndicator.PREFAB_NAME) as GameObject);
                    }
                }

                mCurrentLoadingIndicator.Initialize(go);
                mCurrentLoadingIndicator.GameObject.name = MyUGUILoadingIndicator.PREFAB_NAME;
                mCurrentLoadingIndicator.GameObject.SetActive(false);
                mCurrentLoadingIndicator.Transform.SetParent(mCanvasOnTopLoadingIndicator.transform, false);
            }
        }

        /// <summary>
        /// Init running text.
        /// </summary>
        private void _InitRunningText(MyUGUIRunningText.EType type = MyUGUIRunningText.EType.Default)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF7777FF>_InitRunningText()</color>: type=" + type);
#endif

            if (mCurrentRunningText != null && mCurrentRunningText.GameObject != null && mCurrentRunningText.Type != type)
            {
                mCurrentRunningText.HideImmedialy();
                mCurrentRunningText = null;
                for (int i = 0, count = mListRunningText.Count; i < count; i++)
                {
                    if (mListRunningText[i].Type == type)
                    {
                        mCurrentRunningText = mListRunningText[i];
                        break;
                    }
                }
            }

            if (mCurrentRunningText == null || mCurrentRunningText.GameObject == null)
            {
                mCurrentRunningText = new MyUGUIRunningText(type);
                mCurrentRunningText.GameObject = MyUtilities.FindObjectInFirstLayer(mCanvasOnTop, MyUGUIRunningText.GetGameObjectName(type));

                if (mCurrentRunningText.GameObject == null)
                {
                    if (mCoreAssetBundleConfig != null && !string.IsNullOrEmpty(mCoreAssetBundleConfig.URL))
                    {
                        AssetBundle bundle = MyAssetBundleManager.Get(mCoreAssetBundleConfig.URL, mCoreAssetBundleConfig.Version);
                        if (bundle == null)
                        {
                            Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitRunningText(): Could not get asset bundle which contains Running Text. A template was created instead.");
                            mCurrentRunningText.GameObject = MyUGUIRunningText.CreateTemplate(type);
                        }
                        else
                        {
                            mCurrentRunningText.GameObject = Instantiate(bundle.LoadAsset(MyUGUIRunningText.GetGameObjectName(type)) as GameObject);
                        }
                    }
                    else
                    {
                        mCurrentRunningText.GameObject = Instantiate(Resources.Load(SPECIAL_DIRECTORY + MyUGUIRunningText.GetGameObjectName(type)) as GameObject);
                    }
                }

                mCurrentRunningText.GameObject.name = MyUGUIRunningText.GetGameObjectName(type);
                mCurrentRunningText.GameObject.SetActive(false);
                mCurrentRunningText.Transform.SetParent(mCanvasOnTop.transform, false);

                mListRunningText.Add(mCurrentRunningText);
            }

            mCurrentRunningText.Transform.SetAsLastSibling();
        }

        /// <summary>
        /// Init toast.
        /// </summary>
        private void _InitToast()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF7777FF>_InitToast()</color>");
#endif

            if (mCurrentToast == null || mCurrentToast.GameObject == null)
            {
                mCurrentToast = new MyUGUIToast();
                mCurrentToast.GameObject = MyUtilities.FindObjectInFirstLayer(mCanvasOnTop, MyUGUIToast.PREFAB_NAME);

                if (mCurrentToast.GameObject == null)
                {
                    if (mCoreAssetBundleConfig != null && !string.IsNullOrEmpty(mCoreAssetBundleConfig.URL))
                    {
                        AssetBundle bundle = MyAssetBundleManager.Get(mCoreAssetBundleConfig.URL, mCoreAssetBundleConfig.Version);
                        if (bundle == null)
                        {
                            Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitToast(): Could not get asset bundle which contains Toast. A template was created instead.");
                            mCurrentToast.GameObject = MyUGUIToast.CreateTemplate();
                        }
                        else
                        {
                            mCurrentToast.GameObject = Instantiate(bundle.LoadAsset(MyUGUIToast.PREFAB_NAME) as GameObject);
                        }
                    }
                    else
                    {
                        mCurrentToast.GameObject = Instantiate(Resources.Load(SPECIAL_DIRECTORY + MyUGUIToast.PREFAB_NAME) as GameObject);
                    }
                }

                mCurrentToast.GameObject.name = MyUGUIToast.PREFAB_NAME;
                mCurrentToast.GameObject.SetActive(false);
                mCurrentToast.Transform.SetParent(mCanvasOnTop.transform, false);
            }

            mCurrentToast.Transform.SetAsLastSibling();
        }

        /// <summary>
        /// Show a popup.
        /// </summary>
        private MyUGUIPopup _ShowPopup(EPopupID popupID, bool isRepeatable, object attachedData, Action onCloseCallback = null)
        {
            MyUGUIPopup popup = null;

            for (int i = mListPopup.Count - 1; i >= 0; i--)
            {
                MyUGUIPopup tmpPopup = mListPopup[i];
                if (tmpPopup == null)
                {
                    mListPopup.RemoveAt(i);
                }
                else if (!isRepeatable && popupID == tmpPopup.ID)
                {
                    popup = tmpPopup;
                }
            }

            if (popup == null && mDictPopupConfig.ContainsKey(popupID))
            {
                MyUGUIConfigPopup popupConfig = mDictPopupConfig[popupID];
                popup = (MyUGUIPopup)Activator.CreateInstance(Type.GetType(popupConfig.ScriptName), popupConfig.ID, popupConfig.PrefabName, false, isRepeatable);
                if (popup != null)
                {
                    popup.SetAssetBundle(popupConfig.AssetBundleURL, popupConfig.AssetBundleVersion);
                }
            }

            if (popup == null)
            {
                Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _ShowPopup(): Could not create popup " + popupID + ". Please re-check popup config.");
            }
            else
            {
                mCurrentPopup = null;

                popup.AttachedData = attachedData;
                popup.OnCloseCallback = onCloseCallback;
                popup.State = popup.State == EBaseState.Idle && popup.IsLoaded ? EBaseState.Enter : EBaseState.LoadAssetBundle;

                _UpdatePopup(ref popup, 0f);

                mListPopup.Add(popup);
            }

            return popup;
        }

        /// <summary>
        /// Show a float popup.
        /// </summary>
        private MyUGUIPopup _ShowFloatPopup(EPopupID popupID, bool isRepeatable, object attachedData)
        {
            MyUGUIPopup popup = null;

            if (mDictPopupConfig.ContainsKey(popupID))
            {
                MyUGUIConfigPopup popupConfig = mDictPopupConfig[popupID];
                popup = (MyUGUIPopup)Activator.CreateInstance(Type.GetType(popupConfig.ScriptName), popupConfig.ID, popupConfig.PrefabName, true, isRepeatable);
                if (popup != null)
                {
                    popup.SetAssetBundle(popupConfig.AssetBundleURL, popupConfig.AssetBundleVersion);
                }
            }

            if (popup == null)
            {
                Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _ShowFloatPopup(): Could not create float popup " + popupID + ". Please re-check popup config.");
            }
            else
            {
                if (!popup.IsRepeatable)
                {
                    mCurrentFloatPopup = null;

                    int countPopup = mListFloatPopup.Count;
                    for (int i = countPopup - 1; i >= 0; i--)
                    {
                        MyUGUIPopup tmpPopup = mListFloatPopup[i];
                        if (tmpPopup == null || (tmpPopup.ID == popup.ID && !tmpPopup.IsRepeatable))
                        {
                            mListFloatPopup.RemoveAt(i);
                        }
                    }
                }

                popup.AttachedData = attachedData;
                popup.State = EBaseState.LoadAssetBundle;

                _UpdateFloatPopup(ref popup, 0f);

                mListFloatPopup.Add(popup);
            }

            return popup;
        }

        /// <summary>
        /// Update unity scene.
        /// </summary>
        private void _UpdateUnityScene()
        {
            if (mCurrentUnityScene == null)
            {
                return;
            }

#if DEBUG_MY_UI
            if (mCurrentUnityScene.State != EUnitySceneState.Update && mCurrentUnityScene.State != EUnitySceneState.Idle)
            {
                Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FFFF00FF>_UpdateUnityScene()</color>: " + mCurrentUnityScene.Name + " - " + mCurrentUnityScene.State);
            }
#endif

            switch (mCurrentUnityScene.State)
            {
                case EUnitySceneState.Unload:
                    {
                        if (mCurrentSceneFading == null || !mCurrentSceneFading.IsFading)
                        {
                            mCurrentPopupOverlay = null;
                            mCurrentRunningText = null;
                            mCurrentToast = null;

                            if (mCurrentUnityScene.HUD != null && mCurrentUnityScene.HUD.IsLoaded)
                            {
                                mCurrentUnityScene.HUD.OnUGUIExit();
                                mCurrentUnityScene.HUD.OnUGUIInvisible();
                            }

                            MyUGUIBase scene;
                            int countScene = mCurrentUnityScene.ListScene.Count;
                            for (int i = 0; i < countScene; i++)
                            {
                                scene = mCurrentUnityScene.ListScene[i];
                                if (scene != null)
                                {
                                    scene.OnUGUIDestroy();
                                }
                            }

                            mListPopup.Clear();
                            mListRunningText.Clear();

                            mUnitySceneUnloadUnusedAsset = Resources.UnloadUnusedAssets();

                            mCurrentUnityScene.State = EUnitySceneState.Unloading;
                        }
                    }
                    break;
                case EUnitySceneState.Unloading:
                    {
                        if (mUnitySceneUnloadUnusedAsset == null || mUnitySceneUnloadUnusedAsset.isDone)
                        {
                            MyUtilities.StartThread(_CollectGC, null);

                            mCurrentUnityScene = mNextUnityScene;
                            mCurrentUnityScene.State = EUnitySceneState.Load;
                            _UpdateUnityScene();
                        }
                    }
                    break;
                case EUnitySceneState.Load:
                    {
                        if (mCoreAssetBundleConfig != null && !string.IsNullOrEmpty(mCoreAssetBundleConfig.URL))
                        {
                            MyAssetBundleManager.Load(mCoreAssetBundleConfig.URL, mCoreAssetBundleConfig.Version, null, MyAssetBundleManager.ECacheMode.UnremovableCache);
                        }

                        if (mCurrentUnityScene.Name != SceneManager.GetActiveScene().name)
                        {
                            mUnitySceneLoad = SceneManager.LoadSceneAsync(mCurrentUnityScene.Name);
                        }

                        mCurrentUnityScene.State = EUnitySceneState.Loading;
                        _UpdateUnityScene();
                    }
                    break;
                case EUnitySceneState.Loading:
                    {
                        if (mUnitySceneLoad == null || mUnitySceneLoad.isDone)
                        {
                            _InitCanvas();
                            _InitCamera();
                            _InitSceneFading();
                            _InitPopupOverlay();

                            mPreviousInitSceneIndex = 0;
                            mCurrentUnityScene.State = EUnitySceneState.PosLoad;
                        }
                    }
                    break;
                case EUnitySceneState.PosLoad:
                    {
                        int INIT_PER_FRAME = 2;

                        int countScene = mCurrentUnityScene.ListScene.Count;
                        int countInit = 0;

                        while (countInit < INIT_PER_FRAME && mPreviousInitSceneIndex < countScene)
                        {
                            if (mPreviousInitSceneIndex < countScene && mCurrentUnityScene.ListScene[mPreviousInitSceneIndex].IsInitWhenLoadUnityScene)
                            {
#if DEBUG_MY_UI
                                Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FFFF00FF>_UpdateUnityScene()</color>: Init " + mCurrentUnityScene.ListScene[mPreviousInitSceneIndex].ID);
#endif

                                mCurrentUnityScene.ListScene[mPreviousInitSceneIndex].OnUGUIInit();
                                countInit++;
                            }

                            mPreviousInitSceneIndex++;
                        }

                        if (mPreviousInitSceneIndex >= countScene)
                        {
                            if (mNextScene != null)
                            {
                                mCurrentScene = mNextScene;
                                mCurrentScene.State = EBaseState.LoadAssetBundle;
                            }

                            mCurrentUnityScene.State = EUnitySceneState.Update;
                        }
                    }
                    break;
                case EUnitySceneState.Update:
                    {
                        if (mNextUnityScene != mCurrentUnityScene)
                        {
                            mCurrentUnityScene.State = EUnitySceneState.Unload;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Update scene.
        /// </summary>
        private void _UpdateScene(float deltaTime)
        {
            if (mCurrentScene == null)
            {
                return;
            }

#if DEBUG_MY_UI
            if (mCurrentScene.State != EBaseState.Update && mCurrentScene.State != EBaseState.Idle)
            {
                Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#00FF00FF>_UpdateScene()</color>: " + mCurrentScene.ID + " - " + mCurrentScene.State);
            }
#endif

            switch (mCurrentScene.State)
            {
                case EBaseState.LoadAssetBundle:
                    {
                        if (mCurrentScene.OnUGUILoadAssetBundle())
                        {
                            mCurrentScene.State = EBaseState.Init;
                            _UpdateScene(deltaTime);
                        }

                        if (mCurrentUnityScene.HUD != null)
                        {
                            if ((mCurrentUnityScene.HUD.GameObject == null) || (!mCurrentUnityScene.HUD.IsLoaded && !mCurrentScene.IsHideHUD))
                            {
                                mCurrentUnityScene.HUD.State = EBaseState.LoadAssetBundle;
                            }
                        }
                    }
                    break;
                case EBaseState.Init:
                    {
                        if (!mCurrentScene.IsLoaded)
                        {
                            mCurrentScene.OnUGUIInit();
                            mCurrentScene.State = EBaseState.Enter;
                        }
                        else
                        {
                            mCurrentScene.State = EBaseState.Enter;
                            _UpdateScene(deltaTime);
                        }
                    }
                    break;
                case EBaseState.Enter:
                    {
                        HideAllPopups(true, false);

                        if (mPreviousScene != null)
                        {
                            if (mPreviousScene.GameObject != null)
                            {
                                mPreviousScene.GameObject.SetActive(false);
                            }
                        }

                        if (mOnScenePreEnterCallback != null)
                        {
                            mOnScenePreEnterCallback();
                            mOnScenePreEnterCallback = null;
                        }

                        mCurrentScene.OnUGUIEnter();

                        if (mOnScenePostEnterCallback != null)
                        {
                            mOnScenePostEnterCallback();
                            mOnScenePostEnterCallback = null;
                        }

                        if (mCurrentUnityScene.HUD != null && mCurrentUnityScene.HUD.State == EBaseState.Update)
                        {
                            mCurrentUnityScene.HUD.GameObject.SetActive(!mCurrentScene.IsHideHUD);
                            if (mCurrentUnityScene.HUD.IsActive)
                            {
                                mCurrentUnityScene.HUD.OnUGUISceneSwitch(mCurrentScene);
                            }
                        }

                        mCurrentScene.State = EBaseState.Visible;
                    }
                    break;
                case EBaseState.Visible:
                    {
                        if (mCurrentScene.OnUGUIVisible())
                        {
                            if (mOnScenePostVisibleCallback != null)
                            {
                                mOnScenePostVisibleCallback();
                                mOnScenePostVisibleCallback = null;
                            }

                            mCurrentScene.State = EBaseState.Update;
                            _UpdateScene(deltaTime);
                        }
                    }
                    break;
                case EBaseState.Update:
                    {
                        if (mNextScene != mCurrentScene)
                        {
                            mCurrentScene.State = EBaseState.Exit;
                            _UpdateScene(deltaTime);
                        }
                        else
                        {
                            mCurrentScene.OnUGUIUpdate(deltaTime);

#if !UNITY_IOS
                            if (mCurrentPopup == null && Input.GetKeyDown(KeyCode.Escape))
                            {
#if DEBUG_MY_UI
                                Debug.Log("[" + mCurrentScene + "] <color=#00FF00FF>OnUGUIBackKey()</color>");
#endif
                                mCurrentScene.OnUGUIBackKey();
                            }
#endif
                        }
                    }
                    break;
                case EBaseState.Exit:
                    {
                        mCurrentScene.OnUGUIExit();
                        mCurrentScene.State = EBaseState.Invisible;
                    }
                    break;
                case EBaseState.Invisible:
                    {
                        if (mCurrentScene.OnUGUIInvisible())
                        {
                            if (mNextScene == null && mCurrentScene.GameObject != null)
                            {
                                mCurrentScene.GameObject.SetActive(false);
                            }

                            if (mIsHideRunningTextWhenSwitchingScene)
                            {
                                HideRunningText();
                                mIsHideRunningTextWhenSwitchingScene = false;
                            }

                            if (mIsHideToastWhenSwitchingScene)
                            {
                                HideToast();
                                mIsHideToastWhenSwitchingScene = false;
                            }

                            mPreviousScene = mCurrentScene;
                            mCurrentScene = mNextScene;
                            mCurrentScene.State = EBaseState.LoadAssetBundle;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Update HUD.
        /// </summary>
        private void _UpdateHUD(float deltaTime)
        {
            if (mCurrentUnityScene == null || mCurrentUnityScene.HUD == null)
            {
                return;
            }

            if (mCurrentScene == null || mCurrentScene.IsHideHUD)
            {
                return;
            }

#if DEBUG_MY_UI
            if (mCurrentUnityScene.HUD.State != EBaseState.Update && mCurrentUnityScene.HUD.State != EBaseState.Idle)
            {
                Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#00FF00FF>_UpdateHUD()</color>: " + " - " + mCurrentUnityScene.HUD.State);
            }
#endif

            switch (mCurrentUnityScene.HUD.State)
            {
                case EBaseState.LoadAssetBundle:
                    {
                        if (mCurrentUnityScene.HUD.OnUGUILoadAssetBundle())
                        {
                            mCurrentUnityScene.HUD.State = EBaseState.Init;
                            _UpdateHUD(deltaTime);
                        }
                    }
                    break;
                case EBaseState.Init:
                    {
                        if (mCurrentUnityScene.HUD.GameObject == null || !mCurrentUnityScene.HUD.IsLoaded)
                        {
                            mCurrentUnityScene.HUD.OnUGUIInit();
                            mCurrentUnityScene.HUD.State = EBaseState.Enter;
                        }
                        else
                        {
                            mCurrentUnityScene.HUD.State = EBaseState.Enter;
                            _UpdateHUD(deltaTime);
                        }
                    }
                    break;
                case EBaseState.Enter:
                    {
                        mCurrentUnityScene.HUD.OnUGUIEnter();
                        mCurrentUnityScene.HUD.OnUGUIVisible();
                        mCurrentUnityScene.HUD.State = EBaseState.Update;
                    }
                    break;
                case EBaseState.Update:
                    {
                        if (mCurrentScene != null && !mCurrentScene.IsHideHUD)
                        {
                            mCurrentUnityScene.HUD.OnUGUIUpdate(deltaTime);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Update popups.
        /// </summary>
        private void _UpdatePopup(ref MyUGUIPopup popup, float deltaTime)
        {
            if (popup == null)
            {
                return;
            }

#if DEBUG_MY_UI
            if (popup.State != EBaseState.Update && popup.State != EBaseState.Idle)
            {
                Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#00FF00FF>_UpdatePopup()</color>: " + popup.ID + " - " + popup.State);
            }
#endif

            if (mCurrentPopup == null)
            {
                mCurrentPopup = popup;
            }

            switch (popup.State)
            {
                case EBaseState.LoadAssetBundle:
                    {
                        if (popup.OnUGUILoadAssetBundle())
                        {
                            popup.State = EBaseState.Init;
                            _UpdatePopup(ref popup, deltaTime);
                        }
                    }
                    break;
                case EBaseState.Init:
                    {
                        if (!popup.IsLoaded)
                        {
                            popup.OnUGUIInit();
                        }
                        popup.State = EBaseState.Enter;
                    }
                    break;
                case EBaseState.Enter:
                    {
                        mCurrentPopup = popup;

                        popup.OnUGUIEnter();
                        popup.State = EBaseState.Visible;

                        UpdatePopupOverlay();
                    }
                    break;
                case EBaseState.Visible:
                    {
                        if (popup.OnUGUIVisible())
                        {
                            popup.State = EBaseState.Update;
                            _UpdatePopup(ref popup, deltaTime);
                        }
                    }
                    break;
                case EBaseState.Update:
                    {
                        popup.OnUGUIUpdate(deltaTime);

#if !UNITY_IOS
                        if (popup == mCurrentPopup && Input.GetKeyDown(KeyCode.Escape))
                        {
#if DEBUG_MY_UI
                            Debug.Log("[" + mCurrentPopup + "] <color=#00FF00FF>OnUGUIBackKey()</color>");
#endif
                            mCurrentPopup.OnUGUIBackKey();
                        }
#endif
                    }
                    break;
                case EBaseState.Exit:
                    {
                        popup.OnUGUIExit();
                        popup.State = EBaseState.Invisible;
                    }
                    break;
                case EBaseState.Invisible:
                    {
                        if (popup.OnUGUIInvisible())
                        {
                            if (mCurrentPopup.ID == popup.ID)
                            {
                                mCurrentPopup = null;
                            }

                            if (popup.GameObject != null)
                            {
                                if (popup.IsRepeatable)
                                {
                                    Destroy(popup.GameObject);
                                }
                                else
                                {
                                    popup.GameObject.SetActive(false);
                                }
                            }

                            if (popup.IsRetainable)
                            {
                                popup.State = EBaseState.Idle;
                            }
                            else
                            {
                                popup = null;
                            }
                        }

                        UpdatePopupOverlay();
                    }
                    break;
            }
        }

        /// <summary>
        /// Update float popups.
        /// </summary>
        private void _UpdateFloatPopup(ref MyUGUIPopup popup, float deltaTime)
        {
            if (popup == null)
            {
                return;
            }

#if DEBUG_MY_UI
            if (popup.State != EBaseState.Update && popup.State != EBaseState.Idle)
            {
                Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#00FF00FF>_UpdateFloatPopup()</color>: " + popup.ID + " - " + popup.State);
            }
#endif

            if (mCurrentFloatPopup == null)
            {
                mCurrentFloatPopup = popup;
            }

            switch (popup.State)
            {
                case EBaseState.Init:
                    {
                        if (!popup.IsLoaded)
                        {
                            popup.OnUGUIInit();
                        }
                        popup.State = EBaseState.Enter;
                    }
                    break;
                case EBaseState.Enter:
                    {
                        popup.OnUGUIEnter();
                        popup.State = EBaseState.Visible;

                        UpdatePopupOverlay();
                    }
                    break;
                case EBaseState.Visible:
                    {
                        if (popup.OnUGUIVisible())
                        {
                            popup.State = EBaseState.Update;
                            _UpdatePopup(ref popup, deltaTime);
                        }
                    }
                    break;
                case EBaseState.Update:
                    {
                        popup.OnUGUIUpdate(deltaTime);
                    }
                    break;
                case EBaseState.Exit:
                    {
                        popup.OnUGUIExit();
                        popup.State = EBaseState.Invisible;
                    }
                    break;
                case EBaseState.Invisible:
                    {
                        if (popup.OnUGUIInvisible())
                        {
                            if (mCurrentFloatPopup.ID == popup.ID)
                            {
                                mCurrentFloatPopup = null;
                            }

                            if (popup.GameObject != null)
                            {
                                popup.GameObject.SetActive(false);
                            }
                            popup = null;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Updates the popup overlay.
        /// </summary>
        /// <returns>The popup overlay.</returns>
        private IEnumerator _DoUpdatePopupOverlay()
        {
            if (IsShowingLoadingIndicator)
            {
                mCurrentPopupOverlay.Transform.SetParent(mCanvasOnTopLoadingIndicator.transform, false);
                mCurrentPopupOverlay.Transform.SetAsFirstSibling();
                mCurrentPopupOverlay.Show();
            }
            else
            {
                mCurrentPopupOverlay.Transform.SetParent(mCanvasOnTopPopup.transform, false);
                for (int i = mCanvasOnTopPopup.transform.childCount - 1; i >= 0; i--)
                {
                    GameObject popup = mCanvasOnTopPopup.transform.GetChild(i).gameObject;
                    if (popup.activeSelf && !popup.name.Equals(mCurrentPopupOverlay.GameObject.name))
                    {
                        mCurrentPopupOverlay.Transform.SetSiblingIndex(i - 1);
                        mCurrentPopupOverlay.Show();
                        break;
                    }
                }
            }

            yield return null;

            if (IsShowingLoadingIndicator)
            {
                mCurrentPopupOverlay.Transform.SetParent(mCanvasOnTopLoadingIndicator.transform, false);
                mCurrentPopupOverlay.Transform.SetAsFirstSibling();
                mCurrentPopupOverlay.Show();
            }
            else if (mCurrentPopupOverlay != null)
            {
                mCurrentPopupOverlay.Transform.SetParent(mCanvasOnTopPopup.transform, false);

                bool isHasActivedPopup = false;
                foreach (MyUGUIPopup popup in mListPopup)
                {
                    if (popup != null && popup.State != EBaseState.Init && popup.State != EBaseState.Idle)
                    {
                        isHasActivedPopup = true;
                        break;
                    }
                }
                if (isHasActivedPopup)
                {
                    for (int i = mCanvasOnTopPopup.transform.childCount - 1; i >= 0; i--)
                    {
                        GameObject popup = mCanvasOnTopPopup.transform.GetChild(i).gameObject;
                        if (popup.activeSelf && !popup.name.Equals(mCurrentPopupOverlay.GameObject.name))
                        {
                            if (i == 0)
                            {
                                mCurrentPopupOverlay.Transform.SetAsFirstSibling();
                            }
                            else
                            {
                                mCurrentPopupOverlay.Transform.SetSiblingIndex(i - 1);
                            }
                            mCurrentPopupOverlay.Show();
                            break;
                        }
                    }
                }
                else
                {
                    mCurrentPopupOverlay.Transform.SetAsFirstSibling();
                    mCurrentPopupOverlay.Hide();
                }
            }
        }

        /// <summary>
        /// Call GC.
        /// </summary>
        private void _CollectGC()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] _CollectGC()");
#endif

            GC.Collect();
        }

        /// <summary>
        /// Add a new scene to scene stack.
        /// </summary>
        private void _AddSceneIntoSceneStack(MyUGUIScene scene)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] _AddSceneIntoSceneStack(): UnitySceneID=" + scene.UnitySceneID + " - SceneID=" + scene.ID);
#endif

#if DEBUG_MY_UI
            string stringScene = " ";
            for (int j = mListScene.Count - 1; j >= 0; j--)
            {
                stringScene = " " + mListScene[j].ID + stringScene;
            }
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] _AddSceneIntoSceneStack(): before={" + stringScene + "}");
#endif

            int countScene = mListScene.Count;
            for (int i = 0; i < countScene; i++)
            {
                if (mListScene[i].ID == scene.ID && mListScene[i].UnitySceneID == scene.UnitySceneID)
                {
                    for (int j = countScene - 1; j >= i; j--)
                    {
                        mListScene.RemoveAt(j);
                    }
                    break;
                }
            }
            mListScene.Add(scene);

#if DEBUG_MY_UI
            stringScene = " ";
            for (int j = mListScene.Count - 1; j >= 0; j--)
            {
                stringScene = " " + mListScene[j].ID + stringScene;
            }
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] _AddSceneIntoSceneStack(): after={" + stringScene + "}");
#endif
        }

        #endregion
    }

    #region ----- Enumeration -----

    public enum EUnitySceneState
    {
        Idle,
        Unload,
        Unloading,
        Load,
        Loading,
        PosLoad,
        Update,
    }

    public enum EBaseState
    {
        Idle,
        LoadAssetBundle,
        Init,
        Enter,
        Visible,
        Update,
        Exit,
        Invisible
    }

    public enum ERunningTextSpeed
    {
        VerySlow = 50,
        Slow = 100,
        Normal = 150,
        Fast = 200,
        VeryFast = 300
    }

    public enum EToastDuration
    {
        Short = 2,
        Medium = 3,
        Long = 5
    }

    #endregion
}