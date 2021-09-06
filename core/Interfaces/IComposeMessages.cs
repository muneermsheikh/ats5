using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.EmailandSMS;
using core.Entities.HR;
using core.Entities.Orders;
using core.Entities.Process;
using core.Entities.Tasks;
using core.Entities.Users;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface IComposeMessages
    {
        Task<EmailMessage> AckEnquiryToCustomer(OrderMessageParamDto orderMessageDto);
        Task<EmailMessage> ForwardEnquiryToHRDept(Order order);
        Task<ICollection<EmailMessage>> ComposeMessagesToDesignOrderAssessmentQs(int orderId, LoggedInUserDto loggedIn);
        Task<ICollection<EmailMessage>> ComposeMessagesToHRExecToSourceCVs(ICollection<OrderAssignmentDto> orderItemIds, LoggedInUserDto loggedIn);
        Task<ICollection<EmailMessage>> ComposeMessagesToHRSupToReviewCVs(ICollection<CVsSubmittedDto> cvsSubmitted, LoggedInUserDto loggedIn);
        Task<ICollection<EmailMessage>> ComposeMessagesToHRMToReviewCVs(ICollection<CVsSubmittedDto> cvsSubmitted, LoggedInUserDto loggedIn);
        Task<ICollection<EmailMessage>> ComposeMessagesToDocControllerAdminToForwardCVs(ICollection<CVsSubmittedDto> cvsSubmitted, LoggedInUserDto loggedIn);
        ICollection<EmailMessage> ComposeCVFwdMessagesToClient(ICollection<CVRef> cvsReferred, LoggedInUserDto loggedIn);
        Task<EmailMessage> AckToCandidateByEmail(CandidateMessageParamDto candidate);
        Task<SMSMessage> AckToCandidateBySMS(CandidateMessageParamDto candidate);
        Task<ICollection<EmailMessage>> AdviseSelectionStatusToCandidateByEmail(ICollection<SelectionDecisionMessageParamDto> selections);
        Task<SMSMessage> AdviseSelectionStatusToCandidateBySMS(SelectionDecisionMessageParamDto selection);
        ICollection<EmailMessage> AdviseRejectionStatusToCandidateByEmail(ICollection<RejDecisionToAddDto> rejectionsDto);
        Task<SMSMessage> AdviseRejectionStatusToCandidateBySMS(SelectionDecisionMessageParamDto selection);
        Task<EmailMessage> AdviseProcessTransactionUpdatesToCandidateByEmail(DeployMessageParamDto deploy);
        Task<SMSMessage> AdviseProcessTransactionUpdatesToCandidateBySMS(DeployMessageParamDto deploy);
        Task<EmailMessage> Publish_CVReadiedToForwardToClient(ICollection<CommonDataDto> commonDataDtos, LoggedInUserDto loggedInDto, int recipientId);
        Task<EmailMessage> Publish_CVSubmittedToHRSup(ICollection<CommonDataDto> commonDataDtos, LoggedInUserDto loggedInDto, int recipientId);
        Task<EmailMessage> Publish_CVReviewedByHRSup(ICollection<CommonDataDto> commonDataDtos, LoggedInUserDto loggedInDto, int recipientId);
        Task<EmailMessage> Publish_CVReviewedByHRManager(ICollection<CommonDataDto> commonDataDtos, LoggedInUserDto loggedInDto, int recipientId);
        Task<EmailMessage> ComposeAppplicationTaskMessage (int taskId);

    /*
        Task<EmailDto> AcknowledgeApplicationToCandidateByEmail(Candidate candidate);
        Task<SMSDto> AcknowledgeApplicationToCandidateBySMS(Candidate candidate);
        Task<PhoneDto> AcknowledgeApplicationToCandidateByPhone(Candidate candidate);
        Task<EmailDto> AdviseApplicationReferralToCandidateByEmail(Candidate candidate);
        Task<SMSDto> AdviseApplicationReferralToCandidateBySMS(Candidate candidate);
        Task<PhoneDto> AdviseApplicationReferralToCandidateByPhone(Candidate candidate);
        Task<EmailDto> AdviseSelectionRejectionToCandidateByEmail(Candidate candidate);
        Task<SMSDto> AdviseSelectionRejectionToCandidateBySMS(Candidate candidate);
        Task<PhoneDto> AdviseSelectionRejectionToCandidateByPhone(Candidate candidate);
        Task<EmailDto> AdviseProcessUpdateToCandidateByEmail(Deploy deploy);
        Task<SMSDto> AdviseProcessUpdateToCandidateBySMS(Deploy deploy);
        Task<PhoneDto> AdviseProcessUpdateToCandidateByPhone(Deploy deploy);
    */
    }
}