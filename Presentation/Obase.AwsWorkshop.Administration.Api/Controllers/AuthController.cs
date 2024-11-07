using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AwsWorkshop.Application.Abstracts.Services;
using AwsWorkshop.Application.Dtos;
using AwsWorkshop.Application.Dtos.Auth;
using AwsWorkshop.Application.Settings;
using AwsWorkshop.Domain.Entities;
using System.Text.Json;

namespace AwsWorkshop.Administration.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBased
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IServiceGeneric<UserRefreshToken, UserRefreshTokenDto> _userRefreshTokenService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(
            IAuthenticationService authenticationService,
            IServiceGeneric<UserRefreshToken, UserRefreshTokenDto> userRefreshTokenService,
            IUserService userService,
            ILogger<AuthController> logger) 
        {
            _authenticationService = authenticationService;
            _userRefreshTokenService = userRefreshTokenService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken(CreateTokenDto createTokenDto)
        {
            var user = await _userService.GetUserByNameAsync(createTokenDto.UserName);
            _logger.LogWarning($"CreateToken > GetUserByNameAsync : {JsonSerializer.Serialize(user)}");
            if (user.Data == null) return ActionResultInstance(Response<TokenDto>.Fail("User not found.", 404));

            var result = await _authenticationService.CreateToken(createTokenDto);

            return ActionResultInstance(result);
        }

        [HttpPost]
        public IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var result = _authenticationService.CreateTokenByClient(clientLoginDto);
            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.RevokeRefreshToken(refreshTokenDto.Token);
            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.CreateTokenByRefreshToken(refreshTokenDto.Token);

            return ActionResultInstance(result);
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetRefreshTokenByUserName(string userName)
        {
            var user = await _userService.GetUserByNameAsync(userName);
            var result = await _userRefreshTokenService.FirstOrDefaultAsync(x => x.UserId == user.Data.Id);
            return ActionResultInstance(result);
        }
    }
}
