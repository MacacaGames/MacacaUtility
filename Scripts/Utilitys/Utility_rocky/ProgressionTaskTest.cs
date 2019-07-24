using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
using DG.Tweening;
//

public class ProgressionTaskTest : MonoBehaviour
{
    IEnumerator Start()
    {
        float duration = 2f;

        yield return Task1(duration);

        yield return Task2(duration);

        yield return Task3(duration);

        #region Version 1
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            UpdateAction(progress);
            yield return null;
        }
        #endregion

        #region Version2
        yield return
        DOTween.To(() => 0, p =>
        {
            UpdateAction(p);
        },
        1, duration).WaitForCompletion();
        #endregion

        #region Version3
        yield return CoroutineManager.ProgressionTask(duration, UpdateAction);
        #endregion
    }

    void UpdateAction(float progress)
    {
        Debug.Log(progress);
    }

    IEnumerator Task1(float duration)
    {
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            
            //Do What You Want
            UpdateAction(progress);
            //

            yield return null;
        }
    }

    IEnumerator Task2(float duration)
    {
        yield return
            DOTween.To(() => 0, progress =>
            {

                //Do What You Want
                UpdateAction(progress);
                //

            },
            1, duration).WaitForCompletion();
    }

    IEnumerator Task3(float duration)
    {
        yield return CoroutineManager.ProgressionTask(duration,
            progress =>
            {

                //Do What You Want
                UpdateAction(progress);
                //

            });
    }

    IEnumerator Task4(float duration)
    {
        yield return CoroutineManager.ProgressionTask(duration, UpdateAction);
    }
}
