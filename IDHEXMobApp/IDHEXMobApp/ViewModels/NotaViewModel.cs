using CommunityToolkit.Maui.Core.Extensions;
using IDHEXMobApp.Models.Response;
using IDHEXMobApp.Repositories.Database;

namespace IDHEXMobApp.ViewModels
{
    [QueryProperty(nameof(Romaneio), nameof(Romaneio))]
    public partial class NotaViewModel: BaseViewModel
    {
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
        

        private readonly IDatabaseRepository _databaseRepository;        
        public ObservableCollection<PedidoResponse> Pedidos { get; set; } = new ObservableCollection<PedidoResponse>();        
        public NotaViewModel(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;            
        }
        
        internal async Task InitiAsync()
        {
            IsBusy = true;

            await Task.Delay(1000);

            if(NumRomaneio != null)
            {
                var pedidos = _databaseRepository.GetPedidosByNumRomaneioAsync(NumRomaneio!).Where(p => p.Baixado == "NÃO");
                Pedidos = pedidos.ToObservableCollection<PedidoResponse>();
            }
            else
                Pedidos = _databaseRepository.GetAll().Where(p=> p.Baixado == "NÃO").ToObservableCollection<PedidoResponse>();                        

            OnPropertyChanged(nameof(Pedidos));

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
    }
}
