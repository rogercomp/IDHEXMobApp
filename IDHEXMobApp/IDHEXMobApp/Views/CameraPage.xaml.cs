
namespace IDHEXMobApp.Views;

public partial class CameraPage : ContentPage
{
    private int selectedCompressionQuality;
    public CameraPage()
	{
		InitializeComponent();
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

;            }
        }
        else
        {
            await DisplayAlert("Error", "Photo capture is not supported on this device.", "OK");
        }
    }
}