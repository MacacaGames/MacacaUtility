﻿using System.Collections;
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
        ObjectPoolManager.GetObjectPool(this).Recovery(this);
    }
    public virtual void RecoverSelf(float delay)
    {
        ObjectPoolManager.GetObjectPool(this).Recovery(this, delay);
    }

    public virtual void OnRecovery()
    {

    }
    public virtual void OnReUse()
    {

    }
}
