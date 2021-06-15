/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyAdMobManager (version 1.11)
 * Require:     GoogleMobileAds-v6.0.0
 */

#pragma warning disable 0162
#pragma warning disable 0414

#if USE_MY_ADMOB
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;
using GoogleMobileAds.Api;
using System.Collections.Generic;

#if USE_MY_ADMOB_APPLOVIN
using GoogleMobileAds.Api.Mediation.AppLovin;
#endif

#if USE_MY_ADMOB_MOPUB
using GoogleMobileAds.Api.Mediation.MoPub;
#endif

#if USE_MY_ADMOB_UNITY_ADS
using GoogleMobileAds.Api.Mediation.UnityAds;
#endif

#if USE_MY_ADMOB_VUNGLE
using GoogleMobileAds.Api.Mediation.Vungle;
#endif

namespace MyClasses
{
    public class MyAdMobManager : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private bool mTestEnable = true;
        [SerializeField]
        private bool mTestUseGoogleAdsId = true;
        [SerializeField]
        private string mTestDeviceId = "19CEAA9BB2BC2D8C16D6D202AE2A6C1E";
        [SerializeField]
        private string mAndroidDefaultBannerId = string.Empty;
        [SerializeField]
        private string mIosDefaultBannerId = string.Empty;
        [SerializeField]
        private long mLastBannerShowTimestamp = 0;
        [SerializeField]
        private bool mIsBannerShowing = false;
        [SerializeField]
        private string mAndroidDefaultInterstitialAdId = string.Empty;
        [SerializeField]
        private string mIosDefaultInterstitialAdId = string.Empty;
        [SerializeField]
        private long mLastInterstitialShowTimestamp = 0;
        [SerializeField]
        private bool mIsLoadingInterstitial = false;
        [SerializeField]
        private string mAndroidDefaultRewardedAdId = string.Empty;
        [SerializeField]
        private string mIosDefaultRewardedAdId = string.Empty;
        [SerializeField]
        private long mLastRewardedShowTimestamp = 0;
        [SerializeField]
        private bool mIsLoadingRewardred = false;
        [SerializeField]
        private bool mIsHasReward = false;

        private BannerView mBanner;
        private AdRequest mBannerRequest;
        private Action mOnBannerLoadedCallback;
        private Action mOnBannerFailedToLoadCallback;
        private Action mOnBannerOpeningCallback;
        private Action mOnBannerClosedCallback;

        private InterstitialAd mInterstitialAd;
        private Action mOnInterstitialAdLoadedCallback;
        private Action mOnInterstitialAdFailedToLoadCallback;
        private Action mOnInterstitialAdOpeningCallback;
        private Action mOnInterstitialAdClosedCallback;

        private RewardedAd mRewardedAd;
        private Action mOnRewardedAdLoadedCallback;
        private Action mOnRewardedAdFailedToLoadCallback;
        private Action mOnRewardedAdOpeningCallback;
        private Action mOnRewardedAdFailedToShowCallback;
        private Action mOnRewardedAdUserEarnedRewardCallback;
        private Action mOnRewardedAdSkippedCallback;
        private Action mOnRewardedAdClosedCallback;

        public long LastBannerShowTimestamp
        {
            get { return mLastBannerShowTimestamp; }
            set { mLastBannerShowTimestamp = value; }
        }

        public long LastInterstitialShowTimestamp
        {
            get { return mLastInterstitialShowTimestamp; }
            set { mLastInterstitialShowTimestamp = value; }
        }

        public long LastRewardedShowTimestamp
        {
            get { return mLastRewardedShowTimestamp; }
            set { mLastRewardedShowTimestamp = value; }
        }

        #endregion

        #region ----- Singleton -----

        private static object mSingletonLock = new object();
        private static MyAdMobManager mInstance;

        public static MyAdMobManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mSingletonLock)
                    {
                        mInstance = (MyAdMobManager)FindObjectOfType(typeof(MyAdMobManager));
                        if (mInstance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyAdMobManager).Name);
                            mInstance = obj.AddComponent<MyAdMobManager>();
                        }
                        DontDestroyOnLoad(mInstance);
                    }
                }
                return mInstance;
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Initialize.
        /// </summary>
        public void Initialize(Action onCompleteCallback = null)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] Initialize()");
