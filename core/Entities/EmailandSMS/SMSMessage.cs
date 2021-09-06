using System;

namespace core.Entities.EmailandSMS
{
    public class SMSMessage: BaseEntity
    {
        public string UserId { get; set; }
        public DateTime SMSDateTime { get; set; }
        public string PhoneNo { get; set; }
        public string SMSText { get; set; }
        public string DeliveryResult { get; set; }
    }
}