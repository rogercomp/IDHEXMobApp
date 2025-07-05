namespace IDHEXMobApp.Views;

public partial class CameraPage : ContentPage
{
    private CameraViewModel _viewModel;   
    

    public CameraPage(CameraViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = _viewModel = viewModel;
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

                // Optionally, display the captured image
                // var image = new Image { Source = ImageSource.FromFile(localFilePath) };
                ImagePreview.Source = ImageSource.FromFile(localFilePath);                
                //var result = File.ReadAllBytes(localFilePath);
                //_imageBase64 = Convert.ToBase64String(result);
                ////Preferences.Set("srcImg", imageBase64);
                SalvarBT.IsEnabled = true;
            }
        }
        else
        {
            await DisplayAlert("Error", "Photo capture is not supported on this device.", "OK");
        }
    }
}