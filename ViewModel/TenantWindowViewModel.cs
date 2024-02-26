using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LLMS;

namespace LLMS.ViewModel
{
    public class TenantWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private testdb1Entities db;

        public TenantWindowViewModel()
        {
            try
            {
                db = new testdb1Entities();
                AddCommand = new RelayCommand(Add);
                UpdateCommand = new RelayCommand(Update, CanUpdate);
                DeleteCommand = new RelayCommand(Delete, CanDelete);

                LoadTenantData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tenant data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadTenantData()
        {
            Tenants = new ObservableCollection<tenant>(db.tenants.ToList());
        }

        private ObservableCollection<tenant> _tenants;
        public ObservableCollection<tenant> Tenants
        {
            get { return _tenants; }
            set
            {
                _tenants = value;
                OnPropertyChanged(nameof(Tenants));
            }
        }

        private tenant _selectedTenant;
        public tenant SelectedTenant
        {
            get { return _selectedTenant; }
            set
            {
                _selectedTenant = value;
                OnPropertyChanged(nameof(SelectedTenant));

                // Update text box bindings when a new item is selected
                OnPropertyChanged(nameof(Id));
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(FirstName));
                OnPropertyChanged(nameof(LastName));
                OnPropertyChanged(nameof(StreetNumber));
                OnPropertyChanged(nameof(StreetName));
                OnPropertyChanged(nameof(CityName));
                OnPropertyChanged(nameof(Postcode));
                OnPropertyChanged(nameof(Province));
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public string Id
        {
            get { return SelectedTenant?.id.ToString(); }
            set
            {
                if (SelectedTenant != null && int.TryParse(value, out int result))
                {
                    SelectedTenant.id = result;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string Email
        {
            get { return SelectedTenant?.email; }
            set
            {
                if (SelectedTenant != null)
                {
                    SelectedTenant.email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string FirstName
        {
            get { return SelectedTenant?.first_name; }
            set
            {
                if (SelectedTenant != null)
                {
                    SelectedTenant.first_name = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        public string LastName
        {
            get { return SelectedTenant?.last_name; }
            set
            {
                if (SelectedTenant != null)
                {
                    SelectedTenant.last_name = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        public string StreetNumber
        {
            get { return SelectedTenant?.street_number; }
            set
            {
                if (SelectedTenant != null)
                {
                    SelectedTenant.street_number = value;
                    OnPropertyChanged(nameof(StreetNumber));
                }
            }
        }

        public string StreetName
        {
            get { return SelectedTenant?.street_name; }
            set
            {
                if (SelectedTenant != null)
                {
                    SelectedTenant.street_name = value;
                    OnPropertyChanged(nameof(StreetName));
                }
            }
        }

        public string CityName
        {
            get { return SelectedTenant?.city_name; }
            set
            {
                if (SelectedTenant != null)
                {
                    SelectedTenant.city_name = value;
                    OnPropertyChanged(nameof(CityName));
                }
            }
        }

        public string Postcode
        {
            get { return SelectedTenant?.postcode; }
            set
            {
                if (SelectedTenant != null)
                {
                    SelectedTenant.postcode = value;
                    OnPropertyChanged(nameof(Postcode));
                }
            }
        }

        public string Province
        {
            get { return SelectedTenant?.province; }
            set
            {
                if (SelectedTenant != null)
                {
                    SelectedTenant.province = value;
                    OnPropertyChanged(nameof(Province));
                }
            }
        }

        public string PhoneNumber
        {
            get { return SelectedTenant?.phone_number; }
            set
            {
                if (SelectedTenant != null)
                {
                    SelectedTenant.phone_number = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        private bool CanUpdate(object parameter) => SelectedTenant != null;

        private void Add(object parameter)
        {
            try
            {
                tenant newTenant = new tenant
                {
                    email = Email,
                    first_name = FirstName,
                    last_name = LastName,
                    street_number = StreetNumber,
                    street_name = StreetName,
                    city_name = CityName,
                    postcode = Postcode,
                    province = Province,
                    phone_number = PhoneNumber
                };

                db.tenants.Add(newTenant);
                db.SaveChanges();

                LoadTenantData();
                MessageBox.Show("Tenant added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding tenant: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Update(object parameter)
        {
            try
            {
                db.SaveChanges();
                LoadTenantData();
                MessageBox.Show("Tenant updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating tenant: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanDelete(object parameter) => SelectedTenant != null;

        private void Delete(object parameter)
        {
            try
            {
                db.tenants.Remove(SelectedTenant);
                db.SaveChanges();
                LoadTenantData();
                MessageBox.Show("Tenant deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting tenant: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
