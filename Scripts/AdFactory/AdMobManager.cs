using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;


public class AdMobManager : IAdManager
{
    static string _admobAppId;
    static string _rewaredPlacement;
    static string _iterstitialPlacement;
    static string _bannerPlacement;

    public AdMobManager(string AppId, string RewaredPlacement, string IterstitialPlacement, string BannerPlacement)
    {
        _admobAppId = AppId;
        _rewaredPlacement = RewaredPlacement;
        _iterstitialPlacement = IterstitialPlacement;
        _bannerPlacement = BannerPlacement;
    }
    public void Init()
    {
        GoogleMobileAds.Api.MobileAds.Initialize(_admobAppId);
        RegistRewardedAdEvent();
    }

    public void Destroy()
    {
        UnRegistRewardedAdEvent();
    }
    #region BannerAd
    BannerView bannerView;
    public bool ShowBannerAd()
    {
        string adUnitId = _bannerPlacement;
        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        bannerView.LoadAd(request);
        return true;
    }
    public int GetBannerHeight()
    {
        #if UNITY_IOS
        if(UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX ){
            return 50 * Mathf.RoundToInt(Screen.dpi / 160);
        }
        #endif
        Debug.Log(Screen.height);
        if (Screen.height <= 400 * Mathf.RoundToInt(Screen.dpi / 160))
        {
            return 32 * Mathf.RoundToInt(Screen.dpi / 160);
        }
        else if (Screen.height <= 720 * Mathf.RoundToInt(Screen.dpi / 160))
        {
            return 50 * Mathf.RoundToInt(Screen.dpi / 160);
        }
        else
        {
            return 90 * Mathf.RoundToInt(Screen.dpi / 160);
        }
    }
    public bool HasBannerView()
    {
        return bannerView == null ? false : true;
    }
    public bool RemoveBannerView()
    {
        if (bannerView == null) return false;
        bannerView.Hide();
        return true;
    }
    #endregion
    #region InterstitialAd
    public AdFactory.AdsLoadState loadState_interstitialAds = AdFactory.AdsLoadState.Exception;
    bool isInterstitialAdClose = false;
    InterstitialAd interstitial;
    bool isShowedInterstitialAds;

    public IEnumerator ShowInterstitialAds(Action<AdFactory.RewardResult> callback)
    {
        string id = _iterstitialPlacement;
        AdFactory.RewardResult result = AdFactory.RewardResult.Error;

        isInterstitialAdClose = false;
        int try_preload_times = 0;

        //等一秒，騙使用者很忙
        yield return Yielders.GetWaitForSeconds(1f);

        //編輯器的情況
#if UNITY_EDITOR
        result = AdFactory.RewardResult.Success;
        goto FINISH;
#endif

        //沒有讀到的情況
        if (loadState_interstitialAds != AdFactory.AdsLoadState.Loaded)
        {
            while (try_preload_times < 3)
            {
                PreloadInterstitial(id);

                float wait = 0;
                while (wait < 3)
                {
                    wait += Time.deltaTime;
                    if (loadState_interstitialAds == AdFactory.AdsLoadState.Loaded)
                    {
                        goto SHOW;
                    }
                    yield return null;
                }
                try_preload_times++;
                Debug.Log("Try load times : " + try_preload_times);
            }
            result = AdFactory.RewardResult.Faild;
            goto FINISH;
        }


    SHOW:
        if (interstitial != null)
        {
            if (interstitial.IsLoaded())
            {
                result = AdFactory.RewardResult.Success;
                _ShowInterstitialAds();
            }
        }

        while (!isInterstitialAdClose)
        {
            yield return null;
        }

    FINISH:
        PreloadInterstitial(id);

        DestroyInterstitial();

        if (callback != null)
            callback(result);
    }

