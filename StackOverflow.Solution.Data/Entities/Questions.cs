using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Solution.Data.Entities
{
    public class Questions: Auditable
    {
        public string Name { get; set; }
        public long UserId { get; set; }
        public long CategoryId { get; set; }
        public long AnswerCount { get; set; }
        public long viewsCount { get; set; }
    }
}
