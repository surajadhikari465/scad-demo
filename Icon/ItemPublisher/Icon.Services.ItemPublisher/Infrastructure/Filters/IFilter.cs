using Icon.Services.ItemPublisher.Infrastructure.Models;

namespace Icon.Services.ItemPublisher.Infrastructure.Filters
{
    public interface IFilter
    {
        bool Filter(ItemModel item);
    }
}
