using System;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;

namespace Patterns.DevBreadboard
{
    public class DevelopmentOrleansClientContext : IDisposable
    {
        public DevelopmentOrleansClientContext()
        {
            var config = ClientConfiguration.LocalhostSilo(30000);
            config.DefaultTraceLevel = Severity.Warning;
            GrainClient.Initialize(config);
        }

        public void Dispose()
        {
        }
    }
}