using UnityEngine;
using System;
using System.Collections;
using MacacaGames;

public class EaseUtility
{
    /// <summary>
    /// Represents an eased interpolation w/ respect to time.
    /// 
    /// float t, float b, float c, float d
    /// </summary>
    /// <param name="current">how long into the ease are we</param>
    /// <param name="initialValue">starting value if current were 0</param>
    /// <param name="totalChange">total change in the value (not the end value, but the end - start)</param>
    /// <param name="duration">the total amount of time (when current == duration, the returned value will == initial + totalChange)</param>
    /// <returns></returns>
    delegate float EaseDelegate(float current, float initialValue, float totalChange, float duration);
    public static float EasedLerp(float from, float to, float t, EaseStyle easeStyle)
    {
        var f = Get(easeStyle);
        return f(t, from, to - from, 1f);
    }

    public static float EasedLerp(float from, float to, float t, AnimationCurve curve)
    {
        var f = FromAnimationCurve(curve);
        return f(t, from, to - from, 1f);
    }

    public static Vector3 EasedLerp(Vector3 from, Vector3 to, float t, EaseStyle easeStyle)
    {
        return new Vector3(EasedLerp(from.x, to.x, t, easeStyle), EasedLerp(from.y, to.y, t, easeStyle), EasedLerp(from.z, to.z, t, easeStyle));
    }

    public static Vector3 EasedLerp(Vector3 from, Vector3 to, float t, AnimationCurve curve)
    {
        return new Vector3(EasedLerp(from.x, to.x, t, curve), EasedLerp(from.y, to.y, t, curve), EasedLerp(from.z, to.z, t, curve));
    }

    public static Vector2 EasedLerp(Vector2 from, Vector2 to, float t, EaseStyle easeStyle)
    {
        return new Vector2(EasedLerp(from.x, to.x, t, easeStyle), EasedLerp(from.y, to.y, t, easeStyle));
    }

    public static Vector2 EasedLerp(Vector2 from, Vector2 to, float t, AnimationCurve curve)
    {
        return new Vector2(EasedLerp(from.x, to.x, t, curve), EasedLerp(from.y, to.y, t, curve));
    }

    public static IEnumerator To(float start, float end, float duration, EaseStyle ease, Action<float> OnUpdate, Action OnComplete = null, float delay = 0)
    {
        float t = 0f;
        OnUpdate?.Invoke(start);
        while (t < delay)
        {
            yield return null;

#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                t += GlobalTimer.deltaTime;
            }
            else
            {
                t += (float)GlobalTimer.editorDeltaTime;
            }
#else
            t += GlobalTimer.deltaTime;
#endif
        }
        t = 0f;
        while (t < duration)
        {
            float sc = EasedLerp(start, end, t / duration, ease);
            OnUpdate?.Invoke(sc);
            yield return null;
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                t += GlobalTimer.deltaTime;
            }
            else
            {
                t += (float)GlobalTimer.editorDeltaTime;
            }
#else
            t += GlobalTimer.deltaTime;
#endif
        }
        OnUpdate.Invoke(end);
        OnComplete?.Invoke();
    }
    public static IEnumerator To(Vector3 start, Vector3 end, float duration, EaseStyle ease, Action<Vector3> OnUpdate, Action OnComplete = null, float delay = 0)
    {
        float t = 0f;
        OnUpdate?.Invoke(start);
        while (t < delay)
        {
            yield return null;
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                t += GlobalTimer.deltaTime;
            }
            else
            {
                t += (float)GlobalTimer.editorDeltaTime;
            }
#else
            t += GlobalTimer.deltaTime;
#endif
        }
        t = 0f;
        Vector3 sc;
        while (t < duration)
        {
            sc = EasedLerp(start, end, t / duration, ease);
            OnUpdate?.Invoke(sc);
            yield return null;
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                t += GlobalTimer.deltaTime;
            }
            else
            {
                t += (float)GlobalTimer.editorDeltaTime;
            }
#else
            t += GlobalTimer.deltaTime;
