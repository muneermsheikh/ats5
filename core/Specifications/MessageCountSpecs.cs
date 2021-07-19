using System;
using System.Linq;
using core.Entities;
using core.Entities.Identity;
using core.Entities.Users;

namespace core.Specifications
{
    public class MessageCountSpecs: BaseSpecification<Message>
    {
        public MessageCountSpecs(MessageSpecParams msgParams)
            : base(x => 
                (msgParams.SenderId != 0  || 
                  x.SenderId == msgParams.SenderId) &&
                (msgParams.RecipientId != 0 ||
                  x.RecipientId == msgParams.RecipientId)
                )
        {
        }

        public MessageCountSpecs(string currentusername, string recipientusername) : base
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

        public MessageCountSpecs(string currentusername): base 
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