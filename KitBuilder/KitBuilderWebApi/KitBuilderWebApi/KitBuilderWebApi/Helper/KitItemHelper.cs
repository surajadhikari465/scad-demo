using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using KitBuilder.DataAccess.Dto;

namespace KitBuilderWebApi.Helper
{
  public class KitItemHelper : IHelper<ItemsDto, KitItemParameters>
  {
    private IUrlHelper urlHelper;

    public KitItemHelper(IUrlHelper argUrlHelper) { urlHelper = argUrlHelper; }

    public object GetPaginationData(PagedList<ItemsDto> DataAfterPaging, KitItemParameters Parameters)
    {
#if false //It's unlikely DM will require this fuctionality. Enable it here if needed
      return new { previousPageLink = DataAfterPaging.HasPrevious ? CreateItemsResourceUri(Parameters, ResourceUriType.PreviousPage) : null,
                   PageLink = DataAfterPaging.HasNext ? CreateItemsResourceUri(Parameters, ResourceUriType.NextPage) : null,
                   totalCount = DataAfterPaging.TotalCount,
                   pageSize = DataAfterPaging.PageSize,
                   currentPage = DataAfterPaging.CurrentPage,
                   totalPages = DataAfterPaging.TotalPages };
#else
      return null;
#endif
    }

    internal string CreateItemsResourceUri(KitItemParameters ItemsParameters, ResourceUriType type)
    {
#if false  //It's unlikely DM will require this fuctionality. Enable it here if needed
      return urlHelper.Link("GetKitItems", new { fields = ItemsParameters.Fields,
                                                orderBy = ItemsParameters.OrderBy,
                                                pageNumber = ItemsParameters.PageNumber + (type == ResourceUriType.PreviousPage ? - 1 : type == ResourceUriType.NextPage ? 1 : 0),
                                                pageSize = ItemsParameters.PageSize});
#else
      return "GetKitItems";
#endif
    }

    public IQueryable<ItemsDto> SetOrderBy(IQueryable<ItemsDto> dataSource, KitItemParameters Parameters)
    {
      try { return SetOrderBy<ItemsDto>(dataSource, Parameters.OrderBy); }
      catch { throw; }
    }

    public static IEnumerable<string> GetFieldList<T>(string fields)
    {
      if(String.IsNullOrWhiteSpace(fields)) return new HashSet<string>();

      var hsFields = new HashSet<string>(fields.Split(",").Select(x => x.Trim()).Where(x => !String.IsNullOrEmpty(x)), StringComparer.InvariantCultureIgnoreCase);
      var names = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Select(x => x.Name).ToArray();
      
      if(hsFields.Except(names, StringComparer.InvariantCultureIgnoreCase).Any())
      {
        throw new Exception($"Invalid properties in object of type {typeof(T).Name}: {String.Join(",", hsFields.Except(names, StringComparer.InvariantCultureIgnoreCase) )}");
      }

      return hsFields;
    }

    public static IQueryable<T> SetOrderBy<T>(IQueryable<T> dataSource, string orderBy)
    {
      try
      {
        if(dataSource is null || String.IsNullOrEmpty(orderBy)) return dataSource;

        foreach(string order in GetFieldList<T>(orderBy).Reverse())
        {
          dataSource = dataSource.OrderBy(order);
        }

        return dataSource;
      }
      catch { throw; }
    }
  }
}