using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MacacaGames
{
    public class MathUtility : MonoBehaviour
    {
        /// <summary>
        /// 將數字格式化為帶有單位後綴的字串（k/m/b）
        /// </summary>
        /// <param name="num">要格式化的數字</param>
        /// <param name="threshold">幾（含）以上開始使用 kbm 規則，預設為 1000</param>
        /// <param name="useUpperCase">後綴是否使用大寫，預設為 false（小寫）</param>
        /// <param name="showDecimal">是否顯示小數點，預設為 true</param>
        /// <returns>格式化後的字串</returns>
        public static string NumberFormat(long num, long threshold = 1000, bool useUpperCase = false, bool showDecimal = true)
        {
            if (num < threshold)
            {
                return num.ToString();
            }

            char endUnit = useUpperCase ? 'K' : 'k';
            double divisor = 1000.0;
            if (num >= 1000000000)
            {
                endUnit = useUpperCase ? 'B' : 'b';
                divisor = 1000000000.0;
            }
            else if (num >= 1000000)
            {
                endUnit = useUpperCase ? 'M' : 'm';
                divisor = 1000000.0;
            }

            double transformedNum = System.Math.Floor(num / divisor * 100) / 100;
            string formattedString;
            if (showDecimal)
            {
                formattedString = String.Format("{0:N2}", transformedNum);
                formattedString = formattedString.TrimEnd('0').Replace(",", ".").TrimEnd('.');
            }
            else
            {
                formattedString = ((long)transformedNum).ToString();
            }
            return formattedString + endUnit;
        }
        public static Vector3 Remap(float input, float fromMin, float fromMax, Vector3 toMin, Vector3 toMax)
        {
            var result = new Vector3(Remap(input, fromMin, fromMax, toMin.x, toMax.x),
                                    Remap(input, fromMin, fromMax, toMin.y, toMax.y),
                                    Remap(input, fromMin, fromMax, toMin.z, toMax.z));
            return result;
        }
        public static Vector3 Remap(Vector3 input, Vector3 fromMin, Vector3 fromMax, Vector3 toMin, Vector3 toMax)
        {
            var result = new Vector3(Remap(input.x, fromMin.x, fromMax.x, toMin.x, toMax.x),
                                    Remap(input.y, fromMin.y, fromMax.y, toMin.y, toMax.y),
                                    Remap(input.z, fromMin.z, fromMax.z, toMin.z, toMax.z));
            return result;
        }
        public static Vector2 Remap(float input, float fromMin, float fromMax, Vector2 toMin, Vector2 toMax)
        {
            var result = new Vector2(Remap(input, fromMin, fromMax, toMin.x, toMax.x),
                                    Remap(input, fromMin, fromMax, toMin.y, toMax.y));
            return result;
        }
        public static Vector2 Remap(Vector2 input, Vector2 fromMin, Vector2 fromMax, Vector2 toMin, Vector2 toMax)
        {
            var result = new Vector2(Remap(input.x, fromMin.x, fromMax.x, toMin.x, toMax.x),
                                    Remap(input.y, fromMin.y, fromMax.y, toMin.y, toMax.y));
            return result;
        }
        public static float Remap(float input, float fromMin, float fromMax, float toMin, float toMax)
        {
            return toMin + (input - fromMin) * (toMax - toMin) / (fromMax - fromMin);
        }
        
        public static float CurveLerp(float a, float b, AnimationCurve curve, float t)
        {
            float evaluatedT = Mathf.Clamp01(curve.Evaluate(Mathf.Clamp01(t)));
            return Mathf.Lerp(a, b, evaluatedT);
        }
        public static Vector2 CurveLerp(Vector2 a, Vector2 b, AnimationCurve curve, float t)
        {
            float evaluatedT = Mathf.Clamp01(curve.Evaluate(Mathf.Clamp01(t)));
            return Vector2.Lerp(a, b, evaluatedT);
        }
        public static Vector3 CurveLerp(Vector3 a, Vector3 b, AnimationCurve curve, float t)
        {
            float evaluatedT = Mathf.Clamp01(curve.Evaluate(Mathf.Clamp01(t)));
            return Vector3.Lerp(a, b, evaluatedT);
        }
        public static float CurveLerpUnclamped(float a, float b, AnimationCurve curve, float t)
        {
            float evaluatedT = curve.Evaluate(t);
            return Mathf.LerpUnclamped(a, b, evaluatedT);
        }
        public static Vector2 CurveLerpUnclamped(Vector2 a, Vector2 b, AnimationCurve curve, float t)
        {
            float evaluatedT = curve.Evaluate(t);
            return Vector2.LerpUnclamped(a, b, evaluatedT);
        }
        public static Vector3 CurveLerpUnclamped(Vector3 a, Vector3 b, AnimationCurve curve, float t)
        {
            float evaluatedT = curve.Evaluate(t);
            return Vector3.LerpUnclamped(a, b, evaluatedT);
        }
    }
}
