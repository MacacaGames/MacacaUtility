//Source from https://archive.codeplex.com/?p=fastreflectionlib

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace MacacaGames
{
    public interface IAccessor
    {
        object GetValue(object instance);
        void SetValue(object instance, object value);
    }

    public class PropertyAccessor : IAccessor
    {
        private Func<object, object> m_getter;
        private MethodInvoker m_setMethodInvoker;

        public PropertyInfo PropertyInfo { get; private set; }

        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            this.PropertyInfo = propertyInfo;
            this.InitializeGet(propertyInfo);
            this.InitializeSet(propertyInfo);
        }

        private void InitializeGet(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanRead) return;

            // Target: (object)(((TInstance)instance).Property)

            // preparing parameter, object type
            var instance = Expression.Parameter(typeof(object), "instance");

            // non-instance for static method, or ((TInstance)instance)
            var instanceCast = propertyInfo.GetGetMethod(true).IsStatic ? null :
                Expression.Convert(instance, propertyInfo.ReflectedType);

            // ((TInstance)instance).Property
            var propertyAccess = Expression.Property(instanceCast, propertyInfo);

            // (object)(((TInstance)instance).Property)
            var castPropertyValue = Expression.Convert(propertyAccess, typeof(object));

            // Lambda expression
            var lambda = Expression.Lambda<Func<object, object>>(castPropertyValue, instance);

            this.m_getter = lambda.Compile();
        }

        private void InitializeSet(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanWrite) return;
            this.m_setMethodInvoker = new MethodInvoker(propertyInfo.GetSetMethod(true));
        }

        public object GetValue(object o)
        {
            if (this.m_getter == null)
            {
                throw new NotSupportedException("Get method is not defined for this property.");
            }

            return this.m_getter(o);
        }

        public void SetValue(object o, object value)
        {
            if (this.m_setMethodInvoker == null)
            {
                throw new NotSupportedException("Set method is not defined for this property.");
            }

            this.m_setMethodInvoker.Invoke(o, new object[] { value });
        }

        #region IPropertyAccessor Members

        object IAccessor.GetValue(object instance)
        {
            return this.GetValue(instance);
        }

        void IAccessor.SetValue(object instance, object value)
        {
            this.SetValue(instance, value);
        }

        #endregion
    }

    public class FieldAccessor : IAccessor
    {
        private Func<object, object> m_getter;

        public FieldInfo fieldInfo { get; private set; }

        public FieldAccessor(FieldInfo fieldInfo)
        {
            this.fieldInfo = fieldInfo;
            this.m_getter = this.GetDelegate(fieldInfo);
        }

        private Func<object, object> GetDelegate(FieldInfo fieldInfo)
        {
            // target: (object)(((TInstance)instance).Field)

            // preparing parameter, object type
            var instance = Expression.Parameter(typeof(object), "instance");

            // non-instance for static method, or ((TInstance)instance)
            var instanceCast = fieldInfo.IsStatic ? null :
                Expression.Convert(instance, fieldInfo.ReflectedType);

            // ((TInstance)instance).Property
            var fieldAccess = Expression.Field(instanceCast, fieldInfo);

            // (object)(((TInstance)instance).Property)
            var castFieldValue = Expression.Convert(fieldAccess, typeof(object));

            // Lambda expression
            var lambda = Expression.Lambda<Func<object, object>>(castFieldValue, instance);

            return lambda.Compile();
        }

        public object GetValue(object instance)
        {
            return this.m_getter(instance);
        }
        public void SetValue(object instance, object value)
        {
            fieldInfo.SetValue(instance, value);
        }

        #region IFieldAccessor Members

        object IAccessor.GetValue(object instance)
        {
            return this.GetValue(instance);
        }

        void IAccessor.SetValue(object instance, object value)
        {
            this.SetValue(instance, value);
        }
        #endregion
    }
}
