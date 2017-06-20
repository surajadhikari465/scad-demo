using Mammoth.Esb.HierarchyClassListener.Commands;

namespace Mammoth.Esb.HierarchyClassListener.Services
{
    public interface IHierarchyClassService<T> where T : class
    {
        void ProcessHierarchyClasses(T request);
    }
}