#endif

            if (mTestEnable && mTestDeviceId.Length > 0)
            {
                if (mTestUseGoogleAdsId)
                {
                    mAndroidDefaultBannerId = "ca-app-pub-3940256099942544/6300978111";
                    mAndroidDefaultInterstitialAdId = "ca-app-pub-3940256099942544/1033173712";
                    mAndroidDefaultRewardedAdId = "ca-app-pub-3940256099942544/5224354917";

                    mIosDefaultBannerId = "ca-app-pub-3940256099942544/2934735716";
                    mIosDefaultInterstitialAdId = "ca-app-pub-3940256099942544/4411468910";
                    mIosDefaultRewardedAdId = "ca-app-pub-3940256099942544/1712485313";

#if UNITY_ANDROID
                    PlayerPrefs.SetString("MyAdMobManager_BannerId", mAndroidDefaultBannerId);
                    PlayerPrefs.SetString("MyAdMobManager_InterstitialAdId", mAndroidDefaultInterstitialAdId);
                    PlayerPrefs.SetString("MyAdMobManager_RewardedAdId", mAndroidDefaultRewardedAdId);
#elif UNITY_IOS
                    PlayerPrefs.SetString("MyAdMobManager_BannerId", mIosDefaultBannerId);
                    PlayerPrefs.SetString("MyAdMobManager_InterstitialAdId", mIosDefaultInterstitialAdId);
                    PlayerPrefs.SetString("MyAdMobManager_RewardedAdId", mIosDefaultRewardedAdId);
#endif
                }

                List<string> deviceIds = new List<string>();
                deviceIds.AddRange(mTestDeviceId.Split(';'));
                RequestConfiguration requestConfiguration = new RequestConfiguration.Builder().SetTestDeviceIds(deviceIds).build();
                MobileAds.SetRequestConfiguration(requestConfiguration);
            }

            MobileAds.Initialize(initStatus =>
            {
#if DEBUG_MY_ADMOB
                Debug.Log("[" + typeof(MyAdMobManager).Name + "] Initialize(): completed");
#endif

                if (onCompleteCallback != null)
                {
                    onCompleteCallback();
                }
            });

#if USE_MY_ADMOB_APPLOVIN
            AppLovin.Initialize();
            AppLovin.SetHasUserConsent(true);
#endif

#if USE_MY_ADMOB_MOPUB
            MoPub.InitializeSdk("8c09b6f2cb324838acf2fdad6899f5a8");
#endif

#if USE_MY_ADMOB_UNITY_ADS
            UnityAds.SetGDPRConsentMetaData(true);
#endif

#if USE_MY_ADMOB_VUNGLE
            Vungle.UpdateConsentStatus(VungleConsent.ACCEPTED);
#endif
        }

        /// <summary>
        /// Set a new banner id.
        /// </summary>
        public void SetDefaultBannerId(string id)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] SetDefaultBannerId(): id=" + id);
#endif

            if (mTestEnable && mTestUseGoogleAdsId)
            {
                return;
            }

#if UNITY_ANDROID
            PlayerPrefs.SetString("MyAdMobManager_BannerId", string.IsNullOrEmpty(id) ? mAndroidDefaultBannerId : id);
            PlayerPrefs.Save();
#elif UNITY_IOS
            PlayerPrefs.SetString("MyAdMobManager_BannerId", string.IsNullOrEmpty(id) ? mIosDefaultBannerId : id);
            PlayerPrefs.Save();
