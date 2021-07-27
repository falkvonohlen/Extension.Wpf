using Extension.Wpf.UserControls.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf.Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var config = new ConfigurationBuilder()
                .Build();

            var serviceCollection = new ServiceCollection()
                .AddLogging(loggingBuilder =>
                {
                    // configure Logging with NLog
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                    loggingBuilder.AddProvider(
                    new UserControlLoggerProvider(
                        new UserControlLoggerConfiguration
                        {
                            TargetName = "Log1",
                            MinLogLevel = LogLevel.Information,
                        }));
                    loggingBuilder.AddProvider(
                    new UserControlLoggerProvider(
                        new UserControlLoggerConfiguration
                        {
                            TargetName = "Log2",
                            MinLogLevel = LogLevel.Trace,
                        }));
                })
                .AddScoped<MainWindow>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<MainWindow>().Show();
        }
    }
}
