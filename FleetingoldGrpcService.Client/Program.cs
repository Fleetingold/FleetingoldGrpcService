using FleetingoldGrpcService.Shared;
using Grpc.Net.Client;
using MagicOnion.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

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

            _ = UnaryRun(client);

            _ = ClientStreamRun(client);

            _ = ServerStreamRun(client);

            _ = DuplexStreamRun(client);

            Console.ReadLine();
        }

        static async Task UnaryRun(IFleetingoldService client)
        {
            // await(C# 7.0, Unity 2018.3+)
            var vvvvv = await client.SumAsync(10, 20);
            Console.WriteLine("SumAsync:" + vvvvv);

            // if use Task<UnaryResult>(Unity 2018.2), use await await
            var vvvv2 = await await client.SumLegacyTaskAsync(10, 20);
        }

        static async Task ClientStreamRun(IFleetingoldService client)
        {
            var stream = await client.ClientStreamingSampleAsync();

            for (int i = 0; i < 3; i++)
            {
                await stream.RequestStream.WriteAsync(i);
            }
            await stream.RequestStream.CompleteAsync();

            var response = await stream.ResponseAsync;

            Console.WriteLine("Response:" + response);
        }

        static async Task ServerStreamRun(IFleetingoldService client)
        {
            var stream = await client.ServertSreamingSampleAsync(10, 20, 3);

            CancellationToken cancellationToken = new CancellationToken();

            while (await stream.ResponseStream.MoveNext(cancellationToken)) 
            {
                Console.WriteLine("ServerStream Response:" + stream.ResponseStream.Current);
            }
        }

        static async Task DuplexStreamRun(IFleetingoldService client)
        {
            var stream = await client.DuplexStreamingSampleAync();

            var count = 0;
            CancellationToken cancellationToken = new CancellationToken();

            while (await stream.ResponseStream.MoveNext(cancellationToken))
            {
                string x = stream.ResponseStream.Current;
                Console.WriteLine("DuplexStream Response:" + x);

                await stream.RequestStream.WriteAsync(count++);
                if (x == "finish")
                {
                    await stream.RequestStream.CompleteAsync();
                }
            }
        }
    }
}
