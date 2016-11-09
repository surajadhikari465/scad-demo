namespace Icon.Monitoring.DataAccess.Queries
{
    using Common.Enums;
    using Icon.Common.DataAccess;

    public interface IQueryByRegionHandler<TParameters, TResult> where TParameters : IQuery<TResult>
    {
        IrmaRegions TargetRegion { get; set; }

        TResult Search(TParameters parameters);
    }
}
