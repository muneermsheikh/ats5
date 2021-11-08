using System;
using System.ComponentModel.DataAnnotations;
using core.Entities.Admin;

namespace core.ParamsAndDtos
{
    public class UserContactedItemAddDto
    {
        [Required]
        public int CandidateId { get; set; }
        [Required]
        public int LoggedInUserId { get; set; }
        [Required]
        public DateTime DateOfContact { get; set; }=DateTime.Now;
        public int OrderItemId {get; set;}
        public string Subject {get; set;}
        [Required]
        public EnumContactResult enumContactResult { get; set; }
        public string GistOfDiscussions { get; set; }
        public DateTime NextReminderOn { get; set;} 
    }
}