using System;
using System.ComponentModel.DataAnnotations;

namespace core.Entities.Admin
{
    public class UserContactedItem: BaseEntity
    {
        public UserContactedItem()
        {
        }

        public UserContactedItem(int candidateId, DateTime? dateOfContact, int loggedInUserId, 
            EnumContactResult thisEnumContactResult, string gistOfDisc, DateTime nextReminderOn)
        {
            CandidateId = candidateId;
            DateOfContact = Convert.ToDateTime(dateOfContact);
            LoggedInUserId = loggedInUserId;
            enumContactResult = thisEnumContactResult;
            GistOfDiscussions = gistOfDisc;
            NextReminderOn = nextReminderOn;
        }

        [Required]
        public int CandidateId { get; set; }
        public int OrderItemId {get; set;}
        public string Subject {get; set;}
        [Required]
        public DateTime DateOfContact { get; set; }=DateTime.Now;
        [Required]
        public int LoggedInUserId { get; set; }
        [Required]
        public EnumContactResult enumContactResult { get; set; }
        public string GistOfDiscussions { get; set; }
        public DateTime NextReminderOn { get; set; }

    }
}