using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StackOverflow.Solution.Authenticate
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;

        public TokenProviderMiddleware(RequestDelegate next, TokenProviderOptions options)
        {
            _next = next;
            _options = options;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Equals(_options.Path, StringComparison.OrdinalIgnoreCase))
            {
                return _next(httpContext);
            }
            if (httpContext.Request.Method.Equals("POST"))
            {
                httpContext.Response.StatusCode = 400;
                return httpContext.Response.WriteAsync("Bad Request");
            }
            return _next(httpContext);
        }

        //Collect UserClaims if the Username and password is correcct
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var guid = Guid.NewGuid().ToString();
            if (username == "test" && password == "test")
            {
                return new ClaimsIdentity
                    (new System.Security.Principal.GenericIdentity(username, "Token"),
                    new Claim[]
                    {
                    new Claim("sub","test",username,"stackoverflow.hmaths.com","stackoverflow.hmaths.com"),
                    new Claim("jti",guid),
                    new Claim("iat",DateTime.UtcNow.ToString(),ClaimValueTypes.Integer)
                    }
                    );
            }
            return null;
        }

        //Generate JWT Token
        private async Task GenerateToken(HttpContext context)
        {
            string username = "test";
            string password = "test";
            var identity = GetIdentity(username, password);

            if (identity == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid Username or Password");
                return;
            }

            try
            {
                var now = DateTime.UtcNow;
                var claim = new List<Claim>()
                {
                    new Claim("sub","Test","UserName","stackoverflow.hmaths.com","stackoverflow.hmaths.com",identity)
                };

                var jwt = new JwtSecurityToken(
                        issuer: _options.Issuer,
                        audience:_options.Audience,
                        notBefore:now,
                        claims:identity.Claims,
                        expires:now.Add(_options.Expiration),
                        signingCredentials:_options.SigningCredentials                  

                    );

                var encodedJwt=new JwtSecurityTokenHandler().WriteToken(jwt);

                var response = new
                {
                    access_token=encodedJwt,
                    expires=(int)_options.Expiration.TotalSeconds
                };

                context.Response.ContentType="application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response, 
                    new JsonSerializerSettings { Formatting = Formatting.Indented }));


            }catch(Exception e)
            {
                throw e;
            }
        }

        

    }
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenProviderMiddleware>();
        }
    }


}
