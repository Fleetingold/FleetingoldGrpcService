using FleetingoldGrpcService.Shared;
using MagicOnion;
using MagicOnion.Server;
using System;

namespace FleetingoldGrpcService.Services
{
    // Implements RPC service in the server project.
    // The implementation class must inehrit `ServiceBase<IFleetingoldService>` and `IFleetingoldService`
    public class FleetingoldService : ServiceBase<IFleetingoldService>, IFleetingoldService
    {
        // `UnaryResult<T>` allows the method to be treated as `async` method.
        public async UnaryResult<int> SumAsync(int x, int y)
        {
            Console.WriteLine($"Received:{x}, {y}");
            return x + y;
        }
    }
}
