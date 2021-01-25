
using System;
using System.Collections;
using System.Threading;

using UnityEngine;

public class Benchmarker : UnitySingleton<Benchmarker>
{

    static GameObject instanceGameObject;
    public enum BenchMarkResult
    {
        High, Mid, Low, Faild
    }
    public void StarBenchmark(int sampleTime, Action<float> OnComplete)
    {
        StartCoroutine(BenchMark(sampleTime, OnComplete));
    }

    public IEnumerator BenchMark(int sampleTime, Action<float> OnComplete)
    {
        int frameCount = 0;

        float StartTime = Time.time;
        Debug.LogError("BenchMark Start");

        while (Time.time - StartTime < sampleTime)
        {
            ++frameCount;
            yield return null;
        }
        float avgFPS = (float)frameCount / (float)sampleTime;
        Debug.LogError("BenchMark Done");
        Debug.LogError("平均FPS :" + avgFPS);
        OnComplete?.Invoke(avgFPS);
    }

}
