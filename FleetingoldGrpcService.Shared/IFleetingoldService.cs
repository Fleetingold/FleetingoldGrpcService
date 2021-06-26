using MagicOnion;
using System.Threading.Tasks;

namespace FleetingoldGrpcService.Shared
{
    // Defines .NET interface as a Server/Client IDL.
    // The interface is shared between server and client.
    public interface IFleetingoldService : IService<IFleetingoldService>
    {
        // The return type must be `UnaryResult<T>`.
        UnaryResult<int> SumAsync(int x, int y);

        //MagicOnion can define and use primitive gRPC APIs(ClientStreaming, ServerStreaming, DuplexStreaming). 
        Task<UnaryResult<string>> SumLegacyTaskAsync(int x, int y);
        Task<ClientStreamingResult<int, string>> ClientStreamingSampleAsync();
        Task<ServerStreamingResult<string>> ServertSreamingSampleAsync(int x, int y, int z);
        Task<DuplexStreamingResult<int, string>> DuplexStreamingSampleAync();
    }
}
