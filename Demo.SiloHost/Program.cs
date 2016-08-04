using System.Threading.Tasks;
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
                    RunDemos().Wait();
                }
            }
        }

        private static async Task RunDemos()
        {
            SmartCacheDemo.Run();
            RegistryDemo.Run();

            await EventSourcingDemo.Run();

            await AggregateDemo.Run();

            await StateMachineDemo.Run();
        }
    }
}