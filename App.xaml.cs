using LLMS.Service;
using LLMS.View;
using System.Windows;
using Unity;

namespace LLMS
{
    public partial class App : Application
    {
        private IUnityContainer _container = new UnityContainer();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
                var mainWindow = _container.Resolve<MainWindow>();
                mainWindow.Show();
        }

        private void ConfigureContainer()
        {
            _container.RegisterType<IImageService, ImageService>();
            _container.RegisterType<IPropertyService, PropertyService>();
            _container.RegisterType<PropertyView>();
            _container.RegisterType<MainWindow>();

        }
    }
}
