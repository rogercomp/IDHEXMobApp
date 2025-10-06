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
        private int _segundosRestantes;
        private System.Timers.Timer? _cronometro;

        [ObservableProperty]
        private string tempoRestante = "10:00";
        [ObservableProperty]
        private string pendentes = "Pendentes Envio: 0";
        [ObservableProperty]
        private string _numRomaneio = string.Empty;
        public ObservableCollection<PedidoResponse> Pedidos { get; set; } = new ObservableCollection<PedidoResponse>();

        public PedidosBaixaViewModel(IPedidoRepository pedidoRepository, IDatabaseRepository databaseRepository)
        {
            _pedidoRepository = pedidoRepository;
            _databaseRepository = databaseRepository;
            IniciarCronometro();
        }

        internal async Task InitiAsync()
        {
            IsBusy = true;

            int contador = 0;
            Pedidos = _databaseRepository.GetAll().Where(p => p.Baixado == "SIM").ToObservableCollection<PedidoResponse>();

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

        private void IniciarCronometro()
        {
            var ultimaExecucao = Preferences.Get("UltimaExecucaoPedidoService", DateTime.Now);
            var proximaExecucao = ultimaExecucao.AddMinutes(10);
            var segundosRestantes = (int)(proximaExecucao - DateTime.Now).TotalSeconds;
            _segundosRestantes = Math.Max(0, segundosRestantes);
            AtualizarTempoRestante();

            _cronometro = new System.Timers.Timer(1000); // 1 segundo
            _cronometro.Elapsed += async (s, e) =>
            {
                if (_segundosRestantes > 0)
                {
                    _segundosRestantes--;
                    AtualizarTempoRestante();
                }
                else
                {
                    _cronometro?.Stop();
                    IniciarCronometro();
                    await InitiAsync();
                }
            };
            _cronometro.Start();
        }

        private void AtualizarTempoRestante()
        {
            var ts = TimeSpan.FromSeconds(_segundosRestantes);
            TempoRestante = ts.ToString(@"mm\:ss");
        }

        [RelayCommand]
        public async Task GoToBaixarAsync()
        {
            IsBusy = true;

            try
            {
                if (Conexao.CheckConnectivity())
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

                    Pedidos = _databaseRepository.GetAll().Where(p => p.Baixado == "SIM" && p.Enviado == "NÃO").ToObservableCollection<PedidoResponse>();
                    foreach (var item in Pedidos)
                    {
                        if (!String.IsNullOrEmpty(item.ImgCanhoto))
                            imageStream = File.OpenRead(item.ImgCanhoto!);

                        var objectName = $"{Guid.NewGuid()}.jpg";
                        await storageClient.UploadObjectAsync(bucketName, objectName, "image/jpeg", imageStream);

                        item.ImgCanhoto = $"{objectName}";
                        bool ok = await _pedidoRepository.AtualizaPedidoAsync(item.PedidoId, item.EmpresaId, item.CodOcorrencia!, item.ImgCanhoto!, item.DtImgCanhoto!.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (ok)
                            _databaseRepository.DeleteById(item.Id);
                    }
                }

                Pedidos = _databaseRepository.GetAll().Where(p => p.Baixado == "SIM" && p.Enviado == "NÃO").ToObservableCollection<PedidoResponse>();

                OnPropertyChanged(nameof(Pedidos));

            }
            catch (Exception ex )
            {
                await Shell.Current.DisplayAlert("Atenção", $"Erro: {ex.Message}", "OK");
            }

            IsBusy = false;
        }
    }
}
