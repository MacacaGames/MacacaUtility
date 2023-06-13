using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace MacacaGames
{
    /// <summary>
    /// A Time manager in Unity
    /// </summary>
    public class GlobalTimer : UnitySingleton<GlobalTimer>
    {
        static double lastTimeSinceStartup = 0f;
        private static double _editorDeltaTime;
        /// <summary>
        /// The deltaTime between frame and frame, same as Time.deltaTime but cached!
        /// <seealso cref="Time.deltaTime"/>
        /// </summary>
        /// <value></value>
        public static double editorDeltaTime
        {
            get
            {
#if UNITY_EDITOR
                if (lastTimeSinceStartup == 0f)
                {
                    lastTimeSinceStartup = UnityEditor.EditorApplication.timeSinceStartup;
                }
                _editorDeltaTime = UnityEditor.EditorApplication.timeSinceStartup - lastTimeSinceStartup;
                lastTimeSinceStartup = UnityEditor.EditorApplication.timeSinceStartup;
#endif
                return _editorDeltaTime;
            }
        }


        public Action OnResume;
        private float _deltaTime;
        private float _fixedDeltaTime;

        /// <summary>
        /// The deltaTime between frame and frame, same as Time.deltaTime but cached!
        /// <seealso cref="Time.deltaTime"/>
        /// </summary>
        /// <value></value>
        public static float deltaTime
        {
            get
            {
                return Instance._deltaTime;
            }
        }

        /// <summary>
        /// The interval in seconds at which physics and other fixed frame rate updates, same as Time.fixedDeltaTime but cached!
        /// <seealso cref="Time.fixedDeltaTime"/>
        /// </summary>
        /// <value></value>
        public static float fixedDeltaTime
        {
            get
            {
                return Instance._fixedDeltaTime;
            }
        }

        private static double _currentTimeStamp;

        /// <summary>
        /// Current TimeStamp, the total seconds from 1970/1/1
        /// </summary>
        /// <value>Times in secs</value>
        public static int CurrentTimeStamp
        {
            get
            {
                if (_currentTimeStamp == 0)
                {
                    _currentTimeStamp = MacacaGames.Utility.GetTimeStamp();
                }
                return (int)_currentTimeStamp;
            }
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
            if (hasFocus)
            {
                UpdateTimeStampToSystemTime();
                OnResume?.Invoke();
            }
        }

        void Update()
        {
            _deltaTime = Time.deltaTime;
            tempRegulateRate += _deltaTime;

            if (setting.refreshTimeMethod == GlobalTimerSetting.RefreshTimeMethod.SystemDateTime)
                UpdateTimeStampToSystemTime();
            else
                _currentTimeStamp += _deltaTime;

            for (int i = allTimer.Count - 1; i >= 0; --i)
            {
                allTimer[i]?.Update();
            }

            if (tempRegulateRate > setting.RegulateRateIfDelta)
            {
                UpdateTimeStampToSystemTime();
                tempRegulateRate = 0;
            }
        }

        void FixedUpdate()
        {
            _fixedDeltaTime = Time.fixedDeltaTime;
        }

        void LateUpdate()
        {
            allTimer.RemoveAll(r => r.Finished);
        }
        void UpdateTimeStampToSystemTime()
        {
            _currentTimeStamp = MacacaGames.Utility.GetTimeStamp();
        }



        /// <summary>
        /// Registe a timer
        /// </summary>
        /// <param name="id">The timer's id</param>
        /// <param name="completeTimeStamp">The complete timestamp of the timer</param>
        /// <param name="OnUpdate">Invoke during the timer is counting</param>
        /// <param name="OnComplete">Invoke while the timer is done</param>
        /// <param name="AutoRenew">Should renew the timer if the timer with same id is exsit?</param>
        /// <returns>The timer instance</returns>
        public static Timer RegisteTimer(string id, int completeTimeStamp, System.Action<int> OnUpdate, System.Action OnComplete, bool AutoRenew = true)
        {
            if (Instance.allTimer.Count(m => m.id == id) > 0)
            {
                if (AutoRenew == true)
                {
                    return RenewTimer(id, completeTimeStamp, OnUpdate, OnComplete);
                }
                Debug.LogError("Counter with given id is already exist!");
                return null;
            }
            Timer c = new Timer
            {
                id = id,
                OnComplete = OnComplete,
                OnUpdate = OnUpdate,
                completeTimeStamp = completeTimeStamp,
            };
            Instance.allTimer.Add(c);

            //Do one update make sure relative user get the correct lefttime.
            c.Update();

            return c;
        }

        /// <summary>
        /// Renew a exsit timer with parameters, will override all parameters!
        /// </summary>
        /// <param name="id">The target timer id to renew</param>
        /// <param name="completeTimeStamp">Complete timestamp</param>
        /// <param name="OnUpdate">Invoke during the timer is counting</param>
        /// <param name="OnComplete">Invoke while the timer is done</param>
        /// <returns>The timer instance</returns>
        public static Timer RenewTimer(string id, int completeTimeStamp, System.Action<int> OnUpdate, System.Action OnComplete)
        {
            var c = Instance.allTimer.SingleOrDefault(m => m.id == id);
            if (c != null)
            {
                Instance.allTimer.Remove(c);
                c.Dispose();
            }

            return RegisteTimer(id, completeTimeStamp, OnUpdate, OnComplete);
        }

        /// <summary>
        /// Renew a exsit timer with parameters, will override all parameters!
        /// </summary>
        /// <param name="counter">The target timer instancd to renew</param>
        /// <param name="completeTimeStamp">Complete timestamp</param>
        /// <param name="OnUpdate">Invoke during the timer is counting</param>
        /// <param name="OnComplete">Invoke while the timer is done</param>
        /// <returns>The timer instance</returns>
        public static Timer RenewTimer(Timer counter, int completeTimeStamp, System.Action<int> OnUpdate, System.Action OnComplete)
        {
            return RenewTimer(counter.id, completeTimeStamp, OnUpdate, OnComplete);
        }


        [SerializeField]
        List<Timer> allTimer = new List<Timer>();

        /// <summary>
        /// Timer  
        /// </summary>
        [Serializable]
        public class Timer : IDisposable
        {

            public string id;
            public System.Action OnComplete;

            /// <summary>
            /// <paras name="int"> The left time of current counter</paras>
            /// </summary>
            public System.Action<int> OnUpdate;
            public bool Finished = false;

            public int completeTimeStamp = 0;

            /// <summary>
            /// LeftTime of the timer
            /// </summary>
            /// <value>LeftTime of the timer</value>
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
            ~Timer()
            {
                //Debug.LogError($"Counter {id} dispose");
                Dispose(false);
            }
        }

    }
}