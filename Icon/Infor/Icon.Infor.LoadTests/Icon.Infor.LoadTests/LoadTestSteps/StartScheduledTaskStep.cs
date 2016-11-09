using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.LoadTests.LoadTestSteps
{
    public class StartScheduledTaskStep : ILoadTestStep
    {
        public string Server { get; set; }
        public string Name { get; set; }

        public LoadTestStepResult Execute()
        {
            using (TaskService ts = new TaskService(@"\\" + Server))
            {
                var task = ts.FindTask(Name);

                task.Run();
            }

            return true;
        }
    }
}
