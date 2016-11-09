
namespace MammothWebApi.Service.Services
{
    public interface IService<T>
    {
        void Handle(T data);
    }
}
