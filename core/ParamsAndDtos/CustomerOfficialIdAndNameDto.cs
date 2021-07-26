namespace core.ParamsAndDtos
{
    public class CustomerOfficialIdAndNameDto
    {
        public CustomerOfficialIdAndNameDto()
        {
        }

        public CustomerOfficialIdAndNameDto(int id, int customerId, string officialName)
        {
            Id = id;
            CustomerId = customerId;
            OfficialName = officialName;
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName {get; set;}
        public string OfficialName { get; set; }
    }
}