#endif
        }

        /// <summary>
        /// Check if the banner is showing.
        /// </summary>
        public bool IsBannerShowing()
        {
            return mIsBannerShowing;
        }

        /// <summary>
        /// Show a banner.
        /// </summary>
        public void ShowBanner(string adUnitId = null, AdSize size = null, AdPosition position = AdPosition.Bottom, Action onLoadedCallback = null, Action onFailedToLoadCallback = null, Action onOpeningCallback = null, Action onClosedCallback = null)
        {
            if (adUnitId == null)
            {
#if UNITY_ANDROID
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_BannerId", mAndroidDefaultBannerId);
#elif UNITY_IOS
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_BannerId", mIosDefaultBannerId);
#endif
            }

            if (size == null)
            {
                size = AdSize.Banner;
            }

#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] ShowBanner(): adUnitId=" + adUnitId + " | size=" + size.AdType + " | position=" + position);
#endif

            mOnBannerLoadedCallback = onLoadedCallback;
            mOnBannerFailedToLoadCallback = onFailedToLoadCallback;
            mOnBannerOpeningCallback = onOpeningCallback;
            mOnBannerClosedCallback = onClosedCallback;

            mLastBannerShowTimestamp = MyLocalTime.CurrentUnixTime;

            if (mBanner == null)
            {
                mBanner = new BannerView(adUnitId, size, position);
                mBannerRequest = new AdRequest.Builder().Build();
            }
            mBanner.LoadAd(mBannerRequest);
            mBanner.OnAdLoaded += _OnBannerLoaded;
            mBanner.OnAdFailedToLoad += _OnBannerFaiedToLoad;
            mBanner.OnAdOpening += _OnBannerOpening;
            mBanner.OnAdClosed += _OnBannerClosed;
        }

        /// <summary>
        /// Hide the banner.
        /// </summary>
        public void HideBanner()
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] HideBanner()");
#endif

            mOnBannerLoadedCallback = null;
            mOnBannerFailedToLoadCallback = null;
            mOnBannerOpeningCallback = null;
            mOnBannerClosedCallback = null;

            if (mBanner != null)
            {
                mBanner.Hide();
            }
        }

        /// <summary>
        /// Set a new interstitial ad id.
        /// </summary>
        public void SetDefaultInterstitialAdId(string id)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] SetDefaultInterstitialAdId(): id=" + id);
#endif

#if UNITY_ANDROID
            PlayerPrefs.SetString("MyAdMobManager_InterstitialAdId", string.IsNullOrEmpty(id) ? mAndroidDefaultInterstitialAdId : id);
            PlayerPrefs.Save();
#elif UNITY_IOS
            PlayerPrefs.SetString("MyAdMobManager_InterstitialAdId", string.IsNullOrEmpty(id) ? mIosDefaultInterstitialAdId : id);
            PlayerPrefs.Save();
#endif
        }

        /// <summary>
        /// Check if a interstitial ad is loading.
        /// </summary>
        public bool IsInterstitialAdLoading()
        {
#if UNITY_EDITOR
            return false;
#else
            return mInterstitialAd == null || (mIsLoadingInterstitial && !mInterstitialAd.IsLoaded());
#endif
        }

        /// <summary>
        /// Check if an interstitial ad is ready.
        /// </summary>
        public bool IsInterstitialAdLoaded()
        {
            return mInterstitialAd != null && mInterstitialAd.IsLoaded();
        }

        /// <summary>
        /// Load an interstitial ad.
        /// </summary>
        public void LoadInterstitialAd(string adUnitId = null, Action onLoadedCallback = null, Action onFailedToLoadCallback = null)
        {
            if (adUnitId == null)
            {
#if UNITY_ANDROID
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_InterstitialAdId", mAndroidDefaultInterstitialAdId);
#elif UNITY_IOS
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_InterstitialAdId", mIosDefaultInterstitialId);
#endif
            }

#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] LoadInterstitialAd(): adUnitId=" + adUnitId);
#endif

            mOnInterstitialAdLoadedCallback = onLoadedCallback;
            mOnInterstitialAdFailedToLoadCallback = onFailedToLoadCallback;
            mOnInterstitialAdOpeningCallback = null;
            mOnInterstitialAdClosedCallback = null;

            mIsLoadingInterstitial = true;

            mInterstitialAd = new InterstitialAd(adUnitId);
            mInterstitialAd.LoadAd(new AdRequest.Builder().Build());
            mInterstitialAd.OnAdLoaded += _OnInterstitialLoaded;
            mInterstitialAd.OnAdFailedToLoad += _OnInterstitialFaiedToLoad;
            mInterstitialAd.OnAdOpening += _OnInterstitialOpening;
            mInterstitialAd.OnAdClosed += _OnInterstitialClosed;
        }

        /// <summary>
        /// Show an interstitial ad.
        /// </summary>
        public void ShowInterstitialAd(Action onOpeningCallback = null, Action onLeavingApplicationCallback = null, Action onClosedCallback = null)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] ShowInterstitialAd(): isLoaded=" + IsInterstitialAdLoaded());
