using dotenv.net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace LLMS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // get the complete path of the .exe file
            string exePath = Assembly.GetExecutingAssembly().Location;

            // get the directory of the .exe file
            string exeDirectory = Path.GetDirectoryName(exePath);

            //MessageBox.Show($"exeDirectory: {exeDirectory}", "exe file path", MessageBoxButton.OK, MessageBoxImage.Information);

            // suppose the .env file is in the two lever parent directory of the .exe file
            string envFilePath = Path.Combine(exeDirectory, "..", "..", ".env");

            // convert the relative path to the absolute path
            string envFileAbsolutePath = Path.GetFullPath(envFilePath);

            //MessageBox.Show($"envFileAbsolutePath: {envFileAbsolutePath}", ".env file path", MessageBoxButton.OK, MessageBoxImage.Information);

            // use the absolute path to load the .env file
            DotEnv.Load(new DotEnvOptions(envFilePaths: new[] { envFileAbsolutePath }));

            // get the environment variable, copy the following code to the place where you want to use the environment variable
            //string blobconnectionString = Environment.GetEnvironmentVariable("ConnectionString");
            //string containerName = Environment.GetEnvironmentVariable("NameContainer");

            // use message box to display the environment variable
            //MessageBox.Show($"BlobConnectionString: {blobconnectionString}", "Environment Variable", MessageBoxButton.OK, MessageBoxImage.Information);
            //MessageBox.Show($"containerName: {containerName}", "Environment Variable", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}