#endif
        }
        OnUpdate.Invoke(end);
        OnComplete?.Invoke();
    }

    static EaseDelegate Get(EaseStyle style)
    {
        switch (style)
        {
            case EaseStyle.Linear: return LinearEaseNone;
            case EaseStyle.LinearEaseIn: return LinearEaseIn;
            case EaseStyle.LinearEaseOut: return LinearEaseOut;
            case EaseStyle.LinearEaseInOut: return LinearEaseInOut;

            case EaseStyle.BackEaseIn: return BackEaseIn;
            case EaseStyle.BackEaseOut: return BackEaseOut;
            case EaseStyle.BackEaseInOut: return BackEaseInOut;

            case EaseStyle.BounceEaseIn: return BounceEaseIn;
            case EaseStyle.BounceEaseOut: return BounceEaseOut;
            case EaseStyle.BounceEaseInOut: return BounceEaseInOut;

            case EaseStyle.CircleEaseIn: return CircleEaseIn;
            case EaseStyle.CircleEaseOut: return CircleEaseOut;
            case EaseStyle.CircleEaseInOut: return CircleEaseInOut;

            case EaseStyle.CubicEaseIn: return CubicEaseIn;
            case EaseStyle.CubicEaseOut: return CubicEaseOut;
            case EaseStyle.CubicEaseInOut: return CubicEaseInOut;

            case EaseStyle.ElasticEaseIn: return ElasticEaseIn;
            case EaseStyle.ElasticEaseOut: return ElasticEaseOut;
            case EaseStyle.ElasticEaseInOut: return ElasticEaseInOut;

            case EaseStyle.ExpoEaseIn: return ExpoEaseIn;
            case EaseStyle.ExpoEaseOut: return ExpoEaseOut;
            case EaseStyle.ExpoEaseInOut: return ExpoEaseInOut;

            case EaseStyle.QuadEaseIn: return QuadEaseIn;
            case EaseStyle.QuadEaseOut: return QuadEaseOut;
            case EaseStyle.QuadEaseInOut: return QuadEaseInOut;

            case EaseStyle.QuartEaseIn: return QuartEaseIn;
            case EaseStyle.QuartEaseOut: return QuartEaseOut;
            case EaseStyle.QuartEaseInOut: return QuartEaseInOut;

            case EaseStyle.QuintEaseIn: return QuintEaseIn;
            case EaseStyle.QuintEaseOut: return QuintEaseOut;
            case EaseStyle.QuintEaseInOut: return QuintEaseInOut;

            case EaseStyle.SineEaseIn: return SineEaseIn;
            case EaseStyle.SineEaseOut: return SineEaseOut;
            case EaseStyle.SineEaseInOut: return SineEaseInOut;

            case EaseStyle.StrongEaseIn: return StrongEaseIn;
            case EaseStyle.StrongEaseOut: return StrongEaseOut;
            case EaseStyle.StrongEaseInOut: return StrongEaseInOut;
        }

        return null;
    }

    #region Back Ease

    private static EaseDelegate _backEaseIn;
    static EaseDelegate BackEaseIn
    {
        get
        {
            if (_backEaseIn == null) _backEaseIn = EaseImplement.BackEaseIn;
            return _backEaseIn;
        }
    }

    private static EaseDelegate _backEaseOut;
    static EaseDelegate BackEaseOut
    {
        get
        {
            if (_backEaseOut == null) _backEaseOut = EaseImplement.BackEaseOut;
            return _backEaseOut;
        }
    }

    private static EaseDelegate _backEaseInOut;
    static EaseDelegate BackEaseInOut
    {
        get
        {
            if (_backEaseInOut == null) _backEaseInOut = EaseImplement.BackEaseInOut;
            return _backEaseInOut;
        }
    }

    #endregion

    #region Bounce Ease

    private static EaseDelegate _bounceEaseIn;
    static EaseDelegate BounceEaseIn
    {
        get
        {
            if (_bounceEaseIn == null) _bounceEaseIn = EaseImplement.BounceEaseIn;
            return _bounceEaseIn;
        }
    }

    private static EaseDelegate _bounceEaseOut;
    static EaseDelegate BounceEaseOut
    {
        get
        {
            if (_bounceEaseOut == null) _bounceEaseOut = EaseImplement.BounceEaseOut;
            return _bounceEaseOut;
        }
    }

    private static EaseDelegate _bounceEaseInOut;
    static EaseDelegate BounceEaseInOut
    {
        get
        {
            if (_bounceEaseInOut == null) _bounceEaseInOut = EaseImplement.BounceEaseInOut;
            return _bounceEaseInOut;
        }
    }

    #endregion

    #region Circle Ease

    private static EaseDelegate _circleEaseIn;
    static EaseDelegate CircleEaseIn
    {
        get
        {
            if (_circleEaseIn == null) _circleEaseIn = EaseImplement.CircleEaseIn;
            return _circleEaseIn;
        }
    }

    private static EaseDelegate _circleEaseOut;
    static EaseDelegate CircleEaseOut
    {
        get
        {
            if (_circleEaseOut == null) _circleEaseOut = EaseImplement.CircleEaseOut;
            return _circleEaseOut;
        }
    }

    private static EaseDelegate _circleEaseInOut;
    static EaseDelegate CircleEaseInOut
    {
        get
        {
            if (_circleEaseInOut == null) _circleEaseInOut = EaseImplement.CircleEaseInOut;
            return _circleEaseInOut;
        }
    }

    #endregion

    #region Cubic Ease

    private static EaseDelegate _cubicEaseIn;
    static EaseDelegate CubicEaseIn
    {
        get
        {
            if (_cubicEaseIn == null) _cubicEaseIn = EaseImplement.CubicEaseIn;
            return _cubicEaseIn;
        }
    }

    private static EaseDelegate _cubicEaseOut;
    static EaseDelegate CubicEaseOut
    {
        get
        {
            if (_cubicEaseOut == null) _cubicEaseOut = EaseImplement.CubicEaseOut;
            return _cubicEaseOut;
        }
    }

    private static EaseDelegate _cubicEaseInOut;
    static EaseDelegate CubicEaseInOut
    {
        get
        {
            if (_cubicEaseInOut == null) _cubicEaseInOut = EaseImplement.CubicEaseInOut;
            return _cubicEaseInOut;
        }
    }

    #endregion

    #region Elastic Ease

    private static EaseDelegate _elasticEaseIn;
    static EaseDelegate ElasticEaseIn
    {
        get
        {
            if (_elasticEaseIn == null) _elasticEaseIn = EaseImplement.ElasticEaseIn;
            return _elasticEaseIn;
        }
    }

    private static EaseDelegate _elasticEaseOut;
    static EaseDelegate ElasticEaseOut
    {
        get
        {
            if (_elasticEaseOut == null) _elasticEaseOut = EaseImplement.ElasticEaseOut;
            return _elasticEaseOut;
        }
    }

    private static EaseDelegate _elasticEaseInOut;
    static EaseDelegate ElasticEaseInOut
    {
        get
        {
            if (_elasticEaseInOut == null) _elasticEaseInOut = EaseImplement.ElasticEaseInOut;
            return _elasticEaseInOut;
        }
    }

    #endregion

    #region Expo Ease

    private static EaseDelegate _expoEaseIn;
    static EaseDelegate ExpoEaseIn
    {
        get
        {
            if (_expoEaseIn == null) _expoEaseIn = EaseImplement.ExpoEaseIn;
            return _expoEaseIn;
        }
    }

    private static EaseDelegate _expoEaseOut;
    static EaseDelegate ExpoEaseOut
    {
        get
        {
            if (_expoEaseOut == null) _expoEaseOut = EaseImplement.ExpoEaseOut;
            return _expoEaseOut;
        }
    }

    private static EaseDelegate _expoEaseInOut;
    static EaseDelegate ExpoEaseInOut
    {
        get
        {
            if (_expoEaseInOut == null) _expoEaseInOut = EaseImplement.ExpoEaseInOut;
            return _expoEaseInOut;
        }
    }

    #endregion

    #region Linear Ease

    private static EaseDelegate _linearEaseNone;
    static EaseDelegate LinearEaseNone
    {
        get
        {
            if (_linearEaseNone == null) _linearEaseNone = EaseImplement.LinearEaseNone;
            return _linearEaseNone;
        }
    }

    private static EaseDelegate _linearEaseIn;
    static EaseDelegate LinearEaseIn
    {
        get
        {
            if (_linearEaseIn == null) _linearEaseIn = EaseImplement.LinearEaseIn;
            return _linearEaseIn;
        }
    }

    private static EaseDelegate _linearEaseOut;
    static EaseDelegate LinearEaseOut
    {
        get
        {
            if (_linearEaseOut == null) _linearEaseOut = EaseImplement.LinearEaseOut;
            return _linearEaseOut;
        }
    }

    private static EaseDelegate _linearEaseInOut;
    static EaseDelegate LinearEaseInOut
    {
        get
        {
            if (_linearEaseInOut == null) _linearEaseInOut = EaseImplement.LinearEaseInOut;
            return _linearEaseInOut;
        }
    }

    #endregion

    #region Quad Ease

    private static EaseDelegate _quadEaseIn;
    static EaseDelegate QuadEaseIn
    {
        get
        {
            if (_quadEaseIn == null) _quadEaseIn = EaseImplement.QuadEaseIn;
            return _quadEaseIn;
        }
    }

    private static EaseDelegate _quadEaseOut;
    static EaseDelegate QuadEaseOut
    {
        get
        {
            if (_quadEaseOut == null) _quadEaseOut = EaseImplement.QuadEaseOut;
            return _quadEaseOut;
        }
    }

    private static EaseDelegate _quadEaseInOut;
    static EaseDelegate QuadEaseInOut
    {
        get
        {
            if (_quadEaseInOut == null) _quadEaseInOut = EaseImplement.QuadEaseInOut;
            return _quadEaseInOut;
        }
    }

    #endregion

    #region Quart Ease

    private static EaseDelegate _quartEaseIn;
    static EaseDelegate QuartEaseIn
    {
        get
        {
            if (_quartEaseIn == null) _quartEaseIn = EaseImplement.QuartEaseIn;
            return _quartEaseIn;
        }
    }

    private static EaseDelegate _quartEaseOut;
    static EaseDelegate QuartEaseOut
    {
        get
        {
            if (_quartEaseOut == null) _quartEaseOut = EaseImplement.QuartEaseOut;
            return _quartEaseOut;
        }
    }

    private static EaseDelegate _quartEaseInOut;
    static EaseDelegate QuartEaseInOut
    {
        get
        {
            if (_quartEaseInOut == null) _quartEaseInOut = EaseImplement.QuartEaseInOut;
            return _quartEaseInOut;
        }
    }

    #endregion

    #region Quint Ease

    private static EaseDelegate _quintEaseIn;
    static EaseDelegate QuintEaseIn
    {
        get
        {
            if (_quintEaseIn == null) _quintEaseIn = EaseImplement.QuintEaseIn;
            return _quintEaseIn;
        }
    }

    private static EaseDelegate _quintEaseOut;
    static EaseDelegate QuintEaseOut
    {
        get
        {
            if (_quintEaseOut == null) _quintEaseOut = EaseImplement.QuintEaseOut;
            return _quintEaseOut;
        }
    }

    private static EaseDelegate _quintEaseInOut;
    static EaseDelegate QuintEaseInOut
    {
        get
        {
            if (_quintEaseInOut == null) _quintEaseInOut = EaseImplement.QuintEaseInOut;
            return _quintEaseInOut;
        }
    }

    #endregion

    #region Sine Ease

    private static EaseDelegate _sineEaseIn;
    static EaseDelegate SineEaseIn
    {
        get
        {
            if (_sineEaseIn == null) _sineEaseIn = EaseImplement.SineEaseIn;
            return _sineEaseIn;
        }
    }

    private static EaseDelegate _sineEaseOut;
    static EaseDelegate SineEaseOut
    {
        get
        {
            if (_sineEaseOut == null) _sineEaseOut = EaseImplement.SineEaseOut;
            return _sineEaseOut;
        }
    }

    private static EaseDelegate _sineEaseInOut;
    static EaseDelegate SineEaseInOut
    {
        get
        {
            if (_sineEaseInOut == null) _sineEaseInOut = EaseImplement.SineEaseInOut;
            return _sineEaseInOut;
        }
    }

    #endregion

    #region Strong Ease

    private static EaseDelegate _strongEaseIn;
    static EaseDelegate StrongEaseIn
    {
        get
        {
            if (_strongEaseIn == null) _strongEaseIn = EaseImplement.StrongEaseIn;
            return _strongEaseIn;
        }
    }

    private static EaseDelegate _strongEaseOut;
    static EaseDelegate StrongEaseOut
    {
        get
        {
            if (_strongEaseOut == null) _strongEaseOut = EaseImplement.StrongEaseOut;
            return _strongEaseOut;
        }
    }

    private static EaseDelegate _strongEaseInOut;
    static EaseDelegate StrongEaseInOut
    {
        get
        {
            if (_strongEaseInOut == null) _strongEaseInOut = EaseImplement.StrongEaseInOut;
            return _strongEaseInOut;
        }
    }

    #endregion

    #region AnimationCurve

    /// <summary>
    /// Returns an Ease method that ignores start and end. Instead just returning the value in the curve for 'c', as if you called Evaluate(c).
    /// </summary>
    /// <param name="curve"></param>
    /// <returns></returns>
    static EaseDelegate FromAnimationCurve(AnimationCurve curve)
    {
        return (c, s, e, d) =>
        {
            return curve.Evaluate(c);
        };
    }

    /// <summary>
    /// This treats the curve as if it's a scaling factor. The vertical from 0->1 is the value s->e. And the horizontal is just 'c'. 'd' is ignored in favor of the duration of the curve.
    /// </summary>
    /// <param name="curve"></param>
    /// <returns></returns>
    static EaseDelegate FromVerticalScalingAnimationCurve(AnimationCurve curve)
    {
        return (c, s, e, d) =>
        {
            if (d <= 0f) return e;
            return Mathf.LerpUnclamped(s, e, curve.Evaluate(c / d));
        };
    }

    /// <summary>
    /// This treats the curve as if it's a scaling factor. The vertical from 0->1 is the value s->e. And the horizontal from 0->1 is the time from c->d.
    /// </summary>
    /// <param name="curve"></param>
    /// <returns></returns>
    static EaseDelegate FromScalingAnimationCurve(AnimationCurve curve)
    {
        return (c, s, e, d) =>
        {
            if (d <= 0f) return e;
            return Mathf.LerpUnclamped(s, e, curve.Evaluate(c / d));
        };
    }

    #endregion

    #region Configurable Cubic Bezier

    static EaseDelegate CubicBezier(float p0, float p1, float p2, float p3)
    {
        return (c, s, e, d) =>
        {
            var t = c / d;
            var it = 1f - t;
            var r = (Mathf.Pow(it, 3f) * p0)
                  + (3f * Mathf.Pow(it, 2f) * t * p1)
                  + (3f * it * Mathf.Pow(t, 2f) * p2)
                  + (Mathf.Pow(t, 3f) * p3);
            return s + e * r;
        };
    }

    #endregion
}



