using System;
using System.ComponentModel;

namespace LLMS.Dto
{
    public class PropertyDto : IDataErrorInfo
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int NumberOfUnits { get; set; }
        public string PropertyType { get; set; }
        public int SizeInSqFt { get; set; }
        public DateTime YearBuilt { get; set; }
        public decimal RentalPrice { get; set; }
        public string Amenities { get; set; }
        public int Status { get; set; }
        public string LeaseTerms { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }

        // IDataErrorInfo Members
        public string Error => string.Empty; 

        public string this[string propertyName]
        {
            get
            {
                string error = string.Empty;
                switch (propertyName)
                {
                    case nameof(NumberOfUnits):
                        if (NumberOfUnits < 1)
                            error = "Number of units must be greater than 0.";
                        break;
                    case nameof(Address):
                        if (string.IsNullOrWhiteSpace(Address))
                            error = "Address is required.";
                        break;
                    case nameof(PropertyType):
                        if (string.IsNullOrWhiteSpace(PropertyType))
                            error = "Property type is required.";
                        break;
                    case nameof(SizeInSqFt):
                        if (SizeInSqFt <= 0)
                            error = "Size must be greater than 0 square feet.";
                        break;
                    case nameof(YearBuilt):
                        if (YearBuilt.Year < 1800 || YearBuilt.Year > DateTime.Now.Year)
                            error = $"Year built must be between 1800 and {DateTime.Now.Year}.";
                        break;
                    case nameof(RentalPrice):
                        if (RentalPrice <= 0)
                            error = "Rental price must be greater than 0.";
                        break;
                    case nameof(LeaseTerms):
                        if (string.IsNullOrWhiteSpace(LeaseTerms))
                            error = "Lease terms are required.";
                        break;
                    case nameof(ImageUrl):
                        if (string.IsNullOrWhiteSpace(ImageUrl))
                            error = "Image URL is required.";
                        break;
                }

                return error;
            }
        }
    }
}
