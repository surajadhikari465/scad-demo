using System.Collections.Generic;

namespace PushController.Controller.UdmUpdateServices
{
    public interface IUdmUpdateService<T>
    {
        void SaveEntitiesBulk(List<T> entitiesToSave);
        void SaveEntitiesRowByRow(List<T> entitiesToSave);
    }
}
