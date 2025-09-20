using CommunityToolkit.Maui.Core.Extensions;
using Google.Protobuf.Collections;
using IDHEXMobApp.Models.Response;
using IDHEXMobApp.Repositories.Database;

namespace IDHEXMobApp.ViewModels
{
    [QueryProperty(nameof(Romaneio), nameof(Romaneio))]
    public partial class NotaViewModel : BaseViewModel
    {
        //// Variáveis de controle de paginação
        private int _startIndex = 0;
        private const int PageSize = 1;
        //private int _pageNumber = 0;        
        //private const int PageSize = 5;
        // Usaremos esta variável para saber se chegamos ao final da fonte de dados
        private bool _hasMoreData = true;

        private RomaneioResponse _romaneio;
        public RomaneioResponse Romaneio
        {
            get => _romaneio;
            set
            {
                SetProperty(ref _romaneio, value);

                if (value != null)
                {
                    NumRomaneio = value.NumRomaneio;
                    TotalNotas = value.TotalNotas;
                    DataPrevisaoSaida = value.DataPrevisaoSaida;
                }
            }
        }

        [ObservableProperty]
        string numRomaneio;

        [ObservableProperty]
        Int32 totalNotas;

        [ObservableProperty]
        DateTime? dataPrevisaoSaida;

        [ObservableProperty]
        string filtroPesquisa;

        [ObservableProperty]
        private bool isRefreshing;


        private readonly IDatabaseRepository _databaseRepository;
        public ObservableCollection<PedidoResponse> Pedidos { get; set; } = new ObservableCollection<PedidoResponse>();
        public ObservableCollection<PedidoResponse> PedidosFiltrados { get; set; } = new ObservableCollection<PedidoResponse>();
        public NotaViewModel(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        internal async Task InitiAsync()
        {
            IsBusy = true;

            await Task.Delay(1000);

            if (NumRomaneio != null)
            {
                var pedidos = _databaseRepository.GetPedidosByNumRomaneioAsync(NumRomaneio!).Where(p => p.Baixado == "NÃO");
                PedidosFiltrados = pedidos.ToObservableCollection<PedidoResponse>();
            }
            else
                PedidosFiltrados = _databaseRepository.GetAll().Where(p => p.Baixado == "NÃO").ToObservableCollection<PedidoResponse>();

            OnPropertyChanged(nameof(PedidosFiltrados));

            Pedidos = new ObservableCollection<PedidoResponse>(PedidosFiltrados);

            IsBusy = false;

            await Task.CompletedTask;
        }

        //[RelayCommand]
        //public async Task CameraAsync()
        // => await Shell.Current.GoToAsync(nameof(CameraPage));

        [RelayCommand]
        public async Task CameraAsync(PedidoResponse pedido)
        {
            if (pedido is null)
                return;

            var navigationParams = new Dictionary<string, object>
            {
                {"Pedido", pedido }
            };

            await Shell.Current.GoToAsync(nameof(CameraPage), navigationParams);
        }

        public async Task CarregaRomaneiosAsync()
        {
            IsBusy = true;

            if (NumRomaneio != null)
            {
                var pedidos = _databaseRepository.GetPedidosByNumRomaneioAsync(NumRomaneio!).Where(p => p.Baixado == "NÃO");
                PedidosFiltrados = pedidos.ToObservableCollection<PedidoResponse>();
            }

            Pedidos = new ObservableCollection<PedidoResponse>(PedidosFiltrados);

            IsBusy = false;

            await Task.CompletedTask;
        }

        public void AtualizarFiltroAsync()
        {

            IsBusy = true;

            PedidosFiltrados.Clear();
            var termo = FiltroPesquisa?.ToLower() ?? "";

            var filtrados = string.IsNullOrWhiteSpace(termo)
                ? Pedidos
                : Pedidos.Where(x =>
                    (x.NumNotaFiscal.ToString().Contains(termo))
                );

            if (filtrados.Any())
            {
                foreach (var item in filtrados)
                    PedidosFiltrados.Add(item);
            }
            else
            {
                var pedidos = _databaseRepository.GetAll().Where(p => p.Baixado == "NÃO" && p.NumNotaFiscal.ToString() == termo).FirstOrDefault();
                if (pedidos != null)
                    PedidosFiltrados.Add(pedidos);
            }

            IsBusy = false;
            //var filtrados = string.IsNullOrWhiteSpace(termo)
            //    ? Pedidos
            //    : Pedidos.Where(x =>
            //        (x.NumNotaFiscal.ToString().Contains(termo))
            //    );
            //foreach (var item in filtrados)
            //    PedidosFiltrados.Add(item);
        }        
    }
}
