using System;
using core.Entities.EmailandSMS;
using core.Params;

namespace core.Specifications
{
     public class EmailMessagesSpecs : BaseSpecification<EmailMessage>
     {
          public EmailMessagesSpecs(EmailMessageSpecParams specParams)
            : base(x => 
                (!specParams.Id.HasValue || x.Id == specParams.Id ) &&
                (string.IsNullOrEmpty(specParams.SenderEmail)  || x.SenderEmailAddress == specParams.SenderEmail) &&
                (string.IsNullOrEmpty(specParams.RecipientEmail) || x.RecipientEmailAddress == specParams.RecipientEmail)
                 &&
                (!specParams.MessageSentFrom.HasValue && !specParams.MessageSentUpto.HasValue || 
                  (DateTime.Compare(x.MessageSentOn.Date, ((DateTime)specParams.MessageSentUpto).Date) >= 0 && 
                    DateTime.Compare(x.MessageSentOn.Date, ((DateTime)specParams.MessageSentFrom).Date) >= 0)) &&
                (!specParams.MessageSentFrom.HasValue && specParams.MessageSentUpto.HasValue || 
                  (DateTime.Compare(x.MessageSentOn.Date, ((DateTime)specParams.MessageSentFrom).Date) == 0)) && 
                (!specParams.MessageRecdFrom.HasValue && !specParams.MessageRecdUpto.HasValue || 
                    DateTime.Compare((DateTime)x.DateReadOn, ((DateTime)specParams.MessageRecdFrom).Date) <= 0 &&
                    DateTime.Compare((DateTime)x.DateReadOn, (DateTime)specParams.MessageRecdUpto) >= 0 ) &&
                (!specParams.MessageRecdFrom.HasValue && !specParams.MessageRecdUpto.HasValue || 
                    DateTime.Compare(((DateTime)x.DateReadOn).Date, (DateTime)specParams.MessageRecdUpto) <= 0 &&
                    DateTime.Compare(((DateTime)x.DateReadOn).Date, (DateTime)specParams.MessageRecdFrom) >= 0) &&
                (!specParams.MessageRecdFrom.HasValue && specParams.MessageRecdUpto.HasValue || 
                    DateTime.Compare(((DateTime)x.DateReadOn).Date, ((DateTime)specParams.MessageRecdFrom).Date) == 0 ) &&
                (string.IsNullOrEmpty(specParams.Subject) || x.Subject.ToLower().Contains(specParams.Subject.ToLower())) &&
                (!specParams.MessageTypeId.HasValue || x.MessageTypeId == (int)specParams.MessageTypeId) &&
                (string.IsNullOrEmpty(specParams.ContentsLike) || x.Content.ToLower().Contains(specParams.ContentsLike.ToLower())) 
            )
            
          {
              /*
              ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

              if (!string.IsNullOrEmpty(specParams.Sort)) {

                switch(specParams.Sort.ToLower()) {
                  case "messagesenteasc":
                    AddOrderBy(x => x.MessageSentOn);
                    break;
                  case "messagesentdesc":
                    AddOrderByDescending(x => x.MessageSentOn);
                    break;
                  
                  case "senderasc":
                    AddOrderBy(x => x.SenderUserName);
                    break;
                  
                  case "senderdesc":
                    AddOrderByDescending(x => x.SenderUserName);
                    break;
                  
                  default: 
                    AddOrderBy(x => x.MessageSentOn);
                    break;
                }
              }
              */
          }
     }
}