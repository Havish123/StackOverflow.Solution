using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using StackOverflow.Solution.Helper;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using StackOverflow.Solution.Authenticate.Common;

namespace StackOverflow.Solution.Authenticate
{
    public class FirstLevelMiddleWare
    {
        private readonly RequestDelegate _next;

        public FirstLevelMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {

            if (context.Request.Path.Value.ToLower() == Config.tokenAPi)
            {

                return _next(context);

            }
            string jwtToken;
            if(!IsComeJWTToken(context.Request,out jwtToken))
            {
                var response = AuthHelper.BuildResponseErrorMessage(context, HttpStatusCode.Unauthorized);
                return Task.FromResult(response);
            }
            try
            {
                string newToken;
                ClaimsPrincipal principal;

                //Token time out is less for the token generated through app/web key verification than the login validation
              
                principal = AuthHelper.ValidateBasicToken(jwtToken, out newToken, Config.firstlevelliftTime);

                if (!IsCustomSystemTokenValid(principal, context.Request))
                {
                    var response = AuthHelper.BuildResponseErrorMessage(context, HttpStatusCode.Unauthorized);
                    return Task.FromResult(response);
                }
                context.User = principal;
                context.Response.Headers.Add("Authorization", "Bearer " + newToken);


                return _next.Invoke(context);
            }
            catch(Exception ex)
            {
                var response = AuthHelper.BuildResponseErrorMessage(context, HttpStatusCode.Unauthorized);
                return Task.FromResult(response);
            }

        }

        private bool IsCustomSystemTokenValid(ClaimsPrincipal principal, HttpRequest request)
        {
            var decyptedClaimValue = string.Empty;
            if (principal != null)
            {
                var claims = principal.Claims.ToList();
                if (claims.Any())
                {
                    var systemClaim = claims.FirstOrDefault(d => d.Type == "FirstLevel");
                    if (systemClaim != null)
                    {
                        var encryptedClaimValue = systemClaim.Value;
                        if (string.IsNullOrEmpty(encryptedClaimValue))
                        {
                            return false;
                        }
                        decyptedClaimValue = HelperTool.Decrypt(encryptedClaimValue);
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            var localPath = request.Path.Value.ToLower();

            if (localPath== Config.tokenAPi)
            {
                return true;
            }
            else
            {
                return decyptedClaimValue == Config.LoginApiTokenKey;
            }
        }

        private bool IsComeJWTToken(HttpRequest request,out string token)
        {
            token = null;
            if (!request.Headers.ContainsKey("Authorization"))
            {
                return false;
            }
            var authzHeader = request.Headers["Authorization"].First();
            token = authzHeader.StartsWith("BaseToken") ? authzHeader.Split(' ')[1] : null;
            if (null == token)
            {
                return false;
            }
            return false;
        }
    }
}
