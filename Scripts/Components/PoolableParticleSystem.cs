using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (ParticleSystem))]
public class PoolableParticleSystem : PoolableObject
{
    [SerializeField]
    bool isReusableWhenPlaying = true;
    ParticleSystem _particleSystem;
    ParticleSystem particleSystem
    {
        get
        {
            if (_particleSystem == null)
            {
                _particleSystem = GetComponent<ParticleSystem>();
            }
            return _particleSystem;
        }
    }

    bool isDetecting = false;

    public bool isUsedInThisFrame { get; private set; } = false;
    public void PlayParticle()
    {
        particleSystem.Play();
        isDetecting = true;
        isUsedInThisFrame = true;
    }

    private void Update()
    {
        if (!isDetecting)
            return;

        if (isUsedInThisFrame && isReusableWhenPlaying)
        {
            RecoverSelf();
            return;
        }

        if (!particleSystem.isPlaying && isDetecting)
        {
            RecoverSelf();
        }
    }

    private void OnDisable()
    {
        if (isDetecting)
            CoroutineManager.Instance.StartCoroutine(RecoverSelfNextFrame());
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
    }

    public override void OnReUse()
    {
        isUsedInThisFrame = false;
    }
}
