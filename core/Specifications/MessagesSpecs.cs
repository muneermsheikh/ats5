using core.Entities.Admin;

namespace core.Specifications
{
     public class MessagesSpecs : BaseSpecification<Message>
     {
          public MessagesSpecs(MessageSpecParams specParams)
            : base(x => 
                (specParams.SenderId != 0  || x.SenderId == specParams.SenderId) &&
                (specParams.RecipientId != 0 || x.RecipientId == specParams.RecipientId) &&
                (specParams.MessageSentOn.Year > 2000 || x.MessageSent.Date == specParams.MessageSentOn.Date)
            )
            
          {
              ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

              if (!string.IsNullOrEmpty(specParams.Sort)) {
                switch(specParams.Sort.ToLower()) {
                  case "messagesenteasc":
                    AddOrderBy(x => x.MessageSent);
                    break;
                  case "messagesentdesc":
                    AddOrderByDescending(x => x.MessageSent);
                    break;
                  
                  case "senderasc":
                    AddOrderBy(x => x.SenderUsername);
                    break;
                  
                  case "senderdesc":
                    AddOrderByDescending(x => x.SenderUsername);
                    break;
                  
                  default: AddOrderBy(x => x.MessageSent);
                    break;
                }
              }
          }

          public MessagesSpecs(string currentusername, string recipientusername) : base
            (
              x => 
                x.Recipient.UserName == currentusername && 
                  x.Sender.UserName == recipientusername && 
                  x.RecipientDeleted == false ||
                x.Recipient.UserName == recipientusername &&
                  x.Sender.UserName == currentusername &&
                  x.SenderDeleted == false
            )
          {
          }
          public MessagesSpecs(string currentusername): base 
          (
            x =>
                x.Recipient.UserName == currentusername && 
                  x.RecipientDeleted == false ||
                x.Sender.UserName == currentusername &&
                  x.SenderDeleted == false
          )
          {
          }
          
     }
}