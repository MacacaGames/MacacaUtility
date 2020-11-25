using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Sirenix.OdinInspector;

public class PriceHelperTester:MonoBehaviour
{
    [SerializeField]
    string originalPrice;
    [SerializeField]
    int times;

    [Button]
    public void Test()
    {
        var s = PriceHelper.GetTimesStringResultByPriceText(originalPrice, times);
        Debug.Log(s);
    }
}
