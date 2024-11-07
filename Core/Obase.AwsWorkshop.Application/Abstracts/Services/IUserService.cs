using AwsWorkshop.Application.Dtos;
using AwsWorkshop.Application.Dtos.Auth;
using AwsWorkshop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsWorkshop.Application.Abstracts.Services
{
    public interface IUserService
    {
        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
        Task<Response<UserAppRole>> CreateRoleAsync(UserAppRoleDto userAppRoleDto);
        Task<Response<UserRoleListDto>> SetRoleToUser(SetRoleToUserDto setRoleByUserDto);
        Task<Response<UserRoleListDto>> GetUserRoles(string userName);
        Task<Response<NoContent>> DeleteUser(string userName);

    }
}
