using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MacacaGames;

public class TimeStampHelper
{

    static int _first_time_stamp = 0;
    static int _first_unity_time = 0;
    public static int RoughTimeStamp
    {
        get
        {
            if (!isEverTimeStamped)
                RefreshTimeStampReference();

            return _first_time_stamp + Mathf.FloorToInt(Time.time) - _first_unity_time;
        }
    }

    static bool isEverTimeStamped { get { return _first_time_stamp != 0; } }

    public static void RefreshTimeStampReference()
    {
        _first_time_stamp = Utility.GetTimeStamp();
        _first_unity_time = Mathf.FloorToInt(Time.time);
    }

    public static int GetRoughTimeStamp(TimeStampUnit timeStampUnit)
    {
        int roughTimeStamp = RoughTimeStamp;

        if (timeStampUnit == TimeStampUnit.Second)
            return roughTimeStamp;

        //分
        roughTimeStamp = Mathf.FloorToInt(roughTimeStamp / 60f);

        if (timeStampUnit == TimeStampUnit.Minute)
            return roughTimeStamp;

        //時
        roughTimeStamp = Mathf.FloorToInt(roughTimeStamp / 60f);

        if (timeStampUnit == TimeStampUnit.Hour)
            return roughTimeStamp;

        //日
        roughTimeStamp = Mathf.FloorToInt(roughTimeStamp / 24f);

        if (timeStampUnit == TimeStampUnit.Day)
            return roughTimeStamp;

        throw new System.NullReferenceException("Invalid TimeStampUnit");
    }

}

public enum TimeStampUnit { Second, Minute, Hour, Day }
