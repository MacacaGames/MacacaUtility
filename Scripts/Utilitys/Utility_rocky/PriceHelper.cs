using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PriceHelper
{
    /// <summary>
    /// Get the price string mulptiple by times, times should bigger than 1
    /// </summary>
    /// <param name="originalPrice"></param>
    /// <param name="times"></param>
    /// <returns></returns>
    static char[] numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    public static string GetTimesStringResultByPriceText(string originalPrice, float times)
    {
        int firstNumberIndex = originalPrice.IndexOfAny(numbers);
        int lastNumberIndex = originalPrice.LastIndexOfAny(numbers);

        string pre = firstNumberIndex > 0 ? originalPrice.Substring(0, firstNumberIndex) : string.Empty;
        string last = lastNumberIndex + 1 < originalPrice.Length ? originalPrice.Substring(lastNumberIndex + 1) : string.Empty;

        string midNumberString = originalPrice.Substring(firstNumberIndex, lastNumberIndex - firstNumberIndex + 1);

        var midCharArray = midNumberString.ToCharArray();

        //Reverse Array!!!
        Array.Reverse(midCharArray);

        List<int> numberList = new List<int>();
        List <(int index, char character)> split = new List<(int index, char character)>();
        for (int i = 0; i < midCharArray.Length; i++)
        {
            char c = midCharArray[i];
            if (numbers.Contains(c))
            {
                numberList.Add(c - '0');
            }
            else
                split.Add((i, c));
        }

        int number = 0;
        for (int n = 0; n < numberList.Count; n++)
        {
            number += Mathf.RoundToInt(numberList[n] * Mathf.Pow(10, n));
        }

        //解析完畢 動工

        if (number % 10 == 9)
            number += 1;

        number = Mathf.RoundToInt(number * times);

        //反解析開始
        char[] newNumberString = number.ToString().ToCharArray();
        Array.Reverse(newNumberString);

        string reversedNewNumberString = new string(newNumberString);

        for (int i = 0; i < split.Count; i++)
        {
            int fixedIndex = split[i].index + i;
            string insertChar = split[i].character.ToString();

            reversedNewNumberString = reversedNewNumberString.Insert(fixedIndex, insertChar);
        }

        var s = reversedNewNumberString.ToArray();

        Array.Reverse(s);

        string s2 = new string(s);

        return pre + s2 + last;
    }
}
