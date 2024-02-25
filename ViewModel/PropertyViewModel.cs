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
    private readonly IPropertyService _propertyService;
    private readonly IImageService _imageService;
    private PropertyDto _selectedProperty;
    private string _statusMessage;

    public ObservableCollection<PropertyDto> Properties { get; private set; } = new ObservableCollection<PropertyDto>();
    public PropertyDto SelectedProperty
    {
        get => _selectedProperty;
        set => SetProperty(ref _selectedProperty, value, OnSelectedPropertyChanged);
    }
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public DelegateCommand UploadImageCommand { get; private set; }
    public DelegateCommand SavePropertyCommand { get; private set; }
    public DelegateCommand DeletePropertyCommand { get; private set; }

    public PropertyViewModel(IPropertyService propertyService, IImageService imageService)
    {
        _propertyService = propertyService ?? throw new ArgumentNullException(nameof(propertyService));
        _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));

        UploadImageCommand = new DelegateCommand(ExecuteUploadImageAsync, CanExecuteUploadImage);
        SavePropertyCommand = new DelegateCommand(ExecuteSaveProperty, CanExecuteSaveProperty);
        DeletePropertyCommand = new DelegateCommand(ExecuteDeleteProperty, CanExecuteDeleteProperty);

        Properties.Add(new PropertyDto { Address = "Add new property..." });
        LoadPropertiesAsync();
    }

    private bool CanExecuteUploadImage() => SelectedProperty != null && !string.IsNullOrEmpty(SelectedProperty.ImageUrl);

    private async void ExecuteUploadImageAsync()
    {
        var openFileDialog = new OpenFileDialog();
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

    private bool CanExecuteSaveProperty() => SelectedProperty != null && (_isAddingNewProperty || SelectedProperty.Id > 0);

    private async void ExecuteSaveProperty()
    {
        if (_isAddingNewProperty || SelectedProperty.Id == 0)
        {
            var createdProperty = await _propertyService.CreatePropertyAsync(SelectedProperty);
            if (createdProperty != null)
            {
                Properties.Add(createdProperty);
                StatusMessage = "Property added successfully.";
            }
        }
        else
        {
            var updatedProperty = await _propertyService.UpdatePropertyAsync(SelectedProperty);
            var index = Properties.IndexOf(SelectedProperty);
            if (index != -1)
            {
                Properties[index] = updatedProperty;
                RaisePropertyChanged(nameof(Properties));
                StatusMessage = "Property updated successfully.";
            }
        }
    }

    private bool CanExecuteDeleteProperty() => SelectedProperty != null && SelectedProperty.Id > 0;

    private async void ExecuteDeleteProperty()
    {
        var result = await _propertyService.DeletePropertyAsync(SelectedProperty.Id);
        if (result)
        {
            Properties.Remove(SelectedProperty);
            SelectedProperty = null;
            StatusMessage = "Property deleted successfully.";
        }
    }

    private async void LoadPropertiesAsync()
    {
        var properties = await _propertyService.GetAllPropertiesAsync();
        Properties.Clear();
        Properties.Add(new PropertyDto { Address = "Add new property..." });
        foreach (var property in properties)
        {
            Properties.Add(property);
        }
    }

    private void OnSelectedPropertyChanged()
    {
        if (SelectedProperty?.Address == "Add new property...")
        {
            SelectedProperty = new PropertyDto();
            _isAddingNewProperty = true;
        }
        else
        {
            _isAddingNewProperty = false;
        }

        UploadImageCommand.RaiseCanExecuteChanged();
        SavePropertyCommand.RaiseCanExecuteChanged();
        DeletePropertyCommand.RaiseCanExecuteChanged();
    }
}

