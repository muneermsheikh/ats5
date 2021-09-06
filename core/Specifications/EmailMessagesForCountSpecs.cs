using System;
using core.Entities.EmailandSMS;
using core.Params;

namespace core.Specifications
{
     public class EmailMessagesForCountSpecs : BaseSpecification<EmailMessage>
     {
          public EmailMessagesForCountSpecs(EmailMessageSpecParams specParams)
            : base(x => 
                (!specParams.Id.HasValue || x.Id == specParams.Id ) &&
                (string.IsNullOrEmpty(specParams.SenderEmail)  || x.SenderEmailAddress == specParams.SenderEmail) &&
                (string.IsNullOrEmpty(specParams.RecipientEmail) || x.RecipientEmailAddress == specParams.RecipientEmail) &&
                (!specParams.MessageSentFrom.HasValue && !specParams.MessageSentUpto.HasValue || 
                  (Nullable.Compare(x.MessageSentOn.Date, specParams.MessageSentUpto) >= 0 && 
                    Nullable.Compare(x.MessageSentOn.Date, specParams.MessageSentFrom) >= 0)) &&
                (!specParams.MessageSentFrom.HasValue && specParams.MessageSentUpto.HasValue || 
                  (Nullable.Compare(x.MessageSentOn.Date, specParams.MessageSentFrom) == 0)) && 
                (!specParams.MessageRecdFrom.HasValue && !specParams.MessageRecdUpto.HasValue || 
                    Nullable.Compare(x.DateReadOn, specParams.MessageRecdFrom) <= 0 &&
                    Nullable.Compare(x.DateReadOn, specParams.MessageRecdUpto) >= 0 ) &&
                (!specParams.MessageRecdFrom.HasValue && !specParams.MessageRecdUpto.HasValue || 
                    Nullable.Compare(x.DateReadOn, specParams.MessageRecdUpto) >= 0 &&
                    Nullable.Compare(x.DateReadOn, specParams.MessageRecdFrom) >= 0) &&
                (!specParams.MessageRecdFrom.HasValue && specParams.MessageRecdUpto.HasValue || 
                    Nullable.Compare(x.DateReadOn, specParams.MessageRecdFrom) == 0 ) &&
                (string.IsNullOrEmpty(specParams.Subject) || x.Subject.ToLower().Contains(specParams.Subject.ToLower())) &&
                (!specParams.MessageTypeId.HasValue || x.MessageTypeId == (int)specParams.MessageTypeId) &&
                (string.IsNullOrEmpty(specParams.ContentsLike) || x.Subject.ToLower().Contains(specParams.ContentsLike.ToLower())) 
            
            )
            
          {
          }
     }
}