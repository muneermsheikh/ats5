using System.ComponentModel.DataAnnotations.Schema;

namespace core.Entities.Users
{
    public class UserAttachment: BaseEntity
    {
        public UserAttachment()
        {
        }

        public UserAttachment(string appUserId, string fileName)
        {
            AppUserId = appUserId;
            //AttachmentType = attachmentType;
            //AttachmentSizeInKB = attachmentSizeInKB;
            //AttachmentUrl = attachmentUrl;

        }

        public int CandidateId { get; set; }
        public string AppUserId { get; set; }
        //public string AttachmentType { get; set; }      //cv, photo, educertificate, expcertificate, pp
        //public int AttachmentSizeInKB { get; set; }
        //public string AttachmentUrl {get; set;}
        //public Candidate Candidate {get; set;}
        public string FileName { get; set; }
    }
}