using System.Collections;

namespace Icon.ApiController.Controller.CollectionProcessors
{
    public interface ICollectionProcessor<T> where T : ICollection
    {
        void GenerateMessages(T collection);
    }
}
