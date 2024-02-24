using LLMS.Dto;
using LLMS.Service;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

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

        Properties = new ObservableCollection<PropertyDto>();

        UploadImageCommand = new DelegateCommand(ExecuteUploadImage, CanExecuteUploadImage);

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
        catch (Exception e)
        {
            // 在这里处理异常，例如记录日志或显示错误消息
            Debug.WriteLine($"load property load error: {e.Message}");
        }
    }

}
