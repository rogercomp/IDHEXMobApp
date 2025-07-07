using IDHEXMobApp.Models.Request;
using IDHEXMobApp.Models.Response;

namespace IDHEXMobApp.Repositories.Database
{
    public interface IDatabaseRepository
    {
        void Add(PedidoResponse pedido);
        void Delete(PedidoResponse pedido);
        IEnumerable<PedidoResponse> GetAll();
        IEnumerable<PedidoResponse> GetPedidosByNumRomaneioAsync(string numRomaneio);
        PedidoResponse GetPedidosByRomaneioNotaPedidoEmpresaAsync(string numRomaneio, long numNotaFiscal, long pedidoId, long empresaId);
        void Update(PedidoResponse pedido);
    }
}
