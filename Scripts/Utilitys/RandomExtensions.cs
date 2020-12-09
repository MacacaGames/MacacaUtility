using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MacacaGames
{
    public static class CMRandomExtensions
    {
        /// <summary>
        /// Get the index of random item by weight;
        /// </summary>
        /// <param name="itemWeightList"></param>
        /// <returns></returns>
        public static int RandomIndexByWeight(List<int> itemWeightList)
        {
            int totalWeight = 0;

            itemWeightList.All(i => { totalWeight += i; return true; });

            int randomSelectPointer = Random.Range(1, totalWeight + 1);

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
        public static List<T> RandomItemByWeight<T>(List<(T item, int weight)> itemWeightList, int pickAmount = 1)
        {
            List<T> result = new List<T>();

            for (int pickTimes = 0; pickTimes < pickAmount; pickTimes++)
            {

                int totalWeight = 0;

                itemWeightList.All(_ => { totalWeight += _.weight; return true; });

                int randomSelectPointer = Random.Range(1, totalWeight + 1);

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
    }
}