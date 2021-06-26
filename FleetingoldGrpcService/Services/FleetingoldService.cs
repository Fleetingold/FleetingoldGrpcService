using FleetingoldGrpcService.Shared;
using MagicOnion;
using MagicOnion.Server;
using Microsoft.Extensions.Logging;
using System;

namespace FleetingoldGrpcService.Services
{
    // Implements RPC service in the server project.
    // The implementation class must inehrit `ServiceBase<IFleetingoldService>` and `IFleetingoldService`
    public class FleetingoldService : ServiceBase<IFleetingoldService>, IFleetingoldService
    {
        private readonly ILogger logger;

        public FleetingoldService(ILogger<IFleetingoldService> logger)
        {
            this.logger = logger;
        }

        // `UnaryResult<T>` allows the method to be treated as `async` method.
        public async UnaryResult<int> SumAsync(int x, int y)
        {
            logger.LogInformation($"---------------------------------- param x:{x},y:{y}");
            //Console.WriteLine($"Received:{x}, {y}");
            logger.LogInformation($"---------------------------------- result: {x + y}");

            return x + y;
        }
    }
}
