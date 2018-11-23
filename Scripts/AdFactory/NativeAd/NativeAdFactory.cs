using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NativeAdFactory : MonoBehaviour
{

    INativeAdManager nativeAdManager;

    /// <summary>
    /// 初始化 AdFactory 並指定實做的廣告供應者
    /// </summary>
    public void Init(string AD_UNIT)
    {
        nativeAdManager.Init(AD_UNIT);
    }


}

public interface INativeAdManager
{
    void Init(string AD_UNIT);
    void Destroy();
}

