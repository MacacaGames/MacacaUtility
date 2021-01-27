//Source from https://archive.codeplex.com/?p=fastreflectionlib
using System.Reflection;

namespace MacacaGames
{
    internal class MethodInvokerCache : ReflectorCache<MethodInfo, IMethodInvoker>
    {
        protected override IMethodInvoker Create(MethodInfo key)
        {
            return CacheFactories.MethodInvokerFactory.Create(key);
        }
    }
}