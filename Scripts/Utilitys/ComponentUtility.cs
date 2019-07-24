using System.Collections.Generic;
using UnityEngine;

public static class ComponentUtility
{

    /// <summary>
    /// 可嘗試取得Component，並回傳取得成功與否。out的value為取得的Component，若取得失敗則value為null。
    /// </summary>
    public static bool TryGetComponent<T>(this Component target, out T value)
    {
        value = target.GetComponent<T>();
        return value == null;
    }
    /// <summary>
    /// 可嘗試取得Component，並回傳取得成功與否。out的value為取得的Component，若取得失敗則value為null。
    /// </summary>
    public static bool TryGetComponent<T>(this GameObject target, out T value)
    {
        value = target.GetComponent<T>();
        return value == null;
    }

    /// <summary>
    /// 可嘗試取得掛載的所有Component，並回傳取得成功與否。out的values為取得的Component，若取得失敗則value為null。
    /// </summary>
    public static bool TryGetComponent<T>(this Component target, out IEnumerable<T> values)
    {
        values = target.GetComponents<T>();
        return values == null;
    }
    /// <summary>
    /// 可嘗試取得掛載的所有Components，並回傳取得成功與否。out的values為取得的Component，若取得失敗則value為null。
    /// </summary>
    public static bool TryGetComponent<T>(this GameObject target, out IEnumerable<T> values)
    {
        values = target.GetComponents<T>();
        return values == null;
    }

}
