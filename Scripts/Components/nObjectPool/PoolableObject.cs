using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : TransformCacheBase
{
    public int poolKey { get; private set; }

    public void SetPoolKey(int key)
    {
        poolKey = key;
    }

    public virtual void RecoverSelf()
    {
        if (this == null)
            return;

        if (ObjectPoolManager.TryGetExistingObjectPool(this, out var pool))
        {
            pool.Recovery(this);
            return;
        }

        // Scene-authored objects have no pool key until their first recovery and retain
        // the legacy create-on-recovery behavior. A non-zero key belongs to a pool that
        // has already been torn down, so a delayed recovery must not recreate it.
        if (poolKey == 0)
            ObjectPoolManager.GetObjectPool(this)?.Recovery(this);
    }
    public virtual void RecoverSelf(float delay)
    {
        if (this == null)
            return;

        if (ObjectPoolManager.TryGetExistingObjectPool(this, out var pool))
        {
            pool.Recovery(this, delay);
            return;
        }

        if (poolKey == 0)
            ObjectPoolManager.GetObjectPool(this)?.Recovery(this, delay);
    }

    /// <summary>
    /// 如果希望 onRecovery 的時候關閉 gameObject，請記得呼叫 base.OnRecovery(),並且 OnReUse 記得呼叫 base.OnReuse() 來 active GameObject
    /// </summary>
    public virtual void OnRecovery()
    {
        gameObject.SetActive(false);
    }
    public virtual void OnReUse()
    {
        gameObject.SetActive(true);
    }
}
