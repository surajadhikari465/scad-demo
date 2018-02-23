using Mammoth.Esb.HierarchyClassListener.Commands;
using Mammoth.Esb.HierarchyClassListener.Models;

namespace Mammoth.Esb.HierarchyClassListener.Services
{
    public interface IHierarchyClassService<T> where T : IHierarchyClassRequest
    {
        void ProcessHierarchyClasses(T request);
    }
}
