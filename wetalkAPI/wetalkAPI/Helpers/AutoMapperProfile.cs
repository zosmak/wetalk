using AutoMapper;
using System.Collections.Generic;
using WetalkAPI.Entities;
using WetalkAPI.Models.Files;
using WetalkAPI.Models.Users;

namespace WetalkAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // user
            CreateMap<User, UserModel>();
            CreateMap<RegisterUserModel, User>();
            CreateMap<UpdateUserModel, User>();

            // files
            CreateMap<UserFile, FileModel>();

        }
    }
}