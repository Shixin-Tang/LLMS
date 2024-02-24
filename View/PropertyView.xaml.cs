using LLMS.Service;
using System.Windows;
using System.Windows.Controls;

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

        private void Image_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                // Assuming the first file is the one we're interested in
                var viewModel = this.DataContext as PropertyViewModel;
                viewModel?.HandleFileDrop(files[0]);
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = this.DataContext as PropertyViewModel;
            viewModel?.OnSelectedPropertyChanged();
        }
    }
}
