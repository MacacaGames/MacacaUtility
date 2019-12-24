using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimerSetting : ScriptableObject
{
    public static int RegulateRateIfDelta = 60;
    public static RefreshTimeMethod refreshTimeMethod = RefreshTimeMethod.UnityDeltaTime;
    public enum RefreshTimeMethod
    {
        SystemDateTime,
        UnityDeltaTime
    }
}
