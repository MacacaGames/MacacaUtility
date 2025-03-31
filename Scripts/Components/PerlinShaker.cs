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
    /// <param name="motion"> Perlin 在 Sample 時遊走的速度。震度速率。 </param>
    /// <param name="isSexy"> Motion 會隨著時間遞減/削弱。 </param>
    /// <param name="isIgnoreTimeScale"></param>
    /// <param name="easeStyle"></param>
    /// <param name="timeAttenuate">每次震動時間的衰減因子</param>
    /// <param name="horPowerAttenuate">水平震動強度衰減</param>
    /// <param name="verPowerAttenuate">垂直震動強度衰減</param>
    /// <returns></returns>
    public static Coroutine ShakePosition(
        Transform transform,
        Vector3 magnetude,
        float duration,
        float motion,
        bool isSexy = false,
        bool isIgnoreTimeScale = false,
        EaseStyle easeStyle = EaseStyle.QuadEaseOut,
        float timeAttenuate = 1f,
        float horPowerAttenuate = 1f,
        float verPowerAttenuate = 1f)
    {
        return Instance.StartCoroutine(Instance.Shaking(transform, magnetude, duration, motion, isSexy, isIgnoreTimeScale, easeStyle, timeAttenuate, horPowerAttenuate, verPowerAttenuate));
    }

    public Coroutine ShakePositionInstance(
        Transform transform,
        Vector3 magnetude,
        float duration,
        float motion,
        bool isSexy = false,
        bool isIgnoreTimeScale = false,
        EaseStyle easeStyle = EaseStyle.QuadEaseOut,
        float timeAttenuate = 1f,
        float horPowerAttenuate = 1f,
        float verPowerAttenuate = 1f)
    {
        return StartCoroutine(Instance.Shaking(transform, magnetude, duration, motion, isSexy, isIgnoreTimeScale, easeStyle, timeAttenuate, horPowerAttenuate, verPowerAttenuate));
    }

    IEnumerator Shaking(
        Transform _transform,
        Vector3 magnetude,
        float duration,
        float motion,
        bool isSexy = false,
        bool isIgnoreTimeScale = false,
        EaseStyle easeStyle = EaseStyle.QuadEaseOut,
        float timeAttenuate = 1f,
        float horPowerAttenuate = 1f,
        float verPowerAttenuate = 1f)
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

        while (_transform != null && age > 0)
        {
            _transform.position -= offset;

            float deltaTime = (isIgnoreTimeScale) ? Time.unscaledDeltaTime : Time.deltaTime;
            age -= deltaTime;
            float power = (age / lifeTime);

            // Apply ease style for smooth shaking
            power *= EaseUtility.EasedLerp(0f, 1f, power, easeStyle);

            // Adjust seed and offsets for motion
            seedX += ((isSexy) ? motionVectX * power : motionVectX) * deltaTime;
            seedY += ((isSexy) ? motionVectY * power : motionVectY) * deltaTime;
            seedZ += ((isSexy) ? motionVectZ * power : motionVectZ) * deltaTime;

            offset = new Vector3(
                Mathf.PerlinNoise(seedX.x, seedX.y),
                Mathf.PerlinNoise(seedY.x, seedY.y),
                Mathf.PerlinNoise(seedZ.x, seedZ.y));

            // Center the noise signal
            offset -= Vector3.one * 0.5f;
            offset *= 2f;
            offset *= power;
            offset = new Vector3(
                offset.x * magnetude.x,
                offset.y * magnetude.y,
                offset.z * magnetude.z);

            // Apply time attenuation to slow down the shake over time
            if (timeAttenuate > 0f)
            {
                offset *= timeAttenuate;
            }

            // Apply horizontal and vertical power attenuation to adjust shaking strength
            offset.x *= horPowerAttenuate;
            offset.y *= verPowerAttenuate;

            _transform.position += offset;
            yield return null;
        }

        if (_transform != null) _transform.position -= offset;
        yield break;
    }
}
