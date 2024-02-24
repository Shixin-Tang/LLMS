using LLMS.Service;
using LLMS.View;
using LLMS.ViewModel;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Unity;

namespace LLMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Define LeaseWindow object
        //private LeaseWindow leaseWindow;


        // Define AzureDbContext object
        private testdb1Entities db = new testdb1Entities();
        private readonly IUnityContainer _container;

        public MainWindow(IUnityContainer container)
        {
            try
            {
                InitializeComponent();
                _container = container;
                this.Loaded += Window_Loaded;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during MainWindow initialization: {ex.Message}");
            }
        }

        private void PropertyDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var propertyView = _container.Resolve<PropertyView>();
                propertyView.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //load all lease information from the database
            LoadMainWindow();
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

        /*private void OpenLeaseWindow_Click(object sender, RoutedEventArgs e)
        {
            // Hide main window content
            this.Visibility = Visibility.Collapsed;
            // Show lease window
            leaseWindow.ShowDialog();
            // Show main window content again when the lease window is closed
            this.Visibility = Visibility.Visible;
        }*/

        /*private void OpenTenantWindow_Click(object sender, RoutedEventArgs e)
        {
            // Handle opening Tenant window
        }*/

       /* private void OpenPropertyView_Click(object sender, RoutedEventArgs e)
        {

        }
       */


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listView.SelectedItem is MainWindowViewModel selectedLease)
            {
                // update right panel with selected lease information
                UpdateRightPanel(selectedLease);
            }
        }

        private void LeaseDetail_Click(object sender, RoutedEventArgs e)
        {
            // Handle Lease Detail button click
            LeaseWindow leaseWindow = new LeaseWindow();
            leaseWindow.ShowDialog();
        }

        private void TenantDetail_Click(object sender, RoutedEventArgs e)
        {
            // Handle Tenant Detail button click
            //TenantWindow tenantWindow = new TenantWindow();
            //tenantWindow.ShowDialog();
        }

        private void LoadMainWindow()
        {
            var MainWindowViewModels = db.leases.Select(lease => new MainWindowViewModel
            {
                LeaseId = lease.id,
                PaymentDueDay = lease.payment_due_day,
                RentAmount = lease.rent_amount,
                Address = lease.property.address,
                TenantName = lease.tenant.first_name + " " + (lease.tenant.last_name ?? ""), // tenantname is first name + last name
                PhoneNo = lease.tenant.phone_number,
                Email = lease.tenant.email,
                ImageUrl = lease.property.image.image_url, // image_url: url of the image store in Azure blob Storage
                EmergencyContactName = lease.tenant.emergency_contact_name,
                EmergencyContactNo = lease.tenant.emergency_contact_number
            }).ToList();

            listView.ItemsSource = MainWindowViewModels;

            // default select the first item in the list
            if (listView.Items.Count > 0)
            {
                listView.SelectedItem = MainWindowViewModels[0];
            }
        }

        private void UpdateRightPanel(MainWindowViewModel lease)
        {
            lblAddress.Content = lease.Address;
            lblTenantName.Content = lease.TenantName;
            lblPhoneNo.Content = lease.PhoneNo;
            lblEmail.Content = lease.Email;
            lblEmergencyContactName.Content = lease.EmergencyContactName;
            lblEmergencyContactNo.Content = lease.EmergencyContactNo;
            //update image URL
            imageDisplay.Source = new BitmapImage(new Uri(lease.ImageUrl, UriKind.Absolute));
        }

    }
}