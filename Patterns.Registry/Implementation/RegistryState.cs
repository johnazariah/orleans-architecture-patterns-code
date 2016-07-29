using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace Patterns.Registry.Implementation
{
    [Serializable]
    public class RegistryState<TRegistryItemGrain>
    {
        public HashSet<TRegistryItemGrain> ItemGrains { get; set; }
    }
}