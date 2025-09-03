using CommunityToolkit.Maui.Core.Extensions;
using IDHEXMobApp.Models.Response;
using IDHEXMobApp.Repositories;
using IDHEXMobApp.Repositories.Database;

namespace IDHEXMobApp.ViewModels
{

    public partial class NotaCleanViewModel : BaseViewModel
    {

        [ObservableProperty]
        string filtroPesquisa;

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

        public async Task CarregaRomaneiosAsync()
        {
            //IsBusy = true;

            //if (NumRomaneio != null)
            //{
            //    var pedidos = _databaseRepository.GetPedidosByNumRomaneioAsync(NumRomaneio!).Where(p => p.Baixado == "NÃO");
            //    PedidosFiltrados = pedidos.ToObservableCollection<PedidoResponse>();
            //}

            //Pedidos = new ObservableCollection<PedidoResponse>(PedidosFiltrados);

            //IsBusy = false;

            //await Task.CompletedTask;
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
