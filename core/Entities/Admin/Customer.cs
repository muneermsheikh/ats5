using System.Collections.Generic;

namespace core.Entities
{
    public class Customer: BaseEntity
    {
        public string CustomerType { get; set; }
        public string CustomerName { get; set; }
        public string KnownAs { get; set; }
        public string Add { get; set; }
        public string Add2 { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public ICollection<CustomerIndustry> CustomerIndustries { get; set; }
        public ICollection<CustomerOfficial> CustomerOfficials { get; set; }

    }
}