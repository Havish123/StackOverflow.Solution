

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using StackOverflow.Solution.DTO;
using StackOverflow.Solution.DTO.Common;
using StackOverflow.Solution.DTO.Enums;
using StackOverflow.Solution.Services.Common;
using System.Net;
using System.Web.Http;

namespace StackOverflow.Solution.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IHttpActionResult Result(string methodName, Func<ResultDto> delegatemethod)
        {
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                result = delegatemethod.Invoke();
            }
            catch (Exception exception)
            {
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = "Internal Server Error";
                result.ErrorDto.Message = "Internal Server Error";
            }
            if (result.ErrorDto.ErrorCode == "Internal Server Error")
            {
                errorDto.ErrorCode = "Internal Server Error";
                errorDto.Message = result.ErrorDto.Message;
                contentDto.RXHJZXB0 = HelperTool.EncryptObject(errorDto);
                return (IHttpActionResult)Ok(contentDto);
                //return Content(HttpStatusCode.InternalServerError, HelperTool.EncryptObject(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.U3VJY2VZ = HelperTool.EncryptObject(successDto);
                return (IHttpActionResult)Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.RXJYB3JY = HelperTool.EncryptObject(errorDto);
                return (IHttpActionResult)Ok(contentDto);
            }
        }

        protected IHttpActionResult Result<T>(string inputKey, string methodName, Func<T, ResultDto> delegatemethod)
        {
            {
                var result = new ResultDto();
                var errorDto = new ErrorDto();
                var successDto = new SuccessDto();
                var contentDto = new ContentDto();
                T input;
                try
                {
                    string decryptedInput;
                    try
                    {
                        decryptedInput = HelperTool.Decrypt(inputKey);
                    }
                    catch (Exception exception)
                    {
                        errorDto.ErrorCode = "Invalid Request";
                        errorDto.Message = "Invalid Request";
                        contentDto.RXHJZXB0 = HelperTool.EncryptObject(errorDto);
                        return (IHttpActionResult)Ok(contentDto);
                    }
                    try
                    {
                        input = HelperTool.ConvertJsonToObject<T>(decryptedInput);
                        //_logger.Info($"Json Input : {JsonConvert.SerializeObject(input)}");
                    }
                    catch (Exception exception)
                    {
                        errorDto.ErrorCode = "Internal Server Error";
                        errorDto.Message = "Internal Server Error";
                        contentDto.RXHJZXB0 = HelperTool.EncryptObject(errorDto);
                        return (IHttpActionResult)Ok(contentDto);
                    }
                    result = delegatemethod.Invoke(input);
                }
                catch (Exception exception)
                {
                    result.IsSuccess = false;
                    result.ErrorDto.ErrorCode = "Internal Server Error";
                    result.ErrorDto.Message = "Internal Server Error";
                }
                if (result.ErrorDto.ErrorCode == "Internal Server Error")
                {
                    errorDto.ErrorCode = "Internal Server Error";
                    errorDto.Message = result.ErrorDto.Message;
                    contentDto.RXHJZXB0 = HelperTool.EncryptObject(errorDto);
                    return (IHttpActionResult)Ok(contentDto);
                    //return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
                }
                if (result.IsSuccess)
                {
                    successDto.Response = result.SuccessDto.Response;
                    successDto.Message = result.SuccessDto.Message;
                    contentDto.U3VJY2VZ = HelperTool.EncryptObject(successDto);
                    return (IHttpActionResult)Ok(contentDto);
                }
                else
                {
                    errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                    errorDto.Message = result.ErrorDto.Message;
                    contentDto.RXJYB3JY = HelperTool.EncryptObject(errorDto);
                    return (IHttpActionResult)Ok(contentDto);
                }
            }
        }

    }
}
