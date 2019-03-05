using System.Linq;
using Microsoft.AspNetCore.Mvc;
using KitBuilderWebApi.Controllers;
using KitBuilderWebApi.Models;
using KitBuilderWebApi.QueryParameters;

namespace KitBuilderWebApi.Helper
{
  public class VenueHelper : IHelper<VenueInfo, VenueParameters>
  {
    private IUrlHelper urlHelper;

    public VenueHelper(IUrlHelper argUrlHelper) { urlHelper = argUrlHelper; }

    public IQueryable<VenueInfo> SetOrderBy(IQueryable<VenueInfo> dataSource, VenueParameters Parameters)
    {
      try { return KitItemHelper.SetOrderBy<VenueInfo>(dataSource, Parameters.OrderBy); }
      catch { throw; }
    }

    public object GetPaginationData(PagedList<VenueInfo> DataAfterPaging, VenueParameters Parameters)
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

    internal string CreateItemsResourceUri(VenueParameters ItemsParameters, ResourceUriType type)
    {
#if false  //It's unlikely DM will require this fuctionality. Enable it here if needed

      return urlHelper.Link("GetVenue", new { fields = ItemsParameters.Fields,
                                                 orderBy = ItemsParameters.OrderBy,
                                                 pageNumber = ItemsParameters.PageNumber + (type == ResourceUriType.PreviousPage ? - 1 : type == ResourceUriType.NextPage ? 1 : 0),
                                                 pageSize = ItemsParameters.PageSize});
#else
      return "GetVenue";
#endif
    }
  }
}