using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LLMS.Model
{
    internal class Lease : INotifyPropertyChanged
    {
    private int _id;
    private string _address;
    private string _tenant;
    private DateTime _expiryDate;

    public int Id
    {
        get { return _id; }
        set
        {
            _id = value;
            OnPropertyChanged(nameof(Id));
        }
    }

    public string Address
    {
        get { return _address; }
        set
        {
            _address = value;
            OnPropertyChanged(nameof(Address));
        }
    }

    public string Tenant
    {
        get { return _tenant; }
        set
        {
            _tenant = value;
            OnPropertyChanged(nameof(Tenant));
        }
    }

    public DateTime ExpiryDate
    {
        get { return _expiryDate; }
        set
        {
            _expiryDate = value;
            OnPropertyChanged(nameof(ExpiryDate));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void AddLease()
    {
        // Add logic to add lease to database or data source
        // For example, you can use Entity Framework or any other ORM
    }

    public void UpdateLease()
    {
        // Add logic to update lease in the database or data source
    }

    public void DeleteLease()
    {
        // Add logic to delete lease from the database or data source
    }
}
}
  