public static class EaseImplement
{
    private const float _2PI = 6.28318530717959f;
    private const float _HALF_PI = 1.5707963267949f;

    #region Back Ease
    public static float BackEaseIn(float t, float b, float c, float d)
    {
        return BackEaseInFull(t, b, c, d);
    }

    public static float BackEaseOut(float t, float b, float c, float d)
    {
        return BackEaseOutFull(t, b, c, d);
    }
    public static float BackEaseInOut(float t, float b, float c, float d)
    {
        return BackEaseInOutFull(t, b, c, d);
    }

    public static float BackEaseInFull(float t, float b, float c, float d, float s = 1.70158f)
    {
        return c * (t /= d) * t * ((s + 1) * t - s) + b;
    }

    public static float BackEaseOutFull(float t, float b, float c, float d, float s = 1.70158f)
    {
        return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
    }
    public static float BackEaseInOutFull(float t, float b, float c, float d, float s = 1.70158f)
    {
        if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
        return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
    }
    #endregion

    #region Bounce Ease
    public static float BounceEaseOut(float t, float b, float c, float d)
    {
        if ((t /= d) < (1 / 2.75f))
        {
            return c * (7.5625f * t * t) + b;
        }
        else if (t < (2 / 2.75))
        {
            return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
        }
        else if (t < (2.5f / 2.75f))
        {
            return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
        }
        else
        {
            return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
        }
    }
    public static float BounceEaseIn(float t, float b, float c, float d)
    {
        return c - BounceEaseOut(d - t, 0, c, d) + b;
    }
    public static float BounceEaseInOut(float t, float b, float c, float d)
    {
        if (t < d / 2) return BounceEaseIn(t * 2, 0, c, d) * .5f + b;
        else return BounceEaseOut(t * 2 - d, 0, c, d) * .5f + c * .5f + b;
    }
    #endregion

