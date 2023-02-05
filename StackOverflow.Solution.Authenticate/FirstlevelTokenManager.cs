using Microsoft.IdentityModel.Tokens;
using StackOverflow.Solution.Authenticate.Common;
using StackOverflow.Solution.Helper;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Solution.Authenticate
{
    public class FirstlevelTokenManager
    {
        public static string GenerateToken(List<Claim> claims)
        {
           var tokenHandler=new JwtSecurityTokenHandler() { TokenLifetimeInMinutes=Config.firstlevelliftTime };

            // Create a security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.LoginApiTokenKey));

            // Create a signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create a JWT token
            var token = tokenHandler.CreateJwtSecurityToken(MakeTokenDescriptor(creds,claims));

            // Return the token as a string
            return tokenHandler.WriteToken(token);
        }
        private static SecurityTokenDescriptor MakeTokenDescriptor(SigningCredentials sSKey, IEnumerable<Claim> claimList)
        {
            var now = DateTime.UtcNow;
            var claims = claimList.ToArray();
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = Constants.TokenIssuer,
                Audience = Constants.TokenAudience,
                Expires = now.AddMinutes(Config.firstlevelliftTime),
                SigningCredentials = sSKey,

            };
        }
    }
}
