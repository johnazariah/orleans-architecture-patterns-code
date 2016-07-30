using System;
using System.Net;
using Orleans.Runtime;
using Orleans.Runtime.Host;

namespace Patterns.DevBreadboard
{
    public class DevelopmentSiloHost : IDisposable
    {
        private readonly AppDomain _hostDomain;

        public DevelopmentSiloHost(string configurationFileName = "OrleansConfiguration.xml", string appDomainName = "DevelopmentSiloHost")
        {
            var appDomainSetup = new AppDomainSetup
            {
                AppDomainInitializer = InitSilo,
                AppDomainInitializerArguments = new [] {configurationFileName}
            };

            _hostDomain = AppDomain.CreateDomain(appDomainName, null, appDomainSetup);
        }

        public void Dispose()
        {
            _hostDomain.DoCallBack(ShutdownSilo);
        }

        private static void InitSilo(string[] args)
        {
            var siloName = Dns.GetHostName();
            var siloHost = new SiloHost(siloName)
            {
                ConfigFileName = args[0],
                Type = Silo.SiloType.Primary
            };

            siloHost.InitializeOrleansSilo();

            if (!siloHost.StartOrleansSilo())
            {
                throw new SystemException(
                    $"Failed to start Orleans silo '{siloHost.Name}' as a {siloHost.Type} node");
            }

            AppDomain.CurrentDomain.SetData("siloHost", siloHost);
        }

        private static void ShutdownSilo()
        {
            var siloHost = AppDomain.CurrentDomain.GetData("siloHost") as SiloHost;
            if (siloHost == null) return;

            siloHost.Dispose();
            GC.SuppressFinalize(siloHost);

            AppDomain.CurrentDomain.SetData("siloHost", null);
        }

        public static void WaitForInteraction()
        {
            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();
        }
    }
}