using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using StackOverflow.Solution.Authenticate.Common;
using StackOverflow.Solution.Helper;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Solution.Authenticate
{
    public class AuthHelper
    {
        public static HttpResponse BuildResponseErrorMessage(HttpContext httpContext, HttpStatusCode statusCode)
        {
            var response = httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.Headers.Add("Bearer", "authorization_uri=\"" + "tenanturi" + "\"" + "," + "resource_id=" + "audience");
            return httpContext.Response;
        }
        public static ClaimsPrincipal ValidateBasicToken(string jwtToken, out string newToken, int tokenLifetimeMinutes)
        {
            return ValidateToken(jwtToken, out newToken, true, tokenLifetimeMinutes);
        }

        private static ClaimsPrincipal ValidateToken(string jwtToken, out string newToken, bool isValidateLifeTime, int tokenLifetimeMinutes)
        {
            var tokenHandler = new JwtSecurityTokenHandler { TokenLifetimeInMinutes = tokenLifetimeMinutes };

            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = Constants.TokenAudience,
                //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityConstants.KeyForHmacSha256)),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.LoginApiTokenKey)),
                ValidIssuer = Constants.TokenIssuer,
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = isValidateLifeTime,
                ValidateActor = false,
            };

            SecurityToken validatedToken;

            var principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out validatedToken);
            if (isValidateLifeTime)
            {
                if (validatedToken.ValidTo < DateTime.UtcNow)
                {
                    throw new UnauthorizedAccessException();
                }
            }
            newToken = FirstlevelTokenManager.GenerateToken(principal.Claims.ToList().Where(s => s.Type != "aud").ToList());
            return principal;
        }
    }
}
