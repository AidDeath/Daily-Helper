using Daily_Helper.Models;
using Daily_Helper.Services;
using Daily_Helper.Views;
using Daily_Helper.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Threading.Tasks;
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
            services.AddScoped<RoutinesSaveLoadService>();


            //Here we add database context


            //Add hosted setvice and variable provider
            services.AddSingleton<RoutineTestsProvider>();
            services.AddSingleton<SettingsSingleton>();
            services.AddHostedService<BackgroundHostedService>();
            services.AddDbContext<DailyHelperDbContext>();
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            AppHost.Start();
            var mainWindow = AppHost.Services.GetService<MainWindow>();

            var saveLoadService = AppHost.Services.GetService<RoutinesSaveLoadService>();
            if (saveLoadService is not null)
            {
                var loadedRoutines = await saveLoadService.LoadOnStartUp();
                if (loadedRoutines is not null && loadedRoutines.Count > 0)
                {
                    var routines = AppHost.Services.GetService<RoutineTestsProvider>()?.Routines;
                    try
                    {
                        foreach (var routine in loadedRoutines)
                            if (routine?.Type is not null && routine.JsonString is not null)
                            {
                                routines.Add((RoutineBase)JsonSerializer.Deserialize(routine.JsonString, routine.Type));
                            }
                    }
                    catch
                    {
                        //await DialogHost.Show(MaterialMessageBox.Create($"Ошибка импорта: {ex.GetBaseException().Message}", MessageType.Error), "MaterialMessageBox");
                        MessageBox.Show("При загрузке списка заданий произошла ошибка.\nЗагружены не все задания.", "Ошибка импорта");
                    }

                }

            }
                

            mainWindow?.Show();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            var saveLoadService = AppHost.Services.GetService<RoutinesSaveLoadService>();

            if (saveLoadService is not null)
                try
                {
                    await saveLoadService.SaveFileOnExit();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.GetBaseException().Message);
                }
        }
    }
}
