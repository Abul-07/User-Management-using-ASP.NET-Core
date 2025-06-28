﻿using System.Security.Claims;
using UserManagement.Core.Dtos.General;
using UserManagement.Core.Dtos.Message;

namespace UserManagement.Core.Interfaces
{
    public interface IMessageService
    {
        Task<GeneralServiceResponseDto> CreateNewMessageAsync(ClaimsPrincipal User,CreateMessageDto createMessageDto);

        Task<IEnumerable<GetMessageDto>> GetMessagesAsync();

        Task <IEnumerable<GetMessageDto>> GetMyMessagesAsync(ClaimsPrincipal user);
    }
}
