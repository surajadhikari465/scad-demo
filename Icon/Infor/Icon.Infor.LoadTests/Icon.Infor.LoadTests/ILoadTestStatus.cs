namespace Icon.Infor.LoadTests
{
    public interface ILoadTestStatus
    {
        double ElapsedTime { get; set; }
        int ProcessedEntities { get; set; }
        int UnprocessedEntities { get; set; }
        int FailedEntities { get; set; }
    }
}
