using CommunityToolkit.Maui.Core.Extensions;
using IDHEXMobApp.Models.Response;
using IDHEXMobApp.Repositories.Database;

namespace IDHEXMobApp.ViewModels
{
    [QueryProperty(nameof(Romaneio), nameof(Romaneio))]
    public partial class NotaViewModel : BaseViewModel
    {
        private int _currentPage = 0;
        private const int PageSize = 20; // ajuste conforme necessário
        private List<PedidoResponse> _todosPedidos = new();
        private bool _isLoadingMore = false;
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

            try
            {
                // Carrega todos os pedidos de uma vez só
                var todosPedidos = _databaseRepository.GetAll().ToList();

                if (!string.IsNullOrEmpty(NumRomaneio))
                {
                    // Filtra por romaneio se informado
                    var pedidos = todosPedidos
                        .Where(p => p.NumRomaneio == NumRomaneio && p.Baixado == "NÃO")
                        .ToList();

                    PedidosFiltrados.Clear();
                    foreach (var pedido in pedidos)
                        PedidosFiltrados.Add(pedido);
                }
                else
                {
                    // Se não houver romaneio, mostra todos não baixados
                    var pedidos = todosPedidos
                        .Where(p => p.Baixado == "NÃO")
                        .ToList();

                    PedidosFiltrados.Clear();
                    foreach (var pedido in pedidos)
                        PedidosFiltrados.Add(pedido);
                }

                Pedidos = new ObservableCollection<PedidoResponse>(PedidosFiltrados);

                OnPropertyChanged(nameof(PedidosFiltrados));

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Atenção", $"Erro: {ex.Message} ", "OK");
            }

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

            try
            {
                if (NumRomaneio != null)
                {
                    var pedidos = _databaseRepository.GetPedidosByNumRomaneioAsync(NumRomaneio!).Where(p => p.Baixado == "NÃO");
                    PedidosFiltrados = pedidos.ToObservableCollection<PedidoResponse>();
                }

                Pedidos = new ObservableCollection<PedidoResponse>(PedidosFiltrados);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Atenção", $"Erro: {ex.Message} ", "OK");
            }

            IsBusy = false;

            await Task.CompletedTask;
        }

        public void AtualizarFiltroAsync()
        {

            IsBusy = true;

            try
            {

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

            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Atenção", $"Erro: {ex.Message} ", "OK");
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

        [RelayCommand]
        public async Task CarregarMaisPedidosAsync()
        {
            if (_isLoadingMore || !_hasMoreData)
                return;

            _isLoadingMore = true;

            var nextPage = _todosPedidos
                .Skip(_currentPage * PageSize)
                .Take(PageSize)
                .ToList();

            foreach (var pedido in nextPage)
                PedidosFiltrados.Add(pedido);

            _currentPage++;

            if (nextPage.Count < PageSize)
                _hasMoreData = false;

            _isLoadingMore = false;
        }
    }
}
