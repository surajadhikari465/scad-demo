using Icon.Esb.EwicAplListener.Common.Models;

namespace Icon.Esb.EwicAplListener.MappingGenerators
{
    public interface IMappingGenerator
    {
        void GenerateMappings(EwicItemModel item);
    }
}
