using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.EmailandSMS;
using core.Entities.HR;
using core.Entities.Orders;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface IComposeMessagesForAdmin
    {
         //Admin
        Task<EmailMessage> AckEnquiryToCustomer(OrderMessageParamDto orderMessageDto);
        Task<EmailMessage> ForwardEnquiryToHRDept(Order order);
        ICollection<EmailMessage> ComposeCVFwdMessagesToClient(ICollection<CVRef> cvsReferred, LoggedInUserDto loggedIn);
        Task<ICollection<EmailMessage>> AdviseSelectionStatusToCandidateByEmail(ICollection<SelectionDecisionMessageParamDto> selections);
        Task<SMSMessage> AdviseSelectionStatusToCandidateBySMS(SelectionDecisionMessageParamDto selection);
        ICollection<EmailMessage> AdviseRejectionStatusToCandidateByEmail(ICollection<RejDecisionToAddDto> rejectionsDto);
        Task<SMSMessage> AdviseRejectionStatusToCandidateBySMS(SelectionDecisionMessageParamDto selection);
    }
}