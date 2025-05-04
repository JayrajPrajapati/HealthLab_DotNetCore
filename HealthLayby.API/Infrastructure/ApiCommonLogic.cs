using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiCustomerModel = HealthLayby.Models.ApiViewModels.Common.CustomerModel;

namespace HealthLayby.API.Infrastructure
{
    public class ApiCommonLogic
    {
        public static string GenerateJWTToken(ApiCustomerModel data, IConfiguration configuration)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                     new Claim("CustomerId", data.CustomerId.ToString()),
                     //new Claim("EmailAddress", data.EmailAddress),
                     //new Claim("FirstName", data.FirstName),
                     //new Claim("LastName", data.LastName),
                     //new Claim("FullName", data.FullName),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                //Expires = DateTime.UtcNow.AddMinutes(5),
                Expires = DateTime.UtcNow.AddMonths(6),
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials
                (
                    key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    algorithm: SecurityAlgorithms.HmacSha512Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
