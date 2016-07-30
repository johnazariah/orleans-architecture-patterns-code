using System;
using System.Collections.Generic;

namespace Patterns.Registry.Implementation
{
    [Serializable]
    public class RegistryState<TRegistryItemGrain>
    {
        public HashSet<TRegistryItemGrain> ItemGrains { get; set; } = new HashSet<TRegistryItemGrain>();
    }
}