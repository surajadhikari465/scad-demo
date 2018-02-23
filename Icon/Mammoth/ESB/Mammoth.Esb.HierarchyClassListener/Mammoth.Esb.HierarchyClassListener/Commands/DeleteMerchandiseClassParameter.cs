using Mammoth.Esb.HierarchyClassListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener.Commands
{
    public class DeleteMerchandiseClassParameter : DeleteHierarchyClassesParameter, IHierarchyClassesParameter
    {
        public List<HierarchyClassModel> MerchandiseClasses
        {
            get => base.HierarchyClasses;
            set => base.HierarchyClasses = value;
        }
    }
}
