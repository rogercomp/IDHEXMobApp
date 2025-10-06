using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using IDHEXMobApp.Helpers.Uteis;
using IDHEXMobApp.Models.Response;
using IDHEXMobApp.Repositories.Database;

namespace IDHEXMobApp.Repositories.Services
{
    public class PedidoService : IDisposable //: BackgroundService
    {
        private readonly IDatabaseRepository _databaseRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private Timer _timer = null;
        public PedidoService(IDatabaseRepository databaseRepository, IPedidoRepository pedidoRepository)
        {
            //_timer = new Timer(Timer)
            System.Diagnostics.Debug.WriteLine("PedidoService CONSTRUTOR chamado!");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
            Task.Delay(Timeout.Infinite, new CancellationToken());
            _databaseRepository = databaseRepository;
            _pedidoRepository = pedidoRepository;
        }

        private async void DoWork(object? state)
        {
            try
            {
                if (Conexao.CheckConnectivity())
                {
                    List<PedidoResponse> pedidos = _databaseRepository.GetAll().Where(p => p.ImgCanhoto is not null && p.CodOcorrencia is not null).ToList();

                    foreach (var item in pedidos)
                    {
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

                        if (!String.IsNullOrEmpty(item.ImgCanhoto))
                            imageStream = File.OpenRead(item.ImgCanhoto!);

                        var objectName = $"{Guid.NewGuid()}.jpg";
                        await storageClient.UploadObjectAsync(bucketName, objectName, "image/jpeg", imageStream);

                        item.ImgCanhoto = $"{objectName}";                        

                        bool ok = _pedidoRepository.AtualizaPedidoAsync(item.PedidoId, item.EmpresaId, item.CodOcorrencia!, item.ImgCanhoto!, item.DtImgCanhoto!.Value.ToString("yyyy-MM-dd HH:mm:ss")).GetAwaiter().GetResult();
                        if (ok)
                            _databaseRepository.DeleteById(item.Id);
                    }
                    Preferences.Set("UltimaExecucaoPedidoService", DateTime.Now);

                }
                else
                {
                    Preferences.Set("UltimaExecucaoPedidoService", DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PedidoService Erro: {ex.Message}");
            }

        }
        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
