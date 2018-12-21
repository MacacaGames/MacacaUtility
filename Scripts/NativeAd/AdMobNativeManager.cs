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
   

    public void SetUpNativeAd(string AD_UNIT, Action<UnifiedNativeAd> OnLoadSuccess, Action OnLoadFaild)
    {
        var adLoader = new AdLoader.Builder(AD_UNIT)
        .ForUnifiedNativeAd()
        .Build();

        adLoader.OnUnifiedNativeAdLoaded += delegate (object sender, UnifiedNativeAdEventArgs e)
        {
            //Debug.LogError("NativeAd Loaded : " + AD_UNIT);
            OnLoadSuccess(e.nativeAd);
        };

        adLoader.OnAdFailedToLoad += delegate (object sender, AdFailedToLoadEventArgs e)
        {
            //Debug.LogError("NativeAd OnAdFailedToLoad : " + AD_UNIT + " ,msg: " + e.Message);
            OnLoadFaild();
        };
        adLoader.LoadAd(new AdRequest.Builder().Build());

    }
 
}
