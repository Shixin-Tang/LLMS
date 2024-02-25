using LLMS.Service;
using LLMS.View;
using System;
using System.Windows;
using Unity;
using Unity.Injection;
using dotenv.net;
using System.Reflection;
using System.IO;

namespace LLMS
{
    public partial class App : Application
    {
        private IUnityContainer _container = new UnityContainer();
        private string blobconnectionString;

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
            blobconnectionString = Environment.GetEnvironmentVariable("ConnectionString");
            //string containerName = Environment.GetEnvironmentVariable("NameContainer");


            _container = new UnityContainer();
            ConfigureContainer();
            
            var mainWindow = _container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureContainer()
        {
            //var connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            var connectionString = blobconnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Azure Storage connection String is not set.");
            }

            _container.RegisterType<IAzureBlobStorageClient, AzureBlobStorageClient>(
                new InjectionConstructor(connectionString));

            _container.RegisterType<IImageService, ImageService>();
            _container.RegisterType<IPropertyService, PropertyService>();
            _container.RegisterType<PropertyView>();
            _container.RegisterType<MainWindow>();
        }
    }

}

