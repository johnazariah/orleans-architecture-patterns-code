using Patterns.DevBreadboard;

namespace Patterns.SmartCache.Host
{
    internal class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            using (new DevelopmentSiloHost())
            {
                using (new DevelopmentOrleansClientContext())
                {
                    SmartCacheDemo.Run();
                    RegistryDemo.Run();
                    EventSourcingDemo.Run();
                    AggregateDemo.Run();
                }
            }
        }
    }
}