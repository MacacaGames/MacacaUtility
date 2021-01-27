//Source from https://archive.codeplex.com/?p=fastreflectionlib
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;

namespace MacacaGames
{
    public static class ReflectorCaches
    {
        static ReflectorCaches()
        {
            MethodInvokerCache = new MethodInvokerCache();
            PropertyAccessorCache = new PropertyAccessorCache();
            FieldAccessorCache = new FieldAccessorCache();
            // ConstructorInvokerCache = new ConstructorInvokerCache();
        }

        public static IReflactorCache<MethodInfo, IMethodInvoker> MethodInvokerCache { get; set; }

        public static IReflactorCache<PropertyInfo, IAccessor> PropertyAccessorCache { get; set; }

        public static IReflactorCache<FieldInfo, IAccessor> FieldAccessorCache { get; set; }

        //public static IReflactorCache<ConstructorInfo, IConstructorInvoker> ConstructorInvokerCache { get; set; }
    }
    public abstract class ReflectorCache<TKey, TValue> : IReflactorCache<TKey, TValue>
    {
        private Dictionary<TKey, TValue> m_cache = new Dictionary<TKey, TValue>();
        private ReaderWriterLockSlim m_rwLock = new ReaderWriterLockSlim();

        public TValue Get(TKey key)
        {
            TValue value = default(TValue);

            this.m_rwLock.EnterReadLock();
            bool cacheHit = this.m_cache.TryGetValue(key, out value);
            this.m_rwLock.ExitReadLock();

            if (cacheHit) return value;

            this.m_rwLock.EnterWriteLock();
            if (!this.m_cache.TryGetValue(key, out value))
            {
                try
                {
                    value = this.Create(key);
                    this.m_cache[key] = value;
                }
                finally
                {
                    this.m_rwLock.ExitWriteLock();
                }
            }

            return value;
        }

        protected abstract TValue Create(TKey key);
    }
}
