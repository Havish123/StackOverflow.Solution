using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Solution.Data.Entities
{
    public class Auditable
    {
        public long Id { get; set; }
        public DateTime CreatedDate{get;set;}
        public long CreatedBy { get; set; }
    }
}
