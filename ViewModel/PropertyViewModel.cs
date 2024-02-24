using LLMS.Dto;
using LLMS.Service;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Win32;
using System.Windows;

public class PropertyViewModel : BindableBase
{
    /*-- fields --*/
    // Dependency Injection
    private readonly IPropertyService _propertyService;
    private readonly IImageService _imageService;

    // Property List
    public ObservableCollection<PropertyDto> Properties { get; private set; } = new ObservableCollection<PropertyDto>();

    // Property Detail
    private PropertyDto _selectedProperty;
    public PropertyDto SelectedProperty
    {
        get => _selectedProperty;
        set => SetProperty(ref _selectedProperty, value, OnSelectedPropertyChanged);
    }

    // Status Message
    private string _statusMessage;
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    // CRUD Commands 
    public DelegateCommand UploadImageCommand { get; private set; }
    public DelegateCommand CreatePropertyCommand { get; private set; }
    public DelegateCommand UpdatePropertyCommand { get; private set; }
    public DelegateCommand DeletePropertyCommand { get; private set; }


    /*-- Constructor --*/
    public PropertyViewModel(IPropertyService propertyService, IImageService imageService)
    {
        _propertyService = propertyService ?? throw new ArgumentNullException(nameof(propertyService));
        _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));

        UploadImageCommand = new DelegateCommand(ExecuteUploadImageAsync, CanExecuteUploadImage)
            .ObservesProperty(() => SelectedProperty);
        UpdatePropertyCommand = new DelegateCommand(ExecuteUpdateProperty, CanExecuteUpdateProperty)
            .ObservesProperty(() => SelectedProperty);
        DeletePropertyCommand = new DelegateCommand(ExecuteDeleteProperty, CanExecuteDeleteProperty)
            .ObservesProperty(() => SelectedProperty);
        CreatePropertyCommand = new DelegateCommand(ExecuteCreateProperty, CanExecuteCreateProperty);
        LoadPropertiesAsync();
    }

    /*-- Upload Image --*/
    private bool CanExecuteUploadImage()
    {
        return SelectedProperty != null && !string.IsNullOrEmpty(SelectedProperty.ImageUrl);
    }

    private async void ExecuteUploadImageAsync()
    {
        try
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                using (var stream = File.OpenRead(openFileDialog.FileName))
                {
                    var imageUrl = await _imageService.UploadImageAsync(stream, Path.GetFileName(openFileDialog.FileName));
                    SelectedProperty.ImageUrl = imageUrl;
                    RaisePropertyChanged(nameof(SelectedProperty));
                    StatusMessage = "Image uploaded successfully.";
                }
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to upload image: {ex.Message}";
        }
    }


    public async void HandleFileDrop(string filePath)
    {
        try
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var fileName = Path.GetFileName(filePath);
                var imageUrl = await _imageService.UploadImageAsync(stream, fileName);
                if (SelectedProperty != null)
                {
                    SelectedProperty.ImageUrl = imageUrl;
                    RaisePropertyChanged(nameof(SelectedProperty));
                    StatusMessage = "Image uploaded successfully from drag and drop.";
                }
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to upload image from drag and drop: {ex.Message}";
        }
    }


    /*-- CRUD Implements --*/
    private bool CanExecuteCreateProperty() => true; 

    private async void ExecuteCreateProperty()
    {
        try
        {
            var newProperty = new PropertyDto(); 

            var createdProperty = await _propertyService.CreatePropertyAsync(newProperty);
            if (createdProperty != null)
            {
                Properties.Add(createdProperty);
                StatusMessage = "Property created successfully.";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to create property: {ex.Message}";
        }
    }


    private bool CanExecuteUpdateProperty() => SelectedProperty != null;

    private async void ExecuteUpdateProperty()
    {
        try
        {
            if (SelectedProperty == null) return;

            var updatedProperty = await _propertyService.UpdatePropertyAsync(SelectedProperty);
            var index = Properties.IndexOf(SelectedProperty);
            if (index != -1)
            {
                Properties[index] = updatedProperty;
                RaisePropertyChanged(nameof(Properties));
            }
            StatusMessage = "Property updated successfully.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to update property: {ex.Message}";
        }
    }

    private bool CanExecuteDeleteProperty() => SelectedProperty != null;

    private async void ExecuteDeleteProperty()
    {
        try
        {
            if (SelectedProperty == null) return;

            var result = await _propertyService.DeletePropertyAsync(SelectedProperty.Id);
            if (result)
            {
                Properties.Remove(SelectedProperty);
                SelectedProperty = null; // 或选择列表中的另一个项
                StatusMessage = "Property deleted successfully.";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to delete property: {ex.Message}";
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

    /*-- Helper --*/
    private void OnSelectedPropertyChanged()
    {
        UploadImageCommand.RaiseCanExecuteChanged();
    }

}
