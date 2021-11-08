using System;
using System.Collections.Generic;

namespace core.ParamsAndDtos
{
    public class InterviewDto
    {
        public InterviewDto()
        {
        }

        public InterviewDto(string companyName, string interviewVenue, int orderId, int orderNo, DateTime orderDate, ICollection<InterviewItemDto> interviewItemsDto)
        {
            CompanyName = companyName;
            InterviewVenue = interviewVenue;
            OrderId = orderId;
            OrderNo = orderNo;
            OrderDate = orderDate;
            InterviewItemsDto = interviewItemsDto;
        }

        public string CompanyName { get; set; }
        public string InterviewVenue { get; set; }
        public int OrderId {get; set;}
        public int OrderNo {get; set;}
        public DateTime OrderDate {get; set;}
        public ICollection<InterviewItemDto> InterviewItemsDto {get; set;}
    }
}