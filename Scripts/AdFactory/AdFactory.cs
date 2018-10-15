using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AdFactory 統一對外提供所有廣告的顯示與撥放，具體的廣告供應者則以 IAdManager 的實作為主
/// </summary>
public class AdFactory : UnitySingleton<AdFactory>
{
    IAdManager adManager;

    [SerializeField]
    AdFactory.RewardResult EditorTestResult = AdFactory.RewardResult.Success;

    /// <summary>
    /// Add two number
    /// </summary>
    public delegate void AdViewEvent();

    /// <summary>
    /// 註冊一個事件，該事件將會於 廣告顯示「前」執行
    /// </summary>
    public event AdViewEvent OnLoadViewShow;

    /// <summary>
    /// 註冊一個事件，該事件將會於 廣告顯示「後」執行
    /// </summary>
    public event AdViewEvent OnLoadViewLeave;

    /// <summary>
    /// 初始化 AdFactory 並指定實做的廣告供應者
    /// </summary>
    public void InitFactory(
        AdProvider provider,
        string AppId = "",
        string RewaredPlacement = "",
        string IterstitialPlacement = "",
        string BannerPlacement = "")
    {
        Debug.LogWarning("Init AdFactory with " + provider);
        switch (provider)
        {
            case AdProvider.AdMob:
                adManager = new AdMobManager(AppId, RewaredPlacement, IterstitialPlacement, BannerPlacement);
                break;
            case AdProvider.UnityAd:
                adManager = new UnityAdManager(AppId, RewaredPlacement, IterstitialPlacement);
                break;
        }

        adManager.Init();
    }

    /// <summary>
    /// 請求並顯示橫幅廣告
    /// </summary>
    /// <returns>true 代表請求成功, false 代表請求失敗或是或是廣告提供者不支援橫幅廣告</returns>
    public bool ShowBannerAd()
    {
        if (!CheckInit())
        {
            Debug.LogError("AdFactory is not Init");
            return false;
        }

        return adManager.ShowBannerAd();
    }

    public int GetBannerHeight(){
        if (!CheckInit())
        {
            Debug.LogError("AdFactory is not Init");
            return 0;
        }

        return adManager.GetBannerHeight();
    }

    /// <summary>
    /// 查詢目前畫面上是否有橫幅顯示
    /// </summary>
    /// <returns>true 代表有, false 代表無</returns>
    public bool HasBannerView()
    {
        if (!CheckInit())
        {
            Debug.LogError("AdFactory is not Init");
            return false;
        }

        return adManager.HasBannerView();
    }

    /// <summary>
    /// 移除目前畫面上的橫幅顯示
    /// </summary>
    /// <returns>true 代表移除成功, false 代表移除失敗或該廣告提供者的橫幅無法移除</returns>
    public bool RemoveBannerView()
    {
        if (!CheckInit())
        {
            Debug.LogError("AdFactory is not Init");
            return false;
        }
        return false;
    }

    /// <summary>
    /// 顯示一則插業廣告
    /// </summary>
    /// <returns>一個代表廣告顯示進程的 Coroutine</returns>
    public Coroutine ShowInterstitialAds(Action<AdFactory.RewardResult> OnFinish)
    {
        if (!CheckInit())
        {
            Debug.LogError("AdFactory is not Init");
            return null;
        }
        return StartCoroutine(ShowInterstitialAdsRunner(OnFinish));
    }

    IEnumerator ShowInterstitialAdsRunner(Action<AdFactory.RewardResult> OnFinish)
    {
        //顯示讀取，如果有的話
        if (OnLoadViewShow != null) OnLoadViewShow();

       
#if UNITY_EDITOR
        yield return Yielders.GetWaitForSeconds(1f);
        OnFinish(EditorTestResult);
#else
        yield return adManager.ShowInterstitialAds(OnFinish);
#endif

        //關閉讀取，如果有的話
        if (OnLoadViewLeave != null) OnLoadViewLeave();
    }

    /// <summary>
    /// 顯示一則獎勵廣告
    /// </summary>
    /// <returns>一個代表廣告顯示進程的 Coroutine</returns>
    public Coroutine ShowRewardedAds(Action<AdFactory.RewardResult> OnFinish, string extraData = "")
    {
        if (!CheckInit())
        {
            Debug.LogError("AdFactory is not Init");
            return null;
        }
        return StartCoroutine(ShowRewardedAdsRunner(OnFinish, extraData));
    }

    IEnumerator ShowRewardedAdsRunner(Action<AdFactory.RewardResult> OnFinish, string extraData = "")
    {
        //顯示讀取，如果有的話
        if (OnLoadViewShow != null) OnLoadViewShow();
#if UNITY_EDITOR
        yield return Yielders.GetWaitForSeconds(1f);
        OnFinish(EditorTestResult);
#else
        yield return adManager.ShowRewardedAds(OnFinish, extraData);
#endif
        //關閉讀取，如果有的話
        if (OnLoadViewLeave != null) OnLoadViewLeave();
    }

    public bool CheckInit()
    {

#if UNITY_EDITOR
        return true;
#endif
        return adManager != null;
    }
    public enum AdProvider
    {
        AdMob = 0,
        UnityAd = 1,
        FacebookAudienceNetwork = 2
    }

    public enum AdType
    {
        Banner,
        Reward,
        Interstitial
    }
    public enum RewardResult
    {
        Success,
        Declined,
        Faild,
        Error
    }

    public enum AdsLoadState
    {
        Loading,
        Loaded,
        Failed,
        Rewarded,
        RewardSuccess,
        Declined,
        Exception,
        Complete,
    }
}

public interface IAdManager
{
    void Init();
    void Destroy();

    /// <summary>
    /// Add two number
    /// </summary>
    /// <returns>true 代表請求成功, false 代表請求失敗或是 VIP 用戶或是還沒玩超過三次</returns>
    bool ShowBannerAd();
    bool HasBannerView();
    bool RemoveBannerView();
    int GetBannerHeight();

    /// <summary>
    /// 顯示一則插業廣告
    /// </summary>
    /// <returns>一個代表廣告顯示進程的 Coroutine</returns>
    IEnumerator ShowInterstitialAds(Action<AdFactory.RewardResult> OnFinish);

    /// <summary>
    /// 顯示一則獎勵廣告
    /// </summary>
    /// <returns>一個代表廣告顯示進程的 Coroutine</returns>
    IEnumerator ShowRewardedAds(Action<AdFactory.RewardResult> OnFinish, string extraData = "");
}
