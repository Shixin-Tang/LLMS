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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LLMS.View;

namespace LLMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Define LeaseWindow object
        private LeaseWindow leaseWindow;

        public MainWindow()
        {
            InitializeComponent();
            // Initialize LeaseWindow object
            leaseWindow = new LeaseWindow();
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
            // Hide main window content
            this.Visibility = Visibility.Collapsed;
            // Show lease window
            leaseWindow.ShowDialog();
            // Show main window content again when the lease window is closed
            this.Visibility = Visibility.Visible;
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
            // Handle selection change in ListView
        }

        private void LeaseDetail_Click(object sender, RoutedEventArgs e)
        {
            // Handle Lease Detail button click
        }

        private void TenantDetail_Click(object sender, RoutedEventArgs e)
        {
            // Handle Tenant Detail button click
        }

        private void PropertyDetail_Click(object sender, RoutedEventArgs e)
        {
            // Handle Property Detail button click
        }
    }
}