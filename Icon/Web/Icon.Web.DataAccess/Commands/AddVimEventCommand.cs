namespace Icon.Web.DataAccess.Commands
{
    public class AddVimEventCommand
    {
        public int EventReferenceId { get; set; }
        public int EventTypeId { get; set; }
        public string EventMessage { get; set; }
    }
}

