using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Solution.Authenticate
{
    public class TokenValidationResult
    {
        public IEnumerable<Claim> Claims { get; set; }
        public bool IsValid { get; set; }
    }
}
