using core.Entities.Admin;

namespace core.Specifications
{
     public class MessageCountSpecs: BaseSpecification<Message>
    {
        public MessageCountSpecs(MessageSpecParams specParams)
            : base(x => 
                (specParams.SenderId != 0  || x.SenderId == specParams.SenderId) &&
                (specParams.RecipientId != 0 || x.RecipientId == specParams.RecipientId) &&
                (specParams.MessageSentOn.Year > 2000 || x.MessageSent.Date == specParams.MessageSentOn.Date)
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