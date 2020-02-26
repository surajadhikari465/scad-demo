﻿using Mammoth.Esb.HierarchyClassListener.Models;
using System.Collections.Generic;

namespace Mammoth.Esb.HierarchyClassListener.Commands
{
    public class DeleteManufacturerParameter : DeleteHierarchyClassesParameter, IHierarchyClassesParameter
    {
        public List<HierarchyClassModel> Manufacturer
        {
            get { return base.HierarchyClasses; }
            set { base.HierarchyClasses = value; }
        }
    }
}