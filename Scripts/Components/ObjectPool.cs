using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MacacaGames
{
    [System.Obsolete("Use ObjectPoolManager with nObjectPool insteads")]
    public class ObjectPool : MonoBehaviour
    {
        private GameObject poolRoot;

        [SerializeField]
        private GameObject prefab;
        [SerializeField]
        private int initailSize = 20;
        [SerializeField]
        private Queue<GameObject> m_pool = new Queue<GameObject>();
        [SerializeField]
        private HashSet<GameObject> m_pool_using = new HashSet<GameObject>();

        [SerializeField] bool autoInit = false;
        [SerializeField] bool stayTransform = true;
        void Awake()
        {
            if (autoInit)
            {
                Init(prefab, initailSize);
            }
        }
        public void Init()
        {
            Init(prefab, initailSize);
        }
        public void Init(GameObject obj, int size)
        {
            prefab = obj;
            initailSize = size;

            poolRoot = new GameObject("Pool_" + prefab.name);
            poolRoot.transform.SetParent(transform, stayTransform);

            for (int cnt = 0; cnt < initailSize; cnt++)
            {
                GameObject go = Instantiate(prefab) as GameObject;
                m_pool.Enqueue(go);
                go.SetActive(false);
                go.transform.SetParent(poolRoot.transform);
            }
        }

        public GameObject ReUse(Vector3 position, Quaternion rotation)
        {
            GameObject reuse;
            if (m_pool.Count > 0)
            {
                reuse = m_pool.Dequeue();
            }
            else
            {
                reuse = Instantiate(prefab) as GameObject;
                reuse.transform.SetParent(poolRoot.transform);
            }

            reuse.transform.position = position;
            reuse.transform.rotation = rotation;
            reuse.SetActive(true);

            m_pool_using.Add(reuse);
            return reuse;
        }

        public T ReUse<T>(Vector3 position, Quaternion rotation) where T : Component
        {
            T reuse = ReUse(position, rotation).GetComponent<T>();

            if (reuse == null)
                throw new NullReferenceException("Object doesn't contain given Components");

            return reuse;
        }

        public void Recovery(GameObject recovery)
        {
            m_pool.Enqueue(recovery);
            recovery.SetActive(false);
            recovery.transform.SetParent(poolRoot.transform);

            if (m_pool_using.Contains(recovery))
            {
                m_pool_using.Remove(recovery);
            }
        }
        public void Recovery(GameObject recovery, float sec)
        {
            StartCoroutine(DelayRecovery(recovery, sec));
        }
        IEnumerator DelayRecovery(GameObject recovery, float time)
        {
            yield return Yielders.GetWaitForSeconds(time);
            Recovery(recovery);
        }
        public bool isReachPoolMax()
        {
            return m_pool.Count <= 0 ? true : false;
        }

        public void RecoveryAll()
        {
            foreach (GameObject obj in m_pool_using)
            {
                if (obj != null)
                {
                    m_pool.Enqueue(obj);
                    obj.SetActive(false);
                    obj.transform.SetParent(poolRoot.transform);
                }
                else
                {
                    Debug.LogWarningFormat("{0} 池內物件發生未回收就被刪除的情況", poolRoot.name);
                }
            }
            m_pool_using.Clear();
        }

        public int Count()
        {
            return m_pool.Count;
        }
    }
}