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

            pedido.Id = Guid.NewGuid();

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

        public IEnumerable<PedidoResponse> GetPedidosByNumRomaneioAsync(string numRomaneio)
        {
            return _database
                .GetCollection<PedidoResponse>(collectionName)
                .Query()
                .Where(p => p.NumRomaneio == numRomaneio)
                .OrderByDescending(a => a.NumNotaFiscal)
                .ToList();
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
            var itemPedido = _database
                .GetCollection<PedidoResponse>(collectionName)
                .Query()
                .Where(p => p.PedidoId == pedido.PedidoId && p.EmpresaId == pedido.EmpresaId && p.NumRomaneio == pedido.NumRomaneio && p.NumNotaFiscal == pedido.NumNotaFiscal)
                .FirstOrDefault();

            if (itemPedido != null)
            {
                itemPedido.DtImgCanhoto = DateTime.Now;
                itemPedido.ImgCanhoto = pedido.ImgCanhoto;
                itemPedido.CodOcorrencia = pedido.CodOcorrencia;
            }

            _database.GetCollection<PedidoResponse>(collectionName)
                     .Update(itemPedido!);
        }

        //public void Baixar(PedidoResponse pedido)
        //    {
        //    var itemPedido = _database
        //        .GetCollection<PedidoResponse>(collectionName)
        //        .Query()
        //        .Where(p => p.PedidoId == pedido.PedidoId && p.EmpresaId == pedido.EmpresaId && p.NumRomaneio == pedido.NumRomaneio && p.NumNotaFiscal == pedido.NumNotaFiscal)
        //        .FirstOrDefault();         
            
        //    _database.GetCollection<PedidoResponse>(collectionName)
        //             .Delete(itemPedido.Id);
        //}
    }
}
