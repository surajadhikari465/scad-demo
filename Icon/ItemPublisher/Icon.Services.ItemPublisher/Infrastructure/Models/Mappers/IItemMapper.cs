using Icon.Services.ItemPublisher.Repositories.Entities;

namespace Icon.Services.ItemPublisher.Infrastructure.Models.Mappers
{
    public interface IItemMapper
    {
        ItemModel MapEntityToModel(Item entity);
    }
}