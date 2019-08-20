using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CloudMacaca
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
    }
}