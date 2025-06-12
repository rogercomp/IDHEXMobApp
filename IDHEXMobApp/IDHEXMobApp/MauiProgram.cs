using IDHEXMobApp.Repositories.Login;
using IDHEXMobApp.Repositories.Pedido;

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
        //ViewModels
        builder.Services.AddTransient<PrincipalViewModel>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<PedidosViewModel>();

        //Views
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<PedidosPage>();

        //Repositories
        builder.Services.AddScoped<ILoginRepository, LoginRepository>();
		builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();


        return builder.Build();
	}
}
