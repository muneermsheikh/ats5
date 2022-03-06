using System;
using System.ComponentModel.DataAnnotations;

namespace core.Entities.Admin
{
    public class UserHistoryItem: BaseEntity
    {
        public UserHistoryItem()
        {
        }

        public UserHistoryItem(int userHistoryId, string phoneno, DateTime? dateOfContact, int loggedInUserId, 
            string subject, string categoryref, int contactResult, string gistOfDisc)
        {
            UserHistoryId = UserHistoryId;
            PhoneNo = phoneno;
            DateOfContact = Convert.ToDateTime(dateOfContact);
            LoggedInUserId = loggedInUserId;
            Subject = subject;
            CategoryRef = categoryref;
            ContactResult = contactResult;
            GistOfDiscussions = gistOfDisc;
        }

        public int UserHistoryId {get; set;}
        public string PhoneNo {get; set;} 
        public string Subject {get; set;}
        public string CategoryRef {get; set;}
        [Required]
        public DateTime DateOfContact { get; set; }=DateTime.Now;
        [Required]
        public int LoggedInUserId { get; set; }
        [Required]
        public int ContactResult { get; set; }
        public string GistOfDiscussions { get; set; }

    }
}