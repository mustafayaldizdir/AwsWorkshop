using AwsWorkshop.Application.Dtos;
using AwsWorkshop.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsWorkshop.Application.Abstracts.Services
{
    public interface IAuthenticationService
    {
        Task<Response<TokenDto>> CreateToken(CreateTokenDto createTokenDto);
        Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
        Task<Response<NoContent>> RevokeRefreshToken(string refreshToken);
        Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
    }
}
