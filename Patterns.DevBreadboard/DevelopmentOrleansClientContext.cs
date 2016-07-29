using System;
using Orleans;
using Orleans.Runtime.Configuration;

namespace Patterns.DevBreadboard
{
    public class DevelopmentOrleansClientContext : IDisposable
    {
        public DevelopmentOrleansClientContext()
        {
            var config = ClientConfiguration.LocalhostSilo(30000);
            GrainClient.Initialize(config);
        }

        public void Dispose()
        {
        }
    }
}