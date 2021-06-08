using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WetalkAPI.Entities;
using WetalkAPI.Helpers;
using WetalkAPI.Models.Chat;

namespace WetalkAPI.Services
{
    public interface IChatService
    {
        #region CHAT
        List<Chat> GetAll();
        List<ChatResponse> GetAllUserChat(int userID);
        ChatResponse GetChatById(int userID, int id);
        Chat CreateChat(Chat chat);
        void DeleteChat(int id);
        #endregion

        #region CHAT MEMBER
        List<ChatMember> GetChatMembers(int chatID);
        bool CheckIfChatMember(int userID, int chatID);
        ChatMember CreateChatMember(int chatID, int userID);
        void DeleteChatMember(int chatID, int userID);
        #endregion

        #region CHAT MESSAGE
        Message GetMessageByID(int messageID);
        Message CreateChatMessage(int chatID, int userID, string message);
        void MarkMessageAsRead(int userID, int messageID);
        void DeleteChatMessage(int messageID);
        #endregion
    }

    public class ChatService : IChatService
    {
        private DataContext _context;

        public ChatService(DataContext context)
        {
            _context = context;
        }

        #region CHAT
        public List<Chat> GetAll()
        {
            return _context.Chats.AsNoTracking()
                .Include(x => x.Messages.OrderByDescending(m => m.CreatedAt))
                .ToList();
        }

        public List<ChatResponse> GetAllUserChat(int userID)
        {
            var currentUserChats = _context.ChatMembers.Where(x => x.UserID == userID).Select(x => x.ChatID);

            return _context.Chats.AsNoTracking()
                .Include(x => x.Messages.OrderByDescending(m => m.CreatedAt))
                .Select(y => new ChatResponse { ID = y.ID, Name = y.Name, OwnerID = y.OwnerID, Messages = y.Messages.Select(x => new ChatResponse.ResponseMessage { ChatID = x.ChatID, ID = x.ID, SenderID = x.SenderID, CreatedAt = x.CreatedAt, Description = x.Description, Read = _context.MessagesReads.Any(r => r.UserID == userID && r.MessageID == x.ID) }).ToList() })
                .Where(x => currentUserChats.Contains(x.ID))
                .ToList();
        }

        public ChatResponse GetChatById(int userID, int id)
        {
            return _context.Chats.AsNoTracking()
            .Include(x => x.Messages.OrderByDescending(m => m.CreatedAt))
            .Select(y => new ChatResponse { ID = y.ID, Name = y.Name, OwnerID = y.OwnerID, Messages = y.Messages.Select(x => new ChatResponse.ResponseMessage { ChatID = x.ChatID, ID = x.ID, SenderID = x.SenderID, CreatedAt = x.CreatedAt, Description = x.Description, Read = _context.MessagesReads.Any(r => r.UserID == userID && r.MessageID == x.ID) }).ToList() })
            .FirstOrDefault(x => x.ID == id);
        }

        public Chat CreateChat(Chat chat)
        {
            // validations
            if (string.IsNullOrEmpty(chat.Name))
                throw new Exception("Name is required");
            if (chat.OwnerID == 0)
                throw new Exception("OwnerID is required");

            _context.Chats.Add(chat);
            _context.SaveChanges();

            return chat;
        }

        public void DeleteChat(int id)
        {
            var chat = _context.Chats.Find(id);
            if (chat != null)
            {
                _context.Chats.Remove(chat);
                _context.SaveChanges();
            }
        }
        #endregion


        #region CHAT MEMBER
        public bool CheckIfChatMember(int userID, int chatID)
        {
            var chatMember = _context.ChatMembers.FirstOrDefault(x => x.ChatID == chatID && x.UserID == userID);
            if (chatMember != null)
                return true;
            return false;
        }

        public List<ChatMember> GetChatMembers(int chatID)
        {
            return _context.ChatMembers
                .Where(x => x.ChatID == chatID)
                .ToList();
        }

        public ChatMember CreateChatMember(int chatID, int userID)
        {
            var newChatMember = new ChatMember()
            {
                ChatID = chatID,
                UserID = userID
            };

            _context.ChatMembers.Add(newChatMember);
            _context.SaveChanges();

            return newChatMember;
        }

        public void DeleteChatMember(int chatID, int userID)
        {
            var chatMember = _context.ChatMembers.FirstOrDefault(x => x.ChatID == chatID && x.UserID == userID);
            if (chatMember != null)
            {
                _context.ChatMembers.Remove(chatMember);
                _context.SaveChanges();
            }
        }
        #endregion

        #region CHAT MESSAGE
        public Message GetMessageByID(int id)
        {
            return _context.Messages.Find(id);
        }

        public Message CreateChatMessage(int chatID, int userID, string message)
        {
            var newChatMessage = new Message()
            {
                ChatID = chatID,
                SenderID = userID,
                Description = message,
                CreatedAt = DateTime.Now
            };

            _context.Messages.Add(newChatMessage);
            _context.SaveChanges();

            return newChatMessage;
        }

        public void DeleteChatMessage(int messageID)
        {
            var chatMessage = _context.Messages.FirstOrDefault(x => x.ID == messageID);
            if (chatMessage != null)
            {
                _context.Messages.Remove(chatMessage);
                _context.SaveChanges();
            }
        }

        public void MarkMessageAsRead(int userID, int messageID)
        {
            var userChats = GetAllUserChat(userID);

            if (userChats.Any(x => x.Messages.Any(y => y.ID == messageID)))
            {
                var newReadItem = new MessageRead()
                {
                    MessageID = messageID,
                    ReadAt = DateTime.Now,
                    UserID = userID,
                };

                _context.MessagesReads.Add(newReadItem);
                _context.SaveChanges();
                return;
            }

            throw new Exception("Message doens't exit or user doens't have permission to see it");
        }
        #endregion
    }
}