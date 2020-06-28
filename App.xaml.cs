using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;

namespace ezmute
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration BaseConfiguration { get; private set; }
        public Tools.Configuration Configuration { get; private set; }

        protected void AppStart(object sender, StartupEventArgs e)
        {
            string configFile = "config.json";

            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "custom-config.json")))
            {
                configFile = "custom-config.json";
            }

            IConfigurationBuilder builder = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile(configFile, optional: false, reloadOnChange: true);

            BaseConfiguration = builder.Build();
            Configuration = new Tools.Configuration();

            BaseConfiguration.GetSection("config").Bind(Configuration);

            var window = new MainWindow(Configuration);
            window.Show();
        }
    }
}
