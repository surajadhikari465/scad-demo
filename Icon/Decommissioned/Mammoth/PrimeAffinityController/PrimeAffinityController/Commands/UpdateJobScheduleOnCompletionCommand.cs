using PrimeAffinityController.Models;

namespace PrimeAffinityController.Commands
{
    public class UpdateJobScheduleOnCompletionCommand
    {
        public JobScheduleModel JobSchedule { get; set; }
        public string Status { get; set; }
    }
}
