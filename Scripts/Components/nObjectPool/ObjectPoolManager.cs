using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPoolManager : UnitySingleton<ObjectPoolManager>
{
    static Dictionary<int, nObjectPool> PoolDict = new Dictionary<int, nObjectPool>();

    public static nObjectPool GetObjectPool(PoolableObject poolableObject, int defaultSize = DEFAULT_POOL_SIZE)
    {
        if (poolableObject == null)
            return null;

        int poolKey = poolableObject.poolKey;
        if (poolableObject.gameObject.scene.name == null)
        {
            poolKey = poolableObject.GetInstanceID();
        }

        nObjectPool poolDictCache;
        bool isHasPool = PoolDict.TryGetValue(poolKey, out poolDictCache);

        if (!isHasPool)
        {
            poolDictCache = CreateNewObjectPool(poolableObject, defaultSize);
        }

        return poolDictCache;
    }

    /// <summary>
    /// Returns an existing pool without creating a replacement. Recovery paths use this
    /// after a pool teardown so delayed callbacks cannot recreate pools from stale objects.
    /// </summary>
    public static bool TryGetExistingObjectPool(PoolableObject poolableObject, out nObjectPool pool)
    {
        pool = null;
        if (poolableObject == null)
            return false;

        int poolKey = poolableObject.poolKey;
        if (poolKey == 0)
            poolKey = poolableObject.GetInstanceID();

        return PoolDict.TryGetValue(poolKey, out pool) && pool != null;
    }

    public static PoolableObject GetObject(PoolableObject poolableObject, Vector3 pos, Quaternion rot, Transform parent = null)
    {
        var pool = GetObjectPool(poolableObject);
        return pool != null ? pool.ReUse(pos, rot, parent) : null;
    }
    public static T GetObject<T>(PoolableObject poolableObject, Vector3 pos, Quaternion rot, Transform parent = null) where T : Component
    {
        var pool = GetObjectPool(poolableObject);
        return pool != null ? pool.ReUse<T>(pos, rot, parent) : null;
    }

    public static T GetObject<T>(PoolableObject poolableObject, Transform parent = null) where T : Component
    {
        if (poolableObject == null)
            return null;

        var transformCache = poolableObject.transformCache;
        var pool = GetObjectPool(poolableObject);
        return pool != null ? pool.ReUse<T>(transformCache.position, transformCache.rotation, parent) : null;
    }

    public static void RecoveryAllPools()
    {
        foreach (var pool in PoolDict.Values)
        {
            if (pool != null)
                pool.RecoveryAll();
        }
        Debug.Log("[PoolManager] RecoveryAllPools completed.");
    }

    /// <summary>
    /// Destroys only currently idle instances. Active instances and their prefab owners
    /// remain valid, so this is safe to use while gameplay is still running.
    /// </summary>
    public static int DestroyIdleObjectsInAllPools()
    {
        int destroyedCount = 0;
        foreach (var pool in PoolDict.Values)
        {
            if (pool != null)
                destroyedCount += pool.DestroyIdleObjects();
        }

        Debug.Log($"[PoolManager] DestroyIdleObjectsInAllPools completed: destroyed={destroyedCount}.");
        return destroyedCount;
    }

    /// <summary>
    /// 銷毀所有池中的閒置物件並清空池字典，真正釋放記憶體。
    /// 應在戰鬥結束等重大場景轉換時呼叫。
    /// </summary>
    public static void DestroyAllPools()
    {
        foreach (var pool in PoolDict.Values)
        {
            if (pool != null)
                pool.DestroyAll();
        }
        PoolDict.Clear();
        Debug.Log("[PoolManager] DestroyAllPools completed.");
    }

    const int DEFAULT_POOL_SIZE = 3;
    static nObjectPool CreateNewObjectPool(PoolableObject refIPoolable, int size = DEFAULT_POOL_SIZE)
    {
        nObjectPool pool = Instance.gameObject.AddComponent<nObjectPool>();
        pool.Init(refIPoolable, size);
        Debug.LogFormat("[PoolManager][CreateNewPool] 創建新池: {0}", pool.poolKey);
        PoolDict.Add(pool.poolKey, pool);
        return pool;
    }
}
