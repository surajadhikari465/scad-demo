using PrimeAffinityController.Models;

namespace PrimeAffinityController.Commands
{
    public class UpdateJobStatusCommand
    {
        public JobScheduleModel JobSchedule { get; set; }
        public string Status { get; set; }
    }
}
