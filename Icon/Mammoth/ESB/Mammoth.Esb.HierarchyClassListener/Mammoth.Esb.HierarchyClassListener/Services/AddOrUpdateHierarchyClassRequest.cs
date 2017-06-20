using Mammoth.Esb.HierarchyClassListener.Models;
using System.Collections.Generic;

namespace Mammoth.Esb.HierarchyClassListener.Services
{
    public class AddOrUpdateHierarchyClassRequest
    {
        public List<HierarchyClassModel> HierarchyClasses { get; set; }
    }
}
