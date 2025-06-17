using IDHEXMobApp.Models.Pedido;
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

        public void Add(Pedido pedido)
        {
            var col = _database.GetCollection<Pedido>(collectionName);
            col.Insert(pedido);
            col.EnsureIndex(a => a.NumRomaneio);
        }

        public void Delete(Pedido pedido)
        {
            throw new NotImplementedException();
        }

        public List<Pedido> GetAll()
        {
            return _database
             .GetCollection<Pedido>(collectionName)
             .Query()
             .OrderByDescending(a => a.NumRomaneio)
             .ToList();
        }

        public void Update(Pedido pedido)
        {
            throw new NotImplementedException();
        }
    }
}
