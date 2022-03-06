using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.EmailandSMS;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface IComposeMessagesForHR
    {
          //HR
        Task<ICollection<EmailMessage>> ComposeMessagesToDesignOrderAssessmentQs(int orderId, LoggedInUserDto loggedIn);
        Task<ICollection<EmailMessage>> ComposeMessagesToHRExecToSourceCVs(ICollection<OrderAssignmentDto> orderItemIds);
        Task<ICollection<EmailMessage>> ComposeMessagesToHRSupToReviewCVs(ICollection<CVsSubmittedDto> cvsSubmitted, LoggedInUserDto loggedIn);
        Task<ICollection<EmailMessage>> ComposeMessagesToHRMToReviewCVs(ICollection<CVsSubmittedDto> cvsSubmitted, LoggedInUserDto loggedIn);
        Task<ICollection<EmailMessage>> ComposeMessagesToDocControllerAdminToForwardCVs(ICollection<CVsSubmittedDto> cvsSubmitted, LoggedInUserDto loggedIn);
        Task<MessagesToReturnDto> ComposeHTMLToForwardEnquiryToAgents   (OrderItemsAndAgentsToFwdDto fwdDto,int LoggedInUserId);
        Task<EmailMessage> ComposeHTMLToAckToCandidateByEmail(CandidateMessageParamDto candidate);
        Task<SMSMessage> ComposeMsgToAckToCandidateBySMS(CandidateMessageParamDto candidate);
        Task<EmailMessage> ComposeHTMLToPublish_CVReadiedToForwardToClient(ICollection<CommonDataDto> commonDataDtos, LoggedInUserDto loggedInDto, int recipientId);
        Task<EmailMessage> ComposeHTMLToPublish_CVSubmittedToHRSup(ICollection<CommonDataDto> commonDataDtos, LoggedInUserDto loggedInDto, int recipientId);
        Task<EmailMessage> ComposeHTMLToPublish_CVReviewedByHRSup(ICollection<CommonDataDto> commonDataDtos, LoggedInUserDto loggedInDto, int recipientId);
        Task<EmailMessage> ComposeHTMLToPublish_CVReviewedByHRManager(ICollection<CommonDataDto> commonDataDtos, LoggedInUserDto loggedInDto, int recipientId);
      
    }
}