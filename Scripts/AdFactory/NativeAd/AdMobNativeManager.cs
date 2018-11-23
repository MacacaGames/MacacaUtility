using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdMobNativeManager
{
    static AdMobNativeManager _Instance;
    public static AdMobNativeManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new AdMobNativeManager();
            }
            return _Instance;
        }
    }
    AdLoader adLoader;

    public void Init(string AD_UNIT)
    {
        adLoader = new AdLoader.Builder(AD_UNIT)
        .ForUnifiedNativeAd()
        .Build();

        adLoader.OnUnifiedNativeAdLoaded += this.HandleUnifiedNativeAdLoaded;
        adLoader.OnAdFailedToLoad += this.OnAdFailedToLoad;
    }

    public delegate void AdLoadEvent();
    public event AdLoadEvent OnAdLoadFaild;


    private void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        Debug.LogError("NativeAd OnAdFailedToLoad : " + e.Message);
        if (OnAdLoadFaild != null) OnAdLoadFaild();
    }

    private UnifiedNativeAd nativeAd;
    public bool isNativeAdLoaded = false;
    private void HandleUnifiedNativeAdLoaded(object sender, UnifiedNativeAdEventArgs e)
    {
        this.nativeAd = e.nativeAd;
        isNativeAdLoaded = true;
    }

    public UnifiedNativeAd GetNativeAdObject()
    {
        if (nativeAd != null)
            return nativeAd;
        else
            return null;
    }

    public void LoadNativeAd()
    {
        adLoader.LoadAd(new AdRequest.Builder().Build());
    }
}
