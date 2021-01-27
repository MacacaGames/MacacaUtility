//Source from https://archive.codeplex.com/?p=fastreflectionlib
using System.Reflection;

namespace MacacaGames
{
    internal class FieldAccessorCache : ReflectorCache<FieldInfo, IAccessor>
    {
        protected override IAccessor Create(FieldInfo key)
        {
            return CacheFactories.FieldAccessorFactory.Create(key);
        }
    }
}