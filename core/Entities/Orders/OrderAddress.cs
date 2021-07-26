using System.ComponentModel.DataAnnotations;

namespace core.Entities.Orders
{
    public class OrderAddress
    {
          public OrderAddress()
          {
          }

          public OrderAddress(string companyName, string add, string streetAdd, string location, string city, string district, 
            string state, string pin, string country)
          {
               CompanyName = companyName;
               Add = add;
               StreetAdd = streetAdd;
               Location = location;
               City = city;
               District = district;
               State = state;
               Pin = pin;
               Country = country;
          }

          public OrderAddress(string companyName, string add, string streetAdd, string location, string city, string district, 
            string state, string pin, string country, int orderId, Order order)
          {
               CompanyName = companyName;
               Add = add;
               StreetAdd = streetAdd;
               Location = location;
               City = city;
               District = district;
               State = state;
               Pin = pin;
               Country = country;
               OrderId = orderId;
               Order = order;
          }

          public string CompanyName { get; set; }
        public string Add { get; set; }
        public string StreetAdd { get; set; }
        public string Location { get; set; }
        [Required]
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Pin { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set;}
    }
}