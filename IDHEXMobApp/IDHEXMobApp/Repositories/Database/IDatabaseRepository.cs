using IDHEXMobApp.Models.Response;

namespace IDHEXMobApp.Repositories.Database
{
    public interface IDatabaseRepository
    {
        void Add(PedidoResponse pedido);
        void Delete(PedidoResponse pedido);
        Task<IEnumerable<PedidoResponse>> GetAll();
        void Update(PedidoResponse pedido);
    }
}
