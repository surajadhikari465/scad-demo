using Icon.Services.ItemPublisher.Repositories.Entities;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue
{
    public interface IValueFormatter
    {
        string FormatValueForMessage(Attributes attribute, string attributeValue);
    }
}