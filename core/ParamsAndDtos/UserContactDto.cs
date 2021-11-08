using System;
using core.Entities.Admin;

namespace core.ParamsAndDtos
{
    public class UserContactDto
    {
          public UserContactDto()
          {
          }

          public UserContactDto(int id, string candidateName, int applicationNo, int orderItemId, string orderRef, 
            string orderItemCategory, string subject, DateTime dateOfContact, string loggedInUserName, 
            string userPhoneNoContacted, string contactResult, string gistOfDiscussions, DateTime nextReminderOn)
          {
               Id = id;
               CandidateName = candidateName;
               ApplicationNo = applicationNo;
               OrderItemId = orderItemId;
               OrderRef = orderRef;
               OrderItemCategory = orderItemCategory;
               Subject = subject;
               DateOfContact = dateOfContact;
               LoggedInUserName = loggedInUserName;
               UserPhoneNoContacted = userPhoneNoContacted;
               ContactResult = contactResult;
               GistOfDiscussions = gistOfDiscussions;
               NextReminderOn = nextReminderOn;
          }

        public int Id {get; set;}
        public string CandidateName { get; set; }
        public int ApplicationNo {get; set;}
        public int OrderItemId {get; set;}
        public string OrderRef {get; set;}
        public string OrderItemCategory {get; set;}
        public string Subject {get; set;}
        public DateTime DateOfContact { get; set; }
        public string LoggedInUserName { get; set; }
        public string UserPhoneNoContacted {get; set;}
        public string ContactResult { get; set; }
        public string GistOfDiscussions { get; set; }
        public DateTime NextReminderOn { get; set; }
    }
}