using UnityEngine;

//namespace CloudMacaca
//{
    public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T instance;

        static object lockObj = new object();

        public static T Instance
        {
            get
            {
                // if (applicationIsQuitting)
                // {
                //     Debug.LogWarningFormat("[Singleton] Instance {0} have destroyed (Maybe application quit)",
                //         typeof(T));

                //     return null;
                // }

                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = (T)FindObjectOfType(typeof(T));

                            int howManyObjOfType = FindObjectsOfType(typeof(T)).Length;

                            if (howManyObjOfType == 1)
                            {
                                Debug.LogFormat("[Singleton] {0} was created", instance);
                            }
                            else if (howManyObjOfType > 1)
                            {
                                Debug.LogErrorFormat("[Singleton] {0} already has {1} in the scene", typeof(T), howManyObjOfType);
                            }
                            else
                            {
                                GameObject singleton = new GameObject();
                                instance = singleton.AddComponent<T>();
                                singleton.name = string.Format("Singleton_{0}", typeof(T));

                                DontDestroyOnLoad(singleton);

                                Debug.LogWarningFormat("[Singleton] Use Lazy Initialization\n{1} was created with DontDestroyOnLoad",
                                    typeof(T),
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
    }
//}