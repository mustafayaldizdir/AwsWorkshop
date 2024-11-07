using AutoMapper;
using AwsWorkshop.Application.Dtos.Auth;
using AwsWorkshop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsWorkshop.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRefreshToken, UserRefreshTokenDto>().ReverseMap();
        }
    }
}
