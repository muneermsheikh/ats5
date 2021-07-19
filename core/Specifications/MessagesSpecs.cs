using System;
using System.Linq;
using System.Linq.Expressions;
using core.Entities;
using core.Entities.Identity;
using core.Entities.Users;

namespace core.Specifications
{
     public class MessagesSpecs : BaseSpecification<Message>
     {
          public MessagesSpecs(MessageSpecParams messageParams)
            : base(x => 
                (messageParams.SenderId != 0  || 
                  x.SenderId == messageParams.SenderId) &&
                (messageParams.RecipientId != 0 ||
                  x.RecipientId == messageParams.RecipientId)
                )
          {
              ApplyPaging(messageParams.PageSize * (messageParams.PageIndex - 1), messageParams.PageSize);

              if (!string.IsNullOrEmpty(messageParams.Sort)) {
                switch(messageParams.Sort.ToLower()) {
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