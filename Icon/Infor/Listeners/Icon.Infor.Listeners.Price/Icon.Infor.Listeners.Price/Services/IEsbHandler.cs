namespace Icon.Infor.Listeners.Price.Services
{
    public interface IEsbHandler<TRequest>
    {
        void Send(TRequest request);
    }
}
