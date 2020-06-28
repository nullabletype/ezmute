using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ezmute
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration BaseConfiguration { get; private set; }
        public Classes.Configuration Configuration { get; private set; }

        protected void AppStart(object sender, StartupEventArgs e)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("config.json", optional: false, reloadOnChange: true);

            BaseConfiguration = builder.Build();
            Configuration = new Classes.Configuration();

            BaseConfiguration.GetSection("config").Bind(Configuration);

            var window = new MainWindow(Configuration);
            window.Show();
        }
    }
}
