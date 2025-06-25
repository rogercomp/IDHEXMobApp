using IDHEXMobApp.Models.Response;

namespace IDHEXMobApp.Repositories.Database
{
    public interface IDatabaseRepository
    {
        void Add(PedidoResponse pedido);
        void Delete(PedidoResponse pedido);
        IEnumerable<PedidoResponse> GetAll();
        IEnumerable<PedidoResponse> GetPedidosByNumRomaneioAsync(string numRomaneio);
        void Update(PedidoResponse pedido);
    }
}
