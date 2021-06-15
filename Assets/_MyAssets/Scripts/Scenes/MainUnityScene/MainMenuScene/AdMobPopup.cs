using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using MyClasses;
using MyClasses.UI;

namespace MyApp
{
    public class AdMobPopup : MyUGUIPopup
    {
        #region ----- Variable -----

        private MyUGUIButton _btnClose;
        private MyUGUIButton _btnShowBanner;
        private MyUGUIButton _btnLoadInterstitial;
        private MyUGUIButton _btnShowInterstitial;
        private MyUGUIButton _btnLoadRewardedVideo;
        private MyUGUIButton _btnShowRewardedVideo;

        #endregion

        #region ----- Constructor -----

        public AdMobPopup(EPopupID id, string prefabName, bool isFloat = false, bool isRepeatable = false)
            : base(id, prefabName, isFloat, isRepeatable)
        {
        }

        #endregion

        #region ----- MyUGUIPopup Implementation -----

        public override void OnUGUIInit()
        {
            Debug.Log("AdMobPopup.OnUGUIInit()");

            base.OnUGUIInit();

            _btnClose = MyUtilities.FindObject(GameObject, "Container/ButtonClose").GetComponent<MyUGUIButton>();
            _btnShowBanner = MyUtilities.FindObject(GameObject, "Container/Buttons/ButtonShowBanner").GetComponent<MyUGUIButton>();
            _btnLoadInterstitial = MyUtilities.FindObject(GameObject, "Container/Buttons/ButtonLoadInterstitial").GetComponent<MyUGUIButton>();
            _btnShowInterstitial = MyUtilities.FindObject(GameObject, "Container/Buttons/ButtonShowInterstitial").GetComponent<MyUGUIButton>();
            _btnLoadRewardedVideo = MyUtilities.FindObject(GameObject, "Container/Buttons/ButtonLoadRewardedVideo").GetComponent<MyUGUIButton>();
            _btnShowRewardedVideo = MyUtilities.FindObject(GameObject, "Container/Buttons/ButtonShowRewardedVideo").GetComponent<MyUGUIButton>();
        }

        public override void OnUGUIEnter()
        {
            Debug.Log("AdMobPopup.OnUGUIEnter()");

            base.OnUGUIEnter();

            _btnClose.OnEventPointerClick.AddListener(_OnClickClose);
            _btnShowBanner.OnEventPointerClick.AddListener(_OnClickShowBanner);
            _btnLoadInterstitial.OnEventPointerClick.AddListener(_OnClickLoadInterstitial);
            _btnShowInterstitial.OnEventPointerClick.AddListener(_OnClickShowInterstitial);
            _btnLoadRewardedVideo.OnEventPointerClick.AddListener(_OnClickLoadRewardedVideo);
            _btnShowRewardedVideo.OnEventPointerClick.AddListener(_OnClickShowRewardedVideo);

            MyAdMobManager.Instance.Initialize(() =>
            {
                Debug.Log("AdMobPopup.OnUGUIEnter(): AdMob was initialized");
            });
        }

        public override bool OnUGUIVisible()
        {
            Debug.Log("AdMobPopup.OnUGUIVisible()");

            if (base.OnUGUIVisible())
            {
                return true;
            }
            return false;
        }

        public override void OnUGUIUpdate(float deltaTime)
        {
        }

        public override void OnUGUIExit()
        {
            Debug.Log("AdMobPopup.OnUGUIExit()");

            base.OnUGUIExit();

            _btnClose.OnEventPointerClick.RemoveAllListeners();
            _btnShowBanner.OnEventPointerClick.RemoveAllListeners();
            _btnLoadInterstitial.OnEventPointerClick.RemoveAllListeners();
            _btnShowInterstitial.OnEventPointerClick.RemoveAllListeners();
            _btnLoadRewardedVideo.OnEventPointerClick.RemoveAllListeners();
            _btnShowRewardedVideo.OnEventPointerClick.RemoveAllListeners();
        }

        public override bool OnUGUIInvisible()
        {
            Debug.Log("AdMobPopup.OnUGUIInvisible()");

            if (base.OnUGUIInvisible())
            {
                return true;
            }
            return false;
        }

        public override void OnUGUIBackKey()
        {
            MyUGUIManager.Instance.Back();
        }

        #endregion

        #region ----- Button Event -----

