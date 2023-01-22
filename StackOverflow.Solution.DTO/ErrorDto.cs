using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Solution.DTO
{
    public class ErrorDto
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "errorcode")]
        public string ErrorCode { get; set; }

        [JsonProperty(PropertyName = "response")]
        public object Response { get; set; }

        public ErrorDto()
        {
            Message = string.Empty;
            ErrorCode = string.Empty;
        }
    }
}
