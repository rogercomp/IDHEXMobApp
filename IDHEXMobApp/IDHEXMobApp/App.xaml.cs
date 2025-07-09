
namespace IDHEXMobApp;

public partial class App : Application
{   
    public App()
	{
		InitializeComponent();         
    }

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

    //public async Task AgendarJobAsync()
    //{
    //    var jobInfo = new JobInfo("PedidoServiceJob", null!, true, null, InternetAccess.Any, false, false, true)
    //    {
    //        Parameters = new Dictionary<string, string>
    //        {
    //            { "Nome", "IDHEXMobApp" },
    //            { "Descricao", "Job de sincronização de pedidos" }
    //        },
    //        RunOnForeground = true            
    //    };

    //    _jobManager.Register(jobInfo);

    //    await Task.CompletedTask;
    //}
}
