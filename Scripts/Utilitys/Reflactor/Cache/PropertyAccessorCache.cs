//Source from https://archive.codeplex.com/?p=fastreflectionlib
using System.Reflection;

namespace MacacaGames
{
    internal class PropertyAccessorCache : ReflectorCache<PropertyInfo, IAccessor>
    {
        protected override IAccessor Create(PropertyInfo key)
        {
            return CacheFactories.PropertyAccessorFactory.Create(key);
        }
    }
}