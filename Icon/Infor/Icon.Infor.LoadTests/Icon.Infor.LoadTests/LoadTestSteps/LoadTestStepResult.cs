using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.LoadTests.LoadTestSteps
{
    public class LoadTestStepResult
    {
        public bool Success { get; set; }

        public static implicit operator bool(LoadTestStepResult result)
        {
            return result.Success;
        }

        public static implicit operator LoadTestStepResult(bool b)
        {
            return new LoadTestStepResult { Success = b };
        }
    }
}
