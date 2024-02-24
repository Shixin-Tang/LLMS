using LLMS.Dto;
using LLMS.Service;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

public class PropertyViewModel : BindableBase
{
    private readonly IPropertyService _propertyService;
    private readonly IImageService _imageService;

    public ObservableCollection<PropertyDto> Properties { get; private set; } = new ObservableCollection<PropertyDto>();

    private PropertyDto _selectedProperty;
    public PropertyDto SelectedProperty
    {
        get => _selectedProperty;
        set => SetProperty(ref _selectedProperty, value, OnSelectedPropertyChanged);
    }

    public DelegateCommand UploadImageCommand { get; private set; }

    public PropertyViewModel(IPropertyService propertyService, IImageService imageService)
    {
        _propertyService = propertyService ?? throw new ArgumentNullException(nameof(propertyService));
        _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));

        UploadImageCommand = new DelegateCommand(ExecuteUploadImageAsync, CanExecuteUploadImage).ObservesProperty(() => SelectedProperty);

        LoadPropertiesAsync();
    }


    private bool CanExecuteUploadImage()
    {
        return SelectedProperty != null && !string.IsNullOrEmpty(SelectedProperty.ImageUrl);
    }

    private async void ExecuteUploadImageAsync()
    {
        try
        {
            // 上传图片逻辑
            MessageBox.Show("Upload image feature is not implemented yet.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            // 注意: 实际上传逻辑需要用户交互来选择图片文件，这里只是简化示例
            await Task.CompletedTask; // 模拟异步操作
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to upload image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void LoadPropertiesAsync()
    {
        try
        {
            var properties = await _propertyService.GetAllPropertiesAsync();
            Properties.Clear();
            foreach (var property in properties)
            {
                Properties.Add(property);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load properties: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OnSelectedPropertyChanged()
    {
        UploadImageCommand.RaiseCanExecuteChanged();
    }
}
