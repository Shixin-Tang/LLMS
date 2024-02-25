using LLMS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
