using IDHEXMobApp.Models.Response;

namespace IDHEXMobApp.Repositories.Pedido
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<PedidoResponse>> GetPedidosAsync();
        
    }
}
