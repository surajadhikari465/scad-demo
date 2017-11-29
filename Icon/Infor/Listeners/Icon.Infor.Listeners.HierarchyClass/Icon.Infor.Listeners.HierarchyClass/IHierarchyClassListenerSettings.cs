namespace Icon.Infor.Listeners.HierarchyClass
{
    public interface IHierarchyClassListenerSettings
    {
        bool EnableNationalClassEventGeneration { get; set; }
        bool ValidateSequenceId { get; set; }
        bool EnableConfirmBods { get; set; }
        int MaxNumberOfRetries { get; set; }
        int RetryDelayInMilliseconds { get; set; }
    }
}