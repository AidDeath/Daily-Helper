using Daily_Helper.Models;
using Daily_Helper.Services;
using Daily_Helper.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace Daily_Helper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public readonly IHost AppHost;
        public new static App Current => (App)Application.Current;

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
           .ConfigureServices(ConfigureServices)
           .Build();

        }

        //Configuring services with IHost 
        private static void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<MainWindow>();

            //Here we add services


            //Here we add database context


            //Add hosted setvice and variable provider
            services.AddSingleton<RoutineTestsProvider>();
            services.AddHostedService<BackgroundHostedService>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            AppHost.Start();
            var mainWindow = AppHost.Services.GetService<MainWindow>();
            mainWindow?.Show();
        }
    }
}
