using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using AwsWorkshop.Application.Abstracts.Services;
using AwsWorkshop.Application.Dtos;
using AwsWorkshop.Application.Dtos.Auth;
using AwsWorkshop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsWorkshop.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly RoleManager<UserAppRole> _roleManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<UserApp> userManager, IMapper mapper, RoleManager<UserAppRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new UserApp { Email = createUserDto.Email, UserName = createUserDto.Email};

            var result = await _userManager.CreateAsync(user, createUserDto.Password);
            var setToRoleResponse = await SetRoleToUser(new SetRoleToUserDto
            {
                RoleName = "Admin",
                UserName = user.UserName
            });
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();

                return Response<UserAppDto>.Fail(errors, 400);
            }

            return Response<UserAppDto>.Success(_mapper.Map<UserAppDto>(user), 200);
        }
        public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return Response<UserAppDto>.Fail("UserName not found", 404);
            }

            return Response<UserAppDto>.Success(_mapper.Map<UserAppDto>(user), 200);
        }
        public async Task<Response<UserAppRole>> CreateRoleAsync(UserAppRoleDto userAppRoleDto)
        {
            var role = _mapper.Map<UserAppRole>(userAppRoleDto);
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded) return Response<UserAppRole>.Fail("Role not created.", 500);
            return Response<UserAppRole>.Success(role, 200);
        }
        public async Task<Response<UserRoleListDto>> SetRoleToUser(SetRoleToUserDto setRoleByUserDto)
        {
            var user = await _userManager.FindByNameAsync(setRoleByUserDto.UserName);
            var role = await _roleManager.FindByNameAsync(setRoleByUserDto.RoleName);

            if (user == null || role == null) return Response<UserRoleListDto>.Fail("User or Role not found", 404);

            var response = await _userManager.AddToRoleAsync(user, role.Name);
            var userRoles = await _userManager.GetRolesAsync(user);
            if (!response.Succeeded) return Response<UserRoleListDto>.Fail("Role not added.", 404);

            return Response<UserRoleListDto>.Success(new UserRoleListDto
            {
                UserName = user.UserName,
                Roles = userRoles.ToList()
            }, 200);
        }
        public async Task<Response<UserRoleListDto>> GetUserRoles(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) return Response<UserRoleListDto>.Fail("User not found", 404);

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles == null || userRoles.Count() == 0) return Response<UserRoleListDto>.Fail("Role not fount.", 404);

            return Response<UserRoleListDto>.Success(new UserRoleListDto
            {
                UserName = user.UserName,
                Roles = userRoles.ToList()
            }, 200);
        }
        public async Task<Response<NoContent>> DeleteUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var response = await _userManager.DeleteAsync(user);
            return response.Succeeded ? Response<NoContent>.Success(204) : Response<NoContent>.Fail(500);
        }
    }
}
