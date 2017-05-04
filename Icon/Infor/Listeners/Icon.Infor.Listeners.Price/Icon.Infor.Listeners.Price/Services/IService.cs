using Icon.Esb.Subscriber;
using System.Collections.Generic;

namespace Icon.Infor.Listeners.Price.Services
{
    public interface IService<TRequest>
    {
        void Process(IEnumerable<TRequest> data, IEsbMessage message);
    }

    public interface IServiceRequest<TRequest, TResponse>
    {
        IEnumerable<TResponse> Handle(IEnumerable<TRequest> data, IEsbMessage message);
    }
}
