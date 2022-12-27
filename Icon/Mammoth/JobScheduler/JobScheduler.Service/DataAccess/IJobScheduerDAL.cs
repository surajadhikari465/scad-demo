using JobScheduler.Service.Model.DBModel;
using System.Collections.Generic;

namespace JobScheduler.Service.DataAccess
{
    internal interface IJobScheduerDAL
    {
        List<GetJobSchedulesQueryModel> GetJobSchedules();
        void UpdateLastRunDateTime(int jobScheduleId);
    }
}
