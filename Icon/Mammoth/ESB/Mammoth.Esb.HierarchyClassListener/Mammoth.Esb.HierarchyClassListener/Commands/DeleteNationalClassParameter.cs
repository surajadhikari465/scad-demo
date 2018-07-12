using Mammoth.Esb.HierarchyClassListener.Models;
using System.Collections.Generic;

namespace Mammoth.Esb.HierarchyClassListener.Commands
{
    public class DeleteNationalClassParameter : DeleteHierarchyClassesParameter, IHierarchyClassesParameter
    {
        public List<HierarchyClassModel> NationalClasses
        {
            get { return base.HierarchyClasses; }
            set { base.HierarchyClasses = value; }
        }
    }
}
