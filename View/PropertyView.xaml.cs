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


        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            var vm = DataContext as PropertyViewModel;
            if (vm != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                // Assuming the first file is the one we're interested in
                vm.HandleFileDrop(files[0]);
            }
        }
    }
}
