/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUnityAdsManager (version 1.0)
 */

#pragma warning disable 0162
#pragma warning disable 0414

#if USE_MY_UNITY_ADS

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Advertisements;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyClasses
{
    public class MyUnityAdsManager : MonoBehaviour, IUnityAdsListener
    {
        #region ----- Variable -----

        [SerializeField]
        private string mAndroidGameId = string.Empty;
        [SerializeField]
        private string mIosGameId = string.Empty;
        [SerializeField]
        private bool mIsTestMode = true;

        private Dictionary<string, Action[]> mDictCallbacks = new Dictionary<string, Action[]>();
        private bool mIsShowBanner = false;

        #endregion

        #region ----- Singleton -----

        private static object mSingletonLock = new object();
        private static MyUnityAdsManager mInstance;

        public static MyUnityAdsManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mSingletonLock)
                    {
                        mInstance = (MyUnityAdsManager)FindObjectOfType(typeof(MyUnityAdsManager));
                        if (mInstance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyUnityAdsManager).Name);
                            mInstance = obj.AddComponent<MyUnityAdsManager>();
                        }
                        DontDestroyOnLoad(mInstance);
                    }
                }
                return mInstance;
            }
        }

        #endregion

        #region ----- IUnityAdsListener -----

        /// <summary>
        /// OnUnityAdsReady.
        /// </summary>
        public void OnUnityAdsReady(string placementId)
        {
#if DEBUG_MY_UNITY_ADS
            Debug.Log("[" + typeof(MyUnityAdsManager).Name + "] OnUnityAdsReady(): placementId=" + placementId);
#endif
        }

        /// <summary>
        /// OnUnityAdsDidError.
        /// </summary>
        public void OnUnityAdsDidError(string message)
        {
#if DEBUG_MY_UNITY_ADS
            Debug.LogError("[" + typeof(MyUnityAdsManager).Name + "] OnUnityAdsReady(): message=" + message);
#endif
        }

        /// <summary>
        /// OnUnityAdsDidStart.
        /// </summary>
        public void OnUnityAdsDidStart(string placementId)
        {
#if DEBUG_MY_UNITY_ADS
            Debug.Log("[" + typeof(MyUnityAdsManager).Name + "] OnUnityAdsDidStart(): placementId=" + placementId);
#endif

            if (mDictCallbacks.ContainsKey(placementId) && mDictCallbacks[placementId][3] != null)
            {
                mDictCallbacks[placementId][3]();
            }
        }

        /// <summary>
        /// OnUnityAdsDidFinish.
        /// </summary>
        public void OnUnityAdsDidFinish(string placementId, UnityEngine.Advertisements.ShowResult showResult)
        {
#if DEBUG_MY_UNITY_ADS
            Debug.Log("[" + typeof(MyUnityAdsManager).Name + "] OnUnityAdsDidFinish(): placementId=" + placementId + " | showResult=" + showResult.ToString());
#endif

            switch (showResult)
            {
                case ShowResult.Failed:
                    {
                        if (mDictCallbacks.ContainsKey(placementId) && mDictCallbacks[placementId][0] != null)
                        {
                            mDictCallbacks[placementId][0]();
                        }
                    }
                    break;

                case ShowResult.Skipped:
                    {
                        if (mDictCallbacks.ContainsKey(placementId) && mDictCallbacks[placementId][1] != null)
                        {
                            mDictCallbacks[placementId][1]();
                        }
                    }
                    break;

                case ShowResult.Finished:
                    {
                        if (mDictCallbacks.ContainsKey(placementId) && mDictCallbacks[placementId][2] != null)
                        {
                            mDictCallbacks[placementId][2]();
                        }
                    }
                    break;
            }

            if (mDictCallbacks.ContainsKey(placementId))
            {
                mDictCallbacks.Remove(placementId);
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Initialize.
        /// </summary>
        public void Initialize()
        {
#if DEBUG_MY_UNITY_ADS
            Debug.Log("[" + typeof(MyUnityAdsManager).Name + "] Initialize(): mAndroidGameId=" + mAndroidGameId + " | mIosGameId=" + mIosGameId + " | mIsTestMode=" + mIsTestMode);
#endif

#if UNITY_ANDROID
            Advertisement.AddListener(this);
            Advertisement.Initialize(mAndroidGameId, mIsTestMode);
#elif UNITY_IOS
            Advertisement.AddListener(this);
            Advertisement.Initialize(mIosGameId, mIsTestMode);
#endif
        }

        /// <summary>
        /// Check if a interstitial video is ready.
        /// </summary>
        public bool IsInterstitialVideoReady(string placementId = "video")
        {
            return Advertisement.IsReady(placementId);
        }

        /// <summary>
        /// Show an interstitial video.
        /// </summary>
        public void ShowInterstitialVideo(Action onStartedCallback = null, Action onFinishedCallback = null, Action onFailedCallback = null, Action onSkippedCallback = null)
        {
            ShowInterstitialVideoByPlacementId("video", onStartedCallback, onFinishedCallback, onFailedCallback, onSkippedCallback);
        }

        /// <summary>
        /// Show an interstitial video.
        /// </summary>
        public void ShowInterstitialVideoByPlacementId(string placementId, Action onStartedCallback = null, Action onFinishedCallback = null, Action onFailedCallback = null, Action onSkippedCallback = null)
        {
#if DEBUG_MY_UNITY_ADS
            Debug.Log("[" + typeof(MyUnityAdsManager).Name + "] ShowInterstitialVideoByPlacementId(): placementId=" + placementId);
#endif

#if UNITY_ANDROID || UNITY_IOS
            _ShowVideo(placementId, onStartedCallback, onFinishedCallback, onFailedCallback, onSkippedCallback);
#endif
        }

        /// <summary>
        /// Check if a rewarded video is ready.
        /// </summary>
        public bool IsRewardedVideoReady(string placementId = "rewardedVideo")
        {
            return Advertisement.IsReady(placementId);
        }

        /// <summary>
        /// Show an rewarded video.
        /// </summary>
        public void ShowRewardedVideo(Action onStartedCallback = null, Action onFinishedCallback = null, Action onFailedCallback = null, Action onSkippedCallback = null)
        {
            ShowRewardedVideoByPlacementId("rewardedVideo", onStartedCallback, onFinishedCallback, onFailedCallback, onSkippedCallback);
        }

        /// <summary>
        /// Show a rewarded video.
        /// </summary>
        public void ShowRewardedVideoByPlacementId(string placementId, Action onStartedCallback = null, Action onFinishedCallback = null, Action onFailedCallback = null, Action onSkippedCallback = null)
        {
#if DEBUG_MY_UNITY_ADS
            Debug.Log("[" + typeof(MyUnityAdsManager).Name + "] ShowRewardedVideoByPlacementId(): placementId=" + placementId);
#endif

#if UNITY_ANDROID || UNITY_IOS
            _ShowVideo(placementId, onStartedCallback, onFinishedCallback, onFailedCallback, onSkippedCallback);
#endif
        }

        /// <summary>
        /// Show a banner.
        /// </summary>
        public void ShowBanner(BannerPosition position, string placementId = "banner")
        {
#if DEBUG_MY_UNITY_ADS
            Debug.Log("[" + typeof(MyUnityAdsManager).Name + "] ShowBanner(): placementId=" + placementId + " | position=" + position.ToString());
#endif

#if UNITY_ANDROID || UNITY_IOS
            mIsShowBanner = true;
            StartCoroutine(_DoShowBanner(position, placementId));
#endif
        }

        /// <summary>
        /// Hide the banner.
        /// </summary>
        public void HideBanner()
        {
#if DEBUG_MY_UNITY_ADS
            Debug.Log("[" + typeof(MyUnityAdsManager).Name + "] HideBanner()");
#endif

#if UNITY_ANDROID || UNITY_IOS
            if (mIsShowBanner)
            {
                mIsShowBanner = false;
                Advertisement.Banner.Hide();
            }
#endif
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Show a video.
        /// </summary>
        private void _ShowVideo(string placementId = "video", Action onStartedCallback = null, Action onFinishedCallback = null, Action onFailedCallback = null, Action onSkippedCallback = null)
        {
#if UNITY_ANDROID || UNITY_IOS
            if (onStartedCallback != null || onFinishedCallback != null || onFailedCallback != null || onSkippedCallback != null)
            {
                mDictCallbacks[placementId] = new Action[4];
                mDictCallbacks[placementId][0] = onFailedCallback;
                mDictCallbacks[placementId][1] = onSkippedCallback;
                mDictCallbacks[placementId][2] = onFinishedCallback;
                mDictCallbacks[placementId][3] = onStartedCallback;
            }
            else if (mDictCallbacks.ContainsKey(placementId))
            {
                mDictCallbacks.Remove(placementId);
            }

            if (Advertisement.IsReady(placementId))
            {
                Advertisement.Show(placementId);
            }
            else
            {
                StartCoroutine(_DoShowVideo(placementId));
            }
#endif
        }

        /// <summary>
        /// Show a video.
        /// </summary>
        private IEnumerator _DoShowVideo(string placementId = "video")
        {
            while (!Advertisement.IsReady(placementId))
            {
                yield return null;
            }

            Advertisement.Show(placementId);
        }

        /// <summary>
        /// Show a banner.
        /// </summary>
        private IEnumerator _DoShowBanner(BannerPosition position, string placementId = "banner")
        {
            while (!Advertisement.IsReady(placementId) && mIsShowBanner)
            {
                yield return null;
            }

            if (mIsShowBanner)
            {
                Advertisement.Banner.SetPosition(position);
                Advertisement.Banner.Show(placementId);
            }
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUnityAdsManager))]
    public class MyUnityAdsManagerEditor : Editor
    {
        private MyUnityAdsManager mScript;
        private SerializedProperty mAndroidGameId;
        private SerializedProperty mIosGameId;
        private SerializedProperty mIsTestMode;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUnityAdsManager)target;
            mAndroidGameId = serializedObject.FindProperty("mAndroidGameId");
            mIosGameId = serializedObject.FindProperty("mIosGameId");
            mIsTestMode = serializedObject.FindProperty("mIsTestMode");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUnityAdsManager), false);

            serializedObject.Update();

            mIosGameId.stringValue = EditorGUILayout.TextField("IOS Game ID", mIosGameId.stringValue);
            mAndroidGameId.stringValue = EditorGUILayout.TextField("Android Game ID", mAndroidGameId.stringValue);
            mIsTestMode.boolValue = EditorGUILayout.Toggle("Enable Test Mode", mIsTestMode.boolValue);

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}

#endif