    #region Circle Ease
    public static float CircleEaseIn(float t, float b, float c, float d)
    {
        return -c * ((float)Math.Sqrt(1 - (t /= d) * t) - 1) + b;
    }
    public static float CircleEaseOut(float t, float b, float c, float d)
    {
        return c * (float)Math.Sqrt(1 - (t = t / d - 1) * t) + b;
    }
    public static float CircleEaseInOut(float t, float b, float c, float d)
    {
        if ((t /= d / 2) < 1) return -c / 2 * ((float)Math.Sqrt(1 - t * t) - 1) + b;
        return c / 2 * ((float)Math.Sqrt(1 - (t -= 2) * t) + 1) + b;
    }
    #endregion

    #region Cubic Ease

    public static float CubicEaseIn(float t, float b, float c, float d)
    {
        return c * (t /= d) * t * t + b;
    }
    public static float CubicEaseOut(float t, float b, float c, float d)
    {
        return c * ((t = t / d - 1) * t * t + 1) + b;
    }
    public static float CubicEaseInOut(float t, float b, float c, float d)
    {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t + 2) + b;
    }

    #endregion

    #region Elastic Ease
    public static float ElasticEaseIn(float t, float b, float c, float d)
    {
        return ElasticEaseInFull(t, b, c, d, 0, 0);
    }

    public static float ElasticEaseOut(float t, float b, float c, float d)
    {
        return ElasticEaseOutFull(t, b, c, d, 0, 0);
    }

    public static float ElasticEaseInOut(float t, float b, float c, float d)
    {
        return ElasticEaseInOutFull(t, b, c, d, 0, 0);
    }

    public static float ElasticEaseInFull(float t, float b, float c, float d, float a, float p)
    {
        float s;
        if (t == 0f) return b; if ((t /= d) == 1) return b + c;
        if (p == 0f) p = d * 0.3f;
        if (a == 0f || a < Math.Abs(c)) { a = c; s = p / 4; }
        else s = p / _2PI * (float)Math.Asin(c / a);
        return -(a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t * d - s) * _2PI / p)) + b;
    }
    public static float ElasticEaseOutFull(float t, float b, float c, float d, float a = 0, float p = 0)
    {
        float s;
        if (t == 0f) return b;
        if ((t /= d) == 1) return b + c;
        if (p == 0f) p = d * 0.3f;
        if (a == 0f || a < Math.Abs(c)) { a = c; s = p / 4; }
        else s = p / _2PI * (float)Math.Asin(c / a);
        return (a * (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t * d - s) * _2PI / p) + c + b);
    }
    public static float ElasticEaseInOutFull(float t, float b, float c, float d, float a = 0, float p = 0)
    {
        float s;
        if (t == 0f) return b; if ((t /= d / 2) == 2) return b + c;
        if (p == 0f) p = d * (0.3f * 1.5f);
        if (a == 0f || a < Math.Abs(c)) { a = c; s = p / 4; }
        else s = p / _2PI * (float)Math.Asin(c / a);
        if (t < 1) return -.5f * (a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t * d - s) * _2PI / p)) + b;
        return a * (float)Math.Pow(2, -10 * (t -= 1)) * (float)Math.Sin((t * d - s) * _2PI / p) * .5f + c + b;
    }

    #endregion

    #region Expo Ease
    public static float ExpoEaseIn(float t, float b, float c, float d)
    {
        return (t == 0) ? b : c * (float)Math.Pow(2, 10 * (t / d - 1)) + b - c * 0.001f;
    }
    public static float ExpoEaseOut(float t, float b, float c, float d)
    {
        return (t == d) ? b + c : c * (-(float)Math.Pow(2, -10 * t / d) + 1) + b;
    }
    public static float ExpoEaseInOut(float t, float b, float c, float d)
    {
        if (t == 0) return b;
        if (t == d) return b + c;
        if ((t /= d / 2) < 1) return c / 2 * (float)Math.Pow(2, 10 * (t - 1)) + b;
        return c / 2 * (-(float)Math.Pow(2, -10 * --t) + 2) + b;
    }
    #endregion

    #region Linear Ease
    public static float LinearEaseNone(float t, float b, float c, float d)
    {
        return c * t / d + b;
    }
    public static float LinearEaseIn(float t, float b, float c, float d)
    {
        return c * t / d + b;
    }
    public static float LinearEaseOut(float t, float b, float c, float d)
    {
        return c * t / d + b;
    }
    public static float LinearEaseInOut(float t, float b, float c, float d)
    {
        return c * t / d + b;
    }
    #endregion

    #region Quad Ease
    public static float QuadEaseIn(float t, float b, float c, float d)
    {
        return c * (t /= d) * t + b;
    }
    public static float QuadEaseOut(float t, float b, float c, float d)
    {
        return -c * (t /= d) * (t - 2) + b;
    }
    public static float QuadEaseInOut(float t, float b, float c, float d)
    {
        if ((t /= d / 2) < 1) return c / 2 * t * t + b;
        return -c / 2 * ((--t) * (t - 2) - 1) + b;
    }
    #endregion

    #region Quart Ease
    public static float QuartEaseIn(float t, float b, float c, float d)
    {
        return c * (t /= d) * t * t * t + b;
    }
    public static float QuartEaseOut(float t, float b, float c, float d)
    {
        return -c * ((t = t / d - 1) * t * t * t - 1) + b;
    }
    public static float QuartEaseInOut(float t, float b, float c, float d)
    {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
        return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
    }
    #endregion

    #region Quint Ease
    public static float QuintEaseIn(float t, float b, float c, float d)
    {
        return c * (t /= d) * t * t * t * t + b;
    }
    public static float QuintEaseOut(float t, float b, float c, float d)
    {
        return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
    }
    public static float QuintEaseInOut(float t, float b, float c, float d)
    {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
    }
    #endregion

    #region Sine Ease
    public static float SineEaseIn(float t, float b, float c, float d)
    {
        return -c * (float)Math.Cos(t / d * _HALF_PI) + c + b;
    }
    public static float SineEaseOut(float t, float b, float c, float d)
    {
        return c * (float)Math.Sin(t / d * _HALF_PI) + b;
    }
    public static float SineEaseInOut(float t, float b, float c, float d)
    {
        return -c / 2 * ((float)Math.Cos(Math.PI * t / d) - 1) + b;
    }
    #endregion

    #region Strong Ease
    public static float StrongEaseIn(float t, float b, float c, float d)
    {
        return c * (t /= d) * t * t * t * t + b;
    }
    public static float StrongEaseOut(float t, float b, float c, float d)
    {
        return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
    }
    public static float StrongEaseInOut(float t, float b, float c, float d)
    {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
    }
    #endregion
}

