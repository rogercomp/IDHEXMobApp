using IDHEXMobApp.Models.Response;
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
                    CodOcorrencia = value.CodOcorrencia;
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

        private readonly IDatabaseRepository _databaseRepository;
        public PedidoResponse Pedidos { get; set; } = new PedidoResponse();
        public CameraViewModel(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        internal async Task InitiAsync()
        {
            IsBusy = true;

            await Task.Delay(1000);

            if (NumNotaFiscal != null)
            {
                var pedido = _databaseRepository.GetPedidosByRomaneioNotaPedidoEmpresaAsync(NumRomaneio!, decimal.Parse(NumNotaFiscal), long.Parse(PedidoId), long.Parse(EmpresaId));
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
                    decimal.Parse(NumNotaFiscal),
                    DateTime.Now,
                    CodOcorrencia,
                    ImgCanhoto
                );

            _databaseRepository.Update(pedidoResponse);

            //ProductId,
            //Descricao,
            //Estoque.Value,
            //Barcode,
            //Preco.Value
            //);

            //if (pedido != null)
            //{
            //pedido.ImgCanhoto = pedido.ImgCanhoto;
            //pedido.DtImgCanhoto = DateTime.Now;
            //pedido.CodOcorrencia = CodOcorrencia;
            //_databaseRepository.Update(pedido);
            //await Shell.Current.GoToAsync("..");
            //}
            //else
            //{
            //  await Shell.Current.DisplayAlert("Atenção", "Pedido não encontrado.", "OK");
            //}

            await Task.CompletedTask;
        }

        public static ImageSource Base64ToImageSource(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                return null;

            byte[] imageBytes = Convert.FromBase64String(base64);
            return ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }
    }
}
