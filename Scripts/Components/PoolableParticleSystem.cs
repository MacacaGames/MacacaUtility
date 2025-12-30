using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (ParticleSystem))]
public class PoolableParticleSystem : PoolableVfx
{
    ParticleSystem _particleSystem;
    protected ParticleSystem particleSystem
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

    protected override void StopVfx()
    {
        particleSystem.Stop();
    }

    protected override void PlayVfx()
    {
        particleSystem.Play();
    }

    public override bool IsVfxPlaying()
    {
        return particleSystem.isPlaying;
    }
}