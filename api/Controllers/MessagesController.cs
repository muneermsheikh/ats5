using System.Collections.Generic;
using System.Threading.Tasks;
using api.Errors;
using api.Extensions;
using AutoMapper;
using core.Entities.Admin;
using core.Entities.EmailandSMS;
using core.Entities.Identity;
using core.Interfaces;
using core.Params;
using core.ParamsAndDtos;
using core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     public class MessagesController : BaseApiController
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly IMapper _mapper;
          private readonly IComposeMessages _composeMessages;
          private readonly IEmailService _emailService;
          private readonly ISMSService _smsService;
          private readonly UserManager<AppUser> _userManager;
          public MessagesController(IMapper mapper, IUnitOfWork unitOfWork, IComposeMessages composeMessages,
               IEmailService emailService, ISMSService smsService, UserManager<AppUser> userManager)
          {
               _userManager = userManager;
               _smsService = smsService;
               _emailService = emailService;
               _composeMessages = composeMessages;
               _mapper = mapper;
               _unitOfWork = unitOfWork;
          }

          [Authorize]
          [HttpPost]
          public async Task<ActionResult<MessageDto>> SendNewMessage(EmailMessage message)
          {
               if (User == null) return BadRequest(new ApiResponse(400, "the user must log in to invoke this function"));
               var sender = await _userManager.FindByEmailAsync(message.SenderEmailAddress);
               if(sender == null) return BadRequest(new ApiResponse(400, "invalid sender user id"));
               var recipient = await _userManager.FindByEmailAsync(message.RecipientEmailAddress);
               if(recipient == null) return BadRequest(new ApiResponse(400, "invalid recipient user id"));

               message.SenderId = sender.Id;      //User.GetIdentityUserId();
               message.SenderUserName = sender.UserName; 
               //message.Sender = sender;
               
               message.RecipientId = recipient.Id;
               message.RecipientUserName=recipient.UserName;
               var AttachmentFilePaths = new List<string>();
               //message.Recipient = recipient;
               var sentMsg = _emailService.SendEmail(message, AttachmentFilePaths);
               
               if (sentMsg == null) {
                    return BadRequest(new ApiResponse(400, "Failed to send the email"));
               } else {
                    return new MessageDto {
                         Id = sentMsg.Id, 
                         //SenderId = sentMsg.SenderId, 
                         SenderUsername = sentMsg.SenderUserName,
                         //RecipientId = sentMsg.RecipientId, 
                         RecipientUsername = sentMsg.RecipientUserName,
                         MessageSent= sentMsg.MessageSentOn, DateRead = sentMsg.DateReadOn,
                         //SenderDeleted = sentMsg.SenderDeleted, RecipientDeleted = sentMsg.RecipientDeleted,
                         Content = sentMsg.Content
                    };
               }
          }

          [Authorize]
          [HttpGet("loggedInUser")]
          public async Task<ActionResult<Pagination<MessageDto>>> GetMessagesForLoggedInUser(EmailMessageSpecParams messageParams)
          {
               var loggedInUser = await _userManager.FindByEmailAsync(User.GetIdentityUserEmailId());
               if (string.IsNullOrEmpty(loggedInUser.Email)) 
                    return Unauthorized(new ApiResponse(404, "Only the loggedIn user can access this feature"));
               messageParams.SenderEmail = loggedInUser.Email;
               var spec = new EmailMessagesSpecs(messageParams);
               var CtSpec = new EmailMessagesForCountSpecs(messageParams);
               var totalItems = await _unitOfWork.Repository<EmailMessage>().CountAsync(CtSpec);
               if (totalItems == 0) return NotFound(new ApiResponse(400, "No records found"));
               var messages = await _unitOfWork.Repository<EmailMessage>().ListAsync(spec);

               return Ok(new Pagination<MessageDto>(messageParams.PageIndex,
                    messageParams.PageSize, totalItems, _mapper.Map<List<MessageDto>>(messages)));
          }

          [HttpGet("usermessages")]
          public async Task<ActionResult<Pagination<MessageDto>>> GetMessagesForUser(EmailMessageSpecParams messageParams)
          {
               /*
               if (!User.IsInRole("Admin"))
               {
                    if (string.IsNullOrEmpty(messageParams.SenderEmail)) return BadRequest(new ApiResponse(400, "Sender email not available"));
                    var loggedInUser = await _userManager.FindByEmailAsync(User.GetIdentityUserEmailId());
                    if (!messageParams.SenderEmail.Equals(loggedInUser.Email)) 
                         return Unauthorized(new ApiResponse(404, "Only the loggedIn user can access this feature"));
               }
               */
               var spec = new EmailMessagesSpecs(messageParams);
               var CtSpec = new EmailMessagesForCountSpecs(messageParams);
               var totalItems = await _unitOfWork.Repository<EmailMessage>().CountAsync(CtSpec);
               //if (totalItems == 0) return NotFound(new ApiResponse(400, "No records found"));
               var messages = await _unitOfWork.Repository<EmailMessage>().ListAsync(spec);

               return Ok(new Pagination<MessageDto>(messageParams.PageIndex,
                    messageParams.PageSize, totalItems, _mapper.Map<List<MessageDto>>(messages)));
          }

          [HttpGet("msgthreadforuser/{ReceiptUserName}/{pageIndex}/{pageSize}")]
          public async Task<ActionResult<Pagination<MessageDto>>> GetMessageThreadForLoggedInUser(
          string RecipientEmail, int pageIndex, int pageSize)
          {
               var loggedInUserEmail = User.GetIdentityUserEmailId();
               if (string.IsNullOrEmpty(loggedInUserEmail)) return Unauthorized();
               
               var specParams = new EmailMessageSpecParams{RecipientEmail=RecipientEmail, SenderEmail = loggedInUserEmail };
               var spec = new EmailMessagesSpecs(specParams);
               var CtSpec = new EmailMessagesForCountSpecs(specParams);

               var totalItems = await _unitOfWork.Repository<EmailMessage>().CountAsync(CtSpec);

               var messages = await _unitOfWork.Repository<EmailMessage>().ListAsync(spec);
               //messages.MarkUnreadAsRead(loggedInUser);
               return Ok(new Pagination<MessageDto>(pageIndex,
                    pageSize, totalItems, _mapper.Map<List<MessageDto>>(messages)));
          }

          [HttpDelete("{id}")]
          public async Task<ActionResult> DeleteMessage(int id)
          {
               var username = User.GetUsername();

               var message = await _unitOfWork.Repository<EmailMessage>().GetByIdAsync(id);

               if (!message.SenderId.Equals(message.RecipientId))
                    return Unauthorized();

               if (message.SenderUserName.Equals(username)) message.SenderDeleted = true;

               if (message.RecipientUserName.Equals(username)) message.RecipientDeleted = true;

               if (message.SenderDeleted && message.RecipientDeleted)
                    _unitOfWork.Repository<EmailMessage>().Delete(message);

               if (await _unitOfWork.Complete() > 0) return Ok();

               return BadRequest("Problem deleting the message");
          }

          [HttpGet("ComposeCVAcknToCandidateByEmail")]
          public async Task<EmailMessage> ComposeCVAcknEmailMessage(CandidateMessageParamDto paramDto)
          {
               var msg = await _composeMessages.AckToCandidateByEmail(paramDto);
               var msgToReturn = new EmailMessage();
               var AttachmentFilePaths = new List<string>();
               if (paramDto.DirectlySendMessage)
               {
                    msgToReturn = _emailService.SendEmail(msg, AttachmentFilePaths);
                    msgToReturn.MessageSentOn = System.DateTime.Now;
               }

               return msgToReturn;
          }

          [HttpGet("ComposeSelAdvToCandidateByEmail")]
          public async Task<ICollection<EmailMessage>> ComposeSelAdviseToCandidateByemail(ICollection<SelectionDecisionMessageParamDto> paramDto)
          {
               var msgs = await _composeMessages.AdviseSelectionStatusToCandidateByEmail(paramDto);
               var AttachmentFilePaths = new List<string>();
               foreach(var msg in msgs)
               {
                    _emailService.SendEmail(msg, AttachmentFilePaths);                    
               }

               return msgs;
          }

          [HttpGet("ComposeRejAdvToCandidateByEmail")]
          public ICollection<EmailMessage> ComposeRejAdviseToCandidateByemail(ICollection<RejDecisionToAddDto> paramDto)
          {
               var msgs = _composeMessages.AdviseRejectionStatusToCandidateByEmail(paramDto);
               var AttachmentFilePaths = new List<string>();
               foreach(var msg in msgs)
               {
                    _emailService.SendEmail(msg, AttachmentFilePaths);
               }

               return msgs;
          }

          [HttpGet("ComposeProcessAdvToCandidateByEmail")]
          public async Task<EmailMessage> ComposeProcessAdviseToCandidateByemail(DeployMessageParamDto paramDto)
          {
               var msg = await _composeMessages.AdviseProcessTransactionUpdatesToCandidateByEmail(paramDto);
               var msgToReturn = new EmailMessage();
               var AttachmentFilePaths = new List<string>();
               if (paramDto.DirectlySendMessage)
               {
                    msgToReturn = _emailService.SendEmail(msg, AttachmentFilePaths);
                    msgToReturn.MessageSentOn = System.DateTime.Now;
               }
               return msgToReturn;
          }

          [HttpGet("ComposeCVAcknToCandidateBySMS")]
          public async Task<SMSMessage> ComposeCVAcknBySMS(CandidateMessageParamDto paramDto)
          {
               var msg = await _composeMessages.AckToCandidateBySMS(paramDto);
               if (paramDto.DirectlySendMessage)
               {
                    _smsService.sendMessage(msg.PhoneNo, msg.SMSText);
                    msg.DeliveryResult = "success";
               }

               return msg;
          }

          [HttpGet("ComposeSelAdvToCandidateBySMS")]
          public async Task<SMSMessage> ComposeSelAdviseToCandidateBySMS(SelectionDecisionMessageParamDto paramDto)
          {
               var msg = await _composeMessages.AdviseSelectionStatusToCandidateBySMS(paramDto);
               if (paramDto.DirectlySendMessage)
               {
                    _smsService.sendMessage(msg.PhoneNo, msg.SMSText);
                    msg.DeliveryResult = "success";
               }

               return msg;
          }

          [HttpGet("ComposeRejAdvToCandidateBySMS")]
          public async Task<SMSMessage> ComposeRejAdviseToCandidateBySMS(SelectionDecisionMessageParamDto paramDto)
          {
               var msg = await _composeMessages.AdviseRejectionStatusToCandidateBySMS(paramDto);
               if (paramDto.DirectlySendMessage)
               {
                    _smsService.sendMessage(msg.PhoneNo, msg.SMSText);
                    msg.DeliveryResult = "success";
               }

               return msg;
          }

          [HttpGet("ComposeProcessAdvToCandidateBySMS")]
          public async Task<SMSMessage> ComposeProcessAdviseToCandidateBySMS(DeployMessageParamDto paramDto)
          {
               var msg = await _composeMessages.AdviseProcessTransactionUpdatesToCandidateBySMS(paramDto);
               if (paramDto.DirectlySendMessage)
               {
                    _smsService.sendMessage(msg.PhoneNo, msg.SMSText);
                    msg.DeliveryResult = "success";
               }

               return msg;
          }


     }
}