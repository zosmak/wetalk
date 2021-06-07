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
        IEnumerable<ChatMember> GetAll(int userID);
        Chat GetChatById(int id);
        Chat CreateChat(Chat chat);
        void DeleteChat(int id);
        #endregion

        #region CHAT MEMBER
        ChatMember CreateChatMember(int chatID, int userID);
        void DeleteChatMember(int chatID, int userID);
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
        public IEnumerable<ChatMember> GetAll(int userID)
        {
            return _context.ChatMembers;
        }

        public Chat GetChatById(int id)
        {
            return _context.Chats.Find(id);
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

    }
}