using IDHEXMobApp.Models.Response;
using LiteDB;

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
        }

        public void Delete(PedidoResponse pedido)
        {
            //var col = _database.GetCollection<PedidoResponse>(collectionName);
            _database.DropCollection("pedidos");
            //col.DeleteAll();
        }

        public IEnumerable<PedidoResponse> GetAll()
        {   
                return _database
                    .GetCollection<PedidoResponse>(collectionName)
                    .Query()
                    .OrderByDescending(a => a.NumRomaneio)
                    .ToList();            
        }

        public void Update(PedidoResponse pedido)
        {
            throw new NotImplementedException();
        }
    }
}
