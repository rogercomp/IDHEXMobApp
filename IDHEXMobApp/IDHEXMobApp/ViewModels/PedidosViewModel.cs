using IDHEXMobApp.Models.Response;
using IDHEXMobApp.Repositories;
using IDHEXMobApp.Repositories.Database;
using System.Windows.Input;


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
        public ObservableCollection<RomaneioResponse> Romaneios { get; set; } = new ObservableCollection<RomaneioResponse>();
        public ICommand NotasCommand { get; }
        public PedidosViewModel(IPedidoRepository pedidoRepository, IDatabaseRepository databaseRepository)
        {
            _pedidoRepository = pedidoRepository;
            _databaseRepository = databaseRepository;
            NotasCommand = new Command<RomaneioResponse>(async (romaneio) => await OnNotasAsync(romaneio));
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


                var resultado = (from p in pedidos
                                 group p by new { p.NumRomaneio, p.DataPrevisaoSaida } into g
                                 select new RomaneioResponse
                                 {
                                     NumRomaneio = g.Key.NumRomaneio,
                                     TotalNotas = g.Count(),
                                     DataPrevisaoSaida = g.Key.DataPrevisaoSaida
                                 }).ToList();

                Romaneios.Clear();

                foreach (var item in resultado)
                {
                    Romaneios.Add(new RomaneioResponse
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
        public async Task GoToNotasCommand()
            => await Shell.Current.GoToAsync(nameof(NotasPage));

        private async Task OnNotasAsync(RomaneioResponse romaneio)
        {
            await Shell.Current.DisplayAlert("Atenção", romaneio.NumRomaneio, "OK");
        }


        public async Task CarregaRomaneiosAsync()
        {
            IsBusy = true;

            var itens = _databaseRepository.GetAll();


            var resultado = (from p in itens
                             group p by new { p.NumRomaneio, p.DataPrevisaoSaida } into g
                             select new RomaneioResponse
                             {
                                 NumRomaneio = g.Key.NumRomaneio,
                                 TotalNotas = g.Count(),
                                 DataPrevisaoSaida = g.Key.DataPrevisaoSaida
                             }).ToList();

            Romaneios.Clear();

            foreach (var item in resultado)
            {
                Romaneios.Add(new RomaneioResponse
                {
                    NumRomaneio = item.NumRomaneio!,
                    TotalNotas = item.TotalNotas,
                    DataPrevisaoSaida = item.DataPrevisaoSaida
                });
            }

            await Task.CompletedTask;
        }


        //[RelayCommand]
        //public async Task Notas(RomaneioResponse romaneio)
        //{
        //    if (romaneio is null)
        //        return;

        //    //var navigationParams = new Dictionary<string, object>
        //    //{
        //    //    {"Pedido", romaneio }
        //    //};

        //    return;
        //    //await Shell.Current.DisplayAlert("Atenção", romaneio.NumRomaneio, "OK");
        //}
    }
}
