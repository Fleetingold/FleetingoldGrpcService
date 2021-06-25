using FleetingoldGrpcService.Shared;
using Grpc.Net.Client;
using MagicOnion.Client;
using System;

namespace FleetingoldGrpcService.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Fleetingold Grpc Service!");

            // Connect to the server using gRPC channel.
            var channel = GrpcChannel.ForAddress("https://localhost:5001");

            // Create a proxy to call the server transparently.
            var client = MagicOnionClient.Create<IFleetingoldService>(channel);

            // Call the server-side method using the proxy.
            var result = client.SumAsync(123, 456).GetAwaiter().GetResult();
            Console.WriteLine($"Result: {result}");
            Console.ReadLine();
        }
    }
}
