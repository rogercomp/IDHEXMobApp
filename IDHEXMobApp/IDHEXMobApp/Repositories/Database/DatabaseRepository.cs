
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
            
            var item = GetPedidosByRomaneioNotaPedidoEmpresaAsync(pedido.NumRomaneio!, pedido.NumNotaFiscal, pedido.PedidoId, pedido.EmpresaId);
            //var item = GetPedidosByNumRomaneioAsync(pedido.NumRomaneio!);

            if (item == null)
            {
                pedido.Id = Guid.NewGuid();
                col.Insert(pedido);
            }
        }

        public void DeleteAll(PedidoResponse pedido)
        {
            //var col = _database.GetCollection<PedidoResponse>(collectionName);
            //_database.DropCollection("pedidos");

            var collection = _database.GetCollection<PedidoResponse>(collectionName);

            // Delete all documents in the collection
            // The predicate "1=1" is a common way to match all documents in LiteDB queries.
            var deletedCount = collection.DeleteMany("1=1");

            //col.DeleteAll();
        }
        public void DeleteById(Guid Id)
        {
            var col = _database.GetCollection<PedidoResponse>(collectionName);
            col.Delete(Id);
        }

        public IEnumerable<PedidoResponse> GetAll()
        {
            return _database
                .GetCollection<PedidoResponse>(collectionName)
                .Query()
                .OrderByDescending(a => a.NumRomaneio)
                .ToList();
        }

        public IEnumerable<PedidoResponse> GetPedidosByNumRomaneioAsync(string numRomaneio)
        {
            return _database
                .GetCollection<PedidoResponse>(collectionName)
                .Query()
                .Where(p => p.NumRomaneio == numRomaneio && p.Baixado == "NÃO")
                .OrderByDescending(a => a.NumNotaFiscal)
                .ToList();
        }

        public PedidoResponse GetPedidosByNumNotaAsync(string numNota)
        {
            return _database
                .GetCollection<PedidoResponse>(collectionName)
                .Query()
                .Where(p => p.NumNotaFiscal == long.Parse(numNota) && p.Baixado == "NÃO").FirstOrDefault();

        }

        public PedidoResponse GetPedidosByRomaneioNotaPedidoEmpresaAsync(string numRomaneio, long numNotaFiscal, long pedidoId, long empresaId)
        {
            return _database
               .GetCollection<PedidoResponse>(collectionName)
               .Query()
               .Where(p => p.NumRomaneio == numRomaneio && p.NumNotaFiscal == numNotaFiscal && p.PedidoId == pedidoId && p.EmpresaId == empresaId)
               .OrderByDescending(a => a.NumNotaFiscal).FirstOrDefault();
        }

        public void Update(PedidoResponse pedido)
        {
            //var connectionString = "Filename=database.db; Connection=Shared;";
            //using (var db = new LiteDatabase(connectionString))
            //{
                // Obtém a coleção de pedidos
                var pedidos = _database.GetCollection<PedidoResponse>(collectionName);

                // 1. Encontra um pedido pelo ID
                var ped = pedidos.FindById(pedido.Id);

                if (ped != null)
                {
                    // 2. Modifica vários campos
                    ped.DtImgCanhoto = pedido.DtImgCanhoto ?? DateTime.Now;
                    ped.ImgCanhoto = pedido.ImgCanhoto;
                    ped.CodOcorrencia = pedido.CodOcorrencia;

                    // 3. Atualiza o documento completo no banco de dados
                    pedidos.Update(ped);
                }                           
        }        
    }
}
