//Source from https://archive.codeplex.com/?p=fastreflectionlib
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MacacaGames
{
    public interface IReflactorCache<TKey, TValue>
    {
        TValue Get(TKey key);
    }
}
