using System;
using System.Net;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace Patterns.SmartCache.Host
{
    public class OrleansHost : IDisposable
    {
        private static SiloHost _siloHost;
        private readonly AppDomain _hostDomain;

        // The Cluster config is quirky and weird to configure in code, so we're going to use a config file
        public OrleansHost()
        {
            _hostDomain = AppDomain.CreateDomain("OrleansHost", null,
                new AppDomainSetup
                {
                    AppDomainInitializer = InitSilo
                });

            // Configure and Initialize GrainClient
            var config = ClientConfiguration.LocalhostSilo(30000);
            GrainClient.Initialize(config);
        }

        public void Dispose()
        {
            _hostDomain.DoCallBack(ShutdownSilo);
        }

        private static void InitSilo(string[] args)
        {
            var siloName = Dns.GetHostName();

            _siloHost = new SiloHost(siloName)
            {
                ConfigFileName = "OrleansConfiguration.xml",
                Type = Silo.SiloType.Primary
            };
            _siloHost.InitializeOrleansSilo();

            if (!_siloHost.StartOrleansSilo())
            {
                throw new SystemException(
                    $"Failed to start Orleans silo '{_siloHost.Name}' as a {_siloHost.Type} node");
            }
            ;
        }

        private static void ShutdownSilo()
        {
            if (_siloHost == null) return;

            _siloHost.Dispose();
            GC.SuppressFinalize(_siloHost);
            _siloHost = null;
        }

        public static void WaitForInteraction()
        {
            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();
        }
    }
}