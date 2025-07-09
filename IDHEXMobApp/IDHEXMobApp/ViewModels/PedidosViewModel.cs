using IDHEXMobApp.Models.Response;
using IDHEXMobApp.Repositories;
using IDHEXMobApp.Repositories.Database;

namespace IDHEXMobApp.ViewModels
{
    
    public partial class PedidosViewModel : BaseViewModel
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IDatabaseRepository _databaseRepository;
        
        [ObservableProperty]
        private string baixados = "Entregues: 0";
        [ObservableProperty]
        private string sincronizados = "Sinc:";
        [ObservableProperty]
        private string _numRomaneio = string.Empty;
        [ObservableProperty]
        string filtroPesquisa;

        public ObservableCollection<RomaneioResponse> Romaneios { get; set; } = new ObservableCollection<RomaneioResponse>();
        public ObservableCollection<RomaneioResponse> RomaneiosFiltrados { get; } = new();
        public PedidosViewModel(IPedidoRepository pedidoRepository, IDatabaseRepository databaseRepository)
        {
            _pedidoRepository = pedidoRepository;
            _databaseRepository = databaseRepository;
            //AtualizarFiltroAsync();
        }

        internal async Task InitiAsync()
        {
            IsBusy = true;

            //_databaseRepository.Delete(new PedidoResponse());

            var pedidos = await _pedidoRepository.GetPedidosAsync();

            int Total = pedidos.Count();

            if (Total > 0)
            {

                int contador = 0;

                //_databaseRepository.Delete(new PedidoResponse());

                foreach (var pedido in pedidos)
                {
                    var existingPedido = false;// await _databaseRepository.GetPedidoByNumRomaneioAsync(pedido.NumRomaneio!);

                    if (!existingPedido)
                    {
                        contador++;
                        await Task.Delay(1000);
                        Sincronizados = $"Sinc: {contador}/{Total}";
                        SaveOrderInDatabase(pedido);
                        bool isSyncronized = await _pedidoRepository.AtualizaSincronismoAsync(pedido.PedidoId, pedido.EmpresaId);
                        if (!isSyncronized)
                        {
                            await Shell.Current.DisplayAlert("Erro", "Erro ao sincronizar pedidos.", "OK");
                            return;
                        }
                    }
                }

                var resultado = (from p in pedidos.Where(p=> p.Baixado == "NÃO")
                                 group p by new { p.NumRomaneio, p.DataPrevisaoSaida } into g                                 
                                 select new RomaneioResponse
                                 {
                                     NumRomaneio = g.Key.NumRomaneio,
                                     TotalNotas = g.Count(),
                                     DataPrevisaoSaida = g.Key.DataPrevisaoSaida
                                 }).ToList();

                RomaneiosFiltrados.Clear();

                foreach (var item in resultado)
                {
                    RomaneiosFiltrados.Add(new RomaneioResponse
                    {
                        NumRomaneio = item.NumRomaneio!,
                        TotalNotas = item.TotalNotas,
                        DataPrevisaoSaida = item.DataPrevisaoSaida
                    });
                }
            }
            else
            {
                await CarregaRomaneiosAsync();
            }

            IsBusy = false;
        }

        private void SaveOrderInDatabase(PedidoResponse item)
        {
            PedidoResponse pedido = new PedidoResponse()
            {
                PedidoId = item.PedidoId,
                EmpresaId = item.EmpresaId,
                MotoristaId = item.MotoristaId,
                NumRomaneio = item.NumRomaneio,
                DataPrevisaoSaida = item.DataPrevisaoSaida,
                NomeTomador = item.NomeTomador,
                NumNotaFiscal = item.NumNotaFiscal,
                VlrNotaFiscal = item.VlrNotaFiscal,
                Volumes = item.Volumes,
                Nome = item.Nome,
                CNPJ = item.CNPJ,
                Logradouro = item.Logradouro,
                Bairro = item.Bairro,
                Cidade = item.Cidade,
                ImgCanhoto = item.ImgCanhoto,
                DtImgCanhoto = item.DtImgCanhoto
            };

            _databaseRepository.Add(pedido);
        }

        [RelayCommand]
        public async Task GoToNotas()
            => await Shell.Current.GoToAsync(nameof(NotasPage));

        [RelayCommand]
        public async Task GoToEdit(RomaneioResponse romaneio)
        {
            if (romaneio is null)
                return;

            var navigationParams = new Dictionary<string, object>
            {
                {"Romaneio", romaneio }
            };

            await Shell.Current.GoToAsync(nameof(NotasPage), navigationParams);
        }

        public async Task CarregaRomaneiosAsync()
        {
            IsBusy = true;

            //_databaseRepository.Delete(new PedidoResponse());

              var itens = _databaseRepository.GetAll();


              var resultado = (from p in itens.Where(p => p.Baixado == "NÃO")
                               group p by new { p.NumRomaneio, p.DataPrevisaoSaida } into g
                               select new RomaneioResponse
                               {
                                   NumRomaneio = g.Key.NumRomaneio,
                                   TotalNotas = g.Count(),
                                   DataPrevisaoSaida = g.Key.DataPrevisaoSaida
                               }).ToList();

            RomaneiosFiltrados.Clear();

              foreach (var item in resultado)
              {
                RomaneiosFiltrados.Add(new RomaneioResponse
                  {
                      NumRomaneio = item.NumRomaneio!,
                      TotalNotas = item.TotalNotas,
                      DataPrevisaoSaida = item.DataPrevisaoSaida
                  });
              }           

            Romaneios = new ObservableCollection<RomaneioResponse>(RomaneiosFiltrados);

            await Task.CompletedTask;
        }

        public void AtualizarFiltroAsync()
        {
         
            RomaneiosFiltrados.Clear();
            var termo = FiltroPesquisa?.ToLower() ?? "";
            var filtrados = string.IsNullOrWhiteSpace(termo)
                ? Romaneios
                : Romaneios.Where(x =>
                    (x.NumRomaneio?.Contains(termo) ?? false)         
                );
            foreach (var item in filtrados)
                RomaneiosFiltrados.Add(item);
        }
    }
}
