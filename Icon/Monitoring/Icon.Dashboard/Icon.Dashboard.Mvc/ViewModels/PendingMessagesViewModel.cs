namespace Icon.Dashboard.Mvc.ViewModels
{
    using System.Collections.Generic;

    public class PendingMessagesViewModel
    {
        public Dictionary<string, int> PendingMessagesCountByType { get; }

        public PendingMessagesViewModel(
            Dictionary<string, int> pendingMessagesCountByType)
        {
            this.PendingMessagesCountByType = pendingMessagesCountByType;
        }
    }
}