using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<HierarchyClassDataAccessModel> ToDataAccessModels(
            this IEnumerable<InforHierarchyClassModel> hierarchyClasses)
        {
            try
            {
                return hierarchyClasses
                    .Select(hc => new HierarchyClassDataAccessModel(hc));
            }
            catch(Exception ex)
            {
                throw new ArgumentException("Unable to parse HierarchyClassId from message.", ex);
            }
        }
                
        public static IEnumerable<HierarchyClassTraitDataAccessModel> ToTraitDataAccessModels(this IEnumerable<HierarchyClassDataAccessModel> hierarchyClasses)
        {
            return hierarchyClasses
                .SelectMany(hc => new List<HierarchyClassTraitDataAccessModel>(
                        hc.HierarchyClassTraits.Select(hct => new HierarchyClassTraitDataAccessModel
                        {
                            HierarchyClassId = hc.HierarchyClassId,
                            TraitId = hct.Key,
                            TraitValue = hct.Value
                        })
                    )
                );
        }

        public static int? ToHierarchyParentClassId(this int hierarchyParentClassId)
        {
            return hierarchyParentClassId == 0 ? (int?)null : hierarchyParentClassId;
        }

        public static List<string> ToRegionList(this string regionsString)
        {
            return regionsString == null ? new List<string>() : regionsString.Split(',').ToList();
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }
    }
}
