using System.Collections.Generic;
using System.Reflection;
using System;

namespace MacacaGames
{
    public static class ReflectionUtility
    {
        const BindingFlags defaultBindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        /// <summary>取得MemberInfo的值。</summary>
        public static T GetValue<T>(this MemberInfo memberInfo, object forObject) where T : class
        {
            if (memberInfo.TryGetValue(forObject, out T result))
                return result;
            else
                throw new NotImplementedException();
        }

        public struct GetValueResult
        {
            public bool hasValue;
            public object value;
        }
        /// <summary>嘗試取得MemberInfo的值，並回傳取得成功與否。</summary>
        public static bool TryGetValue<T>(this MemberInfo memberInfo, object forObject, out T value) where T : class
        {
            var result = memberInfo.TryGetValue<T>(forObject);
            value = result.value as T;
            return result.hasValue;
        }
        /// <summary>嘗試取得MemberInfo的值，並回傳取得資訊。</summary>
        public static GetValueResult TryGetValue<T>(this MemberInfo memberInfo, object forObject) where T : class
        {
            object tempValue = null;

            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    tempValue = ((FieldInfo)memberInfo).GetValue(forObject);
                    break;
                case MemberTypes.Property:
                    tempValue = ((PropertyInfo)memberInfo).GetValue(forObject);
                    break;
            }

            if (tempValue == null)
            {
                return new GetValueResult
                {
                    hasValue = true,
                    value = null
                };
            }

            T resultValue = tempValue as T;
            return new GetValueResult
            {
                hasValue = tempValue is T,
                value = resultValue
            };
        }

        /// <summary>從MemberInfo集合中，篩選出有特定Attribute的項目。</summary>
        public static IEnumerable<T> FilterWithAttribute<T>(this IEnumerable<T> target, Type attribute) where T : MemberInfo
        {
            List<T> result = new List<T>();
            foreach (var item in target)
            {
                var fieldAttrs = Attribute.GetCustomAttributes(item);
                foreach (Attribute attr in fieldAttrs)
                {
                    if (attr.GetType() == attribute || attr.GetType().IsSubclassOf(attribute))
                    {
                        result.Add(item as T);
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>取得MemberInfo的System.Type。</summary>
        public static Type GetMemberInfoType(this MemberInfo memberInfo)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).PropertyType;
                default:
                    throw new NotImplementedException();
            }
        }


        public static object FastInvoke(this MethodInfo methodInfo, object instance, params object[] parameters)
        {
            return ReflectorCaches.MethodInvokerCache.Get(methodInfo).Invoke(instance, parameters);
        }

        public static void FastSetValue(this PropertyInfo propertyInfo, object instance, object value)
        {
            ReflectorCaches.PropertyAccessorCache.Get(propertyInfo).SetValue(instance, value);
        }

        public static object FastGetValue(this PropertyInfo propertyInfo, object instance, bool cache = false)
        {
            return ReflectorCaches.PropertyAccessorCache.Get(propertyInfo).GetValue(instance);
        }

        public static object FastGetValue(this FieldInfo fieldInfo, object instance)
        {
            return ReflectorCaches.FieldAccessorCache.Get(fieldInfo).GetValue(instance);
        }

        public static void FastSetValue(this FieldInfo fieldInfo, object instance, object value)
        {
            // Due to there is no optimized implemention on FieldAccessor SetValue, we just use built-in SetValue
            //ReflectorCaches.FieldAccessorCache.Get(fieldInfo).SetValue(instance, value);
            fieldInfo.SetValue(instance, value);
        }

        // Get an object property for 1000000 times.
        // Direct: 	6.4138
        // Reflection: 	24.7381
        // FastReflection: 	229.9753
        // RawFastReflection: 	20.8627

        // Get an int property for 1000000 times.
        // Direct: 	6.4555
        // Reflection: 	26.1141
        // FastReflection: 	237.6488
        // RawFastReflection: 	27.8134

        // Execute method with arguments for 1000000 times.
        // Direct: 	6.8089
        // Reflection: 	1158.3469
        // FastReflection: 	243.5252
        // RawFastReflection: 	26.463

        // Execute method without arguments for 1000000 times.
        // Direct: 	6.7822
        // Reflection: 	947.1325
        // FastReflection: 	241.676
        // RawFastReflection: 	26.1743

        // Set an int property for 1000000 times.
        // Direct: 	6.9674
        // Reflection: 	1086.2465
        // FastReflection: 	266.2215
        // RawFastReflection: 	53.7662

        // Set an int field for 1000000 times.
        // Direct: 	6.7136
        // Reflection: 	474.0692
        // FastReflection: 	2.5209
        // RawFastReflection: 	3.0604

        // Set an object field for 1000000 times.
        // Direct: 	16.3675
        // Reflection: 	421.2984
        // FastReflection: 	2.6504
        // RawFastReflection: 	2.8831

    }
}
