using System;
using System.Collections.Generic;
using System.Linq;
using WetalkAPI.Entities;
using WetalkAPI.Helpers;
using WetalkAPI.Models.Files;

namespace WetalkAPI.Services
{
    public interface IFileService
    {
        UserFile GetByName(string fileName);
        List<UserFile> GetUserFiles(int userID);
        UserFile Create(UserFile file);
        void Delete(string fileName);
    }

    public class FileService : IFileService
    {
        private DataContext _context;

        public FileService(DataContext context)
        {
            _context = context;
        }

        public UserFile GetByName(string fileName)
        {
            return _context.UserFiles.Find(fileName);
        }

        public List<UserFile> GetUserFiles(int userID)
        {
            return _context.UserFiles.Where(f => f.UserID == userID).ToList();
        }

        public UserFile Create(UserFile file)
        {
            // validation
            if (file != null && !string.IsNullOrEmpty(file.FileName))
            {
                _context.UserFiles.Add(file);
                _context.SaveChanges();

                return file;
            }

            throw new Exception("File is empty");
        }

        public void Delete(string fileName)
        {
            var file = _context.UserFiles.Find(fileName);
            if (file != null)
            {
                _context.UserFiles.Remove(file);
                _context.SaveChanges();
            }
        }
    }
}