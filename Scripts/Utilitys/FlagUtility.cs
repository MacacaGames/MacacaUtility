using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlagsHelperExtensions
{
    public static bool IsSet<T>(this T flag, T flags) where T : System.Enum
    {
        int flagsValue = (int)(object)flags;
        int flagValue = (int)(object)flag;
        return (flagsValue & flagValue) != 0;
    }
    public static T Set<T>(T flags, T flag) where T : System.Enum
    {
        int flagsValue = (int)(object)flags;
        int flagValue = (int)(object)flag;

        flags = (T)(object)(flagsValue | flagValue);
        return flags;
    }

    public static T Unset<T>(T flags, T flag) where T : System.Enum
    {
        int flagsValue = (int)(object)flags;
        int flagValue = (int)(object)flag;

        flags = (T)(object)(flagsValue & (~flagValue));
        return flags;
    }
}

public static class FlagsHelper
{
    public static bool IsSet<T>(T flags, T flag) where T : System.Enum
    {
        int flagsValue = (int)(object)flags;
        int flagValue = (int)(object)flag;

        return (flagsValue & flagValue) != 0;
    }

    public static void Set<T>(ref T flags, T flag) where T : System.Enum
    {
        int flagsValue = (int)(object)flags;
        int flagValue = (int)(object)flag;

        flags = (T)(object)(flagsValue | flagValue);
    }

    public static void Unset<T>(ref T flags, T flag) where T : System.Enum
    {
        int flagsValue = (int)(object)flags;
        int flagValue = (int)(object)flag;

        flags = (T)(object)(flagsValue & (~flagValue));
    }
}

