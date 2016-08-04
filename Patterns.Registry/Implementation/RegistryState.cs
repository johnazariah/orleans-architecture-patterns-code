using System;
using System.Collections.Generic;

namespace Patterns.Registry.Implementation
{
    [Serializable]
    public class RegistryState<TRegisteredGrain>
    {
        public HashSet<TRegisteredGrain> RegisteredGrains { get; set; } = new HashSet<TRegisteredGrain>();
    }
}