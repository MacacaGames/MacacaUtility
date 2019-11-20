
using System;
using System.Collections;
using System.Threading;
#if UniRx
using UniRx;
#endif
using UnityEngine;

public class Benchmarker : UnitySingleton<Benchmarker>
{

    static GameObject instanceGameObject;
    public enum BenchMarkResult
    {
        High, Mid, Low, Faild
    }
#if UniRx
    public IObservable<float> StarBenchmark(int sampleTime = 10)
    {
        return Observable.FromCoroutine<float>((observer, cancellationToken) => BenchMark(sampleTime, observer, cancellationToken));
    }
#endif
    public IEnumerator BenchMark(int sampleTime, IObserver<float> observer, CancellationToken cancellationToken)
    {
        int frameCount = 0;

        float StartTime = Time.time;
        Debug.LogError("BenchMark Start");

        while (!cancellationToken.IsCancellationRequested && Time.time - StartTime < sampleTime)
        {
            ++frameCount;
            yield return null;
        }
        float avgFPS = frameCount / (float)sampleTime;
        Debug.LogError("BenchMark Done");
        Debug.LogError("平均FPS :" + avgFPS);
        observer.OnNext(avgFPS);
        observer.OnCompleted();
    }

}
