using IdentityModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AwsWorkshop.Application.Abstracts.Services;
using AwsWorkshop.Application.Exceptions;
using AwsWorkshop.Application.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AwsWorkshop.Application.Handlers
{
    public class ClientCredentialTokenHandler : DelegatingHandler
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ClientSettings _clientSettings;
        private readonly ILogger<ClientCredentialTokenHandler> _logger;

        public ClientCredentialTokenHandler(
            IOptions<ClientSettings> clientSettings,
            ILogger<ClientCredentialTokenHandler> logger,
            IAuthenticationService authenticationService)
        {
            _clientSettings = clientSettings.Value;
            _logger = logger;
            _authenticationService = authenticationService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {

                var result = _authenticationService.CreateTokenByClient(new Dtos.Auth.ClientLoginDto
                {
                    ClientId = _clientSettings.ClientId,
                    ClientSecret = _clientSettings.ClientSecret
                });
                _logger.LogError($"ClientCredentialTokenHandler=>AccessTokenResult:{result}");
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Data.AccessToken);


                var response = await base.SendAsync(request, cancellationToken);
                _logger.LogError($"ClientCredentialTokenHandler=>AccessTokenResponse:{response}");
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) throw new UnAuthorizeException();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new UnAuthorizeException();
            }
        }
    }
}
