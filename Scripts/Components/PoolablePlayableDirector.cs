using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof (PlayableDirector))]
public class PoolablePlayableDirector : PoolableVfx
{
    PlayableDirector _playableDirector;
    protected PlayableDirector playableDirector
    {
        get
        {
            if (_playableDirector == null)
            {
                _playableDirector = GetComponent<PlayableDirector>();
            }
            return _playableDirector;
        }
    }

    protected override void StopVfx()
    {
        playableDirector.Stop();
    }

    protected override void PlayVfx()
    {
        playableDirector.Play();
    }

    public override bool IsVfxPlaying()
    {
        return playableDirector.state == PlayState.Playing;
    }
}