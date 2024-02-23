using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LLMS;

namespace LLMS.ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);
    }

    public class LeaseWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private testdb1Entities _db = new testdb1Entities();

        private string _propertyId;
        public string PropertyId
        {
            get { return _propertyId; }
            set
            {
                _propertyId = value;
                OnPropertyChanged(nameof(PropertyId));
            }
        }

        private string _tenantId;
        public string TenantId
        {
            get { return _tenantId; }
            set
            {
                _tenantId = value;
                OnPropertyChanged(nameof(TenantId));
            }
        }

        private string _startDate;
        public string StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private string _endDate;
        public string EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        private string _rentAmount;
        public string RentAmount
        {
            get { return _rentAmount; }
            set
            {
                _rentAmount = value;
                OnPropertyChanged(nameof(RentAmount));
            }
        }

        private string _leaseClauses;
        public string LeaseClauses
        {
            get { return _leaseClauses; }
            set
            {
                _leaseClauses = value;
                OnPropertyChanged(nameof(LeaseClauses));
            }
        }

        private string _paymentDueDay;
        public string PaymentDueDay
        {
            get { return _paymentDueDay; }
            set
            {
                _paymentDueDay = value;
                OnPropertyChanged(nameof(PaymentDueDay));
            }
        }

        private string _utilityByOwner;
        public string UtilityByOwner
        {
            get { return _utilityByOwner; }
            set
            {
                _utilityByOwner = value;
                OnPropertyChanged(nameof(UtilityByOwner));
            }
        }

        private string _utilityByTenant;
        public string UtilityByTenant
        {
            get { return _utilityByTenant; }
            set
            {
                _utilityByTenant = value;
                OnPropertyChanged(nameof(UtilityByTenant));
            }
        }

        private string _renewalTerm;
        public string RenewalTerm
        {
            get { return _renewalTerm; }
            set
            {
                _renewalTerm = value;
                OnPropertyChanged(nameof(RenewalTerm));
            }
        }

        private string _earlyTerminateCon;
        public string EarlyTerminateCon
        {
            get { return _earlyTerminateCon; }
            set
            {
                _earlyTerminateCon = value;
                OnPropertyChanged(nameof(EarlyTerminateCon));
            }
        }

        public LeaseWindowViewModel()
        {
            AddCommand = new RelayCommand(Add, CanAdd);
            UpdateCommand = new RelayCommand(Update, CanUpdate);
            DeleteCommand = new RelayCommand(Delete, CanDelete);

            LoadLeaseData();
        }

        private void LoadLeaseData()
        {
            Leases = new ObservableCollection<leas>(_db.leases.ToList());
        }

        private ObservableCollection<leas> _leases;
        public ObservableCollection<leas> Leases
        {
            get { return _leases; }
            set
            {
                _leases = value;
                OnPropertyChanged(nameof(Leases));
            }
        }

        private leas _selectedLease;
        public leas SelectedLease
        {
            get { return _selectedLease; }
            set
            {
                _selectedLease = value;
                OnPropertyChanged(nameof(SelectedLease));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        private bool CanAdd(object parameter) => true; // Implement your validation logic here

        private void Add(object parameter)
        {
            string commandParameter = parameter as string;
            // Now you can use the commandParameter value as needed
            try
            {

     

                // Create a new lease object
                leas newLease = new leas
                {
                    // Populate properties from the UI
                    property_id = 0, // Set to appropriate value
                    tenant_id = 0, // Set to appropriate value
                    start_date = DateTime.Now, // Set to appropriate value
                    end_date = DateTime.Now, // Set to appropriate value
                    rent_amount = 0, // Set to appropriate value
                    created_at = DateTime.Now,
                    lease_clauses = "Sample clauses", // Set to appropriate value
                    payment_due_day = 1, // Set to appropriate value
                    utility_by_owner = "Sample utility", // Set to appropriate value
                    utility_by_tenant = "Sample tenant utility", // Set to appropriate value
                    renewal_term = "Sample renewal term", // Set to appropriate value
                    early_terminate_con = "Sample termination condition" // Set to appropriate value
                    // Populate other properties as needed
                };

                // Add the new lease to the database context
                _db.leases.Add(newLease);

                // Save changes to the database
                _db.SaveChanges();

                // Reload lease data
                LoadLeaseData();

                MessageBox.Show("Lease added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding lease: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanUpdate(object parameter) => SelectedLease != null && SelectedLease.id > 0; // Implement your validation logic here
        
        private void Update(object parameter)
        {
            string commandParameter = parameter as string;

            try
            {
                if (SelectedLease != null)
                {
                    // Update properties of the selected lease

                    // Save changes to the database
                    _db.SaveChanges();

                    // Reload lease data
                    LoadLeaseData();

                    MessageBox.Show("Lease updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Please select a lease to update.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating lease: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanDelete(object parameter) => SelectedLease != null && SelectedLease.id > 0; // Implement your validation logic here
        
        private void Delete(object parameter)
        {
            string commandParameter = parameter as string;
            try
            {
                if (SelectedLease != null)
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this lease?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        // Remove the selected lease from the database context
                        _db.leases.Remove(SelectedLease);

                        // Save changes to the database
                        _db.SaveChanges();

                        // Reload lease data
                        LoadLeaseData();

                        MessageBox.Show("Lease deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a lease to delete.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting lease: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
