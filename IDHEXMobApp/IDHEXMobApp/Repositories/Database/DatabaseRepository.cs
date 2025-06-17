using IDHEXMobApp.Models.Response;
using LiteDB;
using System.Transactions;

namespace IDHEXMobApp.Repositories.Database
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly LiteDatabase _database;
        private readonly string collectionName = "pedidos";
        public DatabaseRepository(LiteDatabase database)
        {
            _database = database;
        }

        public void Add(PedidoResponse pedido)
        {
            var col = _database.GetCollection<PedidoResponse>(collectionName);
            col.Insert(pedido);
            col.EnsureIndex(a => a.NumRomaneio);
        }

        public void Delete(PedidoResponse pedido)
        {
            //var col = _database.GetCollection<PedidoResponse>(collectionName);
            _database.DropCollection("pedidos");
            //col.DeleteAll();
        }

        public async Task<IEnumerable<PedidoResponse>> GetAll()
        {
            return await Task.Run(() =>
            {
                return _database
                    .GetCollection<PedidoResponse>(collectionName)
                    .Query()
                    .OrderByDescending(a => a.NumRomaneio)
                    .ToList();
            });
        }

        public void Update(PedidoResponse pedido)
        {
            throw new NotImplementedException();
        }
    }
}
