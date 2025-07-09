using CommunityToolkit.Maui.Core.Extensions;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using IDHEXMobApp.Helpers.Uteis;
using IDHEXMobApp.Models.Response;
using IDHEXMobApp.Repositories;
using IDHEXMobApp.Repositories.Database;

namespace IDHEXMobApp.ViewModels
{
    public partial class PedidosBaixaViewModel : BaseViewModel
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IDatabaseRepository _databaseRepository;

        [ObservableProperty]
        private string pendentes = "Pendentes Envio: 0";
        [ObservableProperty]
        private string _numRomaneio = string.Empty;
        public ObservableCollection<PedidoResponse> Pedidos { get; set; } = new ObservableCollection<PedidoResponse>();

        public PedidosBaixaViewModel(IPedidoRepository pedidoRepository, IDatabaseRepository databaseRepository)
        {
            _pedidoRepository = pedidoRepository;
            _databaseRepository = databaseRepository;
        }

        internal async Task InitiAsync()
        {
            IsBusy = true;
            
            int contador = 0;
            Pedidos =  _databaseRepository.GetAll().Where(p=> p.Baixado == "SIM").ToObservableCollection<PedidoResponse>();

            int Total = Pedidos.Count;

            if (Total > 0)
            {
                contador++;
                await Task.Delay(1000);
                Pendentes = $"Pendentes Envio: {Total}";            
            }

            OnPropertyChanged(nameof(Pedidos));

            IsBusy = false;         
        }

        [RelayCommand]
        public async Task GoToBaixarAsync()
        {
            IsBusy = true;

            if (Conexao.CheckConnectivity())
            {
                //var appRoot = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.LastIndexOf("\\bin"));
                //string jsonPath = appRoot + "\\GoogleCred\\idhexmob-bfc45a0f4340.json";

                //var appRoot = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.LastIndexOf("\\bin"));
                //string jsonPath = appRoot + "\\GoogleCred\\idhexmob-bfc45a0f4340.json";
                //var credential = GoogleCredential.FromFile(jsonPath);
                //var bucketName = "idheximages";
                //using var storageClient = StorageClient.Create(credential);
                                
                string credentialsFileName = "idhexmob-bfc45a0f4340.json";
                string localPath = Path.Combine(FileSystem.CacheDirectory, credentialsFileName);                              

                if (!File.Exists(localPath))
                {
                    using var json = await FileSystem.OpenAppPackageFileAsync(credentialsFileName);
                    using var dest = File.Create(localPath);
                    await json.CopyToAsync(dest);
                }

                var credential = GoogleCredential.FromFile(localPath);
                using var storageClient = StorageClient.Create(credential);

                FileStream imageStream = null!;
                var bucketName = "idheximages";

                Pedidos = _databaseRepository.GetAll().Where(p => p.Baixado == "SIM" && p.Enviado == "NÃO").ToObservableCollection<PedidoResponse>();
                foreach (var item in Pedidos)
                {
                    if (!String.IsNullOrEmpty(item.ImgCanhoto))
                        imageStream = File.OpenRead(item.ImgCanhoto!);

                    var objectName = $"{Guid.NewGuid()}.jpg";                    
                    await storageClient.UploadObjectAsync(bucketName, objectName, "image/jpeg", imageStream);

                    item.ImgCanhoto = $"{objectName}";
                    bool ok = await _pedidoRepository.AtualizaPedidoAsync(item.PedidoId, item.EmpresaId, item.CodOcorrencia!, item.ImgCanhoto!);
                    if (ok)
                        _databaseRepository.DeleteById(item.Id);
                }
            }
            await Task.Delay(2000);
            IsBusy = false;
        }
    }
}
