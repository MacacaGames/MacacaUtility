#if UNITY_2019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public static class UIElementExtensions
{
    public static T AddClass<T>(this T el, string className) where T : VisualElement
    {
        el.AddToClassList(className);
        return el;
    }

    public static T Add<T>(this T el, Func<IEnumerable<VisualElement>> childs) where T : VisualElement
    {
        foreach (var child in childs())
        {
            el.Add(child);
        }
        return el;
    }

    public static T Add<T>(this T el, params VisualElement[] childs) where T : VisualElement
    {
        foreach (var child in childs)
        {
            el.Add(child);
        }
        return el;
    }

    public static T Action<T>(this T el, Action<T> action) where T : VisualElement
    {
        action?.Invoke(el);
        return el;
    }

}
#endif