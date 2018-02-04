using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationService
{
    public class EvalResult
    {
        public int EvalId { get; set; }
        public String OwnerCredentials { get; set; }
        public String EvaluationTopic { get; set; }
        public String[] Results { get; set; }
    }
}
