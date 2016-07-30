using System;
using System.Linq;

namespace Patterns.SmartCache.Host
{
    internal static class Constants
    {
        public static readonly Guid RegistryId = Guid.Parse("{d8c4eea3-ddd4-4d55-9287-a3201a698ae6}");

        public static readonly Guid[] ItemIds = new[]
        {
            "{80f81f8c-0d25-4c41-9bb6-1dfe316e2906}",
            "{7ebc2041-c353-4358-a80e-8bb86102b281}",
            "{05ecdb3a-b2bb-44e3-82fa-74a02bddf5fe}",
            "{3e939f25-bd50-4ce3-954a-3879c05122b6}",
            "{680902fd-58e0-4685-8771-4bca361d1964}",
            "{764b2999-81d2-484c-bddc-3b72d1278356}",
            "{fc59711f-5bc5-4023-a066-27c7453dee46}",
            "{b8383295-4db2-4f3a-a29e-d4818d2e9a7d}",
            "{83615992-ac35-476a-b0f1-7e11fdb47966}",
            "{6ed36967-379c-4b46-a7dc-44685b9ee1ae}"
        }.Select(Guid.Parse)
         .ToArray();

        public static readonly Guid BankBalanceGrainId = Guid.Parse("{d77f552a-7b4f-48fd-9f0e-ef4394cee3d2}");
    }
}