using System;

namespace Icon.Monitoring.Service
{
    public interface IDateService
    {
        DateTime UtcNow { get; }
    }
}