﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserManagement.Core.DbContext;
using UserManagement.Core.Dtos.General;
using UserManagement.Core.Dtos.Message;
using UserManagement.Core.Entities;
using UserManagement.Core.Interfaces;

namespace UserManagement.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogService _logService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageService(ApplicationDbContext context, ILogService logService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logService = logService;
            _userManager = userManager;
        }

        public async Task<GeneralServiceResponseDto> CreateNewMessageAsync(ClaimsPrincipal User, CreateMessageDto createMessageDto)
        {
            if (User.Identity.Name == createMessageDto.ReceiverUserName)
                return new GeneralServiceResponseDto()
                {
                    isSucceed = false,
                    StatusCode = 400,
                    Message = "Sender and Receiver can not be same",
                };

            var isReceiverUserNameValid = _userManager.Users.Any(q => q.UserName == createMessageDto.ReceiverUserName);
            if (!isReceiverUserNameValid)
                return new GeneralServiceResponseDto()
                {
                    isSucceed = false,
                    StatusCode = 400,
                    Message = "Receiver UserName is not valid",
                };

            Message newMessage = new Message()
            {
                SenderUserName = User.Identity.Name,
                ReceiverUserName = createMessageDto.ReceiverUserName,
                Text = createMessageDto.Text
            };
            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();
            await _logService.SaveNewLog(User.Identity.Name, "Send Message");

            return new GeneralServiceResponseDto()
            {
                isSucceed = true,
                StatusCode = 201,
                Message = "Message saved successfully",
            };
        }

        public async Task<IEnumerable<GetMessageDto>> GetMessagesAsync()
        {
            var messages = await _context.Messages
                .Select(q => new GetMessageDto()
                {
                    Id = q.Id,
                    SenderUserName = q.SenderUserName,
                    ReceiverUserName = q.ReceiverUserName,
                    Text = q.Text,
                    CreatedAt = q.CreatedAt
                })
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();

            return messages;
        }

        public Task<IEnumerable<GetMessageDto>> GetMessagesAsync(ClaimsPrincipal User)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GetMessageDto>> GetMyMessagesAsync(ClaimsPrincipal User)
        {
            var loggedInUser = User.Identity.Name;

            var messages = await _context.Messages
                .Where(q => q.SenderUserName == loggedInUser || q.ReceiverUserName == loggedInUser)
             .Select(q => new GetMessageDto()
             {
                 Id = q.Id,
                 SenderUserName = q.SenderUserName,
                 ReceiverUserName = q.ReceiverUserName,
                 Text = q.Text,
                 CreatedAt = q.CreatedAt
             })
             .OrderByDescending(q => q.CreatedAt)
             .ToListAsync();

            return messages;
        }
    }
}
