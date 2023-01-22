using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Solution.DTO
{
    public class SuccessDto
    {
        public string Message { get; set; }
        public object Response { get; set; }
        public SuccessDto()
        {
            Message= string.Empty;
        }
    }
}
