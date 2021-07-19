namespace core.Entities
{
    public class CustomerOfficial: BaseEntity
    {
          public CustomerOfficial()
          {
          }

          public CustomerOfficial(int customerId, string gender, string title, 
            string officialName, string designation, string phoneNo, string mobile, string email, string imageUrl, bool isValid)
          {
               CustomerId = customerId;
               Gender = gender;
               Title = title;
               OfficialName = officialName;
               Designation = designation;
               PhoneNo = phoneNo;
               Mobile = mobile;
               Email = email;
               ImageUrl = imageUrl;
               IsValid = isValid;
          }

        public int CustomerId { get; set; }
        public string Gender { get; set; }="M";
        public string Title { get; set; }
        public string OfficialName { get; set; }
        public string Designation { get; set; }
        public string PhoneNo { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public bool IsValid { get; set; }
        //public Customer Customer {get; set;}
    }
}