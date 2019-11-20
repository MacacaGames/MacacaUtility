using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCacheBase : MonoBehaviour
{
    Transform _transformCache;
    public Transform transformCache
    {
        get
        {
            if (_transformCache == null)
                _transformCache = transform;
            return _transformCache;
        }
    }
}