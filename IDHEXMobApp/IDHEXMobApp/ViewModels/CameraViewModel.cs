using IDHEXMobApp.Models.Response;
using IDHEXMobApp.Repositories;
using IDHEXMobApp.Repositories.Database;



namespace IDHEXMobApp.ViewModels
{
    [QueryProperty(nameof(Pedido), nameof(Pedido))]
    public partial class CameraViewModel : BaseViewModel
    {
        private PedidoResponse _pedido;
        public PedidoResponse Pedido
        {
            get => _pedido;
            set
            {
                SetProperty(ref _pedido, value);

                if (value != null)
                {
                    PedidoId = value.PedidoId.ToString();
                    EmpresaId = value.EmpresaId.ToString();
                    NumRomaneio = value.NumRomaneio;
                    NumNotaFiscal = value.NumNotaFiscal.ToString();                   
                    //DataPrevisaoSaida = value.DataPrevisaoSaida;                    
                    ImgCanhoto = value.ImgCanhoto;
                    //ImgCanhoto = value.ImgCanhoto != null ? ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(value.ImgCanhoto))) : null; 
                    //ScreenGrab = value.ImgCanhoto != null ? ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(value.ImgCanhoto))) : null;
                }
            }
        }


        [ObservableProperty]
        string pedidoId;

        [ObservableProperty]
        string empresaId;

        [ObservableProperty]
        string numRomaneio;

        [ObservableProperty]
        string codOcorrencia;

        [ObservableProperty]
        string numNotaFiscal;

        [ObservableProperty]
        public DateTime? dtImgCanhoto;

        [ObservableProperty]
        string? imgCanhoto;

        private readonly IPedidoRepository _pedidoRepository;
        private readonly IDatabaseRepository _databaseRepository;
        public PedidoResponse Pedidos { get; set; } = new PedidoResponse();
        public CameraViewModel(IDatabaseRepository databaseRepository, IPedidoRepository pedidoRepository)
        {
            _databaseRepository = databaseRepository;
            _pedidoRepository = pedidoRepository;
        }

        internal async Task InitiAsync()
        {
            IsBusy = true;

            await Task.Delay(1000);

            if (NumNotaFiscal != null)
            {
                var pedido = _databaseRepository.GetPedidosByRomaneioNotaPedidoEmpresaAsync(NumRomaneio!, long.Parse(NumNotaFiscal), long.Parse(PedidoId), long.Parse(EmpresaId));
                Pedidos = (PedidoResponse)pedido;
                                
            }

            await Task.CompletedTask;

            IsBusy = false;
        }

        [RelayCommand]
        public async Task SalvarAsync()
        {

            var pedidoResponse = new PedidoResponse
                (
                    long.Parse(PedidoId),
                    long.Parse(EmpresaId),
                    NumRomaneio,
                    long.Parse(NumNotaFiscal),
                    DateTime.Now,
                    CodOcorrencia,
                    ImgCanhoto
                );

            _databaseRepository.Update(pedidoResponse);

            RomaneioResponse romaneio = new RomaneioResponse
            {
                NumRomaneio = NumRomaneio,
                DataPrevisaoSaida = DateTime.Now
            };

            var navigationParams = new Dictionary<string, object>
            {
                {"Romaneio", romaneio }
            };

            await Shell.Current.GoToAsync(nameof(NotasPage), navigationParams);            
        }

        public static ImageSource? Base64ToImageSource(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                return null;

            byte[] imageBytes = Convert.FromBase64String(base64);
            return ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }
    }
}
