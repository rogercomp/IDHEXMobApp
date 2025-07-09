using IDHEXMobApp.Models.Ocorrencia;

namespace IDHEXMobApp.Views;

public partial class CameraPage : ContentPage
{
    private CameraViewModel _viewModel;
    public List<TipoOcorrencia> TipoOcorrencias { get; set; }

    public CameraPage(CameraViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = _viewModel = viewModel;
        CarregaTipoOcorrencia();
        PopulatePicker();
    }

    private void CarregaTipoOcorrencia()
    {
        TipoOcorrencias = new List<TipoOcorrencia> 
        {
            new TipoOcorrencia { OcorrenciaId = "01", Descricao = "ENTREGA NORMAL" },
            new TipoOcorrencia { OcorrenciaId = "78", Descricao = "AVARIA" },
            new TipoOcorrencia { OcorrenciaId = "80", Descricao = "EXTRAVIO" },
            new TipoOcorrencia { OcorrenciaId = "46", Descricao = "RECEBEDOR AUSENTE" },
            new TipoOcorrencia { OcorrenciaId = "DEV", Descricao = "DEVOLUÇÃO" },
            new TipoOcorrencia { OcorrenciaId = "06", Descricao = "ENDEREÇO NÃO LOCALIZADO" },
            new TipoOcorrencia { OcorrenciaId = "ENP", Descricao = "ENTREGA PARCIAL" },
            new TipoOcorrencia { OcorrenciaId = "COR", Descricao = "COMPROVANTES RETIDO" },
            new TipoOcorrencia { OcorrenciaId = "20", Descricao = "NÃO ENTREGUE PELO HORARIO" },
            new TipoOcorrencia { OcorrenciaId = "21", Descricao = "ESTABELECIMENTO FECHADO" },
            new TipoOcorrencia { OcorrenciaId = "88", Descricao = "DESTINATARIO RECUSA RECEBER" },
            new TipoOcorrencia { OcorrenciaId = "33", Descricao = "FALTA DE VOLUME" }                       
        };
    }

    private void PopulatePicker()
    {
        foreach (var ocorrencia in TipoOcorrencias)
        {
            EventPicker.Items.Add(ocorrencia.Descricao);
        }

        EventPicker.SelectedIndex = 0;
        _viewModel.CodOcorrencia = TipoOcorrencias[EventPicker.SelectedIndex].OcorrenciaId;
    }

    private void OnEventIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            var selectedOcorrencia = TipoOcorrencias[selectedIndex];
            string ocorrenciaId = selectedOcorrencia.OcorrenciaId;
            _viewModel.CodOcorrencia = ocorrenciaId;
            //DisplayAlert("Ocorrência Selecioanda", $"Id: {ocorrenciaId}, Descrição: {selectedOcorrencia.Descricao}", "OK");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitiAsync();        
    }

    private async void OnTakePhotoClicked(object sender, EventArgs e)
    {
        //var options = new StoreCameraMediaOptions { CompressionQuality = selectedCompressionQuality };
        //var result = await CrossMedia.Current.TakePhotoAsync(options);
        //if (result is null) return;

        //UploadedOrSelectedImage.Source = result?.Path;

        //var fileInfo = new FileInfo(result?.Path);
        //var fileLength = fileInfo.Length;

        //FileSizeLabel.Text = $"Image size: {fileLength / 1000} kB";
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                // Save the file
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                using (var stream = await photo.OpenReadAsync())
                {
                    using (var fileStream = File.OpenWrite(localFilePath))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }               
                
                ImagePreview.Source = ImageSource.FromFile(localFilePath);                
                var result = File.ReadAllBytes(localFilePath);
                _viewModel.ImgCanhoto = Convert.ToBase64String(result);                
                SalvarBT.IsEnabled = true;
            }
        }
        else
        {
            await DisplayAlert("Erro", "Foto capturada não é suportada nesse disopositivo", "OK");
        }
    }

    public static ImageSource Base64ToImageSource(string base64)
    {
        if (string.IsNullOrEmpty(base64))
            return null;

        byte[] imageBytes = Convert.FromBase64String(base64);
        return ImageSource.FromStream(() => new MemoryStream(imageBytes));
    }
}