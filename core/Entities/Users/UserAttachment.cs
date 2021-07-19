namespace core.Entities.Users
{
    public class UserAttachment: BaseEntity
    {
        public UserAttachment()
        {
        }

        public UserAttachment(string appUserId, string attachmentType, 
            int attachmentSizeInKB, string attachmentUrl)
        {
            AppUserId = appUserId;
            AttachmentType = attachmentType;
            AttachmentSizeInKB = attachmentSizeInKB;
            AttachmentUrl = attachmentUrl;
        }

        public int CandidateId { get; set; }
        public string AppUserId { get; set; }
        public string AttachmentType { get; set; }
        public int AttachmentSizeInKB { get; set; }
        public string AttachmentUrl {get; set;}
        public Candidate Candidate {get; set;}
    }
}