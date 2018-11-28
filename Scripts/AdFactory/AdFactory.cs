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

    public delegate void AdViewEventAnalysic(string Data);
    /// <summary>
    /// 註冊一個事件，該事件將會於 廣告顯示「前」執行
    /// </summary>
    public event AdViewEventAnalysic OnAdAnalysic;
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
        string DefaultRewaredPlacement = "",
        string DefaultIterstitialPlacement = "",
        string DefaultBannerPlacement = "")
    {
        if (CheckInit())
        {
            Debug.LogWarning("AdFactory is Inited Return");
            return;
        }
        Debug.LogWarning("Init AdFactory with " + provider);
        switch (provider)
        {
            case AdProvider.AdMob:
                adManager = new AdMobManager(AppId, DefaultRewaredPlacement, DefaultIterstitialPlacement, DefaultBannerPlacement);
                break;
            case AdProvider.UnityAd:
                adManager = new UnityAdManager(AppId, DefaultRewaredPlacement, DefaultIterstitialPlacement);
                break;
        }

        adManager.Init();
        //adManager.PreLoadRewardedAd();
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

    public int GetBannerHeight()
    {
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
        return adManager.RemoveBannerView();
    }

    /// <summary>
    /// 顯示一則插業廣告
    /// </summary>
    /// <returns>一個代表廣告顯示進程的 Coroutine</returns>
    public Coroutine ShowInterstitialAds(Action<AdFactory.RewardResult> OnFinish)
    {
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
        if (CheckInit())
        {
            yield return adManager.ShowInterstitialAds(OnFinish);
        }
        else
        {
            yield return Yielders.GetWaitForSeconds(1.5f);
            OnFinish(AdFactory.RewardResult.Faild);
        }
#endif

        //關閉讀取，如果有的話
        if (OnLoadViewLeave != null) OnLoadViewLeave();
    }

    /// <summary>
    /// 顯示一則獎勵廣告
    /// </summary>
    /// <returns>一個代表廣告顯示進程的 Coroutine</returns>
    public Coroutine ShowRewardedAds(Action<AdFactory.RewardResult> OnFinish, string overwritePlacement = null, string analysicData = "")
    {
        if (OnAdAnalysic != null)
        {
            OnAdAnalysic(analysicData);
        }
        return StartCoroutine(ShowRewardedAdsRunner(OnFinish, overwritePlacement));
    }

    IEnumerator ShowRewardedAdsRunner(Action<AdFactory.RewardResult> OnFinish, string overwritePlacement)
    {
        //顯示讀取，如果有的話
        if (OnLoadViewShow != null) OnLoadViewShow();
#if UNITY_EDITOR
        yield return Yielders.GetWaitForSeconds(1f);
        OnFinish(EditorTestResult);

#else
        if (CheckInit())
        {
            yield return adManager.ShowRewardedAds(OnFinish, overwritePlacement);
        }
        else
        {
            yield return Yielders.GetWaitForSeconds(1.5f);
            CloudMacaca.CM_APIController.ShowToastMessage("Rewarded video is not ready please check your network or try again later.");
            OnFinish(AdFactory.RewardResult.Faild);
        }
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
    IEnumerator ShowRewardedAds(Action<AdFactory.RewardResult> OnFinish, string overwritePlacement);

    void PreLoadRewardedAd();
}
