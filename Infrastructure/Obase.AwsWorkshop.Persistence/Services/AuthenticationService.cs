using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using AwsWorkshop.Application.Abstracts.Services;
using AwsWorkshop.Application.Settings;
using AwsWorkshop.Domain.Entities;
using AwsWorkshop.Application.Abstracts.UnitOfWorks;
using AwsWorkshop.Application.Abstracts.Repositories;
using AwsWorkshop.Application.Dtos;
using AwsWorkshop.Application.Dtos.Auth;

namespace AwsWorkshop.Persistence.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserRefreshToken> _userRefreshTokenRepository;

        public AuthenticationService(
            IUnitOfWork unitOfWork,
            IRepository<UserRefreshToken> userRefreshTokenRepository,
            UserManager<UserApp> userManager,
            IOptions<List<Client>> clients,
            ITokenService tokenService
            )
        {
            _unitOfWork = unitOfWork;
            _userRefreshTokenRepository = userRefreshTokenRepository;
            _userManager = userManager;
            _clients = clients.Value;
            _tokenService = tokenService;
        }


        public async Task<Response<TokenDto>> CreateToken(CreateTokenDto createTokenDto)
        {
            if (createTokenDto == null) throw new ArgumentNullException(nameof(CreateTokenDto));

            var user = await _userManager.FindByEmailAsync(createTokenDto.UserName);
            if (user == null) return Response<TokenDto>.Fail("UserName or password is wrong", 400);
            if (!await _userManager.CheckPasswordAsync(user, createTokenDto.Password)) return Response<TokenDto>.Fail("UserName or password is wrong", 400);

            var token = _tokenService.CreateToken(user);

            var userRefreshToken = await _userRefreshTokenRepository.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

            if (userRefreshToken == null)
            {
                await _userRefreshTokenRepository.AddAsync(new UserRefreshToken
                {
                    UserId = user.Id,
                    RefreshToken = token.RefreshToken,
                    Expiration = token.RefreshTokenExpiration
                });
            }
            else
            {
                userRefreshToken.RefreshToken = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(token, 200);
        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);

            if (client == null)
            {
                return Response<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 404);
            }

            var token = _tokenService.CreateTokenByClient(client);
            return Response<ClientTokenDto>.Success(token, 200);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenRepository.Where(x => x.RefreshToken == refreshToken).SingleOrDefaultAsync();

            if (existRefreshToken == null) return Response<TokenDto>.Fail("RefreshToken not found", 404);

            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId.ToString());

            if (user == null) return Response<TokenDto>.Fail("UserId not found", 404);

            var token = _tokenService.CreateToken(user);

            existRefreshToken.RefreshToken = token.RefreshToken;
            existRefreshToken.Expiration = token.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(token, 200);
        }

        public async Task<Response<NoContent>> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenRepository.Where(x => x.RefreshToken == refreshToken).SingleOrDefaultAsync();

            if (existRefreshToken == null) return Response<NoContent>.Fail("RefreshToken not found", 404);

            _userRefreshTokenRepository.Remove(existRefreshToken);

            await _unitOfWork.CommitAsync();

            return Response<NoContent>.Success(200);
        }
    }
}
