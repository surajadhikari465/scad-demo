using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Services
{
    public interface IItemPublisherService
    {
        Task Process(int batchSize);
    }
}