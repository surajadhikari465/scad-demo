using System.Collections.Generic;

namespace PushController.Controller.UdmDeleteServices
{
    public interface IUdmDeleteService<T>
    {
        void DeleteEntitiesBulk(List<T> entities);

        void DeleteEntitiesRowByRow(List<T> entities);
    }
}
