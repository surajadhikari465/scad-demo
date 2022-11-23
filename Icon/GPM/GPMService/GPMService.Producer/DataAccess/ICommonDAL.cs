namespace GPMService.Producer.DataAccess
{
    internal interface ICommonDAL
    {
        void UpdateStatusToRunning(int jobScheduleID);
        void UpdateStatusToReady(int jobScheduleID);
    }
}
