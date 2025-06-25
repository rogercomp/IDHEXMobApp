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
                var pedidos = _databaseRepository.GetPedidosByNumRomaneioAsync(NumRomaneio!);
                Pedidos = pedidos.ToObservableCollection<PedidoResponse>();
            }
            else
                Pedidos = _databaseRepository.GetAll().ToObservableCollection<PedidoResponse>();                        

            OnPropertyChanged(nameof(Pedidos));

            IsBusy = false;

            await Task.CompletedTask;
        }
    }
}
