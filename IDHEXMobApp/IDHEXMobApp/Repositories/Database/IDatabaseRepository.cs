using IDHEXMobApp.Models.Pedido;

namespace IDHEXMobApp.Repositories.Database
{
    public interface IDatabaseRepository
    {
        void Add(Pedido pedido);
        void Delete(Pedido pedido);
        List<Pedido> GetAll();
        void Update(Pedido pedido);
    }
}
