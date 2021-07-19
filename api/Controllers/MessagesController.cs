using System.Collections.Generic;
using System.Threading.Tasks;
using api.Extensions;
using AutoMapper;
using core.Entities.Identity;
using core.Interfaces;
using core.ParamsAndDtos;
using core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     public class MessagesController : BaseApiController
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly IMapper _mapper;
          public MessagesController(IMapper mapper, IUnitOfWork unitOfWork)
          {
               _mapper = mapper;
               _unitOfWork = unitOfWork;

          }

          [HttpGet]
          public async Task<ActionResult<Pagination<MessageDto>>> GetMessagesForUser([FromQuery]
            MessageSpecParams messageParams)
          {
               var spec = new MessagesSpecs(messageParams);
               var CtSpec = new MessageCountSpecs(messageParams);
               var totalItems = await _unitOfWork.Repository<Message>().CountAsync(CtSpec);

               var messages = await _unitOfWork.Repository<Message>().ListAsync(spec);

               return Ok(new Pagination<MessageDto>(messageParams.PageIndex,
                    messageParams.PageSize, totalItems, _mapper.Map<List<MessageDto>>(messages)));
          }

          [HttpGet("msgthreadforuser/{ReceiptUserName}/{pageIndex}/{pageSize}")]
          public async Task<ActionResult<Pagination<MessageDto>>> GetMessageThreadForLoggedInUser(
            string ReceiptUserName, int pageIndex, int pageSize)
          {
               var loggedInUser = User.GetUsername();
               if (string.IsNullOrEmpty(loggedInUser)) return Unauthorized();

               var spec = new MessagesSpecs(loggedInUser, ReceiptUserName);
               var CtSpec = new MessageCountSpecs(loggedInUser, ReceiptUserName);

               var totalItems = await _unitOfWork.Repository<Message>().CountAsync(CtSpec);

               var messages = await _unitOfWork.Repository<Message>().ListAsync(spec);
               //messages.MarkUnreadAsRead(loggedInUser);
               return Ok(new Pagination<MessageDto>(pageIndex,
                    pageSize, totalItems, _mapper.Map<List<MessageDto>>(messages)));
          }

          [HttpDelete("{id}")]
          public async Task<ActionResult> DeleteMessage(int id)
          {
               var username = User.GetUsername();

               var message = await _unitOfWork.Repository<Message>().GetByIdAsync(id);

               if (message.Sender.UserName != username && message.Recipient.UserName != username)
                    return Unauthorized();

               if (message.Sender.UserName == username) message.SenderDeleted = true;

               if (message.Recipient.UserName == username) message.RecipientDeleted = true;

               if (message.SenderDeleted && message.RecipientDeleted)
                    _unitOfWork.Repository<Message>().Delete(message);

               if (await _unitOfWork.Complete() > 0) return Ok();

               return BadRequest("Problem deleting the message");
          }

     }
}