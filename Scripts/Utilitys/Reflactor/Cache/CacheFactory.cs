//Source from https://archive.codeplex.com/?p=fastreflectionlib
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MacacaGames
{
    public static class CacheFactories
    {
        static CacheFactories()
        {
            MethodInvokerFactory = new MethodInvokerFactory();
            PropertyAccessorFactory = new PropertyAccessorFactory();
            FieldAccessorFactory = new FieldAccessorFactory();
            // ConstructorInvokerFactory = new ConstructorInvokerFactory();
        }

        public static ICacheFactory<MethodInfo, IMethodInvoker> MethodInvokerFactory { get; set; }

        public static ICacheFactory<PropertyInfo, IAccessor> PropertyAccessorFactory { get; set; }

        public static ICacheFactory<FieldInfo, IAccessor> FieldAccessorFactory { get; set; }

        // public static IFastReflectionFactory<ConstructorInfo, IConstructorInvoker> ConstructorInvokerFactory { get; set; }
    }
    public interface ICacheFactory<TKey, TValue>
    {
        TValue Create(TKey key);
    }
    public class PropertyAccessorFactory : ICacheFactory<PropertyInfo, IAccessor>
    {
        public IAccessor Create(PropertyInfo key)
        {
            return new PropertyAccessor(key);
        }

        #region IFastReflectionFactory<PropertyInfo,IPropertyAccessor> Members

        IAccessor ICacheFactory<PropertyInfo, IAccessor>.Create(PropertyInfo key)
        {
            return this.Create(key);
        }

        #endregion
    }
    public class FieldAccessorFactory : ICacheFactory<FieldInfo, IAccessor>
    {
        public IAccessor Create(FieldInfo key)
        {
            return new FieldAccessor(key);
        }

        #region IFastReflectionFactory<FieldInfo,IFieldAccessor> Members

        IAccessor ICacheFactory<FieldInfo, IAccessor>.Create(FieldInfo key)
        {
            return this.Create(key);
        }

        #endregion
    }
    public class MethodInvokerFactory : ICacheFactory<MethodInfo, IMethodInvoker>
    {
        public IMethodInvoker Create(MethodInfo key)
        {
            return new MethodInvoker(key);
        }

        #region IFastReflectionFactory<MethodInfo,IMethodInvoker> Members

        IMethodInvoker ICacheFactory<MethodInfo, IMethodInvoker>.Create(MethodInfo key)
        {
            return this.Create(key);
        }

        #endregion
    }
}
