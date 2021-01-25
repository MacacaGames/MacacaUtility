namespace MacacaGames
{
    public class Singleton<T> where T : class, new()
    {
        static T instance;

        static object lockObj = new object();

        public static T Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                    return instance;
                }
            }
        }
    }
}
