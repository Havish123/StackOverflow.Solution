using StackOverflow.Solution.Authenticate;
using StackOverflow.Solution.DTO;
using StackOverflow.Solution.DTO.Common;
using StackOverflow.Solution.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Solution.Services
{
    public interface IAuthenticateService
    {
        ResultDto GetToken();
    }
    public class AuthenticateService : IAuthenticateService
    {
        public ResultDto GetToken()
        {
            var resultDto=new ResultDto();
            try
            {
                var newSystemToken = FirstlevelTokenManager.GenerateToken(new List<Claim>
                                {
                                    new Claim("FirstLevel",
                                        HelperTool.Encrypt(Config.LoginApiTokenKey))
                                }); 
                resultDto.SuccessDto.Response= newSystemToken;
                resultDto.IsSuccess=true;
                resultDto.SuccessDto.Message = Message.Success;
            }
            catch(Exception ex)
            {
                resultDto.ErrorDto.Response=ex;
                resultDto.IsSuccess=false;
                resultDto.ErrorDto.Message = Message.Exception;
            }
            return resultDto;
        }
    }
}
