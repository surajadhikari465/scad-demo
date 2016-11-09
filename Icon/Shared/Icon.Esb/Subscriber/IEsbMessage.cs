
namespace Icon.Esb.Subscriber
{
    public interface IEsbMessage
    {
        string MessageText { get; set; }
        string GetProperty(string propertyName);
        void Acknowledge();
    }
}
