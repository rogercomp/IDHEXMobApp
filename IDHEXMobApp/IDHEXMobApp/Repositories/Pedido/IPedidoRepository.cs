using IDHEXMobApp.Models.Response;

namespace IDHEXMobApp.Repositories
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<PedidoResponse>> GetPedidosAsync();
        Task<bool> AtualizaSincronismoAsync(long pedidoId, long empresaId);
        Task<bool> AtualizaPedidoAsync(long pedidoId, long empresaId, string ocorrenciaId, string imgCanhoto);
        
    }
}
