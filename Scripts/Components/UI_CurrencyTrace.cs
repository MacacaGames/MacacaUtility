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
    float suckTime = .5f;
    [SerializeField]
    Ease suckEase = Ease.InOutSine;

    IEnumerator TraceTask(Vector3 targetPos, System.Action onEnd)
    {
        float distance = Random.Range(0f, explodeDistance);
        float randomAngle = Random.Range(0f, 360f);
        Vector3 explodePos =
            transformCache.position +
            new Vector3(
                distance * Mathf.Cos(randomAngle),
                distance * Mathf.Sin(randomAngle),
                0);

        yield return transformCache.DOMove(explodePos, explodeTime).SetEase(explodeEase).WaitForCompletion();
        yield return transformCache.DOMove(targetPos, suckTime).SetEase(suckEase).WaitForCompletion();

        onEnd?.Invoke();
    }

}
