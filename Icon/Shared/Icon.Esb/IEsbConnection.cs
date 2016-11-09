using System;
using TIBCO.EMS;

namespace Icon.Esb
{
    public interface IEsbConnection : IDisposable
    {
        EsbConnectionSettings Settings { get; }

        bool IsConnected { get; }

        event EventHandler<EMSException> ExceptionHandlers;

        void OpenConnection();
    }
}
