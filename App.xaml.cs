using LLMS.Service;
using LLMS.View;
using System;
using System.Windows;
using Unity;
using Unity.Injection;

namespace LLMS
{
    public partial class App : Application
    {
        private IUnityContainer _container = new UnityContainer();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _container = new UnityContainer();
            ConfigureContainer();
            
            var mainWindow = _container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureContainer()
        {
            var connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
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

