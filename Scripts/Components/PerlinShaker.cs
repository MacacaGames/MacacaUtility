using UnityEngine;
using System.Collections;

public class PerlinShaker : MonoBehaviour
{
    public static PerlinShaker _instance;
    public static PerlinShaker Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = "PerlinShaker";
                DontDestroyOnLoad(obj);
                _instance = obj.AddComponent<PerlinShaker>();
            }
            return _instance;
        }
        set { Instance = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="magnetude"></param>
    /// <param name="duration"></param>
    /// <param name="motion"> Perlin 在 Sameple 時遊走的速度。白話文：震度速率。 </param>
    /// <param name="isSexy"> Motion 會不會隨著時間遞減/削弱！白話文：會不會漸緩。 </param>
    /// <param name="isIgnoreTimeScale"></param>
    /// <param name="easeStyle"></param>
    /// <returns></returns>
    public static Coroutine ShakePosition(Transform transform, Vector3 magnetude, float duration, float motion, bool isSexy = false, bool isIgnoreTimeScale = false, EaseStyle easeStyle = EaseStyle.QuadEaseOut)
    {
        return Instance.StartCoroutine(Instance.Shaking(transform, magnetude, duration, motion, isSexy, isIgnoreTimeScale));
    }

    public Coroutine ShakePositionInstance(Transform transform, Vector3 magnetude, float duration, float motion, bool isSexy = false, bool isIgnoreTimeScale = false, EaseStyle easeStyle = EaseStyle.QuadEaseOut)
    {
        return StartCoroutine(Instance.Shaking(transform, magnetude, duration, motion, isSexy, isIgnoreTimeScale));
    }

    IEnumerator Shaking(Transform _transform, Vector3 magnetude, float duration, float motion, bool isSexy = false, bool isIgnoreTimeScale = false, EaseStyle easeStyle = EaseStyle.QuadEaseOut)
    {
        float lifeTime = duration;
        float age = lifeTime;
        Vector3 offset = Vector3.zero;
        Vector2 seedX = Vector2Extension.RandomInSqaure * 10f;
        Vector2 seedY = Vector2Extension.RandomInSqaure * 10f;
        Vector2 seedZ = Vector2Extension.RandomInSqaure * 10f;
        Vector2 motionVectX = Vector2Extension.RandomOnCircle * motion;
        Vector2 motionVectY = Vector2Extension.RandomOnCircle * motion;
        Vector2 motionVectZ = Vector2Extension.RandomOnCircle * motion;

        while (_transform != null & age > 0)
        {
            _transform.position -= offset;

            float deltaTime = (isIgnoreTimeScale) ? Time.unscaledDeltaTime : Time.deltaTime;
            age -= deltaTime;
            float power = (age / lifeTime);

            power *= EaseUtility.EasedLerp(0f, 1f, power, easeStyle);

            seedX += ((isSexy) ? motionVectX * power : motionVectX) * deltaTime;
            seedY += ((isSexy) ? motionVectY * power : motionVectY) * deltaTime;
            seedZ += ((isSexy) ? motionVectZ * power : motionVectZ) * deltaTime;

            offset = new Vector3(
                Mathf.PerlinNoise(seedX.x, seedX.y),
                Mathf.PerlinNoise(seedY.x, seedY.y),
                Mathf.PerlinNoise(seedZ.x, seedZ.y));

            //***** Center the noise signal *****
            offset -= Vector3.one * 0.5f;
            offset *= 2f;
            offset *= power;
            offset = new Vector3(
                offset.x * magnetude.x,
                offset.y * magnetude.y,
                offset.z * magnetude.z);
            //***** Center the noise signal *****

            _transform.position += offset;
            yield return null;
        }
        if (_transform != null) _transform.position -= offset;
        yield break;
    }


}