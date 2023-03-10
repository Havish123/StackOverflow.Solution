using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackOverflow.Solution.DTO;
using StackOverflow.Solution.Services;

namespace StackOverflow.Solution.API.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthenticateController : BaseController
    {
        private readonly IAuthenticateService _authenticateService;
        public AuthenticateController(IAuthenticateService authenticateService)
        {
            _authenticateService= authenticateService;
        }
        [HttpGet]
        [Route("token")]
        public ResultDto GetToken()
        {
            return _authenticateService.GetToken();
        }
        [HttpGet]
        [Route("Sample")]
        public ResultDto Sample()
        {
            return new ResultDto();
        }
    }
}
