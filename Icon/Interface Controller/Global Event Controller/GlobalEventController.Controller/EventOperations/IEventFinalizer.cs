namespace GlobalEventController.Controller.EventOperations
{
    public interface IEventFinalizer
    {
        void HandleFailedEvents();
        void DeleteEvents();
    }
}
