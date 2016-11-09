using Icon.Esb.EwicAplListener.Common.Models;

namespace Icon.Esb.EwicAplListener.NewAplProcessors
{
    public interface INewAplProcessor
    {
        void ApplyMappings(AuthorizedProductListModel model);
        void ApplyExclusions(AuthorizedProductListModel model);
    }
}
