using IDHEXMobApp.Helpers;
using IDHEXMobApp.Repositories;
using IDHEXMobApp.Repositories.Database;
using LiteDB;

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
			})
            .RegisterDatabaseAndRepositories()
            .RegisterViews().RegisterViewModels();
#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
	}

    public static MauiAppBuilder RegisterDatabaseAndRepositories(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<LiteDatabase>(
            options =>
            {
                return new LiteDatabase($"Filename={AppSettings.DatabasePath};Connection=Shared");
            }
        );
        
        mauiAppBuilder.Services.AddTransient<ILoginRepository, LoginRepository>();
        mauiAppBuilder.Services.AddTransient<IPedidoRepository, PedidoRepository>();
        mauiAppBuilder.Services.AddTransient<IDatabaseRepository, DatabaseRepository>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<MainPage>();
        mauiAppBuilder.Services.AddTransient<LoginPage>();
        mauiAppBuilder.Services.AddTransient<PedidosPage>();
        
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {

        mauiAppBuilder.Services.AddTransient<PrincipalViewModel>();
        mauiAppBuilder.Services.AddTransient<MainViewModel>();
        mauiAppBuilder.Services.AddTransient<LoginViewModel>();
        mauiAppBuilder.Services.AddTransient<PedidosViewModel>();        

        return mauiAppBuilder;
    }
}
