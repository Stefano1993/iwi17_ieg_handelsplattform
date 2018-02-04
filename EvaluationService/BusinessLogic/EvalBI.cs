using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationService.BusinessLogic
{
    public class EvalBI
    {
        private static Dictionary<int, EvalResult> _results = new Dictionary<int, EvalResult>();

        public EvalBI()
        {
            if (!_results.ContainsKey(1))
            {
                for (int i = 1; i < 5; i++)
                {
                    EvalResult _result = new EvalResult();
                    _result.EvalId = i;
                    _result.OwnerCredentials = "credential"+i;
                    _result.EvaluationTopic = "Topic " + i;
                    _result.Results = new string[] { "xxxx", "yyyy", "cccc", i.ToString() };
                    _results.Add(i, _result);
                }
            }
        }

        public async Task<EvalResult> GetEvalById(int id)
        {

            if (_results.ContainsKey(id))
            {
                return _results[id];
            }
            return null;
        }    

    }
}
