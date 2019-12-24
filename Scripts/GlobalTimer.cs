using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace CloudMacaca
{
    public class GlobalTimer : UnitySingleton<GlobalTimer>
    {
        public static float deltaTime = 0;
        public static int currentTimeStamp;

        void Update()
        {
            deltaTime = Time.deltaTime;
            currentTimeStamp = CloudMacaca.Utility.GetTimeStamp();
            for (int i = allCounter.Count - 1; i >= 0; --i)
            {
                allCounter[i].Update();
            }
            allCounter.RemoveAll(r => r.Finished);
        }

        List<Counter> allCounter = new List<Counter>();
        public static Counter RegiesterTimer(string id, int completeTimeStamp, System.Action<int> OnUpdate, System.Action OnComplete)
        {
            if (Instance.allCounter.Count(m => m.id == id) > 0)
            {
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
            return c;
        }

        public static Counter RenewTimer(string id, int completeTimeStamp, System.Action<int> OnUpdate, System.Action OnComplete)
        {
            var c = Instance.allCounter.SingleOrDefault(m => m.id == id);
            if (c == null)
            {
                return c;
            }
            c.completeTimeStamp = completeTimeStamp;
            c.OnComplete = OnComplete;
            c.OnUpdate = OnUpdate;
            return c;
        }

        public class Counter
        {
            public string id;
            public System.Action OnComplete;

            /// <summary>
            /// <paras name="int"> The left time of current counter</paras>
            /// </summary>
            public System.Action<int> OnUpdate;
            public bool Finished = false;
            public int completeTimeStamp = 0;
            public int leftTime
            {
                get
                {
                    return completeTimeStamp - GlobalTimer.currentTimeStamp;
                }
            }
            public void Update()
            {
                if (Finished == true)
                {
                    return;
                }
                OnUpdate?.Invoke(leftTime);
                if (leftTime <= 0)
                {
                    OnComplete?.Invoke();
                    Finished = true;
                }
            }
        }
    }
}