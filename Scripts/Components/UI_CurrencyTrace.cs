using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MacacaGames;
using UnityEngine.UI;
using DG.Tweening;

public class UI_CurrencyTrace : PoolableObject
{
    [SerializeField]
    Image currencyImage;
    
    public void SetCurrencyImage(Sprite img)
    {
        currencyImage.sprite = img;
    }

    public void StartTrace(Vector3 targetPos, System.Action onEnd)
    {
        CoroutineManager.Instance.StartCoroutine(TraceTask(targetPos, onEnd));
    }

    [SerializeField]
    float explodeDistance = 2.5f;
    [SerializeField]
    float explodeTime = .5f;
    [SerializeField]
    Ease explodeEase = Ease.OutCubic;
    [SerializeField]
    ExplodeShape explodeShape = ExplodeShape.InsideSphere;

    [SerializeField]
    float suckTime = .5f;
    [SerializeField]
    Ease suckEase = Ease.InOutSine;

    IEnumerator TraceTask(Vector3 targetPos, System.Action onEnd )
    {
        Vector3 explodeOffset = GetRandomExplodeOffset();
        var explodePos = transformCache.localPosition + explodeOffset;

        yield return transformCache.DOLocalMove(explodePos, explodeTime).SetEase(explodeEase).WaitForCompletion();
        yield return transformCache.DOMove(targetPos, suckTime).SetEase(suckEase).WaitForCompletion();

        onEnd?.Invoke();
    }

    Vector3 GetRandomExplodeOffset()
    {
        Vector3 result = Vector3.zero;
        switch (explodeShape)
        {
            case ExplodeShape.InsideCircle:
                result = Random.insideUnitCircle;
                break;
            case ExplodeShape.InsideSphere:
                result = Random.insideUnitSphere;
                break;
            case ExplodeShape.OnSpherePlane:
                result= Random.onUnitSphere;
                break;
        }

        result.z = 0;
        result *= explodeDistance;
        return result;
    }

    enum ExplodeShape
    {
        InsideCircle,
        InsideSphere,
        OnSpherePlane,
    }
}
