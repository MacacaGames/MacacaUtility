using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PriceHelperTester : MonoBehaviour
{
    [Button]
    public void Test( string inputText,float times)
    {
        Debug.Log(
            string.Format("inputText: {0}|  inputTimes: {1}|  output: {2}",
            inputText,
            times,
            PriceHelper.GetTimesStringResultByPriceText(inputText, times)));
    }
}
