namespace IconWebApi.Service.Services
{
	public interface IUpdateService<T>
	{
		void Handle(T data);
	}
}