    void _ShowInterstitialAds()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
            isShowedInterstitialAds = true;
        }
        else
        {
            Debug.Log("Cannot show interstitialAds, Handler is loaded, but somehow IsLoaded is still not loaded.");
        }
    }
    void PreloadInterstitial(string id)
    {
        DestroyInterstitial();
        interstitial = new InterstitialAd(id);

        interstitial.OnAdLoaded += HandleOnInterstitialLoaded;
        interstitial.OnAdFailedToLoad += HandleOnInterstitialFailedToLoad;
        interstitial.OnAdClosed += HandleOnInterstitialClosed;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }
    void HandleOnInterstitialLoaded(object sender, EventArgs args)
    {
        // Handle the ad loaded event.
        loadState_interstitialAds = AdFactory.AdsLoadState.Loaded;
    }

    void HandleOnInterstitialFailedToLoad(object sender, EventArgs args)
    {
        loadState_interstitialAds = AdFactory.AdsLoadState.Failed;
    }

    void HandleOnInterstitialClosed(object sender, EventArgs args)
    {
        isInterstitialAdClose = true;
        loadState_interstitialAds = AdFactory.AdsLoadState.Complete;
    }

    void DestroyInterstitial()
    {
        if (interstitial == null)
        {
            return;
        }
        interstitial.OnAdLoaded -= HandleOnInterstitialLoaded;
        interstitial.OnAdFailedToLoad -= HandleOnInterstitialFailedToLoad;
        interstitial.OnAdClosed -= HandleOnInterstitialClosed;
        interstitial.Destroy();
    }


    #endregion
    #region RewardedAd
    public static RewardBasedVideoAd rewardBasedVideo { get { return RewardBasedVideoAd.Instance; } }
    public static AdFactory.AdsLoadState loadState_rewardedAds;
    bool isRewardAdClose = false;

    public IEnumerator ShowRewardedAds(Action<AdFactory.RewardResult> callback, string extraData)
    {
        string id = _rewaredPlacement;
        //初始化
        isRewardAdClose = false;
        int try_preload_times = 0;

        AdFactory.RewardResult result = AdFactory.RewardResult.Error;

        //等一秒，騙使用者很忙
        yield return Yielders.GetWaitForSeconds(1f);

        //編輯器的情況
#if UNITY_EDITOR
        result = AdFactory.RewardResult.Success;
        goto FINISH;
#else

        //沒有讀到的情況
        if (loadState_rewardedAds != AdFactory.AdsLoadState.Loaded) {
            while (try_preload_times < 3) {
                RequestRewardedAds (id);

                float wait = 0;
                while (wait < 3) {
                    wait += Time.deltaTime;
                    if (loadState_rewardedAds == AdFactory.AdsLoadState.Loaded) {
                        goto SHOW;
                    }
                    yield return null;
                }
                try_preload_times++;
                Debug.Log ("Try load times : " + try_preload_times);
            }
            result = AdFactory.RewardResult.Faild;
            goto FINISH;
        }
#endif
    SHOW:
        _ShowRewardedAds();

        while (!isRewardAdClose)
        {
            yield return null;
        }

        switch (loadState_rewardedAds)
        {
            case AdFactory.AdsLoadState.Rewarded:
                result = AdFactory.RewardResult.Success;
                break;
            case AdFactory.AdsLoadState.Declined:
                result = AdFactory.RewardResult.Declined;
                break;
            case AdFactory.AdsLoadState.Failed:
                result = AdFactory.RewardResult.Faild;
                break;
            default:
                result = AdFactory.RewardResult.Error;
                break;
        }

    FINISH:
        RequestRewardedAds(id);


        callback(result);
    }

    void _ShowRewardedAds()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();
        }
        else
        {
            Debug.Log("Cannot show rewaredAds, Handler is loaded, but somehow IsLoaded is still not loaded.");
            isRewardAdClose = true;
        }
    }

    void RequestRewardedAds(string id)
    {
        AdRequest request = new AdRequest.Builder().Build();
        rewardBasedVideo.LoadAd(request, id);
    }


    void RegistRewardedAdEvent()
    {
        // Ad event fired when the rewarded video ad
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
    }

    void UnRegistRewardedAdEvent()
    {
        rewardBasedVideo.OnAdLoaded -= HandleRewardBasedVideoLoaded;
        rewardBasedVideo.OnAdFailedToLoad -= HandleRewardBasedVideoFailedToLoad;
        rewardBasedVideo.OnAdRewarded -= HandleRewardBasedVideoRewarded;
        rewardBasedVideo.OnAdClosed -= HandleRewardBasedVideoClosed;
    }
    void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        loadState_rewardedAds = AdFactory.AdsLoadState.Loaded;
        Debug.Log("HandleRewardBasedVideoLoaded");
    }

    void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        loadState_rewardedAds = AdFactory.AdsLoadState.Failed;
        Debug.Log("HandleRewardBasedVideoFailedToLoad" + args.Message);
    }

    void HandleRewardBasedVideoRewarded(object sender, Reward reward)
    {
        loadState_rewardedAds = AdFactory.AdsLoadState.Rewarded;
    }

    void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        isRewardAdClose = true;
    }
    #endregion


}