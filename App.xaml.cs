using LLMS.Service;
using System.Windows;
using Unity;

namespace LLMS
{
    public partial class App : Application
    {
        private IUnityContainer _container;

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
            _container.RegisterType<IPropertyService, PropertyService>();
            _container.RegisterType<IImageService, ImageService>();

            // 注册 MainWindow，以便能够通过容器解析
            _container.RegisterType<MainWindow>();
        }
    }
}

