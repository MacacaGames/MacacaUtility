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
    public void InitFactory(AdProvider provider,string AppId = "")
    {
        switch (provider)
        {
            case AdProvider.AdMob:
                adManager = new AdMobManager(AppId);
                break;
            case AdProvider.UnityAd:

                break;
        }

        adManager.Init();
    }

    /// <summary>
    /// 請求並顯示橫幅廣告
    /// </summary>
    /// <returns>true 代表請求成功, false 代表請求失敗或是或是廣告提供者不支援橫幅廣告</returns>
    public bool ShowBannerAd(string id)
    {
        if (!CheckInit())
        {
            Debug.LogError("AdFactory is not Init");
            return false;
        }

        return adManager.ShowBannerAd(id);
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
    public Coroutine ShowInterstitialAds(string id, Action<AdFactory.RewardResult> callback)
    {
        if (!CheckInit())
        {
            Debug.LogError("AdFactory is not Init");
            return null;
        }
        return StartCoroutine(ShowInterstitialAdsRunner(id, callback));
    }

    IEnumerator ShowInterstitialAdsRunner(string id, Action<AdFactory.RewardResult> callback)
    {
        //顯示讀取，如果有的話
        if (OnLoadViewShow != null) OnLoadViewShow();

        yield return adManager.ShowInterstitialAds(id, callback);

        //關閉讀取，如果有的話
        if (OnLoadViewLeave != null) OnLoadViewLeave();
    }

    /// <summary>
    /// 顯示一則獎勵廣告
    /// </summary>
    /// <returns>一個代表廣告顯示進程的 Coroutine</returns>
    public Coroutine ShowRewardedAds(string id, Action<AdFactory.RewardResult> callback, string extraData = "")
    {
        if (!CheckInit())
        {
            Debug.LogError("AdFactory is not Init");
            return null;
        }
        return StartCoroutine(ShowRewardedAdsRunner(id, callback, extraData));
    }

    IEnumerator ShowRewardedAdsRunner(string id, Action<AdFactory.RewardResult> callback, string extraData = "")
    {
        //顯示讀取，如果有的話
        if (OnLoadViewShow != null) OnLoadViewShow();

        yield return adManager.ShowRewardedAds(id, callback, extraData);

        //關閉讀取，如果有的話
        if (OnLoadViewLeave != null) OnLoadViewLeave();
    }

    public bool CheckInit()
    {
        return adManager != null;
    }
    public enum AdProvider
    {
        AdMob,
        UnityAd
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
    bool ShowBannerAd(string id);
    bool HasBannerView();
    bool RemoveBannerView();

    /// <summary>
    /// 顯示一則插業廣告
    /// </summary>
    /// <returns>一個代表廣告顯示進程的 Coroutine</returns>
    IEnumerator ShowInterstitialAds(string id, Action<AdFactory.RewardResult> action);

    /// <summary>
    /// 顯示一則獎勵廣告
    /// </summary>
    /// <returns>一個代表廣告顯示進程的 Coroutine</returns>
    IEnumerator ShowRewardedAds(string id, Action<AdFactory.RewardResult> callback, string extraData = "");
}
