namespace OOS.Model
{
    public interface Handles<T> where T : class
    {
        void Handle(T message);
    }
}
