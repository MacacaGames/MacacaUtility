using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text;

namespace MacacaGames
{
    public static class RandomUtility
    {
        static System.Random r = new System.Random();

        /// <summary>
        /// Shuffle a array
        /// </summary>
        /// <param name="source">The source array</param>
        /// <typeparam name="T">Type of the array</typeparam>
        public static IList<T> Shuffle<T>(this IList<T> source)
        {
            for (int n = source.Count - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                T temp = source[n];
                source[n] = source[k];
                source[k] = temp;
            }
            return source;
        }

        /// <summary>
        /// Shuffle a array with a seed, you will always get the same result with same seed
        /// </summary>
        /// <param name="source">The source array</param>
        /// <param name="seed">The seed to shuffle, Seed length must more then source length</param>
        /// <typeparam name="T">Type of the array</typeparam>
        public static IList<T> ShuffleWithSeed<T>(this IList<T> source, string seed)
        {
            if (seed.Length < source.Count)
            {
                throw new System.ArgumentException("Seed length must more then source length");
            }
            for (int i = 0; i < source.Count; i++)
            {
                //把目前的直暫存
                T temp = source[i];

                //取得相同位置的 key 直
                var charInKey = (int)seed[i] % source.Count;

                //把兩個物件交換位置
                source[i] = source[charInKey];
                source[charInKey] = temp;
            }
            return source;
        }


        /// <summary>
        /// Get the index of random item by weight;
        /// </summary>
        /// <param name="itemWeightList"></param>
        /// <returns></returns>
        public static int RandomIndexByWeight(IList<int> itemWeightList)
        {
            int totalWeight = 0;

            itemWeightList.All(i => { totalWeight += i; return true; });

            int randomSelectPointer = UnityEngine.Random.Range(1, totalWeight + 1);

            for (int i = 0; i < itemWeightList.Count; i++)
            {
                randomSelectPointer -= itemWeightList[i];
                if (randomSelectPointer <= 0) return i;
            }

            return -1;
        }

        /// <summary>
        /// Get the index of random item by weight;
        /// </summary>
        /// <param name="itemWeightList"></param>
        /// <returns></returns>
        public static IList<T> RandomItemByWeight<T>(IList<(T item, int weight)> itemWeightList, int pickAmount = 1)
        {
            List<T> result = new List<T>();

            for (int pickTimes = 0; pickTimes < pickAmount; pickTimes++)
            {

                int totalWeight = 0;

                itemWeightList.All(_ => { totalWeight += _.weight; return true; });

                int randomSelectPointer = UnityEngine.Random.Range(1, totalWeight + 1);

                for (int i = 0; i < itemWeightList.Count; i++)
                {
                    randomSelectPointer -= itemWeightList[i].weight;
                    if (randomSelectPointer <= 0)
                    {
                        result.Add(itemWeightList[i].item);
                        itemWeightList[i] = (itemWeightList[i].item, 0);    //抽到的把權重歸零，避免再抽到
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get a random string
        /// </summary>
        /// <param name="length">The length of the string</param>
        /// <returns>A random string</returns>
        public static string GetRandomString(int length)
        {
            string result = Guid.NewGuid().ToString().Replace("-", "");
            while (result.Length < length)
            {
                result += Guid.NewGuid().ToString().Replace("-", "");
            }
            return result.Substring(0, length);
        }

        /// <summary>
        /// Get a random string makes by the source string
        /// </summary>
        /// <param name="source">The char source</param>
        /// <param name="length">The result of the result</param>
        /// <returns>The result random string</returns>
        public static string Random(string source, int length = 8)
        {
            var randomString = new StringBuilder();
            var random = new System.Random();

            for (int i = 0; i < length; i++)
                randomString.Append(source[random.Next(source.Length)]);

            return randomString.ToString();
        }
    }
}