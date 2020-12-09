using System.Collections.Generic;

namespace MacacaGames
{
    public static class ArrayUtilitys
    {
        static System.Random r = new System.Random();
        public static void Shuffle<T>(IList<T> oriArray)
        {
            for (int n = oriArray.Count - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                T temp = oriArray[n];
                oriArray[n] = oriArray[k];
                oriArray[k] = temp;
            }
        }

        public static void ShuffleByString<T>(IList<T> oriArray, string key)
        {
            if(key.Length < oriArray.Count){
                UnityEngine.Debug.LogError("Key's length must more then oriArray's length");
                return;
            }
            for (int i = 0; i < oriArray.Count; i++)
            {
                //把目前的直暫存
                T temp = oriArray[i];

                //取得相同位置的 key 直
                var charInKey = (int)key[i] % oriArray.Count;

                //把兩個物件交換位置
                oriArray[i] = oriArray[charInKey];
                oriArray[charInKey] = temp;
            }
        }
    }
}
