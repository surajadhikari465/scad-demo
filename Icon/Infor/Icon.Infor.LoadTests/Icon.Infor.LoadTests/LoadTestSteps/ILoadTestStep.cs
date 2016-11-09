using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.LoadTests.LoadTestSteps
{
    public interface ILoadTestStep
    {
        LoadTestStepResult Execute();
    }
}