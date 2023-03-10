using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Solution.Authenticate
{
    public class SecondLevelMiddleware
    {
        private readonly RequestDelegate _next;

        public SecondLevelMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Get the token generated by the first middleware component
            var token = context.Items["BaseToken"] as string;

            // Validate the token
            var tokenValidationResult = ValidateToken(token);
            if (!tokenValidationResult.IsValid)
            {
                context.Response.StatusCode = 401; // Unauthorized
                return;
            }

            // Generate a new token for authorization
            var authorizationToken = GenerateAuthorizationToken(tokenValidationResult.Claims);

            // Store the authorization token in the HttpContext for future use
            context.Items["AuthorizationToken"] = authorizationToken;

            // Call the next middleware component in the pipeline
            await _next(context);
        }

        private TokenValidationResult ValidateToken(string token)
        {
            var result = new TokenValidationResult { IsValid = false };

            try
            {
                // Define the security key
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Your Secret Key"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Define the token validation parameters
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "My Web App",
                    ValidAudience = "My Web App",
                    IssuerSigningKey = key
                };

                // Create a claims principal
                ClaimsPrincipal claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                result.Claims = claimsPrincipal.Claims;
                result.IsValid = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        private string GenerateAuthorizationToken(IEnumerable<Claim> claims)
        {
            // Define the claims for the token
            var authorizationClaims = new[]
            {
                new Claim("IsAuthorized", "true"),
            };

            // Create a security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Your Secret Key"));

            // Create a signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create a JWT token
            var token = new JwtSecurityToken(
                issuer: "My Web App",
                audience: "My Web App",
                claims: authorizationClaims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            // Return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
