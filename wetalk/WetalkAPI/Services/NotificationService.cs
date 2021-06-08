using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;
using WetalkAPI.Models.Chat;

namespace WetalkAPI.Services
{
    public class NotificationService : Hub
    {
        private readonly IChatService _chatService;

        public NotificationService(
            IChatService chatService
            )
        {
            _chatService = chatService;
        }

        // update all user messages
        [HubMethodName("UpdateUserChats")]
        public async Task UpdateUserChats(MessageAdded request)
        {
            var chatMembers = _chatService.GetChatMembers(request.ChatID);

            foreach (var chatMember in chatMembers)
            {
                // if it wasn't the sender update
                if (chatMember.UserID != request.SenderID)
                {
                    await Clients.User(chatMember.UserID.ToString()).SendAsync("ReceiveMessage", request);
                }
            }
        }
    }
}