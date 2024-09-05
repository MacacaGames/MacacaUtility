using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MacacaGames;

public class TraceBehaviour : PoolableObject
{
    [SerializeField]
    Vector2 explodeForceRandomRange = new Vector2(-200f, 200f); // 散開範圍
    [SerializeField]
    float acc = 1f; // 加速度
    [SerializeField]
    float destroyRange = 0.05f; // 終點臨界值
    [SerializeField]
    float scatterDuration = 1f; // 散開時間
    [SerializeField]
    float shrinkDuration = 1f; // 收束時間

    public void StartTrace(Vector3 targetPos, System.Action<GameObject, Vector3> onEnd)
    {
        CoroutineManager.Instance.StartCoroutine(TraceTask(targetPos, onEnd));
    }

    IEnumerator TraceTask(Vector3 targetPos, System.Action<GameObject, Vector3> onEnd)
    {
        Vector3 scatterDirection = Random.onUnitSphere * Random.Range(explodeForceRandomRange.x, explodeForceRandomRange.y);

        Vector3 startPosition = transformCache.position;
        Vector3 currentScale = transformCache.localScale;

        float scatterElapsedTime = 0;
        while (scatterElapsedTime < scatterDuration)
        {
            float t = scatterElapsedTime / scatterDuration;
            float easeOutT = 1 - Mathf.Pow(1 - t, 3);
            transformCache.position = Vector3.Lerp(startPosition, startPosition + scatterDirection, easeOutT);

            scatterElapsedTime += Time.deltaTime;
            yield return null;
        }

        float shrinkElapsedTime = 0;
        while (shrinkElapsedTime < shrinkDuration)
        {
            float dt = Time.deltaTime;

            float distanceToTarget = Vector3.Distance(transformCache.position, targetPos);

            if (distanceToTarget <= destroyRange)
            {
                transformCache.position = targetPos;
                break;
            }

            transformCache.position = Vector3.MoveTowards(transformCache.position, targetPos, acc * dt * 1000f);

            transformCache.localScale = Vector3.Lerp(currentScale, Vector3.zero, shrinkElapsedTime / shrinkDuration);

            shrinkElapsedTime += dt;
            yield return null;
        }

        transformCache.position = targetPos;
        transformCache.localScale = Vector3.zero;

        onEnd?.Invoke(gameObject, targetPos);

        RecoverSelf();
    }
}

