using FleetingoldGrpcService.Shared;
using MagicOnion;
using MagicOnion.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FleetingoldGrpcService.Services
{
    // Implements RPC service in the server project.
    // The implementation class must inehrit `ServiceBase<IFleetingoldService>` and `IFleetingoldService`
    public class FleetingoldService : ServiceBase<IFleetingoldService>, IFleetingoldService
    {
        private readonly ILogger Logger;

        public FleetingoldService(ILogger<IFleetingoldService> logger)
        {
            this.Logger = logger;
        }

        // `UnaryResult<T>` allows the method to be treated as `async` method.
        public async UnaryResult<int> SumAsync(int x, int y)
        {
            Logger.LogInformation($"---------------------------------- param x:{x},y:{y}");
            //Console.WriteLine($"Received:{x}, {y}");
            Logger.LogInformation($"---------------------------------- result: {x + y}");

            return x + y;
        }

        // VS2015(C# 6.0), Unity 2018.2 use Task
        public async Task<UnaryResult<string>> SumLegacyTaskAsync(int x, int y)
        {
            Logger.LogDebug($"Called SumAsync - x:{x} y:{y}");

            // use UnaryResult method.
            return UnaryResult((x + y).ToString());
        }

        public async Task<ClientStreamingResult<int, string>> ClientStreamingSampleAsync()
        {
            Logger.LogDebug($"Called ClientStreamingSampleAsync");

            // If ClientStreaming, use GetClientStreamingContext.
            var stream = GetClientStreamingContext<int, string>();

            // receive from client asynchronously
            await stream.ForEachAsync(x =>
            {
                Logger.LogDebug("Client Stream Received:" + x);
            });

            // StreamingContext.Result() for result value.
            return stream.Result("finished");
        }

        public async Task<ServerStreamingResult<string>> ServertSreamingSampleAsync(int x, int y, int z)
        {
            Logger.LogDebug($"Called ServertSreamingSampleAsync - x:{x} y:{y} z:{z}");

            var stream = GetServerStreamingContext<string>();

            var acc = 0;
            for (int i = 0; i < z; i++)
            {
                acc = acc + x + y;
                await stream.WriteAsync(acc.ToString());
            }

            return stream.Result();
        }

        public async Task<DuplexStreamingResult<int, string>> DuplexStreamingSampleAync()
        {
            Logger.LogDebug($"Called DuplexStreamingSampleAync");

            // DuplexStreamingContext represents both server and client streaming.
            var stream = GetDuplexStreamingContext<int, string>();

            var waitTask = Task.Run(async () =>
            {
                // ForEachAsync(MoveNext, Current) can receive client streaming.
                while (await stream.MoveNext()) 
                {
                    Logger.LogDebug($"Duplex Streaming Received:" + stream.Current);
                }
            });

            // WriteAsync is ServerStreaming.
            await stream.WriteAsync("test1");
            await stream.WriteAsync("test2");
            await stream.WriteAsync("finish");

            await waitTask;

            return stream.Result();
        }
    }
}