#endif

            if (IsInterstitialAdLoaded())
            {
                mOnInterstitialAdLoadedCallback = null;
                mOnInterstitialAdFailedToLoadCallback = null;
                mOnInterstitialAdOpeningCallback = onOpeningCallback;
                mOnInterstitialAdClosedCallback = onClosedCallback;

                mLastInterstitialShowTimestamp = MyLocalTime.CurrentUnixTime;

                mInterstitialAd.Show();
            }
        }

        /// <summary>
        /// Set a new rewarded ad id.
        /// </summary>
        public void SetDefaultRewardedAdId(string id)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] SetDefaultRewardedAdId(): id=" + id);
#endif

#if UNITY_ANDROID
            PlayerPrefs.SetString("MyAdMobManager_RewardedAdId", string.IsNullOrEmpty(id) ? mAndroidDefaultRewardedAdId : id);
            PlayerPrefs.Save();
#elif UNITY_IOS
            PlayerPrefs.SetString("MyAdMobManager_RewardedAdId", string.IsNullOrEmpty(id) ? mIosDefaultRewardedAdId : id);
            PlayerPrefs.Save();
#endif
        }

        /// <summary>
        /// Check if a rewarded ad is loading.
        /// </summary>
        public bool IsRewardedAdLoading()
        {
#if UNITY_EDITOR
            return false;
#else
            return mRewardedAd == null || (mIsLoadingRewardred && !mRewardedAd.IsLoaded());
#endif
        }

        /// <summary>
        /// Check if a rewarded ad is ready.
        /// </summary>
        public bool IsRewardedAdLoaded()
        {
            return mRewardedAd != null && mRewardedAd.IsLoaded();
        }

        /// <summary>
        /// Load a rewarded ad.
        /// </summary>
        public void LoadRewardedAd(string adUnitId = null, Action onLoadedCallback = null, Action onFailedToLoadCallback = null)
        {
            if (adUnitId == null)
            {
#if UNITY_ANDROID
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_RewardedAdId", mAndroidDefaultRewardedAdId);
#elif UNITY_IOS
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_RewardedAdId", mIosDefaultRewardedAdId);
#endif
            }

#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] LoadRewardedAd(): adUnitId=" + adUnitId);
#endif

            mOnRewardedAdLoadedCallback = onLoadedCallback;
            mOnRewardedAdFailedToLoadCallback = onFailedToLoadCallback;
            mOnRewardedAdOpeningCallback = null;
            mOnRewardedAdFailedToShowCallback = null;
            mOnRewardedAdUserEarnedRewardCallback = null;
            mOnRewardedAdSkippedCallback = null;
            mOnRewardedAdClosedCallback = null;

            mIsLoadingRewardred = true;

            mRewardedAd = new RewardedAd(adUnitId);
            mRewardedAd.LoadAd(new AdRequest.Builder().Build());
            mRewardedAd.OnAdLoaded += _OnRewardedAdLoaded;
            mRewardedAd.OnAdOpening += _OnRewardedAdOpening;
            mRewardedAd.OnAdFailedToShow += _OnRewardedAdFaiedToShow;
            mRewardedAd.OnUserEarnedReward += _OnRewardedAdUserEarnedReward;
            mRewardedAd.OnAdClosed += _OnRewardedAdClosed;
        }

        /// <summary>
        /// Show an RewardedAd.
        /// </summary>
        public void ShowRewardedAd(Action onOpeningCallback = null, Action onUserEarnedRewardCallback = null, Action onFailedToShowCallback = null, Action onSkippedCallback = null, Action onClosedCallback = null)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] ShowRewardedAd(): isLoaded=" + IsRewardedAdLoaded());
