using LLMS;
using LLMS.Service;
using Unity;

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
        // 注册服务
        _container.RegisterType<IImageService, ImageService>();
        _container.RegisterType<IPropertyService, PropertyService>();

        // 注册视图
        // 如果PropertyView需要通过容器创建以注入服务，也应在这里注册
        _container.RegisterType<PropertyView>();
    }
}



