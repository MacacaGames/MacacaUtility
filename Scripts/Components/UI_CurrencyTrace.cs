using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    EaseStyle explodeEase = EaseStyle.CubicEaseOut;
    [SerializeField]
    ExplodeShape explodeShape = ExplodeShape.InsideSphere;

    [SerializeField]
    float suckTime = .5f;
    [SerializeField]
    EaseStyle suckEase = EaseStyle.SineEaseInOut;

    IEnumerator TraceTask(Vector3 targetPos, System.Action onEnd )
    {
        Vector3 explodeOffset = GetRandomExplodeOffset();
        var startPos = transformCache.position;
        var explodePos = startPos + explodeOffset;

        yield return EaseUtility.To(startPos, explodePos, explodeTime, explodeEase, pos =>
        {
            transformCache.position = pos;
        });
        
        yield return EaseUtility.To(explodePos, targetPos, suckTime, suckEase, pos =>
        {
            transformCache.position = pos;
        });
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