#endif

            if (IsRewardedAdLoaded())
            {
                mOnRewardedAdLoadedCallback = null;
                mOnRewardedAdFailedToLoadCallback = null;
                mOnRewardedAdOpeningCallback = onOpeningCallback;
                mOnRewardedAdFailedToShowCallback = onFailedToShowCallback;
                mOnRewardedAdUserEarnedRewardCallback = onUserEarnedRewardCallback;
                mOnRewardedAdSkippedCallback = onSkippedCallback;
                mOnRewardedAdClosedCallback = onClosedCallback;

                mLastRewardedShowTimestamp = MyLocalTime.CurrentUnixTime;
                mIsHasReward = false;

                mRewardedAd.Show();
            }
        }

        #endregion

        #region ----- Banner Event -----

        private void _OnBannerLoaded(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerLoaded()");
#endif

            if (mOnBannerLoadedCallback != null)
            {
                mOnBannerLoadedCallback();
            }
        }

        private void _OnBannerFaiedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnBannerFaiedToLoad(): message=" + args.Message);
#endif

            if (mOnBannerFailedToLoadCallback != null)
            {
                mOnBannerFailedToLoadCallback();
            }
        }

        private void _OnBannerOpening(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerOpening()");
#endif

            mLastBannerShowTimestamp = MyLocalTime.CurrentUnixTime;

            if (mOnBannerOpeningCallback != null)
            {
                mOnBannerOpeningCallback();
            }
        }

        private void _OnBannerClosed(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerClosed()");
#endif

            if (mOnBannerClosedCallback != null)
            {
                mOnBannerClosedCallback();
            }
        }

        #endregion

        #region ----- Interstitial Event -----

        private void _OnInterstitialLoaded(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialLoaded()");
#endif

            mIsLoadingInterstitial = false;

            if (mOnInterstitialAdLoadedCallback != null)
            {
                mOnInterstitialAdLoadedCallback();
            }
        }

        private void _OnInterstitialFaiedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialFaiedToLoad(): message=" + args.Message);
