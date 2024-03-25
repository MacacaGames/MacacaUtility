using UnityEngine;

namespace MacacaGames
{
    public class ClipBoard
    {
        public ClipBoard(object source)
        {
            json = JsonUtility.ToJson(source);
        }
        public object value;
        public string json;
        public bool HasValue
        {
            get
            {
                return value != null || !string.IsNullOrEmpty(json);
            }
        }

        public T Paste<T>()
        {
            if (!HasValue)
            {
                Debug.LogError("The copyboard doesn't value yet!");
                return default(T);
            }
            return JsonUtility.FromJson<T>(json);
        }

    }
}
