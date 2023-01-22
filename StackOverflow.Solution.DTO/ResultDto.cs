using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Solution.DTO
{
    public class ResultDto
    {
        public bool IsSuccess { get; set; }
        public SuccessDto SuccessDto { get; set; }
        public ErrorDto ErrorDto { get; set; }
        public ResultDto()
        {
            SuccessDto = new SuccessDto();
            ErrorDto = new ErrorDto();
            IsSuccess= false;
        }
    }
}
