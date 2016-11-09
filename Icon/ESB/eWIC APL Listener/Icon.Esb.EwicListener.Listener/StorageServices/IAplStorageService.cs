using Icon.Esb.EwicAplListener.Common.Models;

namespace Icon.Esb.EwicAplListener.StorageServices
{
    public interface IAplStorageService
    {
        void Save(AuthorizedProductListModel model);
    }
}
