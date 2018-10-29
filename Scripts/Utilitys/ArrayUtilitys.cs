using System.Collections.Generic;

namespace CloudMacaca
{
    public static class ArrayUtilitys
    {
        static System.Random r = new System.Random();
        public static void Shuffle<T>(IList<T> deck)
        {
            for (int n = deck.Count - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                T temp = deck[n];
                deck[n] = deck[k];
                deck[k] = temp;
            }
        }
    }
}
