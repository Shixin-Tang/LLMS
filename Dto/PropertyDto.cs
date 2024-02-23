using System;

namespace LLMS.Dto
{
    public class PropertyDto
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
        public string ImageUrl { get; set; } // only url is needed
        public string Description { get; set; }

        // others
    }
}

