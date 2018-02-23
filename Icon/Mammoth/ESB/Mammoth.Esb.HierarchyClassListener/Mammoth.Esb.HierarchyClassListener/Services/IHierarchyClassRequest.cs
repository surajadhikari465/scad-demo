using Mammoth.Esb.HierarchyClassListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener.Services
{
    public interface IHierarchyClassRequest
    {
        List<HierarchyClassModel> HierarchyClasses { get; set; }
    }
}
