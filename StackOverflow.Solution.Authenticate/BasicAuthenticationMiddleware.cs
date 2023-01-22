using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Solution.Authenticate
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class BasicAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        //public readonly NoValidateRoutes _noValidateroutes;
        IConfiguration configuration;

        public BasicAuthenticationMiddleware(RequestDelegate next
            //, IOptions<NoValidateRoutes> noValidateRoutes
            )
        {
            _next = next;
            //_noValidateroutes = noValidateRoutes.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                string authHeader = httpContext.Request.Headers["Authorization"];
                if (_noValidateroutes.AnonymousRoutes.Contains(httpContext.Request.Path.Value.ToLower()))
                {
                    if (authHeader != null && authHeader.StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
                    {
                        string ecodeUserNameAndPassword = authHeader.Substring("Basic ".Length).Trim();
                        Encoding encoding = Encoding.GetEncoding("UTF-8");
                        string usernameAndPassword = encoding.GetString(Convert.FromBase64String(ecodeUserNameAndPassword));
                        int index = usernameAndPassword.IndexOf(":");
                        var username = usernameAndPassword.Substring(0, index);
                        var password = usernameAndPassword.Substring(index + 1);
                        // var passwordEry = UtilityHelper.ConvertMd5ToString(password, SecurityConstants.EncryptionKey);
                        //if (username.Equals(Constants.Username) && password.Equals(Constants.UserPassword))
                        //{
                        //    await _next.Invoke(httpContext);
                        //}
                        //else
                        //{
                        //    httpContext.Response.StatusCode = 401;
                        //    httpContext.Response.Headers.Add("WWW-Authenticate", "Basic realm=\"dotnetthoughtsCredentialsError.net\"");
                        //    return;
                        //}
                    }
                    else
                    {
                        httpContext.Response.StatusCode = 401;
                        httpContext.Response.Headers.Add("WWW-Authenticate", "Basic realm=\"dotnetthoughtsAuth.net\"");
                        return;
                    }
                }
                else
                {
                    await _next.Invoke(httpContext);
                }
            }

            catch (Exception e)
            {
                //_logger.Error(e.ToString());
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BasicAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseBasicAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthenticationMiddleware>();
        }
    }
}
