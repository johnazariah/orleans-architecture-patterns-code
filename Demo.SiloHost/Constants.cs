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

        public static readonly Guid SingleBankAccountGrainId = Guid.Parse("{d77f552a-7b4f-48fd-9f0e-ef4394cee3d2}");

        public static readonly Guid[] BankAccountGrainIds = new[]
        {
            "{6b66f0ea-b329-4483-b78b-108f4df62eee}",
            "{d970e4cd-3a27-4ec4-b356-0e22e67c8424}",
            "{77359a61-1f9e-4152-a0ea-7b55039347d9}",
            "{c0f72fa5-da95-4166-8a60-1fac49820f3a}",
            "{bc29f955-ab93-4e7c-b264-59e1ffbc2ffc}",
            "{090a8ba8-1c17-4338-801f-245be0a610f5}",
            "{9c3d599b-0cac-4882-9244-12df92dc4da5}",
            "{22447804-74e6-4a7f-aa8c-e8cd0dc851aa}",
            "{740cf2dd-30be-4555-8f5b-3ee9f875b187}",
            "{1a5f0a25-7965-4e22-92cf-127f0c462048}"
        }.Select(Guid.Parse)
         .ToArray();

        public static readonly Guid BankAccountAggregateGrainId = Guid.Parse("{d77f552a-7b4f-48fd-9f0e-ef4394cee3d2}");
    }
}