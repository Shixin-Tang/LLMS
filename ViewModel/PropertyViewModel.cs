using LLMS.Dto;
using LLMS.Service;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

public class PropertyViewModel : BindableBase
{
    private readonly IPropertyService _propertyService;
    private readonly IImageService _imageService;

    // BINDING PROPERTIES
    private ObservableCollection<PropertyDto> _properties;
    public ObservableCollection<PropertyDto> Properties
    {
        get => _properties;
        set => SetProperty(ref _properties, value);
    }

    private PropertyDto _selectedProperty;
    public PropertyDto SelectedProperty
    {
        get => _selectedProperty;
        set => SetProperty(ref _selectedProperty, value, () => UploadImageCommand.RaiseCanExecuteChanged());
    }

    // COMMANDS
    public DelegateCommand UploadImageCommand { get; private set; }

    public PropertyViewModel(IPropertyService propertyService, IImageService imageService)
    {
        _propertyService = propertyService;
        _imageService = imageService;

        UploadImageCommand = new DelegateCommand(ExecuteUploadImage, CanExecuteUploadImage);

        // 加载属性列表
        LoadPropertiesAsync();
    }

    private bool CanExecuteUploadImage()
    {
        return SelectedProperty != null;
    }

    private void ExecuteUploadImage()
    {
        // 调用上传图片的逻辑
        // 注意：实际的图片选择和上传逻辑将需要涉及UI交互，可能需要通过服务来实现或使用Messenger模式
    }

    // LoadProperties()
    private async void LoadPropertiesAsync()
    {
        try
        {
            var properties = await _propertyService.GetAllPropertiesAsync();
            Properties = new ObservableCollection<PropertyDto>(properties);
        }
        catch (System.Exception e)
        {
            // handle exception, e.g., log the error or show a message to the user
        }
    }
}
