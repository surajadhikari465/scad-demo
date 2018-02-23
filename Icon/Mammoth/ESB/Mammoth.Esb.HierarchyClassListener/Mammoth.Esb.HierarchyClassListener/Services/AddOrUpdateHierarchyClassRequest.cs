using Mammoth.Esb.HierarchyClassListener.Models;
using System.Collections.Generic;

namespace Mammoth.Esb.HierarchyClassListener.Services
{
    public class AddOrUpdateHierarchyClassRequest : IHierarchyClassRequest
    {
        public List<HierarchyClassModel> HierarchyClasses { get; set; }
    }
}
