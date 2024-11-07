using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AwsWorkshop.Persistence.Services
{
    public class AsymmetricKeyService
    {
        public void Run()
        {
            var tokenHandler = new JsonWebTokenHandler();
            var key = new RsaSecurityKey(RSA.Create(2048))
            {
                KeyId = Guid.NewGuid().ToString()
            };
            Jwt.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSsaPssSha256);
            var lastJws = tokenHandler.CreateToken(Jwt);
            // Store in filesystem
            // Store HMAC as Filesystem, recover and test if it’s valid
            var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(key);
            var filetext = JsonSerializer.Serialize(jwk);
            var fileDeserializedText = JsonSerializer.Deserialize<JsonWebKey>(filetext);
            TokenValidationParams.IssuerSigningKey = fileDeserializedText;
            var validationResult = tokenHandler.ValidateToken(lastJws, TokenValidationParams);
        }
        private static SecurityTokenDescriptor Jwt = new SecurityTokenDescriptor
        {
            Issuer = "www.mysite.com",
            Audience = "your-spa",
            IssuedAt = DateTime.Now,
            NotBefore = DateTime.Now,
            Expires = DateTime.Now.AddHours(1),
            Subject = new ClaimsIdentity(new List<Claim>
                        {
                        new Claim ( JwtRegisteredClaimNames.Email , "meuemail@gmail.com" , ClaimValueTypes.Email ),
                        new Claim ( JwtRegisteredClaimNames.GivenName , "Bruno Brito" ),
                        new Claim ( JwtRegisteredClaimNames.Sub , Guid.NewGuid().ToString())
                        })
        };
        private static TokenValidationParameters TokenValidationParams = new TokenValidationParameters
        {
            ValidIssuer = "www.mysite.com",
            ValidAudience = "your-spa",
        };
    }
}