        private void _OnClickClose(PointerEventData arg0)
        {
            Hide();
        }

        private void _OnClickShowBanner(PointerEventData arg0)
        {
            Debug.Log("AdMobPopup._OnClickShowBanner()");

            MyAdMobManager.Instance.ShowBanner();
        }

        private void _OnClickLoadInterstitial(PointerEventData arg0)
        {
            Debug.Log("AdMobPopup._OnClickLoadInterstitial()");

            if (!MyAdMobManager.Instance.IsInterstitialAdLoaded() && !MyAdMobManager.Instance.IsInterstitialAdLoading())
            {
                MyAdMobManager.Instance.LoadInterstitialAd(null, () =>
                {
                    Debug.Log("AdMobPopup._OnClickLoadInterstitial().LoadInterstitialAd(): onLoadedCallback");
                }, () =>
                {
                    Debug.Log("AdMobPopup._OnClickLoadInterstitial().LoadInterstitialAd(): onFailedToLoadCallback");
                });
            }
            else
            {
                Debug.Log("AdMobPopup._OnClickLoadInterstitial(): interstitial is loading");
            }
        }

        private void _OnClickShowInterstitial(PointerEventData arg0)
        {
            Debug.Log("AdMobPopup._OnClickShowInterstitial()");

            if (!MyAdMobManager.Instance.IsInterstitialAdLoaded() || MyAdMobManager.Instance.IsInterstitialAdLoading())
            {
                MyUGUIManager.Instance.ShowToastMessage("Interstitial hasn't loaded yet");
            }
            else
            {
                MyAdMobManager.Instance.ShowInterstitialAd(() =>
                {
                    Debug.Log("AdMobPopup._OnClickShowInterstitial().ShowInterstitialAd(): onOpeningCallback");
                }, () =>
                {
                    Debug.Log("AdMobPopup._OnClickShowInterstitial().ShowInterstitialAd(): onLeavingApplicationCallback");
                }, () =>
                {
                    Debug.Log("AdMobPopup._OnClickShowInterstitial().ShowInterstitialAd(): onClosedCallback");
                });
            }
        }

        private void _OnClickLoadRewardedVideo(PointerEventData arg0)
        {
            Debug.Log("AdMobPopup._OnClickLoadRewardedVideo()");

            if (!MyAdMobManager.Instance.IsRewardedAdLoaded() && !MyAdMobManager.Instance.IsRewardedAdLoading())
            {
                MyAdMobManager.Instance.LoadRewardedAd(null, () =>
                {
                    Debug.Log("AdMobPopup._OnClickLoadRewardedVideo().LoadRewardedAd(): onLoadedCallback");
                }, () =>
                {
                    Debug.Log("AdMobPopup._OnClickLoadRewardedVideo().LoadRewardedAd(): onFailedToLoadCallback");
                });
            }
            else
            {
                Debug.Log("AdMobPopup._OnClickLoadRewardedVideo(): rewarded video is loading");
            }
        }

        private void _OnClickShowRewardedVideo(PointerEventData arg0)
        {
            Debug.Log("AdMobPopup._OnClickShowRewardedVideo()");

            if (!MyAdMobManager.Instance.IsRewardedAdLoaded() || MyAdMobManager.Instance.IsRewardedAdLoading())
            {
                MyUGUIManager.Instance.ShowToastMessage("Rewarded Video hasn't loaded yet");
            }
            else
            {
                MyAdMobManager.Instance.ShowRewardedAd(() =>
                {
                    Debug.Log("AdMobPopup._OnClickShowRewardedVideo().ShowRewardedAd(): onOpeningCallback");
                }, () =>
                {
                    Debug.Log("AdMobPopup._OnClickShowRewardedVideo().ShowRewardedAd(): onUserEarnedRewardCallback");
                }, () =>
                {
                    Debug.Log("AdMobPopup._OnClickShowRewardedVideo().ShowRewardedAd(): onFailedToShowCallback");
                }, () =>
                {
                    Debug.Log("AdMobPopup._OnClickShowRewardedVideo().ShowRewardedAd(): onSkippedCallback");
                }, () =>
                {
                    Debug.Log("AdMobPopup._OnClickShowRewardedVideo().ShowRewardedAd(): onClosedCallback");
                });
            }
        }

        #endregion

        #region ----- Public Method -----



        #endregion

        #region ----- Private Method -----



        #endregion
    }
}