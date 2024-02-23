using LLMS.Service;
using System.Windows;

namespace LLMS.View
{
    /// <summary>
    /// PropertyView.xaml 的交互逻辑
    /// </summary>
    public partial class PropertyView : Window
    {
        public PropertyView(IPropertyService propertyService, IImageService imageService)
        {
            InitializeComponent();
            this.DataContext = new PropertyViewModel(propertyService, imageService);
        }
    }
}
