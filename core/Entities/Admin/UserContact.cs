using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using core.Entities.Users;

namespace core.Entities.Admin
{
     public class UserContact: BaseEntity
    {
        public UserContact()
        {
        }
        public UserContact(int candidateId, DateTime? dateOfContact, string userPhNoContacted, 
            int loggedInUserId, EnumContactResult thisEnumContactResult, string gistOfDisc, 
            DateTime nextReminderOn, int orderitemId, string subject, int loggedinuserid)
        {
            CandidateId = candidateId;
            DateOfContact = Convert.ToDateTime(dateOfContact);
            LoggedInUserId = loggedInUserId;
            enumContactResult = thisEnumContactResult;
            GistOfDiscussions = gistOfDisc;
            NextReminderOn = nextReminderOn;
            UserPhoneNoContacted = userPhNoContacted;
            OrderItemId = orderitemId;
            Subject = subject;
            LoggedInUserId = loggedinuserid;
        }

        [Required]
        public int CandidateId { get; set; }
        public int OrderItemId {get; set;}
        public int OrderId { get; set; }
        public string Subject {get; set;}
        [Required]
        public DateTime DateOfContact { get; set; }=DateTime.Now;
        [Required]
        public int LoggedInUserId { get; set; }
        [Required, MinLength(10), MaxLength(15)]
        public string UserPhoneNoContacted {get; set;}
        [Required]
        public EnumContactResult enumContactResult { get; set; }
        public string GistOfDiscussions { get; set; }
        public DateTime NextReminderOn { get; set; }
        [ForeignKey("CandidateId")]
        public ICollection<Candidate> Candidates {get; set;}
    }
}