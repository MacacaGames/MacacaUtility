using System.Collections.Generic;
using System.Reflection;
using System;

namespace CloudMacaca
{
    public static class ReflectionUtility
    {
        const BindingFlags defaultBindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        /// <summary>取得MemberInfo的值。</summary>
        public static T GetValue<T>(this MemberInfo memberInfo, object forObject) where T: class
        {
            if (memberInfo.TryGetValue(forObject, out T result))
                return result;
            else
                throw new NotImplementedException();
        }

        /// <summary>嘗試取得MemberInfo的值，並回傳取得成功與否。</summary>
        public static bool TryGetValue<T>(this MemberInfo memberInfo, object forObject, out T value) where T : class
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
                value = null;
                return true;
            }

            value = tempValue as T;
            return value != null;
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


    }
}