#endif

            mIsLoadingInterstitial = false;

            if (mOnInterstitialAdFailedToLoadCallback != null)
            {
                mOnInterstitialAdFailedToLoadCallback();
            }
        }

        private void _OnInterstitialOpening(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialOpening()");
#endif

            mLastInterstitialShowTimestamp = MyLocalTime.CurrentUnixTime;

            if (mOnInterstitialAdOpeningCallback != null)
            {
                mOnInterstitialAdOpeningCallback();
            }
        }

        private void _OnInterstitialClosed(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialClosed()");
#endif

            MyCoroutiner.ExcuteAfterDelayFrame(2, () =>
            {
                mIsLoadingInterstitial = false;

                if (mOnInterstitialAdClosedCallback != null)
                {
                    mOnInterstitialAdClosedCallback();
                }
            });
        }

        #endregion

        #region ----- Rewarded Video Event -----

        private void _OnRewardedAdLoaded(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdLoaded()");
#endif

            mIsLoadingRewardred = false;

            if (mOnRewardedAdLoadedCallback != null)
            {
                mOnRewardedAdLoadedCallback();
            }
        }

        private void _OnRewardedAdFaiedToLoad(object sender, AdErrorEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdFaiedToLoad(): message=" + args.Message);
#endif

            mIsLoadingRewardred = false;

            if (mOnRewardedAdFailedToLoadCallback != null)
            {
                mOnRewardedAdFailedToLoadCallback();
            }
        }

        private void _OnRewardedAdOpening(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdOpening()");
#endif

            mLastRewardedShowTimestamp = MyLocalTime.CurrentUnixTime;

            if (mOnRewardedAdOpeningCallback != null)
            {
                mOnRewardedAdOpeningCallback();
            }
        }

        private void _OnRewardedAdFaiedToShow(object sender, AdErrorEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdFaiedToShow(): message=" + args.Message);
#endif

            if (mOnRewardedAdFailedToShowCallback != null)
            {
                mOnRewardedAdFailedToShowCallback();
            }
        }

        private void _OnRewardedAdUserEarnedReward(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdUserEarnedReward()");
#endif

            mIsHasReward = true;
        }

        private void _OnRewardedAdClosed(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdClosed()");
#endif

            MyCoroutiner.ExcuteAfterDelayFrame(2, () =>
            {
                mIsLoadingRewardred = false;

                if (mIsHasReward)
                {
                    if (mOnRewardedAdUserEarnedRewardCallback != null)
                    {
                        mOnRewardedAdUserEarnedRewardCallback();
                    }
                }
                else
                {
                    if (mOnRewardedAdSkippedCallback != null)
                    {
                        mOnRewardedAdSkippedCallback();
                    }
                }
                if (mOnRewardedAdClosedCallback != null)
                {
                    mOnRewardedAdClosedCallback();
                }
            });
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyAdMobManager))]
    public class MyAdMobManagerEditor : Editor
    {
        private MyAdMobManager mScript;
        private SerializedProperty mTestEnable;
        private SerializedProperty mTestUseGoogleAdsId;
        private SerializedProperty mTestDeviceId;
        private SerializedProperty mAndroidDefaultBannerId;
        private SerializedProperty mIosDefaultBannerId;
        private SerializedProperty mLastBannerShowTimestamp;
        private SerializedProperty mAndroidDefaultInterstitialAdId;
        private SerializedProperty mIosDefaultInterstitialAdId;
        private SerializedProperty mLastInterstitialShowTimestamp;
        private SerializedProperty mAndroidDefaultRewardedAdId;
        private SerializedProperty mIosDefaultRewardedAdId;
        private SerializedProperty mLastRewardedShowTimestamp;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyAdMobManager)target;
            mTestEnable = serializedObject.FindProperty("mTestEnable");
            mTestUseGoogleAdsId = serializedObject.FindProperty("mTestUseGoogleAdsId");
            mTestDeviceId = serializedObject.FindProperty("mTestDeviceId");
            mAndroidDefaultBannerId = serializedObject.FindProperty("mAndroidDefaultBannerId");
            mIosDefaultBannerId = serializedObject.FindProperty("mIosDefaultBannerId");
            mLastBannerShowTimestamp = serializedObject.FindProperty("mLastBannerShowTimestamp");
            mAndroidDefaultInterstitialAdId = serializedObject.FindProperty("mAndroidDefaultInterstitialAdId");
            mIosDefaultInterstitialAdId = serializedObject.FindProperty("mIosDefaultInterstitialAdId");
            mLastInterstitialShowTimestamp = serializedObject.FindProperty("mLastInterstitialShowTimestamp");
            mAndroidDefaultRewardedAdId = serializedObject.FindProperty("mAndroidDefaultRewardedAdId");
            mIosDefaultRewardedAdId = serializedObject.FindProperty("mIosDefaultRewardedAdId");
            mLastRewardedShowTimestamp = serializedObject.FindProperty("mLastRewardedShowTimestamp");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyAdMobManager), false);

            serializedObject.Update();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Test", EditorStyles.boldLabel);
            mTestEnable.boolValue = EditorGUILayout.Toggle("   Enable", mTestEnable.boolValue);
            mTestUseGoogleAdsId.boolValue = EditorGUILayout.Toggle("   Use Google Ads Id", mTestUseGoogleAdsId.boolValue);
            mTestDeviceId.stringValue = EditorGUILayout.TextField("   Device Ids (separate by \";\")", mTestDeviceId.stringValue);

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Banner", EditorStyles.boldLabel);
            mAndroidDefaultBannerId.stringValue = EditorGUILayout.TextField("   Android Default ID", mAndroidDefaultBannerId.stringValue);
            mIosDefaultBannerId.stringValue = EditorGUILayout.TextField("   IOS Default ID", mIosDefaultBannerId.stringValue);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("   Last Show Timestamp", GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.LabelField(mLastBannerShowTimestamp.longValue.ToString());
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Interstitial Ad", EditorStyles.boldLabel);
            mAndroidDefaultInterstitialAdId.stringValue = EditorGUILayout.TextField("   Android Default ID", mAndroidDefaultInterstitialAdId.stringValue);
            mIosDefaultInterstitialAdId.stringValue = EditorGUILayout.TextField("   IOS Default ID", mIosDefaultInterstitialAdId.stringValue);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("   Last Show Timestamp", GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.LabelField(mLastInterstitialShowTimestamp.longValue.ToString());
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Rewarded Ad", EditorStyles.boldLabel);
            mAndroidDefaultRewardedAdId.stringValue = EditorGUILayout.TextField("   Android Default ID", mAndroidDefaultRewardedAdId.stringValue);
            mIosDefaultRewardedAdId.stringValue = EditorGUILayout.TextField("   IOS Default ID", mIosDefaultRewardedAdId.stringValue);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("   Last Show Timestamp", GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.LabelField(mLastRewardedShowTimestamp.longValue.ToString());
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}

#endif