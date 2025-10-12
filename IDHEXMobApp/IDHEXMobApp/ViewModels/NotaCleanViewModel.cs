using IDHEXMobApp.Models.Response;
using IDHEXMobApp.Repositories;
using IDHEXMobApp.Repositories.Database;

namespace IDHEXMobApp.ViewModels
{

    public partial class NotaCleanViewModel : BaseViewModel
    {

        [ObservableProperty]
        string? filtroPesquisa;

        private readonly IDatabaseRepository _databaseRepository;
        public ObservableCollection<PedidoResponse> Pedidos { get; set; } = new ObservableCollection<PedidoResponse>();
        public ObservableCollection<PedidoResponse> PedidosFiltrados { get; set; } = new ObservableCollection<PedidoResponse>();
        public NotaCleanViewModel(IPedidoRepository pedidoRepository, IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;

            //AtualizarFiltroAsync();
        }
        internal async Task InitiAsync()
        {
            //IsBusy = true;
            //PedidosFiltrados = (ObservableCollection<PedidoResponse>)_databaseRepository.GetAll().Where(p => p.Baixado == "NÃO");
            //OnPropertyChanged(nameof(PedidosFiltrados));
            //IsBusy = false;

        }


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


        public void AtualizarFiltroAsync()
        {
            IsBusy = true;

            var termo = FiltroPesquisa?.ToLower() ?? "";

            var filtrado = _databaseRepository.GetPedidosByNumNotaAsync(termo);

            if (filtrado != null)
                PedidosFiltrados.Add(filtrado);
            else
                PedidosFiltrados.Clear();

            IsBusy = false;
        }
    }
}
