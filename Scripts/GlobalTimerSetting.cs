using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CloudMacaca
{
    public class GlobalTimerSetting : ScriptableObject
    {
        public int RegulateRateIfDelta = 60;
        public RefreshTimeMethod refreshTimeMethod = RefreshTimeMethod.UnityDeltaTime;
        public enum RefreshTimeMethod
        {
            SystemDateTime,
            UnityDeltaTime
        }
    }
}