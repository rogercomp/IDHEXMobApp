using IDHEXMobApp.Repositories.Login;

namespace IDHEXMobApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddTransient<MainViewModel>();

		builder.Services.AddTransient<BlankViewModel>();
		
        builder.Services.AddTransient<LoginViewModel>();

        builder.Services.AddTransient<LoginPage>();

		builder.Services.AddTransient<BlankPage>();

        builder.Services.AddScoped<ILoginRepository, LoginRepository>();

        

        return builder.Build();
	}
}
