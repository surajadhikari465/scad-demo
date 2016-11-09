using Icon.Esb.EwicAplListener.Common.Models;

namespace Icon.Esb.EwicAplListener.ExclusionGenerators
{
    public interface IExclusionGenerator
    {
        void GenerateExclusions(EwicItemModel item);
    }
}
