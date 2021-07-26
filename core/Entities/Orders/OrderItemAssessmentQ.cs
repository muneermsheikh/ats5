namespace core.Entities.Orders
{
    public class OrderItemAssessmentQ: BaseEntity
    {
         public OrderItemAssessmentQ()
        {
        }
        
        public OrderItemAssessmentQ(int orderItemId, int questionNo, string subject, string question, int maxMarks)
        {
            OrderItemId = orderItemId;
            QuestionNo = questionNo;
            Subject = subject;
            Question = question;
            MaxMarks = maxMarks;
        }
        public OrderItemAssessmentQ(int orderItemId, int orderAssessmentItemId, int questionNo, string subject, string question, int maxMarks)
        {
            OrderItemId = orderItemId;
            OrderAssessmentItemId = orderAssessmentItemId;
            QuestionNo = questionNo;
            Subject = subject;
            Question = question;
            MaxMarks = maxMarks;
        }

        public int OrderAssessmentItemId { get; set; }
        public int OrderItemId { get; set; }
        public int QuestionNo { get; set; }
        public string Subject { get; set; }
        public string Question { get; set; }
        public int MaxMarks { get; set; }
        public bool IsMandatory { get; set; }
        //public OrderAssessmentItem OrderAssessmentItem {get; set;}
    }
}