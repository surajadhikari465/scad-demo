namespace Services.Extract.DataAccess.Commands
{
    public class UpdateJobStatusCommand
    {
        public int JobScheduleId { get; set; }
        public string Status { get; set; }
    }
}
