using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductGradingTool.Data;
using ProductGradingTool.Data.Sqlite;
using ProductGradingTool.WpfClient.Services;
using ProductGradingTool.WpfClient.ViewModel;
using System.Windows;

namespace ProductGradingTool.WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices)
                .Build();
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            IConfiguration config = context.Configuration;
            string localConnectionString = config.GetConnectionString("Default");

            //-Windows.
            services.AddSingleton<MainWindow>();
            services.AddSingleton<DialogService>();

            //-ViewModels.
            services.AddSingleton<MainViewModel>();
            services.AddTransient<CommandViewModel>();
            services.AddTransient<ProductViewModel>();
            services.AddTransient<WeightCaptureViewModel>();
            services.AddTransient<GradeCaptureViewModel>();

            //-Services.
            services.AddSingleton<IProductHistory, ProductHistory>();
            services.AddSingleton<IProductScanBuilder, ProductScanBuilder>();
            services.AddTransient<IProductScanRepository>((provider) => new SqliteProductScanRepository(localConnectionString));
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();

            var window = _host.Services.GetRequiredService<MainWindow>();
            window.Show();
        }

        private async void Application_Exit(object sender, EventArgs e)
        {
            await _host.StopAsync();
        }

    }

}
