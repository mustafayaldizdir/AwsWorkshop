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
    public class UserAppProfile : Profile
    {
        public UserAppProfile()
        {
            CreateMap<UserAppDto, UserApp>().ReverseMap();
        }
    }
}
