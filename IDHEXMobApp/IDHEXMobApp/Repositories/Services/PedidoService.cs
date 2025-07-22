using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using IDHEXMobApp.Helpers.Uteis;
using IDHEXMobApp.Models.Response;
using IDHEXMobApp.Repositories.Database;

namespace IDHEXMobApp.Repositories.Services
{
    public class PedidoService: IDisposable //: BackgroundService
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
            
            if (Conexao.CheckConnectivity())
            {
                IEnumerable<PedidoResponse> pedidos = _databaseRepository.GetAll().Where(p => p.Baixado == "SIM" && p.Enviado == "NÃO" && p.ImgCanhoto is not null && p.CodOcorrencia is not null);

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

                    //// Corrigido: aguardar o Task para obter o Stream
                    //var resourceStreamTask = FileSystem.OpenAppPackageFileAsync("idhexmob-bfc45a0f4340.json");
                    //resourceStreamTask.Wait();
                    //var resourceStream = resourceStreamTask.Result;

                    //var credential = GoogleCredential.FromStream(resourceStream);

                    //var bucketName = "idheximages";
                    //var objectName = $"{Guid.NewGuid()}.jpg";
                    //using var storageClient = StorageClient.Create(credential);

                    //FileStream imageStream = File.OpenRead(item.ImgCanhoto!);

                    //storageClient.UploadObject(bucketName, objectName, "image/jpeg", imageStream);

                    //item.ImgCanhoto = $"{objectName}";

                    bool ok = _pedidoRepository.AtualizaPedidoAsync(item.PedidoId, item.EmpresaId, item.CodOcorrencia!, item.ImgCanhoto!).GetAwaiter().GetResult();
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
        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        public void Dispose()
        {
            _timer?.Dispose();            
        }
    }
}
    /*public class PedidoService : BackgroundService
    {
        readonly ILogger<PedidoService> _logger;

        public PedidoService(ILogger<PedidoService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }/*
        //private readonly IDatabaseRepository _databaseRepository;
        //private readonly IPedidoRepository _pedidoRepository;
        //private Timer _timer = null;

        //public PedidoService(IDatabaseRepository databaseRepository, IPedidoRepository pedidoRepository)
        //{
        //    // Registre o serviço de background
        //    Debug.WriteLine("PedidoService CONSTRUTOR chamado!");
        //    _databaseRepository = databaseRepository;
        //    _pedidoRepository = pedidoRepository;
        //}

        //public Task StartAsync(CancellationToken cancellationToken)
        //{
        //    _timer = new Timer(ActionToBePerformed, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        //    // Sua lógica de background aqui
        //    Debug.WriteLine("Serviço de background rodando: " + DateTime.Now);

        //    if (Conexao.CheckConnectivity())
        //    {
        //        var pedidos = _databaseRepository.GetAll().Where(p => p.Baixado == "NÃO" && p.ImgCanhoto is not null && p.CodOcorrencia is not null);
        //        foreach (var item in pedidos)
        //        {
        //            bool ok = _pedidoRepository.AtualizaPedidoAsync(item.PedidoId, item.EmpresaId, DateTime.Now.ToString("dd/MM/yyyy"), item.CodOcorrencia!, item.ImgCanhoto!).GetAwaiter().GetResult();
        //            if (ok)
        //                _databaseRepository.Delete(item);
        //        }
        //    }
        //    Debug.WriteLine("Serviço de background finalizado: " + DateTime.Now);

        //    return Task.CompletedTask;
        //}

        //void ActionToBePerformed(object state)
        //{
        //    Debug.WriteLine("Working behind the scenes...");
        //}

        //public Task StopAsync(CancellationToken cancellingToken)
        //{
        //    _timer?.Change(0, 2000);
        //    return Task.CompletedTask;
        //}
    }
}*/

    