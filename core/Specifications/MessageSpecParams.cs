using System;
using core.Params;

namespace core.Specifications
{
    public class MessageSpecParams: ParamPages
    {

        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime MessageSentOn {get; set;}

    }
}