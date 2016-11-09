using Mammoth.Esb.HierarchyClassListener.Commands;

namespace Mammoth.Esb.HierarchyClassListener.Services
{
    public interface IHierarchyClassService
    {
        void AddOrUpdateHierarchyClasses(AddOrUpdateHierarchyClassesCommand command);
    }
}
