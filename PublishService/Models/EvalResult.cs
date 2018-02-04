using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublishService
{
    public class EvalResult
    {
        public int EvalId { get; set; }
        public String EvaluationTopic { get; set; }
        public String[] Results { get; set; }
    }
}
