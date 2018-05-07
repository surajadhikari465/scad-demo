using PrimeAffinityController.Models;

namespace PrimeAffinityController.Commands
{
    public class ClearInstanceIdFromJobScheduleCommand
    {
        public JobScheduleModel JobSchedule { get; set; }
    }
}
