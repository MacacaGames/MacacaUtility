using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class PoolableVfx : PoolableObject
{
    [SerializeField] bool isReusableWhenPlaying = true;
    [SerializeField] private float forceRecoverTime = 0;

    bool isDetecting = false;

    public bool isUsedInThisFrame { get; private set; } = false;

    public Action OnVfxStart;
    public Action OnVfxLeave;
    
    bool isForceRecoverByTime => forceRecoverTime > 0;

    public virtual void Play()
    {
        OnVfxStart?.Invoke();
        PlayVfx();
        isDetecting = true;
        isUsedInThisFrame = true;
        
        if (isForceRecoverByTime)
        {
            Invoke("InvokeRecoverSelf", forceRecoverTime);
        }
    }
    void InvokeRecoverSelf()
    {
        StopVfx();
        RecoverSelf();
    }

    protected abstract void StopVfx();

    protected abstract void PlayVfx();

    protected virtual void Update()
    {
        if (isForceRecoverByTime)
        {
            return;
        }
        if (!isDetecting)
        {
            return;
        }
        
        if (isUsedInThisFrame && isReusableWhenPlaying)
        {
            RecoverSelf();
            return;
        }

        if (!IsVfxPlaying() && isDetecting)
        {
            RecoverSelf();
        }
    }

    protected abstract bool IsVfxPlaying();


    private void OnDisable()
    {
        if (isDetecting)
            CoroutineManager.Instance.StartCoroutine(RecoverSelfNextFrame());
    }
        

    public float GetDelayRecoverTime()
    {
        return forceRecoverTime;
    }

    IEnumerator RecoverSelfNextFrame()
    {
        yield return null;
        RecoverSelf();
    }

    public override void OnRecovery()
    {
        //particleSystem.Stop();
        isDetecting = false;
        OnVfxLeave?.Invoke();
    }

    public override void OnReUse()
    {
        isUsedInThisFrame = false;
    }
}