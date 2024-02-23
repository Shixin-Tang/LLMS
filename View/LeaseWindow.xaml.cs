using LLMS.ViewModel;
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
    /// Interaction logic for LeaseWindow.xaml
    /// </summary>
    public partial class LeaseWindow : Window
    {
        private testdb1Entities db = new testdb1Entities();
        private LeaseWindowViewModel _viewModel;

        public LeaseWindow()
        {
            InitializeComponent();
            _viewModel = new LeaseWindowViewModel();
            this.DataContext = _viewModel;




        }

     
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Handle Save button click
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            // Handle Exit button click
            Close();
        }

        private void OpenLeaseWindow_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void OpenTenantWindow_Click(object sender, RoutedEventArgs e)
        {
            // Handle opening Tenant window
        }

        private void OpenPropertyWindow_Click(object sender, RoutedEventArgs e)
        {
            // Handle opening Property window
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        
     

    }
}