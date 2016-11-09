namespace Icon.Monitoring.Monitors
{
    public interface IMonitor
    {
        void CheckStatusAndNotify();
    }
}