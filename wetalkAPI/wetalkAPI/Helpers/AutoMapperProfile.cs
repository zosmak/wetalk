using AutoMapper;
using WetalkAPI.Entities;
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
        }
    }
}