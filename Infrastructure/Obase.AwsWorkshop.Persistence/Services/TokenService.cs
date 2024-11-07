using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AwsWorkshop.Application.Abstracts.Services;
using AwsWorkshop.Application.Dtos.Auth;
using AwsWorkshop.Application.Settings;
using AwsWorkshop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AwsWorkshop.Persistence.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly TokenOptionsSettings _tokenOptionsSettings;

        public TokenService(UserManager<UserApp> userManager, IOptions<TokenOptionsSettings> options)
        {
            _userManager = userManager;
            _tokenOptionsSettings = options.Value;
        }
        private string CreateRefreshToken()
        {
            var numberByte = new byte[32];

            using var rnd = RandomNumberGenerator.Create();

            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }

        private IEnumerable<Claim> GetClaim(UserApp userApp, List<string> audiences)
        {
            var claimList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userApp.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,userApp.Email),
                new Claim(ClaimTypes.Name,userApp.UserName),
                new Claim(ClaimTypes.Role, "Admin"),

                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            claimList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            if (userApp.Email == "admin@awsworkshop.com")
            {
                claimList.Add(new Claim(ClaimTypes.Role, "SystemAdmin"));
            }
            return claimList;
        }

        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();
            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, "SystemAdmin"));
            return claims;
        }


        public TokenDto CreateToken(UserApp userApp)
        {
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOptionsSettings.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOptionsSettings.RefreshTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(_tokenOptionsSettings.SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOptionsSettings.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.UtcNow,
                claims: GetClaim(userApp, _tokenOptionsSettings.Audience),
                signingCredentials: signingCredentials);
            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };

            return tokenDto;
        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOptionsSettings.AccessTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(_tokenOptionsSettings.SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOptionsSettings.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.UtcNow,
                claims: GetClaimsByClient(client),
                signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var clientTokenDto = new ClientTokenDto
            {
                AccessToken = token,
                AccessTokenExpiration = accessTokenExpiration
            };

            return clientTokenDto;
        }
    }
}
