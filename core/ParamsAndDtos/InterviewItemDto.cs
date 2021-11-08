using System;
using System.Collections.Generic;

namespace core.ParamsAndDtos
{
    public class InterviewItemDto
    {
        public InterviewItemDto()
        {
        }

        public InterviewItemDto(int categoryId, string categoryName, DateTime interviewDate, int applicationNo, 
            string candidateName, string passportNo, string attendanceStatus    //, DateTime attendedOn
            )
        {
            //InterviewId = interviewId;
            CategoryId = categoryId;
            CategoryName = categoryName;
            InterviewDate = interviewDate;
            ApplicationNo = applicationNo;
            CandidateName = candidateName;
            PassportNo = passportNo;
            AttendanceStatus = attendanceStatus;
            //AttendedOn = attendedOn;
        }

        public int InterviewId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName {get; set;}
        public DateTime InterviewDate { get; set; }
        public int ApplicationNo {get; set;}
        public string CandidateName {get; set;}
        public string PassportNo {get; set;}
        public string AttendanceStatus {get; set;}
        //public DateTime AttendedOn {get; set;}
        //public ICollection<InterviewItemCandidateDto> InterviewItemCandidates {get; set;}
    }
}