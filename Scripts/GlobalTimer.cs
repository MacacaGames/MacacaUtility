using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace CloudMacaca
{
    public class GlobalTimer : UnitySingleton<GlobalTimer>
    {
        public float _deltaTime;
        public static float deltaTime{
            get{
                return Instance._deltaTime;
            }
        }
        public static double _currentTimeStamp;

        public static int CurrentTimeStamp
        {
            get
            {
                if (_currentTimeStamp == 0)
                {
                    _currentTimeStamp = CloudMacaca.Utility.GetTimeStamp();
                }
                return (int)_currentTimeStamp;
            }
            // private set
            // {
            //     _currentTimeStamp = value;
            // }
        }
        [SerializeField]
        GlobalTimerSetting setting;
        float tempRegulateRate = 0;
        void Awake()
        {
            setting = Resources.Load<GlobalTimerSetting>("GlobalTimerSetting");
            if (setting == null)
            {
                setting = ScriptableObject.CreateInstance<GlobalTimerSetting>();
            }
            UpdateTimeStampToSystemTime();
        }
        void OnApplicationFocus(bool hasFocus)
        {
            UpdateTimeStampToSystemTime();
        }
        void Update()
        {
            _deltaTime = Time.deltaTime;
            tempRegulateRate += _deltaTime;

            if (setting.refreshTimeMethod == GlobalTimerSetting.RefreshTimeMethod.SystemDateTime)
                UpdateTimeStampToSystemTime();
            else
                _currentTimeStamp += _deltaTime;

            for (int i = allCounter.Count - 1; i >= 0; --i)
            {
                allCounter[i]?.Update();
            }

            if (tempRegulateRate > setting.RegulateRateIfDelta)
            {
                UpdateTimeStampToSystemTime();
                tempRegulateRate = 0;
            }
        }

        void UpdateTimeStampToSystemTime()
        {
            _currentTimeStamp = CloudMacaca.Utility.GetTimeStamp();
        }

        void LateUpdate()
        {
            allCounter.RemoveAll(r => r.Finished);
        }

        [SerializeField]
        List<Counter> allCounter = new List<Counter>();
        public static Counter RegiesterTimer(string id, int completeTimeStamp, System.Action<int> OnUpdate, System.Action OnComplete, bool autoRenew = true)
        {
            if (Instance.allCounter.Count(m => m.id == id) > 0)
            {
                if (autoRenew == true)
                {
                    return RenewTimer(id, completeTimeStamp, OnUpdate, OnComplete);
                }
                Debug.LogError("Counter with given id is already exist!");
                return null;
            }
            Counter c = new Counter
            {
                id = id,
                OnComplete = OnComplete,
                OnUpdate = OnUpdate,
                completeTimeStamp = completeTimeStamp,
            };
            Instance.allCounter.Add(c);

            //Do one update make sure relative user get the correct lefttime.
            c.Update();

            return c;
        }

        public static Counter RenewTimer(string id, int completeTimeStamp, System.Action<int> OnUpdate, System.Action OnComplete)
        {
            var c = Instance.allCounter.SingleOrDefault(m => m.id == id);
            if (c != null)
            {
                Instance.allCounter.Remove(c);
                c.Dispose();
            }

            // c.completeTimeStamp = completeTimeStamp;
            // c.OnComplete = OnComplete;
            // c.OnUpdate = OnUpdate;

            // if (c.Finished == true && completeTimeStamp > GlobalTimer.CurrentTimeStamp)
            // {
            //     c.Finished = false;
            // }

            return RegiesterTimer(id, completeTimeStamp, OnUpdate, OnComplete);
        }
        [Serializable]
        public class Counter : IDisposable
        {


            public string id;
            public System.Action OnComplete;

            /// <summary>
            /// <paras name="int"> The left time of current counter</paras>
            /// </summary>
            public System.Action<int> OnUpdate;
            public bool Finished = false;

            public int completeTimeStamp = 0;
            public int LeftTime
            {
                get
                {
                    return completeTimeStamp > GlobalTimer.CurrentTimeStamp ? completeTimeStamp - GlobalTimer.CurrentTimeStamp : 0;
                }
            }

            public void Update()
            {
                if (Finished == true)
                {
                    return;
                }
                OnUpdate?.Invoke(LeftTime);
                if (LeftTime <= 0)
                {
                    OnComplete?.Invoke();
                    Finished = true;
                }
            }
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            // Protected implementation of Dispose pattern.
            bool disposed = false;
            protected virtual void Dispose(bool disposing)
            {
                if (disposed)
                    return;

                if (disposing)
                {
                    //handle.Dispose();
                    // Free any other managed objects here.
                    //
                    completeTimeStamp = 0;
                    Finished = true;
                }

                disposed = true;
            }
            ~Counter()
            {
                //Debug.LogError($"Counter {id} dispose");
                Dispose(false);
            }
        }

    }
}