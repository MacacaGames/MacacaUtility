#if AdFactory_Unity
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdManager : IAdManager
{
    static string _gameId = "";
    static string _rewaredPlacement;
    static string _iterstitialPlacement;
    public UnityAdManager(string GameId, string RewarePlacement, string IterstitialPlacement)
    {
        _gameId = GameId;
        _rewaredPlacement = RewarePlacement;
        _iterstitialPlacement = IterstitialPlacement;
    }
    public void Init()
    {
        Advertisement.Initialize(_gameId);
    }
    public void Destroy()
    {

    }

    /// <summary>
    /// Add two number
    /// </summary>
    /// <returns>true 代表請求成功, false 代表請求失敗或是 VIP 用戶或是還沒玩超過三次</returns>
    public bool ShowBannerAd()
    {
        return false;
    }
    public bool HasBannerView()
    {
        return false;
    }
    public bool RemoveBannerView()
    {
        return false;
    }
    public int GetBannerHeight()
    {
        return 0;
    }

    AdFactory.RewardResult resultInterstitialAd = AdFactory.RewardResult.Faild;
    bool waitInterstitialAdFinish = false;
    public IEnumerator ShowInterstitialAds(Action<AdFactory.RewardResult> action)
    {
        string id = _iterstitialPlacement;
        waitInterstitialAdFinish = false;
        if (Advertisement.IsReady(id))
        {
            ShowOptions so = new ShowOptions();
            so.resultCallback = HandleShownterstitialResult;
            Advertisement.Show(id, so);
        }
        else
        {
            resultInterstitialAd = AdFactory.RewardResult.Faild;
            waitInterstitialAdFinish = true;
        }

        yield return new WaitUntil(() => waitInterstitialAdFinish == true);
        action(resultInterstitialAd);
    }
    private void HandleShownterstitialResult(ShowResult result)
    {
        Debug.Log("HandleShowResult" + result);
        waitInterstitialAdFinish = true;
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                resultInterstitialAd = AdFactory.RewardResult.Success;
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                resultInterstitialAd = AdFactory.RewardResult.Declined;
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                resultInterstitialAd = AdFactory.RewardResult.Faild;
                break;
        }
    }

    bool waitRewardedAdFinish = false;
    AdFactory.RewardResult resultRewardAd = AdFactory.RewardResult.Faild;
    /// <summary>
    /// 顯示一則獎勵廣告
    /// </summary>
    /// <returns>一個代表廣告顯示進程的 Coroutine</returns>
    public IEnumerator ShowRewardedAds(Action<AdFactory.RewardResult> callback)
    {
        string id = _rewaredPlacement;
        waitRewardedAdFinish = false;
        if (Advertisement.IsReady(id))
        {
            ShowOptions so = new ShowOptions();
            so.resultCallback = HandleShowRewardResult;
            Advertisement.Show(id, so);
        }
        else
        {
            resultRewardAd = AdFactory.RewardResult.Faild;
            waitRewardedAdFinish = true;
        }

        yield return new WaitUntil(() => waitRewardedAdFinish == true);
        callback(resultRewardAd);
    }
    public void PreLoadRewardedAd(){
        //nothinh in Unity Ads
    }
    void HandleShowRewardResult(ShowResult result)
    {
        Debug.Log("HandleShowResult" + result);
        waitRewardedAdFinish = true;
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                resultRewardAd = AdFactory.RewardResult.Success;
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                resultRewardAd = AdFactory.RewardResult.Declined;
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                resultRewardAd = AdFactory.RewardResult.Faild;
                break;
        }
    }
}

#endif