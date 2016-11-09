namespace Icon.Logging
{
    public interface ILogger<T> where T: class
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Debug(string message);
    }
}