using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
	public class GetHierarchyQuery : IQueryHandler<GetHierarchyParameters, List<Hierarchy>>
	{
		private readonly IconContext context;

		public GetHierarchyQuery(IconContext context)
		{
			this.context = context;
		}

		public List<Hierarchy> Search(GetHierarchyParameters parameters)
		{
			IQueryable<Hierarchy> hierarchies = context.Hierarchy;

			if (parameters.IncludeNavigation)
			{
				hierarchies = hierarchies
					.Include(h => h.HierarchyPrototype)
					.Include(h => h.HierarchyClass.Select(hc => hc.HierarchyClassTrait.Select(hct => hct.Trait)));
			}

			if (parameters.HierarchyId > 0)
			{
				hierarchies = hierarchies.Where(h => h.hierarchyID == parameters.HierarchyId);
			}

			if (!String.IsNullOrEmpty(parameters.HierarchyName))
			{
				hierarchies = hierarchies.Where(h => h.hierarchyName == parameters.HierarchyName);
			}

			return hierarchies.ToList();
		}
	}
}
