﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SmartBigNumberString
{
    public static string GetSmartBigNumberString(float value)
    {
        return GetSmartBigNumberString((double)value);
    }

    public static string GetSmartBigNumberString(double value)
    {
        if (value == 0) return "0";
        double pureLog = Math.Log(value, 1000);
        int i = (int)Math.Floor(pureLog);
        if (i < 0) return Math.Round(value).ToString();
        double number = (value/Math.Pow(1000,i));
        int numberCount = (number * 0.01d > 1) ? 3 : 4;
        string numberText = number.ToString();
        string text = numberText.Substring(0, Math.Min(numberCount, numberText.Length));
        string unit = unitArray[i];
        return text + unit;
    }

    static string[] unitArray = new string[]
    {
        "",
        "K",
        "M",
        "B"
    };
}
