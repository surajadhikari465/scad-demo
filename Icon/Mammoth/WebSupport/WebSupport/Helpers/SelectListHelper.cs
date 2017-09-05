using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSupport.ViewModels;

namespace WebSupport.Helpers
{
    public static class SelectListHelper
	{
		public static IEnumerable<SelectListItem> ArrayToSelectList<T>(T[] array, int selectedId = -1)
		{
			return array.Select(i =>
				new SelectListItem
				{
					Selected = (Array.IndexOf(array, i) == selectedId),
					Text = i.ToString(),
					Value = Array.IndexOf(array, i).ToString()
				});
        }

        public static IEnumerable<SelectListItem> StoreArrayToSelectList(StoreViewModel[] array)
        {
            return array.Select(i =>
                new SelectListItem
                {
                    Selected = false,
                    Text = i.BusinessUnit + ": " + i.Name,
                    Value = i.BusinessUnit.ToString()
                });
        }
    }
}