namespace IDHEXMobApp.Helpers.Converters
{
    public class Base64StringToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var base64 = (string)value;
            return base64 != null ? ImageSource.FromStream(() => new MemoryStream(System.Convert.FromBase64String(base64!))) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
