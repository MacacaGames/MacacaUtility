using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class nObjectPool : MonoBehaviour
{
    private GameObject poolRoot;

    [SerializeField]
    private PoolableObject poolableObject;

    public int poolKey { get; private set; }

    [SerializeField]
    private int initailSize = 3;
    [SerializeField]
    private int maxSize = 20;
    [SerializeField]
    private Queue<PoolableObject> m_pool = new Queue<PoolableObject>();
    [SerializeField]
    private HashSet<PoolableObject> m_pool_using = new HashSet<PoolableObject>();

    [SerializeField] bool autoInit = false;
    [SerializeField] bool stayTransform = true;
    void Awake()
    {
        if (autoInit)
        {
            Init(poolableObject, initailSize);
        }
    }

    public void Init()
    {
        Init(poolableObject, initailSize);
    }

    public void Init(PoolableObject obj, int size, int max = 20)
    {
        poolableObject = obj;
        initailSize = size;
        maxSize = max;

        poolKey = poolableObject.GetInstanceID();
        poolRoot = new GameObject("Pool_" + obj.name + "_" + poolKey);
        poolRoot.transform.SetParent(transform, stayTransform);
        for (int cnt = 0; cnt < initailSize; cnt++)
        {
            PoolableObject newOne = NewOne();
            Recovery(newOne);
        }
    }
    public T ReUse<T>(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        PoolableObject poolableObject = ReUse(position, rotation, parent);
        GameObject poolableObj = poolableObject.gameObject;
        return poolableObj.GetComponent<T>();
    }

    public PoolableObject ReUse(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        PoolableObject poolableObject;
        if (m_pool.Count > 0)
            poolableObject = m_pool.Dequeue();
        else
            poolableObject = NewOne();

        GameObject poolableObj = poolableObject.gameObject;
        Transform poolableTransform = poolableObj.transform;
        poolableTransform.SetParent(parent);
        poolableTransform.SetPositionAndRotation(position, rotation);
        m_pool_using.Add(poolableObject);
        poolableObject.OnReUse();
        return poolableObject;
    }

    PoolableObject NewOne()
    {
        PoolableObject newOne = Instantiate(poolableObject) as PoolableObject;
        //newOne.gameObject.transform.SetParent(poolRoot.transform);
        newOne.SetPoolKey(poolKey);
        return newOne;
    }

    public void Recovery(PoolableObject poolableObject)
    {
        if (m_pool.Contains(poolableObject))
            return;

        if (m_pool_using.Contains(poolableObject))
        {
            m_pool_using.Remove(poolableObject);
        }

        poolableObject.OnRecovery();

        if (m_pool.Count >= maxSize)
        {
            Destroy(poolableObject.gameObject);
            return;
        }

        m_pool.Enqueue(poolableObject);
        poolableObject.gameObject.transform.SetParent(poolRoot.transform);
    }

    public void Recovery(PoolableObject poolableObject, float sec)
    {
        StartCoroutine(DelayRecovery(poolableObject, sec));
    }

    IEnumerator DelayRecovery(PoolableObject poolableObject, float time)
    {
        yield return Yielders.GetWaitForSeconds(time);
        Recovery(poolableObject);
    }
    public bool isReachPoolMax()
    {
        return m_pool.Count <= 0 ? true : false;
    }

    public void RecoveryAll()
    {
        foreach (PoolableObject poolableObject in m_pool_using.ToArray())
        {
            if (poolableObject != null)
            {
                poolableObject.RecoverSelf();
                //m_pool.Enqueue(poolableObject);
                //GameObject poolableObj = poolableObject.gameObject;
                //poolableObj.SetActive(false);
                //poolableObj.transform.SetParent(poolRoot.transform);
            }
            else
            {
                Debug.LogWarningFormat("{0} 池內物件發生未回收就被刪除的情況", poolRoot.name);
            }
        }
        m_pool_using.Clear();
    }

    /// <summary>
    /// 銷毀池中所有物件（閒置 + 使用中），並銷毀 poolRoot。
    /// </summary>
    public void DestroyAll()
    {
        foreach (PoolableObject poolableObject in m_pool_using.ToArray())
        {
            if (poolableObject != null)
                Destroy(poolableObject.gameObject);
        }
        m_pool_using.Clear();

        while (m_pool.Count > 0)
        {
            var poolableObject = m_pool.Dequeue();
            if (poolableObject != null)
                Destroy(poolableObject.gameObject);
        }

        if (poolRoot != null)
            Destroy(poolRoot);
    }

    public int Count()
    {
        return m_pool.Count;
    }
}