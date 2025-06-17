using IDHEXMobApp.Models.Response;
using IDHEXMobApp.Repositories.Database;

namespace IDHEXMobApp.ViewModels
{
    public partial class NotaViewModel: BaseViewModel
    {
        private readonly IDatabaseRepository _databaseRepository;
        public ObservableCollection<PedidoResponse> Pedidos { get; set; } = new ObservableCollection<PedidoResponse>();
        public NotaViewModel(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        internal async Task InitiAsync()
        {
            IsBusy = true;

            //_databaseRepository.Delete(new PedidoResponse());

            var pedidos = await _databaseRepository.GetAll();            

            foreach (var pedido in pedidos)
            {
                Pedidos.Add(new PedidoResponse
                {
                    MotoristaId = pedido.MotoristaId,
                    NumRomaneio = pedido.NumRomaneio,
                    NumNotaFiscal = pedido.NumNotaFiscal,
                    DataPrevisaoSaida = pedido.DataPrevisaoSaida,
                    VlrNotaFiscal = pedido.VlrNotaFiscal,
                    NomeTomador = pedido.NomeTomador,
                    Volumes = pedido.Volumes,
                    Nome = pedido.Nome,
                    CNPJ = pedido.CNPJ,
                    Logradouro = pedido.Logradouro,
                    Bairro = pedido.Bairro,
                    Cidade = pedido.Cidade,
                    ImgCanhoto = pedido.ImgCanhoto,
                    DtImgCanhoto = pedido.DtImgCanhoto,
                    CodOcorrencia = pedido.CodOcorrencia
                });            
            }


            IsBusy = false;
        }
    }
}
