namespace core.Entities.Users
{
    public class EmployeePhone: BaseEntity
    {
          public EmployeePhone()
          {
          }

          public EmployeePhone(string phoneNo, string mobileNo, bool isMain)
          {
               PhoneNo = phoneNo;
               MobileNo = mobileNo;
               IsMain = isMain;
          }

          public int EmployeeId { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public bool IsMain {get; set;}=false;
        public bool IsValid { get; set; }=true;
    }
}