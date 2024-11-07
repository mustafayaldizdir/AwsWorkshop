using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AwsWorkshop.Application.Abstracts.Services;
using AwsWorkshop.Application.Dtos.Auth;
using AwsWorkshop.Application.Settings;

namespace AwsWorkshop.Administration.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBased
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            return ActionResultInstance(await _userService.CreateUserAsync(createUserDto));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateRole(UserAppRoleDto userAppRoleDto)
        {
            return ActionResultInstance(await _userService.CreateRoleAsync(userAppRoleDto));
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SetRoleToUser(SetRoleToUserDto setRoleByUserDto)
        {
            return ActionResultInstance(await _userService.SetRoleToUser(setRoleByUserDto));
        }

        [Authorize]
        [HttpGet("{userName}")]
        public async Task<IActionResult> GetRoles(string userName)
        {
            return ActionResultInstance(await _userService.GetUserRoles(userName));
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return ActionResultInstance(await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
        }
        [Authorize]
        [HttpGet("{userName}")]
        public async Task<IActionResult> GetUserByName(string userName)
        {
            return ActionResultInstance(await _userService.GetUserByNameAsync(userName));
        }

        [Authorize]
        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            return ActionResultInstance(await _userService.DeleteUser(userName));
        }


    }
}