public enum EaseStyle : int
{
    Linear = 0,
    LinearEaseIn = 1,
    LinearEaseOut = 2,
    LinearEaseInOut = 3,
    QuadEaseIn = 4,
    QuadEaseInOut = 5,
    QuadEaseOut = 6,
    BounceEaseIn = 7,
    BounceEaseOut = 8,
    BounceEaseInOut = 9,
    CircleEaseIn = 10,
    CircleEaseOut = 11,
    CircleEaseInOut = 12,
    CubicEaseIn = 13,
    CubicEaseOut = 14,
    CubicEaseInOut = 15,
    ElasticEaseIn = 16,
    ElasticEaseOut = 17,
    ElasticEaseInOut = 18,
    ExpoEaseIn = 19,
    ExpoEaseOut = 20,
    ExpoEaseInOut = 21,
    BackEaseIn = 22,
    BackEaseOut = 23,
    BackEaseInOut = 24,
    QuartEaseIn = 25,
    QuartEaseOut = 26,
    QuartEaseInOut = 27,
    QuintEaseIn = 28,
    QuintEaseOut = 29,
    QuintEaseInOut = 30,
    SineEaseIn = 31,
    SineEaseOut = 32,
    SineEaseInOut = 33,
    StrongEaseIn = 34,
    StrongEaseOut = 35,
    StrongEaseInOut = 36,
}
