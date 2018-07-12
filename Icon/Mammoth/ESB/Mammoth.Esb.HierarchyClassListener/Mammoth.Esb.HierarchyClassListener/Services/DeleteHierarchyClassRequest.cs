using Mammoth.Esb.HierarchyClassListener.Models;
using System.Collections.Generic;

namespace Mammoth.Esb.HierarchyClassListener.Services
{
    public class DeleteBrandRequest : IHierarchyClassRequest
    {
        public List<HierarchyClassModel> HierarchyClasses { get; set; }
    }
    public class DeleteMerchandiseClassRequest : IHierarchyClassRequest
    {
        public List<HierarchyClassModel> HierarchyClasses { get; set; }
    }
    public class DeleteNationalClassRequest : IHierarchyClassRequest
    {
        public List<HierarchyClassModel> HierarchyClasses { get; set; }
    }
}