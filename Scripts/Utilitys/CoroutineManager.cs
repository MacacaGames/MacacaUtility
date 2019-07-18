using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoroutineManager : MonoBehaviour
{
    static CoroutineManager instance;

    static object lockObj = new object();

    public static CoroutineManager Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarningFormat("[Singleton] Instance {0} have destroyed (Maybe application quit)", 
                    typeof(CoroutineManager));

                return null;
            }

            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = (CoroutineManager)FindObjectOfType(typeof(CoroutineManager));

                        int howManyObjOfType = FindObjectsOfType(typeof(CoroutineManager)).Length;

                        if (howManyObjOfType == 1)
                        {
                            Debug.LogFormat("[Singleton] {0} was created", instance);
                        }
                        else if (howManyObjOfType > 1)
                        {
                            Debug.LogErrorFormat("[Singleton] {0} already has {1} in the scene", typeof(CoroutineManager), howManyObjOfType);
                        }
                        else
                        {
                            GameObject singleton = new GameObject();
                            instance = singleton.AddComponent<CoroutineManager>();
                            singleton.name = string.Format("Singleton_{0}", typeof(CoroutineManager));

                            DontDestroyOnLoad(singleton);

                            Debug.LogWarningFormat("[Singleton] Use Lazy Initialization\n{1} was created with DontDestroyOnLoad", 
                                typeof(CoroutineManager), 
                                instance);
                        }
                    }
                }
            }

            return instance;
        }
    }

    static bool applicationIsQuitting = false;

    protected virtual void OnDestroy()
    {
        applicationIsQuitting = true;
    }

    /// <summary>
    /// 回傳一個可被 yield return 的協程，做一個由 0 至 1 的 tween 並每個 update 去執行 action，float 則是 progress
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="progressAction"></param>
    /// <param name="easeType"></param>
    /// <returns></returns>
    public static YieldInstruction ProgressionTask(float duration, System.Action<float> progressAction, Ease easeType = Ease.Linear)
    {
        return
            DOTween.To(() => 0f,
            x =>
            {
                progressAction(x);
            },
            1f, duration)
            .SetEase(easeType)
            .WaitForCompletion();
    }
}
